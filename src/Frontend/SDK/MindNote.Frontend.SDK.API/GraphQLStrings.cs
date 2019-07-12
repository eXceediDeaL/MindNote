namespace MindNote.Frontend.SDK.API
{
    public static class GraphQLStrings
    {
        public static readonly string NoteListItem = GraphQLStrings.CreateFragment(nameof(NoteListItem), "Note", @"
{
    id, title, content, creationTime, modificationTime, status
    category {
        id, name, color
    }
    user {
        id, name
    }
}");

        public static string CreateMutation(string name, string content)
        {
            return $"mutation {name}{content}";
        }

        public static string CreateQuery(string name, string content)
        {
            return $"query {name}{content}";
        }

        public static string CreateFragment(string name, string on, string content)
        {
            return $"fragment {name} on {on} {content}";
        }
    }
}
