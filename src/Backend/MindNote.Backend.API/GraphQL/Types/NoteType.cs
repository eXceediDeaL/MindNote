using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using MindNote.Backend.API.GraphQL.Resolvers;
using MindNote.Data;
using MindNote.Data.Providers.SqlServer.Models;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using MindNote.Frontend.SDK.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class NoteType : ObjectType<RawNote>
    {
        protected override void Configure(IObjectTypeDescriptor<RawNote> descriptor)
        {
            descriptor.Name("Note");
            descriptor.Field(x => x.Clone()).Ignore();
            descriptor.Field(x => x.Class).Type<NonNullType<EnumType<ItemClass>>>();
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Title).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CategoryId).Ignore();
            descriptor.Field(x => x.UserId).Ignore();
            ((IObjectTypeDescriptor)descriptor).Field<NoteResolver>(x => x.GetCategory(default, default));
            ((IObjectTypeDescriptor)descriptor).Field<NoteResolver>(x => x.GetUser(default, default));
            ((IObjectTypeDescriptor)descriptor).Field<NoteResolver>(x => x.GetKeywords(default)).Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }
}
