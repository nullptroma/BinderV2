using BinderV2.Commands;
using Hooks.Keyboard;
using Hooks.Mouse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using Trigger.Types;

namespace BinderV2.MVVM.ViewModels
{
    class RecordViewModel : BaseViewModel
    {
        private HashSet<Key> keys = new HashSet<Key>();

        private string keysForRecordString = "<клавиши>";
        public string KeysForRecordString
        {
            get { return keysForRecordString; }
            set 
            {
                keysForRecordString = value;
                OnPropertyChanged("KeysForRecordString");
            }
        }

        private bool recording = false;
        public bool Recording
        {
            get { return recording; }
            set
            {
                recording = value;
                OnPropertyChanged("Recording");
            }
        }

        public string recordedScript = "";
        public string RecordedScript { get { return recordedScript; } set { recordedScript = value; OnPropertyChanged("RecordedScript"); } }
        private Stopwatch timer = new Stopwatch();
        private Queue<string> currentCommands = new Queue<string>();

        public int SelectedKeyboardImitationIndex { get; set; }
        public int SelectedMouseMoveImitationIndex { get; set; }
        public string percentMouseMoveToRecord_string = "100";
        public string PercentMouseMoveToRecord_string { get { return percentMouseMoveToRecord_string; } 
            set 
            { 
                percentMouseMoveToRecord_string = value;

                if (int.TryParse(percentMouseMoveToRecord_string, out int res))
                {
                    if (res < 0)
                        percentMouseMoveToRecord_string = "0";
                    if (res > 100)
                        percentMouseMoveToRecord_string = "100";
                    percentMouseMoveToRecord = int.Parse(percentMouseMoveToRecord_string);
                }
                else
                {
                    MessageBox.Show("Введите проценты верно.", "Ошибка");
                    PercentMouseMoveToRecord_string = "100";
                    percentMouseMoveToRecord = 100;
                }
                OnPropertyChanged("PercentMouseMoveToRecord_string");
            } 
        }

        int percentMouseMoveToRecord = 100;
        int mouseMoveCount = 0;
        string mouseMoveCmd = "";
        string keyDownCmd = "";
        string keyUpCmd = "";

        private void StartRecord()
        {
            currentCommands.Clear();
            SetKeysImitationCommands();
            SetMouseImitationCommands();
            SetHooks();
            mouseMoveCount = 0;
            timer.Start();
        }

        private void SetKeysImitationCommands()
        {
            switch (SelectedKeyboardImitationIndex)
            {
                case 0://AHK
                    {
                        keyDownCmd = @"AHKExecRaw(""Send {{{0} down}}"")";
                        keyUpCmd = @"AHKExecRaw(""Send {{{0} up}}"")";
                        break;
                    }
                case 1://keybd_event
                    {
                        keyDownCmd = @"KeyDown(""{0}"")";
                        keyUpCmd = @"KeyUp(""{0}"")";
                        break;
                    }
            }
        }

        private void SetMouseImitationCommands()
        {
            switch (SelectedMouseMoveImitationIndex)
            {
                case 0://SetCursorPos
                    {
                        mouseMoveCmd = @"SetCursorPos({0}, {1})";
                        break;
                    }
                case 1://MouseMove
                    {
                        mouseMoveCmd = @"MouseMove({0}, {1}, 0)";
                        break;
                    }
            }
        }

        private void StopRecord()
        {
            UnsetHooks();
            timer.Stop();
            while (currentCommands.Count > 0)
                RecordedScript += currentCommands.Dequeue();
            pressedKeys.Clear();
        }

        private Task AddCommand(string cmd)
        {
            return Task.Run(()=> 
            {
                long mcs = timer.ElapsedMilliseconds;
                if (mcs >= 1)
                    currentCommands.Enqueue($"Delay({mcs});" + Environment.NewLine);
                currentCommands.Enqueue(cmd + ";" + Environment.NewLine );

                timer.Reset();
                timer.Start();
            });
        }

        private void SetHooks()
        {
            KeyboardHook.KeyDown += KeyDown;
            KeyboardHook.KeyUp += KeyUp;
            MouseHook.MouseDown += MouseDown;
            MouseHook.MouseUp += MouseUp;
            if(percentMouseMoveToRecord!=0)
                MouseHook.MouseMove += MouseMove;
        }

        private void UnsetHooks()
        {
            KeyboardHook.KeyDown -= KeyDown;
            KeyboardHook.KeyUp -= KeyUp;
            MouseHook.MouseDown -= MouseDown;
            MouseHook.MouseUp -= MouseUp;
            MouseHook.MouseMove -= MouseMove;
        }

        private void KeyDown(object sender, KeyEventArgsCustom e)
        {
            AddCommand(string.Format(keyDownCmd, e.Key));
        }

        private void KeyUp(object sender, KeyEventArgsCustom e)
        {
            AddCommand(string.Format(keyUpCmd, e.Key));
        }

        private void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            AddCommand(string.Format(mouseMoveCmd, e.Location.X, e.Location.Y));
            AddCommand($"MouseEvent(\"{e.Button.ToString().ToUpper() + "DOWN"}\")");
        }

        private void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            AddCommand(string.Format(mouseMoveCmd, e.Location.X, e.Location.Y));
            AddCommand($"MouseEvent(\"{e.Button.ToString().ToUpper() + "UP"}\")");
        }

        private void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (timer.ElapsedMilliseconds < 1)
                return;
            if(mouseMoveCount < percentMouseMoveToRecord)
                AddCommand(string.Format(mouseMoveCmd, e.Location.X, e.Location.Y));
            mouseMoveCount++;
            if (mouseMoveCount >= 100)
                mouseMoveCount = 0;
        }

        private HashSet<Key> pressedKeys = new HashSet<Key>();
        public RecordViewModel()
        {
            SetBindForRecord();
        }

        void SetBindForRecord()
        {
            KeyboardHook.KeyDown += (s, keyArgs) =>
            {
                pressedKeys.Add(keyArgs.Key);
                if (HasNeedKeys() && BaseTrigger.EnableAllTriggers)
                {
                    Recording = !Recording;
                    if (Recording)//если начали запись
                        StartRecord();
                    else//если закончили запись
                        StopRecord();
                }
            };
            KeyboardHook.KeyUp += (s, keyArgs) =>
            {
                pressedKeys.Remove(keyArgs.Key);
            };
        }

        private bool HasNeedKeys()
        {
            if (keys.Count == 0)
                return false;
            foreach (Key k in keys)
                if (!pressedKeys.Contains(k))
                    return false;
            return true;
        }
        

        private RelayCommand clearScriptCommand;
        public RelayCommand ClearScriptCommand
        {
            get
            {
                return clearScriptCommand ??
                  (clearScriptCommand = new RelayCommand(obj =>
                  {
                      StopRecord();
                      RecordedScript = "";
                  }));
            }
        }

        private RelayCommand copyScriptToClipboardCommand;
        public RelayCommand CopyScriptToClipboardCommand
        {
            get
            {
                return copyScriptToClipboardCommand ??
                  (copyScriptToClipboardCommand = new RelayCommand(obj =>
                  {
                      var data = new System.Windows.Forms.DataObject();
                      Thread thread;
                      data.SetData(System.Windows.Forms.DataFormats.UnicodeText, true, RecordedScript);
                      thread = new Thread(() => System.Windows.Forms.Clipboard.SetDataObject(data, true));
                      thread.SetApartmentState(ApartmentState.STA);
                      thread.Start();
                      thread.Join();
                      System.Windows.Forms.MessageBox.Show("Скопировано");
                  }));
            }
        }

        private RelayCommand changeKeysCommand;
        public RelayCommand ChangeKeysCommand
        {
            get
            {
                return changeKeysCommand ??
                  (changeKeysCommand = new RelayCommand(obj =>
                  {
                      BaseTrigger.EnableAllTriggers = false;//отключаем все триггеры, а соотвественно бинды, чтобы записать новые кнопки.
                      keys.Clear();
                      HashSet<Key> buffer = new HashSet<Key>();
                      KeyEventHandlerCustom keyDown = null;
                      keyDown = (s, keyArgs) =>
                      {
                          buffer.Add(keyArgs.Key);
                          KeysForRecordString = string.Join(" + ", buffer);
                      };

                      KeyEventHandlerCustom keyUp = null;
                      keyUp = (s, keyArgs) =>
                      {
                          KeyboardHook.KeyDown -= keyDown;
                          KeyboardHook.KeyUp -= keyUp;
                          BaseTrigger.EnableAllTriggers = true;//когда записали, включаем обратно
                          keys = buffer;
                      };

                      KeyboardHook.KeyDown += keyDown;
                      KeyboardHook.KeyUp += keyUp;

                      KeysForRecordString = "<нажмите сочетание>";
                  }));
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
