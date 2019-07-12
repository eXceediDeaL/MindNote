using System;

namespace MindNote.Data.Mutations
{
    public class Mutation<T>
    {
        public Mutation() { }

        public Mutation(T newValue) : this()
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