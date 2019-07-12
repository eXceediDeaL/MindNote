using HotChocolate.Types;
using MindNote.Data;

namespace MindNote.Backend.API.GraphQL.Types
{
    public class AppMutationType : ObjectType<AppMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<AppMutation> descriptor)
        {
            ((IObjectTypeDescriptor)descriptor).Authorize();
        }
    }
}
