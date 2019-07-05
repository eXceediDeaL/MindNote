using System;
using System.Collections.Generic;

namespace MindNote.Data
{
    public class Note : ICloneable, IEquatable<Note>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int? CategoryId { get; set; }

        public string[] Keywords { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string UserId { get; set; }

        public object Clone() => MemberwiseClone();

        public override bool Equals(object obj)
        {
            return Equals(obj as Note);
        }

        public bool Equals(Note other)
        {
            return other != null &&
                   Id == other.Id &&
                   Title == other.Title &&
                   Content == other.Content &&
                   EqualityComparer<int?>.Default.Equals(CategoryId, other.CategoryId) &&
                   EqualityComparer<string[]>.Default.Equals(Keywords, other.Keywords) &&
                   CreationTime.Equals(other.CreationTime) &&
                   ModificationTime.Equals(other.ModificationTime) &&
                   UserId == other.UserId;
        }

        public override int GetHashCode()
        {
            var hashCode = -1604058068;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Content);
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(CategoryId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(Keywords);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTimeOffset>.Default.GetHashCode(CreationTime);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTimeOffset>.Default.GetHashCode(ModificationTime);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserId);
            return hashCode;
        }

        public static bool operator ==(Note left, Note right)
        {
            return EqualityComparer<Note>.Default.Equals(left, right);
        }

        public static bool operator !=(Note left, Note right)
        {
            return !(left == right);
        }
    }
}
