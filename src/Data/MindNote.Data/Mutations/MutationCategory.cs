namespace MindNote.Data.Mutations
{
    public class MutationCategory
    {
        public Mutation<string> Name { get; set; }

        public Mutation<string> Color { get; set; }

        public Mutation<ItemStatus> Status { get; set; }
    }
}