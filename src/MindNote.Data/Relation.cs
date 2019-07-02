using System;
using System.Collections.Generic;

namespace MindNote.Data
{
    public class Relation : IEquatable<Relation>, ICloneable
    {
        public int Id { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public object Clone() => MemberwiseClone();

        public override bool Equals(object obj)
        {
            return Equals(obj as Relation);
        }

        public bool Equals(Relation other)
        {
            return other != null &&
                   Id == other.Id &&
                   From == other.From &&
                   To == other.To;
        }

        public override int GetHashCode()
        {
            int hashCode = 2078698785;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + From.GetHashCode();
            hashCode = hashCode * -1521134295 + To.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Relation left, Relation right)
        {
            return EqualityComparer<Relation>.Default.Equals(left, right);
        }

        public static bool operator !=(Relation left, Relation right)
        {
            return !(left == right);
        }
    }
}
