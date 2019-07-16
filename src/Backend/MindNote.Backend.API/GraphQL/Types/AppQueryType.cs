using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class AppQueryType : ObjectType<AppQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<AppQuery> descriptor)
        {
            descriptor.Field(x => x.GetUsers(default, default, default, default, default, default, default, default)).Type<NonNullType<ListType<ListType<UserType>>>>().UsePaging<UserType>();
            descriptor.Field(x => x.GetNotes(default, default, default, default, default, default, default, default)).Type<NonNullType<ListType<ListType<NoteType>>>>().UsePaging<NoteType>();
            descriptor.Field(x => x.GetCategories(default, default, default, default, default, default)).Type<NonNullType<ListType<NonNullType<CategoryType>>>>().UsePaging<CategoryType>();

            descriptor.Field(x => x.GetUser(default, default)).Type<UserType>();
            descriptor.Field(x => x.GetNote(default, default)).Type<NoteType>();
            descriptor.Field(x => x.GetCategory(default, default)).Type<CategoryType>();
        }
    }
}
