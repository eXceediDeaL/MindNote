using MindNote.Data.Raws;

namespace MindNote.Data.Mutations
{
    public class MutationNote
    {
        public Mutation<string> Title { get; set; }

        public Mutation<string> Content { get; set; }

        public Mutation<int?> CategoryId { get; set; }

        public Mutation<string[]> Keywords { get; set; }

        public Mutation<ItemClass> Class { get; set; }
    }
}