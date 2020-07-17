using System;
using System.Collections.Generic;
using BinderV2.BindModel.Events;
using BinderV2.BindModel.Exeptions;
using BinderV2.Trigger.Events;
using BinderV2.Trigger.Types;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BinderV2.BindModel
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
            
            //MessageBox.Show("Бинд сработал");
            //MessageBox.Show("Скрипт бинда: " + Script + "\nСкрипт триггера: " + e.TriggerScript);
            
            //TODO
            //Сделать выпонения скрипта триггера, а если встетится команда запуска скрипта бинда, запустить его.
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
