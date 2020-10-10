using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Utilities
{
    class KeyCodeToUnicode
    {
        private const uint VK_CAPITAL = 0x14;

        [DllImport("USER32.dll")]
        private static extern short GetKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags,
            IntPtr dwhkl);


        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(
            uint uCode,
            uint uMapType);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);


        public static string VKCodeToUnicode(uint vkCode)
        {
            StringBuilder buf = new StringBuilder();

            byte[] keyboardState = new byte[255];

            short x;
            byte y;

            for (int i = 0; i < 255; i++)
            {
                if (i == VK_CAPITAL)
                {
                    x = GetKeyState(i);
                }
                else
                {
                    x = GetAsyncKeyState(i);
                }
                y = 0;
                if ((x & 0x8000) != 0) y = (byte)(y | 0x80);
                if ((x & 0x0001) != 0) y = (byte)(y | 0x01);
                keyboardState[i] = y;
            }

            ToUnicodeEx(vkCode, MapVirtualKey(vkCode, 0), keyboardState, buf, 5, 0,
                GetKeyboardLayout(GetWindowThreadProcessId(IntPtr.Zero, IntPtr.Zero)));
            return buf.ToString().Trim() != "" ? buf.ToString() : ((Keys)vkCode).ToString();
        }

    }
}
