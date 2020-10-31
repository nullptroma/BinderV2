using InterpreterScripts;
using System;
using System.Threading.Tasks;
using System.Windows;
using Trigger.Events;

namespace Trigger.Types
{
    public abstract class BaseTrigger : IDisposable
    {
        public static bool EnableAllTriggers { get; set; }
        public string Name { get; set; }
        public abstract string TypeDescription { get; }
        public string Script { get; set; }//каждый триггер может выполнять свои действия, перед действиями бинда
        public event EnableTriggerChangedEventHandler EnableChanged;
        private event TriggeredEventHandler Triggered;
        protected event CallbackEditEventHandler CallbackAdded;
        private bool enableTrigger = true;

        public bool EnableTrigger
        {
            get { return enableTrigger; }
            set
            {
                enableTrigger = value;
                EnableChanged?.Invoke(this, new EnableTriggerChangedEventArgs(enableTrigger));
            }
        }

        public BaseTrigger(string name = "Без имени")
        {
            this.Name = name;
            EnableAllTriggers = true;//при добавлении любого триггера включаем всё
            Script = "StartBind();";
        }

        protected Task Invoke(TriggeredEventArgs e)
        {
            return Task.Run(() =>
            {
                if (EnableTrigger && EnableAllTriggers)
                    Triggered?.Invoke(this, e);
            });
        }

        public void AddCallback(TriggeredEventHandler meth)
        {
            Triggered += meth;
            CallbackAdded?.Invoke(this, new CallbackEditEventArgs(meth));
        }
        public void RemoveCallback(TriggeredEventHandler meth)
        {
            Triggered -= meth;
        }


        public override string ToString()
        {
            return Name;
        }

        public abstract void Dispose();
    }
}
