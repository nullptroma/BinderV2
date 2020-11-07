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
                if (!enable)
                    StopInterpretation?.Invoke();
            }
        }
        public ObservableCollection<BaseTrigger> Triggers { get; private set; }
        private Action StopInterpretation;
        

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
                bt.RemoveCallback(Invoke);
                bt.Dispose();
            }
            Triggers.Clear();
            GC.SuppressFinalize(this);
        }

        public void Invoke(object sender, TriggeredEventArgs e)
        {
            if (!Enable)//если выключено - выходим
                return;
            StopInterpretation += e.triggerData.Stop;
            
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps)=> {
                Interpreter.ExecuteScript(Script, e.triggerData);
                return ps;
            }), FuncType.Parameters, "StartBind"));
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps) => {
                e.triggerData.Stop();
                return ps;
            }), FuncType.Parameters, "Stop"));
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps) => {
                StopInterpretation();
                return ps;
            }), FuncType.Parameters, "StopThisBind"));
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps) => {
                StopInterpretation -= e.triggerData.Stop;
                StopInterpretation?.Invoke();
                StopInterpretation += e.triggerData.Stop;
                return ps;
            }), FuncType.Parameters, "StopAnotherRunsOfThisBind"));

            Interpreter.ExecuteScript(e.TriggerScript, e.triggerData);

            StopInterpretation -= e.triggerData.Stop;
        }
        

        private void TriggersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (BaseTrigger newBt in e.NewItems)
                    newBt.AddCallback(Invoke);
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                foreach (BaseTrigger newBt in e.OldItems)
                    newBt.RemoveCallback(Invoke);
        }

        ~Bind()
        {
            foreach (BaseTrigger bt in Triggers)
                bt.Dispose();
        }
    }
}
