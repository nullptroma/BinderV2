using InterpreterScripts;
using InterpreterScripts.InterpretationFunctions;
using InterpreterScripts.SyntacticConstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BinderV2.MVVM.Models
{
    class HelpModel//представляет текст помощи для 8 типов
    {
        private Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();

        public HelpProperty ParametersFuncsHelp { get; private set; }
        public HelpProperty BoolFuncsHelp { get; private set; }
        public HelpProperty DoubleFuncsHelp { get; private set; }
        public HelpProperty IntFuncsHelp { get; private set; }
        public HelpProperty StringFuncsHelp { get; private set; }
        public HelpProperty OtherFuncsHelp { get; private set; }
        public HelpProperty DynamicFuncsHelp { get; private set; }
        public HelpProperty FuncsHelpByGroups 
        {
            get
            {
                HelpProperty answer = new HelpProperty(_prefix:"{count}"); ;
                foreach (string key in groups.Keys)
                {
                    HelpProperty descs = new HelpProperty(_prefix: "    {count}");
                    foreach (string desc in groups[key])
                        descs.Add( desc);

                    answer.Add("Группа " + key + ":" + Environment.NewLine + descs.AllText);
                }
                return answer;
            }
        }
        public Dictionary<string, string> ConstructionsHelp { get; private set; }
        public HelpProperty TriggersHelp { get; private set; }
        

        public HelpModel()
        {
            SetHelpTexts();
        }

        private void SetHelpTexts()
        {
            DynamicFuncsHelp = GetFuncTypeHelp(FuncType.Dynamic);
            ParametersFuncsHelp = GetFuncTypeHelp(FuncType.Parameters);
            BoolFuncsHelp = GetFuncTypeHelp(FuncType.Boolean);
            DoubleFuncsHelp = GetFuncTypeHelp(FuncType.Double);
            IntFuncsHelp = GetFuncTypeHelp(FuncType.Int);
            StringFuncsHelp = GetFuncTypeHelp(FuncType.String);
            foreach (IInterpreterFunction f in Interpreter.GetFullLibrary())
                AddToGroups(f);
            OtherFuncsHelp = GetOtherFuncsHelp();
            ConstructionsHelp = GetConstructionsHelp();
            TriggersHelp = GetTriggersHelp();
        }

        private HelpProperty GetFuncTypeHelp(FuncType type)
        {
            HelpProperty answer = new HelpProperty(_prefix:"{count}");
            foreach (IInterpreterFunction f in Interpreter.GetFullLibrary().Where(func => func.ReturnType == type))
                answer.Add(f.Description);

            return answer;
        }

        private HelpProperty GetOtherFuncsHelp()
        {
            HelpProperty answer = GetFuncTypeHelp(FuncType.Other);

            string StartBind = "StartBind() - запускает выполнение скрипта бинда.";
            answer.Add(StartBind); 
            AddToGroups("ScriptRuntimeControl", StartBind);

            string Stop = "Stop() - останавливает текущую интерпретацию.";
            answer.Add(Stop);
            AddToGroups("ScriptRuntimeControl", Stop);

            string StopThisBind = "StopThisBind() - останавливает все интерпретации на этом бинде.";
            answer.Add(StopThisBind);
            AddToGroups("ScriptRuntimeControl", StopThisBind);

            string StopAnotherRunsOfThisBind = "StopAnotherRunsOfThisBind() - останавливает все интерпретации на этом бинде, кроме текущей.";
            answer.Add(StopAnotherRunsOfThisBind);
            AddToGroups("ScriptRuntimeControl", StopAnotherRunsOfThisBind);
            return answer;
        }

        private void AddToGroups(IInterpreterFunction f)
        {
            AddToGroups(f.GroupName, f.Description);
        }

        private void AddToGroups(string group, string text)
        {
            if (!groups.ContainsKey(group))
                groups.Add(group, new List<string>());

            groups[group].Add(text);
        }

        private Dictionary<string, string> GetConstructionsHelp()
        {
            var answer = new Dictionary<string, string>();
            string main = "После каждой команды нужно писать ;.\nЧтобы сделать команду асинхронной нужно добавить async перед ней. Конструкции так же считаются командами. Нераспознанные команды превращаются в строку.";
            answer.Add("Основное", main);
            foreach (ISyntacticConstruction sc in SyntacticConstructionsManager.GetSyntacticConstructs())
                answer.Add(sc.Name, sc.Description);
            return answer;
        }

        private HelpProperty GetTriggersHelp()
        {
            HelpProperty answer = new HelpProperty("Триггеры при выполнении сохраняют в своих данных некоторые переменные.\nВсе триггеры передают свои данные (переменные и функции) биндам.\nУ каждого триггера есть следующие переменные по умолчанию:\n", "{count}");
            answer.Add("TriggerName - имя триггера, начавшего выполнение.");
            answer.Add("TriggerScript - скрипт триггера, начавшего выполнение.");
            answer.AllText += "Некоторые типы триггеров имеют дополнительные переменные:\n";
            answer.AllText += Environment.NewLine;

            HelpProperty KeysTriggers = new HelpProperty("Триггеры Keys имеют:\n", "{count}");
            KeysTriggers.Add("Key - название кнопки, начавшей выполнение.");
            answer.AllText += KeysTriggers.AllText;
            answer.AllText += Environment.NewLine;

            HelpProperty MouseTriggers = new HelpProperty("Триггеры Mouse имеют:\n", "{count}");
            MouseTriggers.Add("X, Y - координаты курсора.");
            MouseTriggers.Add("Button - последняя кнопка мыши.");
            answer.AllText += MouseTriggers.AllText;

            return answer;
        }
    }

    struct HelpProperty
    {
        public string AllText;
        public string prefix;
        private int count;

        public HelpProperty(string _text = "", string _prefix = "", int _count = 0)
        {
            AllText = _text;
            prefix = _prefix;
            this.count = _count;
        }

        public int Count()
        {
            return count;
        }

        public void Add(string text)
        {
            AllText += (prefix.Contains("{count}") ? prefix.Replace("{count}", count++.ToString())  + ") " : prefix) + text + Environment.NewLine;
        }
    }
}
