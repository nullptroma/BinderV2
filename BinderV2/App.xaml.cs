using BinderV2.MVVM.Views;
using InterpreterScripts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BinderV2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DependencyResolver.Resolver.RegisterDependencyResolver();//прежде всего подключаем все зависимости
            Interpreter.ExecuteCommand("Delay(1)");//Прогоняем команду в интерпретаторе
            Settings.ProgramSettings.RuntimeSettings.ToString();//обращаемся к настройкам, чтобы они подгрузились

            new MainWindow().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Hooks.Mouse.MouseHook.UnInstallHook();
        }
    }
}
