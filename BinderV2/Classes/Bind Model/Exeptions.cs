using System;

namespace BindModel.Exeptions
{
    [Serializable]
    public sealed class DuplicateBindIDException : Exception
    {
        public int ID { get; private set; }

        public DuplicateBindIDException(int id) : base()
        {
            ID = id;
        }

        public DuplicateBindIDException(string message, int id) : base(message)
        {
            ID = id;
        }
    }
}
