using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
            var result = await roleManager.CreateAsync(new() { Name = name });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string name)
        {
            var result = await roleManager.DeleteAsync(new() { Name = name });
            return result.Succeeded;
        }

        public IDictionary<string, string> GetAllRoles()
        {
            return roleManager.Roles.ToDictionary(role => role.Id, role => role.Name);
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            string role = await roleManager.GetRoleIdAsync(new() { Id = id });
            return (id, role);
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            var result = await roleManager.UpdateAsync(new() { Id = id, Name = name });
            return result.Succeeded;

        }
    }
}
