using MindNote.Frontend.SDK.API;
using System.Threading.Tasks;
using MindNote.Data.Raws;
using MindNote.Data.Mutations;
using MindNote.Frontend.SDK.API.Models;

namespace MindNote.Frontend.Client.Client.Helpers
{
    public class NotesClient
    {
        static readonly string SCreate = GraphQLStrings.CreateMutation(nameof(SCreate), @"
($data: RawNoteInput) {
    createNote(data: $data)
}");
        static readonly string SUpdate = GraphQLStrings.CreateMutation(nameof(SUpdate), @"
($id: Int!, $mutation: MutationNote) {
    updateNote(id: $id, mutation: $mutation)
}");
        static readonly string SDelete = GraphQLStrings.CreateMutation(nameof(SDelete), @"
($id: Int!) {
    deleteNote(id: $id)
}");
        static readonly string SClear = GraphQLStrings.CreateMutation(nameof(SClear), @"
{
    clearNotes
}");
        static readonly string SGet = GraphQLStrings.CreateQuery(nameof(SGet), @"
($id: Int!) {
    note(id: $id){
        ..." + nameof(GraphQLStrings.NoteListItem) + @"
    }
}") + GraphQLStrings.NoteListItem;
        static readonly string SQuery = GraphQLStrings.CreateQuery(nameof(SQuery), @"
($id: Int = null) {
    notes(id: $id) {
        totalCount
        nodes {
            ..." + nameof(GraphQLStrings.NoteListItem) + @"
        }
    }
}") + GraphQLStrings.NoteListItem;

        private IGraphQLClient innerClient;

        public NotesClient(IGraphQLClient innerClient)
        {
            this.innerClient = innerClient;
        }

        public async Task<bool> Clear()
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SClear,
                OperationName = nameof(SClear),
            })).GetDataFieldAs<bool>("clearNotes");
        }

        public async Task<int?> Create(RawNote data)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SCreate,
                OperationName = nameof(SCreate),
                Variables = new { data = data },
            })).GetDataFieldAs<int?>("createNote");
        }

        public async Task<int?> Delete(int id)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SDelete,
                OperationName = nameof(SDelete),
                Variables = new { id = id },
            })).GetDataFieldAs<int?>("deleteNote");
        }

        public async Task<Note> Get(int id)
        {
            return (await innerClient.Query(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SGet,
                OperationName = nameof(SGet),
                Variables = new { id = id },
            })).GetDataFieldAs<Note>("note");
        }

        public async Task<PagingEnumerable<Note>> Query(int? id = null, string name = null, string content = null, int? categoryId = null, string keyword = null, string userId = null)
        {
            return (await innerClient.Query(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SQuery,
                OperationName = nameof(SQuery),
                Variables = new { id = id },
            })).GetDataFieldAs<PagingEnumerable<Note>>("notes");
        }

        public async Task<int?> Update(int id, MutationNote mutation)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SUpdate,
                OperationName = nameof(SUpdate),
                Variables = new { id = id, mutation = mutation },
            })).GetDataFieldAs<int?>("updateNote");
        }
    }
}
