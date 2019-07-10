using HotChocolate.Types;
using MindNote.Data;
using MindNote.Data.Raws;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class CategoryType : ObjectType<RawCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<RawCategory> descriptor)
        {
            descriptor.Name("Category");
            descriptor.Field(x => x.Clone()).Ignore();
            descriptor.Field(x => x.Status).Type<ItemStatusType>();
            descriptor.Field(x => x.Id).Type<IdType>();
            descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
        }
    }
}
