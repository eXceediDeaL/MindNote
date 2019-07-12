namespace MindNote.Frontend.SDK.API.Models
{
    public class PagingItem<T>
    {
        public T Node { get; set; }

        public string Cursor { get; set; }
    }
}
