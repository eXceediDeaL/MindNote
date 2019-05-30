namespace MindNote.Data.Providers
{
    public interface IIdentityProvider
    {
        IUsersProvider UsersProvider { get; }

        IRolesProvider RolesProvider { get; }
    }
}
