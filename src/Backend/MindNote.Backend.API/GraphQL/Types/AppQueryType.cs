using HotChocolate.Types;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class AppQueryType : ObjectType<AppQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<AppQuery> descriptor)
        {
            descriptor.Field(x => x.Users(default)).Type<ListType<UserType>>();
            descriptor.Field(x => x.Notes(default)).Type<ListType<NoteType>>();
            descriptor.Field(x => x.Categories(default)).Type<ListType<CategoryType>>();

            descriptor.Field(x => x.User(default, default)).Type<UserType>();
            descriptor.Field(x => x.Note(default, default)).Type<NoteType>();
            descriptor.Field(x => x.Category(default, default)).Type<CategoryType>();
        }
    }
}
