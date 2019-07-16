using HotChocolate.Types;
using HotChocolate.Types.Relay;
using MindNote.Backend.API.GraphQL.Resolvers;
using MindNote.Data;
using MindNote.Data.Raws;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class CategoryType : ObjectType<RawCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<RawCategory> descriptor)
        {
            descriptor.Name("Category");
            descriptor.Field(x => x.Class).Type<NonNullType<EnumType<ItemClass>>>();
            descriptor.Field(x => x.Clone()).Ignore();
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.UserId).Ignore();
            ((IObjectTypeDescriptor)descriptor).Field<CategoryResolver>(x => x.GetUser(default, default));
            ((IObjectTypeDescriptor)descriptor).Field<CategoryResolver>(x => x.GetNotes(default, default))
                .Type<NonNullType<ListType<NonNullType<NoteType>>>>().UsePaging<NoteType>();
        }
    }
}
