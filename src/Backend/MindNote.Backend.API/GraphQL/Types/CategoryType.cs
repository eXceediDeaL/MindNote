using HotChocolate.Types;
using MindNote.Data;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class CategoryType : ObjectType<Category>
    {
        protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
        {
            descriptor.Field(x => x.Clone()).Ignore();
            descriptor.Field(x => x.Equals(default)).Ignore();
            descriptor.Field(x => x.Equals(default(object))).Ignore();
            descriptor.Field(x => x.GetHashCode()).Ignore();
            descriptor.Field(x => x.Status).Type<ItemStatusType>();
        }
    }
}
