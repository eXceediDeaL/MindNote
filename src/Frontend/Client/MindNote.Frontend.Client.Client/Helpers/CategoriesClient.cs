using MindNote.Frontend.SDK.API;
using System.Threading.Tasks;
using MindNote.Data.Raws;
using MindNote.Data.Mutations;
using MindNote.Frontend.SDK.API.Models;

namespace MindNote.Frontend.Client.Client.Helpers
{
    public class CategoriesClient
    {
        static readonly string SCreate = GraphQLStrings.CreateMutation(nameof(SCreate), @"
($data: RawCategoryInput) {
    createCategory(data: $data)
}");
        static readonly string SUpdate = GraphQLStrings.CreateMutation(nameof(SUpdate), @"
($id: Int!, $mutation: MutationCategoryInput) {
    updateCategory(id: $id, mutation: $mutation)
}");
        static readonly string SDelete = GraphQLStrings.CreateMutation(nameof(SDelete), @"
($id: Int!) {
    deleteCategory(id: $id)
}");
        static readonly string SClear = GraphQLStrings.CreateMutation(nameof(SClear), @"
{
    clearCategories
}");
        static readonly string SGet = GraphQLStrings.CreateQuery(nameof(SGet), @"
($id: Int!) {
    category(id: $id) {
        id, name, color
        user {
            id, name
        }
    }
}");
        static readonly string SQuery = GraphQLStrings.CreateQuery(nameof(SQuery), @"
($id: Int = null, $name: String = null, $color: String = null, $userId: String = null, $first: PaginationAmount = null, $last: PaginationAmount = null, $before: String = null, $after: String = null) {
    categories(id: $id, first: $first, last: $last, before: $before, after: $after, name: $name, color: $color, userId: $userId){
        totalCount
        nodes{
            id, name, color
            user {
                id, name
            }
        }
    }
}");

        private IGraphQLClient innerClient;

        public CategoriesClient(IGraphQLClient innerClient)
        {
            this.innerClient = innerClient;
        }

        public async Task<bool> Clear()
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SClear,
                OperationName = nameof(SClear),
            })).GetDataFieldAs<bool>("clearCategories");
        }

        public async Task<int?> Create(RawCategory data)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SCreate,
                OperationName = nameof(SCreate),
                Variables = new { data = data },
            })).GetDataFieldAs<int?>("createCategory");
        }

        public async Task<int?> Delete(int id)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SDelete,
                OperationName = nameof(SDelete),
                Variables = new { id = id },
            })).GetDataFieldAs<int?>("deleteCategory");
        }

        public async Task<Category> Get(int id)
        {
            return (await innerClient.Query(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SGet,
                OperationName = nameof(SGet),
                Variables = new { id = id },
            })).GetDataFieldAs<Category>("category");
        }

        public async Task<PagingEnumerable<Category>> Query(int? id = null, string name = null, string color = null, string userId = null, int? first = null, int? last = null, string before = null, string after = null)
        {
            return (await innerClient.Query(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SQuery,
                OperationName = nameof(SQuery),
                Variables = new { id, first, last, after, before, name, color, userId},
            })).GetDataFieldAs<PagingEnumerable<Category>>("categories");
        }

        public async Task<int?> Update(int id, MutationCategory mutation)
        {
            return (await innerClient.Mutation(new GraphQL.Common.Request.GraphQLRequest
            {
                Query = SUpdate,
                OperationName = nameof(SUpdate),
                Variables = new { id = id, mutation = mutation },
            })).GetDataFieldAs<int?>("updateCategory");
        }
    }
}
