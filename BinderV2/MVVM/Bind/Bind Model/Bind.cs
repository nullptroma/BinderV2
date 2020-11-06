using System;
using System.Collections.Generic;
using Trigger.Events;
using Trigger.Types;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BinderV2.MVVM.Models.BindModel.Events;
using BinderV2.MVVM.Models.BindModel.Exeptions;
using InterpreterScripts;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.InterpretationScriptData.StandartFunctions;
using System.Reflection;
using System.Windows;
using System.Threading.Tasks;

namespace BinderV2.MVVM.Models
{
    public sealed class Bind : IDisposable
    {
        public event EnableBindChangedEventHandler EnableChanged;
        public string Name { get; set; }
        public string Script { get; set; }
        private bool enable = true;
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
                EnableChanged?.Invoke(this, new EnableBindChangedEventArgs(enable));
            }
        }
        public ObservableCollection<BaseTrigger> Triggers { get; private set; }
        
        

        public Bind()
        {
            Triggers = new ObservableCollection<BaseTrigger>();
            Name = "Без имени";
            Script = "";
            Triggers.CollectionChanged += TriggersChanged;
        }


        public void Dispose()
        {
            Enable = false;
            foreach (BaseTrigger bt in Triggers)
            {
                bt.EnableTrigger = false;
                bt.RemoveCallback(Invoked);
                bt.Dispose();
            }
            Triggers.Clear();
            GC.SuppressFinalize(this);
        }

        private void Invoked(object sender, TriggeredEventArgs e)
        {
            if (!Enable)//если выключено - выходим
                return;

            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps)=> {
                Interpreter.ExecuteScript(Script, e.triggerData);
                return ps;
            }), FuncType.Parameters, "StartBind"));
            Interpreter.ExecuteScript(e.TriggerScript, e.triggerData);
        }

        

        private void TriggersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (BaseTrigger newBt in e.NewItems)
                    newBt.AddCallback(Invoked);
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                foreach (BaseTrigger newBt in e.OldItems)
                    newBt.RemoveCallback(Invoked);
        }

        ~Bind()
        {
            foreach (BaseTrigger bt in Triggers)
                bt.Dispose();
        }
    }
}
