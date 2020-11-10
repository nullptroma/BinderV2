using System;
using System.Collections.Generic;
using Trigger.Events;
using Trigger.Types;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BinderV2.MVVM.Models.BindModel.Events;
using BinderV2.MVVM.Models.BindModel.Exeptions;
using InterpreterScripts;
using InterpreterScripts.InterpretationFunctions;
using InterpreterScripts.InterpretationFunctions.Standart;
using System.Reflection;
using System.Windows;
using System.Threading.Tasks;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.InterpretationScriptData.Stopper;

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
                    stopSender.Stop();
            }
        }
        public ObservableCollection<BaseTrigger> Triggers { get; private set; }
        private StopSender stopSender = new StopSender();
        

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
            e.triggerData.Stopper.RegisterStopper(stopSender);
            
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps)=> {
                Interpreter.ExecuteScript(Script, e.triggerData);
                return ps;
            }), FuncType.Parameters, "StartBind"));
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps) => {
                e.triggerData.Stopper.IsStopped = true;
                return ps;
            }), FuncType.Parameters, "Stop"));
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps) => {
                stopSender.Stop();
                return ps;
            }), FuncType.Parameters, "StopThisBind"));
            e.triggerData.InterpretationFuncs.Add(new Function(new Func<object[], object>((ps) => {
                e.triggerData.Stopper.UnRegisterStopper(stopSender);
                stopSender.Stop();
                e.triggerData.Stopper.RegisterStopper(stopSender);
                return ps;
            }), FuncType.Parameters, "StopAnotherRunsOfThisBind"));

            Interpreter.ExecuteScript(e.TriggerScript, e.triggerData);

            e.triggerData.Stopper.UnRegisterStopper(stopSender);
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
