using InterpreterScripts;
using System;
using System.Threading.Tasks;
using Trigger.Events;

namespace Trigger.Types
{
    public abstract class BaseTrigger : IDisposable
    {
        public static bool EnableAllTriggers { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }//каждый триггер может выполнять свои действия, перед действиями бинда
        public event TriggeredEventHandler Triggered;//при событии, мы передаём имя и скрипт триггера
        public event EnableTriggerChangedEventHandler EnableChanged;
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
            EnableAllTriggers = true;//при добавлелии любого триггера включаем всё
            Script = "StartBind();";
        }

        public Task Invoke()
        {
            return Task.Run(()=> 
            {
                if (EnableTrigger && EnableAllTriggers)
                    Triggered?.Invoke(this, new TriggeredEventArgs(Name, Script));
            });
        }

        public override string ToString()
        {
            return Name;
        }

        public abstract void Dispose();
    }
}
