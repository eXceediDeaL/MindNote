using Microsoft.AspNetCore.Identity;
using MindNote.Data.Identity;
using MindNote.Data.Providers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MindNote.Server.Identity.Database
{
    public class RoleStore : IRoleStore<Role>
    {
        private readonly IRolesProvider provider;

        public RoleStore(IIdentityProvider identityProvider)
        {
            this.provider = identityProvider.RolesProvider;
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await provider.Create(role);
                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Creating role {role.Name} failed." });
            }
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await provider.Delete(role.Id);
                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Deleting role {role.Name} failed." });
            }
        }

        public void Dispose()
        {

        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (int.TryParse(roleId, out int id))
            {
                return await provider.Get(id);
            }
            else
            {
                return null;
            }
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await provider.GetByName(normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await provider.Update(role.Id, role);
                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Updating role {role.Name} failed." });
            }
        }
    }
}
