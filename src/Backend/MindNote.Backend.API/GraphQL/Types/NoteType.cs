using HotChocolate.Types;
using MindNote.Data;
using MindNote.Data.Raws;
using System.Linq;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class NoteType : ObjectType<RawNote>
    {
        protected override void Configure(IObjectTypeDescriptor<RawNote> descriptor)
        {
            descriptor.Name("Note");
            descriptor.Field(x => x.Clone()).Ignore();
            descriptor.Field(x => x.Status).Type<ItemStatusType>();
            descriptor.Field(x => x.Id).Type<IdType>();
            descriptor.Field(x => x.Title).Type<NonNullType<StringType>>();
        }
    }
}
