using MindNote.Data.Raws;

namespace MindNote.Data.Mutations
{
    public class MutationCategory
    {
        public Mutation<string> Name { get; set; }

        public Mutation<string> Color { get; set; }

        public Mutation<ItemClass> Class { get; set; }
    }
}