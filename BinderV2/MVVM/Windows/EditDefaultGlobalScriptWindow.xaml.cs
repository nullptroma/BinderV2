using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BinderV2.Settings;

namespace BinderV2.MVVM.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для GetTextFromUser.xaml
    /// </summary>
    public partial class EditDefaultGlobalScriptWindow : Window
    {
        public EditDefaultGlobalScriptWindow()
        {
            InitializeComponent();
            TextBox.Text = ProgramSettings.RuntimeSettings.InterpreterSettings.DefaultGlobalScript;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox.Text = InterpreterScripts.Script.ScriptTools.FormateScript(TextBox.Text);
            ProgramSettings.RuntimeSettings.InterpreterSettings.DefaultGlobalScript = TextBox.Text;
            InterpreterScripts.Interpreter.UdpateLibraryFromGlobalScript();
            MessageBox.Show("Сохранено","Успех");
        }
    }
}
