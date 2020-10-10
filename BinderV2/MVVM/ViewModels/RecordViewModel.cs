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
using BinderV2.MVVM.Models;

namespace BinderV2.MVVM.ViewModels
{
    class RecordViewModel : BaseViewModel
    {
        #region RecordModel
        private RecordModel model = new RecordModel();

        public string RecordHotkeyString
        {
            get { return model.RecordHotkeyString; }
        }
        public string RecordedScript
        {
            get { return model.RecordedScript; }
            set { model.RecordedScript = value; }
        }

        public int SelectedKeyboardImitationIndex
        {
            get { return model.SelectedKeyboardImitationIndex; }
            set { model.SelectedKeyboardImitationIndex = value; }
        }
        public int SelectedMouseMoveImitationIndex
        {
            get { return model.SelectedMouseMoveImitationIndex; }
            set { model.SelectedMouseMoveImitationIndex = value; }
        }
        public string PercentMouseMoveToRecord_string
        {
            get { return model.PercentMouseMoveToRecord_string; }
            set { model.PercentMouseMoveToRecord_string = value; }
        }

        public bool IsRecording
        {
            get { return model.IsRecording; }
        }

        private void RecordModelPropertyChanded(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        #endregion

        public RecordViewModel()
        {
            model.PropertyChanged += RecordModelPropertyChanded;
        }

        private RelayCommand clearScriptCommand;
        public RelayCommand ClearScriptCommand
        {
            get
            {
                return clearScriptCommand ??
                  (clearScriptCommand = new RelayCommand(obj =>
                  {
                      model.ClearScript();
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
                      model.CopyScriptToClipboard();
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
                      model.SetNewHotkeys();
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
