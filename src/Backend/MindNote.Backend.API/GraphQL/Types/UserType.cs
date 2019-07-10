using HotChocolate.Types;
using MindNote.Data;
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
        }
    }
}
