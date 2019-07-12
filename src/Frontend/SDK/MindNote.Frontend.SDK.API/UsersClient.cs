using MindNote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MindNote.Data.Raws;
using MindNote.Data.Mutations;
using MindNote.Frontend.SDK.API.Models;

namespace MindNote.Frontend.SDK.API
{
    public class UsersClient : IUsersClient
    {
        static readonly string SCreate = GraphQLStrings.CreateMutation(nameof(SCreate), @"
($data: RawUserInput) {
    createUser(data: $data)
}");
        static readonly string SUpdate = GraphQLStrings.CreateMutation(nameof(SUpdate), @"
($id: String, $mutation: MutationUserInput) {
    updateUser(id: $id, mutation: $mutation)
}");
        static readonly string SDelete = GraphQLStrings.CreateMutation(nameof(SDelete), @"
($id: String) {
    deleteUser(id: $id)
}");
        static readonly string SClear = GraphQLStrings.CreateMutation(nameof(SClear), @"
{
    clearUsers
}");
        static readonly string SGet = GraphQLStrings.CreateQuery(nameof(SGet), @"
($id: String) {
    user(id: $id){
        id, name, bio, url, email, company, location
        notes {
            totalCount
            nodes {
                ..." + nameof(GraphQLStrings.NoteListItem) + @"
            }
        }
    }
}") + GraphQLStrings.NoteListItem;

        private IGraphQLClient innerClient;

        public UsersClient(IGraphQLClient innerClient)
        {
            this.innerClient = innerClient;
        }

        public async Task<bool> Clear()
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SClear,
                OperationName = nameof(SClear),
            })).GetDataFieldAs<bool>("clearUsers");
        }

        public async Task<string> Create(RawUser data)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SCreate,
                OperationName = nameof(SCreate),
                Variables = new { data },
            })).GetDataFieldAs<string>("createUser");
        }

        public async Task<string> Delete(string id)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SDelete,
                OperationName = nameof(SDelete),
                Variables = new { id },
            })).GetDataFieldAs<string>("deleteUser");
        }

        public async Task<User> Get(string id)
        {
            return (await innerClient.Query(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SGet,
                OperationName = nameof(SGet),
                Variables = new { id },
            })).GetDataFieldAs<User>("user");
        }

        public async Task<PagingEnumerable<Note>> Query(string id = null)
        {
            return null;
        }

        public async Task<string> Update(string id, MutationUser mutation)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SUpdate,
                OperationName = nameof(SUpdate),
                Variables = new { id = id, mutation = mutation },
            })).GetDataFieldAs<string>("updateUser");
        }
    }
}
