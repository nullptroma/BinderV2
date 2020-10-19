using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Hooks.Keyboard
{
    class KeyboardHook
    {
        // Импортируем необходимые функции WinAPI, объявляем нужные для них структуры и константы
        private static class WinAPI
        {
            public static class Kernel32
            {
                [DllImport("kernel32", CharSet = CharSet.Unicode)]
                public static extern IntPtr LoadLibrary(string lpFileName);
            }

            public static class User32
            {
                // KBDLLHOOKSTRUCT
                // https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms644967(v=vs.85).aspx
                public struct KeyboardHookStruct
                {
                    public uint VKCode;
                    public uint ScanCode;
                    public uint Flags;
                    public uint Time;
                    public IntPtr dwExtraInfo;
                }

                // Константы WH_*
                public enum WindowsHook : int
                {
                    KeyboardLowLevel = 13,
                    MouseLowLevel = 14,
                }

                // Константы WM_*
                public enum WindowsMessage : int
                {
                    KeyDown = 0x100,
                    KeyUp = 0x101,

                    SysKeyDown = 0x104,
                    SysKeyUp = 0x105,

                    MouseMove = 0x200,
                    LeftButtonDown = 0x201,
                    LeftButtonUp = 0x202,

                    RightButtonDown = 0x204,
                    RightButtonUp = 0x205,

                    MiddleButtonDown = 0x207,
                    MiddleButtonUp = 0x208,
                }

                public delegate int KeyboardHookProc(int code,
                    WindowsMessage wParam, ref KeyboardHookStruct lParam);

                [DllImport("user32")]
                public static extern int CallNextHookEx(IntPtr hHk, int nCode,
                    WindowsMessage wParam, ref KeyboardHookStruct lParam);

                [DllImport("user32")]
                public static extern IntPtr SetWindowsHookEx(WindowsHook idHook,
                    KeyboardHookProc lpfn, IntPtr hMod, uint dwThreadId);

            }
        }

        // Дескриптор хука
        private static readonly IntPtr _keyboardHookHandle;

        // Хук
        private static readonly WinAPI.User32.KeyboardHookProc _keyboardCallback;

        // События
        public static event KeyEventHandlerCustom KeyDown;
        public static event KeyEventHandlerCustom KeyUp;

        private static int KeyboardCallBack(int code, WinAPI.User32.WindowsMessage wParam, ref WinAPI.User32.KeyboardHookStruct lParam)
        {
            // Если code < 0, мы не должны обрабатывать это сообщение системы
            if (code >= 0)
            {
                var key = KeyInterop.KeyFromVirtualKey((int)lParam.VKCode);
                KeyEventArgsCustom eventArgs = new KeyEventArgsCustom(key);

                // В зависимости от типа пришедшего сообщения вызовем то или иное событие
                if (lParam.dwExtraInfo == IntPtr.Zero)
                    switch (wParam)
                    {
                        case WinAPI.User32.WindowsMessage.KeyDown:
                        case WinAPI.User32.WindowsMessage.SysKeyDown:
                            KeyDown?.Invoke(null, eventArgs);
                            break;

                        case WinAPI.User32.WindowsMessage.KeyUp:
                        case WinAPI.User32.WindowsMessage.SysKeyUp:
                            KeyUp?.Invoke(null, eventArgs);
                            break;
                    }
                // Если событие помечено приложением как обработанное,
                // прервём дальнейшее распространение сообщения
                if (eventArgs.Handled)
                    return 1;
            }
            // Вызовем следующий обработчик
            return WinAPI.User32.CallNextHookEx(_keyboardHookHandle, code, wParam, ref lParam);
        }

        static KeyboardHook()
        {
            // В SetWindowsHookEx следует передать дескриптор библиотеки user32.dll
            // Библиотека user32 всё равно всегда загружена в приложениях .NET,
            // хранить и освобождать дескриптор или что-либо ещё с ним делать нет необходимости
            IntPtr user32Handle = WinAPI.Kernel32.LoadLibrary("user32");

            // Установим хук
            _keyboardCallback = KeyboardCallBack;
            _keyboardHookHandle = WinAPI.User32.SetWindowsHookEx(
                WinAPI.User32.WindowsHook.KeyboardLowLevel, _keyboardCallback, user32Handle, 0);

        }
    }
}
