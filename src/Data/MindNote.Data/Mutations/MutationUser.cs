namespace MindNote.Data.Mutations
{
    public class MutationUser
    {
        public Mutation<string> Name { get; set; }

        public Mutation<string> Bio { get; set; }

        public Mutation<string> Email { get; set; }

        public Mutation<string> Url { get; set; }

        public Mutation<string> Company { get; set; }

        public Mutation<string> Location { get; set; }
    }
}