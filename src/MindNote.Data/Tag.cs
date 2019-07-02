using System;
using System.Collections.Generic;

namespace MindNote.Data
{
    public class Tag : IEquatable<Tag>, ICloneable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public object Clone() => MemberwiseClone();

        public override bool Equals(object obj)
        {
            return Equals(obj as Tag);
        }

        public bool Equals(Tag other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Color == other.Color;
        }

        public override int GetHashCode()
        {
            int hashCode = -337219602;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Color);
            return hashCode;
        }

        public static bool operator ==(Tag left, Tag right)
        {
            return EqualityComparer<Tag>.Default.Equals(left, right);
        }

        public static bool operator !=(Tag left, Tag right)
        {
            return !(left == right);
        }
    }
}
