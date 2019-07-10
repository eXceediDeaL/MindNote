using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class AppQueryType : ObjectType<AppQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<AppQuery> descriptor)
        {
            descriptor.Field(x => x.Users(default, default, default, default, default, default, default, default)).Type<ListType<UserType>>().UsePaging<UserType>();
            descriptor.Field(x => x.Notes(default, default, default, default, default, default, default)).Type<ListType<NoteType>>().UsePaging<NoteType>();
            descriptor.Field(x => x.Categories(default, default, default, default, default)).Type<ListType<CategoryType>>().UsePaging<CategoryType>();

            descriptor.Field(x => x.User(default, default)).Type<UserType>();
            descriptor.Field(x => x.Note(default, default)).Type<NoteType>();
            descriptor.Field(x => x.Category(default, default)).Type<CategoryType>();
        }
    }
}
