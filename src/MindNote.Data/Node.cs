using System;
using System.Collections.Generic;

namespace MindNote.Data
{
    public class Node : IEquatable<Node>,ICloneable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public int? TagId { get; set; }

        public object Clone() => MemberwiseClone();

        public override bool Equals(object obj)
        {
            return Equals(obj as Node);
        }

        public bool Equals(Node other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Content == other.Content &&
                   EqualityComparer<int?>.Default.Equals(TagId, other.TagId);
        }

        public override int GetHashCode()
        {
            var hashCode = -330879835;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Content);
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(TagId);
            return hashCode;
        }

        public static bool operator ==(Node left, Node right)
        {
            return EqualityComparer<Node>.Default.Equals(left, right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !(left == right);
        }
    }
}
