﻿using System;
using System.Collections.Generic;
using Trigger.Events;
using Trigger.Types;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BindModel.Events;
using BindModel.Exeptions;
using InterpreterScripts;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.InterpretationScriptData.StandartFunctions;
using System.Reflection;
using System.Windows;
using System.Threading.Tasks;

namespace BindModel
{
    public sealed class Bind : IDisposable
    {
        public event EnableBindChangedEventHandler EnableChanged;
        private bool enable = true;
        private int id = 0;
        private static HashSet<int> allIds = new HashSet<int>();
        public string Name { get; set; }
        public string Script { get; set; }
        public ObservableCollection<BaseTrigger> Triggers { get; private set; }
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
                EnableChanged?.Invoke(this, new EnableBindChangedEventArgs(enable));
            }
        }
        public int Id
        {
            get { return id; }
            set
            {
                if (id == value)
                    return;
                if (allIds.Contains(value))
                    throw new DuplicateBindIDException(value);
                allIds.Remove(id);
                id = value;
                allIds.Add(id);
            }
        }
        

        public Bind()
        {
            Triggers = new ObservableCollection<BaseTrigger>();
            while (allIds.Contains(id))
                id++;
            allIds.Add(id);
            Name = "Без имени " + id;
            Script = "";
            Triggers.CollectionChanged += TriggersChanged;
        }


        public void Dispose()
        {
            foreach (BaseTrigger bt in Triggers)
                bt.Dispose();
            allIds.Remove(Id);
            GC.SuppressFinalize(this);
        }

        private void Invoke(object sender, TriggeredEventArgs e)
        {
            if (!Enable)//если выключено - выходим
                return;

            InterpretationData data = new InterpretationData();
            data.AdditionalFunctions.Add(new Function(new Func<object[], object>(StartBindScript), FuncType.Parameters, "StartBind"));
            Interpreter.ExecuteScript(e.TriggerScript, data);
        }

        private object StartBindScript(params object[] ps)
        {
            Interpreter.ExecuteScript(Script);
            return ps;
        }

        private void TriggersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (BaseTrigger newBt in e.NewItems)
                    newBt.Triggered += Invoke;
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                foreach (BaseTrigger newBt in e.OldItems)
                    newBt.Triggered -= Invoke;
        }

        ~Bind()
        {
            allIds.Remove(id);
        }
    }
}
