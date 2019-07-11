using HotChocolate.Types;
using HotChocolate.Types.Relay;
using MindNote.Backend.API.GraphQL.Resolvers;
using MindNote.Data;
using MindNote.Data.Raws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class UserType : ObjectType<RawUser>
    {
        protected override void Configure(IObjectTypeDescriptor<RawUser> descriptor)
        {
            descriptor.Name("User");
            descriptor.Field(x => x.Clone()).Ignore();
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
            ((IObjectTypeDescriptor)descriptor).Field<UserResolver>(x => x.GetNotes(default, default))
                .Type<NonNullType<ListType<NonNullType<NoteType>>>>().UsePaging<NoteType>();
        }
    }
}
