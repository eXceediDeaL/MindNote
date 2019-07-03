using System;
using System.Collections.Generic;

namespace MindNote.Data
{
    public class Category : IEquatable<Category>, ICloneable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public object Clone() => MemberwiseClone();

        public override bool Equals(object obj)
        {
            return Equals(obj as Category);
        }

        public bool Equals(Category other)
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

        public static bool operator ==(Category left, Category right)
        {
            return EqualityComparer<Category>.Default.Equals(left, right);
        }

        public static bool operator !=(Category left, Category right)
        {
            return !(left == right);
        }
    }
}
