using System;

namespace MindNote.Data
{
    public class Mutation<T>
    {
        public Mutation() { }

        public Mutation(T newValue)
        {
            NewValue = newValue;
        }

        public bool? Enable { get; set; }

        public T NewValue { get; set; }

        public void Apply(Action<T> action)
        {
            if (Enable != false)
            {
                action(NewValue);
            }
        }
    }
}
