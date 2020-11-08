using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using InterpreterScripts.InterpretationFunctions.Standart;
using System.Security.Cryptography;
using InterpreterScripts;
using System.Windows.Forms;
using InterpreterScripts.SyntacticConstructions;
using BinderV2.MVVM.Models;

namespace BinderV2.MVVM.ViewModels
{
    class HelpViewModel : BaseViewModel
    {
        HelpModel help = new HelpModel();

        public string ParametersFuncsHelp { get { return help.ParametersFuncsHelp.AllText; } }
        public string BoolFuncsHelp { get { return help.BoolFuncsHelp.AllText; ; } }
        public string DoubleFuncsHelp { get { return help.DoubleFuncsHelp.AllText; } }
        public string IntFuncsHelp { get { return help.IntFuncsHelp.AllText; } }
        public string StringFuncsHelp { get { return help.StringFuncsHelp.AllText; } }
        public string OtherFuncsHelp { get { return help.OtherFuncsHelp.AllText; } }
        public string FuncsHelpByGroups { get { return help.FuncsHelpByGroups.AllText; } }
        public string ConstructionsHelp { get { return help.ConstructionsHelp.AllText; } }
        public string TriggersHelp { get { return help.TriggersHelp.AllText; } }
        public string DynamicFuncsHelp { get { return help.DynamicFuncsHelp.AllText; } }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
