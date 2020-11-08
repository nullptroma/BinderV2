using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using InterpreterScripts.InterpretationFunctions.Standart;
using System.ComponentModel;
using System.Windows.Input;

namespace InterpreterScripts.InterpretationFunctions.Standart.Library
{
    class VoidScripts
    {
        [Description("MsgBox(string title, object text1...) - выводит сообщение, объединяя все переданные аргументы.")]
        public static object[] MsgBox(params object[] ps)
        {
            if (ps.Length > 1)
            {
                string str = "";
                for (int i = 1; i < ps.Length; i++)
                    str += ps[i].ToString();
                MessageBox.Show(str, ps[0].ToString());
            }
            else
            {
                string str = "";
                foreach (var r in ps)
                    str += r.ToString();
                MessageBox.Show(str);
            }
            return ps;
        }

        [FuncGroup("Keys")]
        [Description("KeyDown(string key) - нажимает кнопку.")]
        public static object[] KeyDown(params object[] ps)
        {
            try
            {
                if (ps.Length == 0)
                    return ps;
                string key = ps[0].ToString();
                Meths.keybd_event((int)Enum.Parse(typeof(Keys), key), 0, 0, 0);
                return ps;
            }
            catch { MessageBox.Show("В KeyDown первый аргумент типа, несоотвествующего string"); return ps; }
        }

        [FuncGroup("Keys")]
        [Description("KeyUp(string key) - поднимает кнопку.")]
        public static object[] KeyUp(params object[] ps)
        {
            try
            {
                if (ps.Length == 0)
                    return ps;
                string key = ps[0].ToString();
                Meths.keybd_event((int)Enum.Parse(typeof(Keys), key), 0, 0x02, 0);
                return ps;
            }
            catch { MessageBox.Show("В KeyDown первый аргумент типа, несоотвествующего string"); return ps; }
        }

        [FuncGroup("Desktop")]
        [Description("ShowHideDesktopIcons(bool show) - показывает/скрывает значки рабочего стола.")]
        public static object[] ShowHideDesktopIcons(params object[] ps)
        {
            try
            {
                if (ps.Length == 0)
                    return ps;
                Meths.EnumWindows(new Meths.EnumCallback(Meths.EnumWins), (bool)ps[0] ? (IntPtr)5 : IntPtr.Zero);
                return ps;
            }
            catch { MessageBox.Show("В ShowHideDesktopIcons первый аргумент типа, несоотвествующего Boolean"); return ps; }
        }

        [Description("KillProcessesByName(string name1, string name2...) - находит процессы с именами name... и останавливает их.")]
        public static object[] KillProcessesByName(params object[] ps)
        {
            if (ps.Length == 0)
                return ps;
            foreach (var n in ps)
            {
                var r = Process.GetProcessesByName(n.ToString());
                r.AsParallel().ForAll(p => p.Kill());
            }
            return ps;
        }

        [Description("KillProcessesByID(int id1, int id2...) - находит процессы с ID id1... и останавливает их.")]
        public static object[] KillProcessesByID(params object[] ps)
        {
            if (ps.Length == 0)
                return ps;
            foreach (var n in ps)
            {
                var r = Process.GetProcessById((int)n);
                r.Kill();
            }
            return ps;
        }

        [Description("ProcessRun(string path) - запускает процесс по адрессу path(ссылки, приложения, файлы, пути в проводнике).")]
        public static object[] ProcessRun(params object[] ps)
        {
            if (ps.Length == 0)
                return ps;
            try
            {
                Process.Start(ps[0].ToString());
            }
            catch { MessageBox.Show("ProcessRun не удалось запустить процесс " + ps[0].ToString()); }
            return ps;
        }

        [Description("RunCmd(string command) - выполняет указанную команду в консоли cmd.")]
        public static object[] RunCmd(params object[] ps)
        {
            try
            {
                Process.Start("cmd", ps[0].ToString());
            }
            catch { MessageBox.Show("В RunCmd не передана команда"); return ps; }
            return ps;
        }

        [Description("Shutdown() - выключает компьютер.")]
        public static object[] Shutdown(params object[] ps)
        {
            Process.Start("cmd", "/c shutdown -s -t 00");
            return ps;
        }

        [Description("Restart() - перезагрузка компьютера.")]
        public static object[] Restart(params object[] ps)
        {
            Process.Start("cmd", "/c shutdown -r -t 0");
            return ps;
        }

        [FuncGroup("Keys")]
        [Description("SendKeysWait(string Key) - отправляет активному окну кнопку/текст и ждёт окончания обработки.")]
        public static object[] SendKeysWait(params object[] ps)
        {
            try
            {
                SendKeys.SendWait(ps[0].ToString());
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); return ps; }
            return ps;
        }

        [FuncGroup("Cursor")]
        [Description("SetCursorPos(int x, int y) - перемещает курсор на заданные координаты.")]
        public static object[] SetCursorPos(params object[] ps)
        {
            try
            {
                Meths.SetCursorPos((int)ps[0], (int)ps[1]);
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); return ps; }
            return ps;
        }

        [FuncGroup("Cursor")]
        [Description("MoveCursor(int x, int y, int delay) - перемещает курсор на заданные координаты с промежутком между перемещениями delay.")]
        public static object[] MoveCursor(params object[] ps)
        {
            try
            {
                int endX = (int)ps[0];
                int endY = (int)ps[1];
                int delay = (int)ps[2];
                string delayCmd = $"Delay({delay})";
                while (true)
                {
                    var pos = System.Windows.Forms.Cursor.Position;
                    if (pos.X != endX)
                    {
                        pos.X += pos.X < endX ? 1 : -1;
                    }
                    if (pos.Y != endY)
                    {
                        pos.Y += pos.Y < endY ? 1 : -1;
                    }
                    Meths.SetCursorPos(pos.X, pos.Y);
                    if (pos.X == endX && pos.Y == endY)
                        break;
                    Interpreter.ExecuteCommand(delayCmd);
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); return ps; }
            return ps;
        }

        [FuncGroup("Cursor")]
        [Description("MoveCursorBy(int x, int y) - изменяет позицию курсора на x и y.")]
        public static object[] MoveCursorBy(params object[] ps)
        {
            try
            {
                int X = (int)ps[0];
                int Y = (int)ps[1];
                Meths.mouse_event(0x00000001, X, Y, 0, 0);
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); return ps; }
            return ps;
        }

        [FuncGroup("Mouse")]
        [Description("MouseEvent(string event) - имитарует событие мыши. Для получения доступных событий использовать GetMouseEventsHelp().")]
        public static object[] MouseEvent(params object[] ps)
        {
            try
            {
                var Mevent = Enum.Parse(typeof(Meths.MouseEventFlags), ps[0].ToString());
                Meths.mouse_event((uint)(int)Mevent, 0, 0, 0, 0);
            }
            catch (NullReferenceException) { MessageBox.Show("В MouseEvent не передано событие " + ps[0]); return ps; }
            catch { MessageBox.Show("В MouseEvent не найдено событие " + ps[0]); return ps; }
            return ps;
        }

        [FuncGroup("Clipboard")]
        [Description("SetClipboardText(string text) - вставляет строку text в буфер обмена Windows.")]
        public static object[] SetClipboardText(params object[] ps)
        {
            try
            {
                var data = new DataObject();
                Thread thread;
                data.SetData(DataFormats.UnicodeText, true, ps[0]);
                thread = new Thread(() => Clipboard.SetDataObject(data, true));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch (IndexOutOfRangeException) { MessageBox.Show("В SetClipboardText не передан текст."); }
            catch (Exception e) { MessageBox.Show(e.Message); }
            return ps;
        }
    }
}
