using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    public class IdentityProvider : IIdentityProvider
    {
        public IdentityProvider(IdentityDataContext context)
        {
            UsersProvider = new UsersProvider(context, this);
            RolesProvider = new RolesProvider(context, this);
        }

        public IUsersProvider UsersProvider { get; }

        public IRolesProvider RolesProvider { get; }
    }
}
