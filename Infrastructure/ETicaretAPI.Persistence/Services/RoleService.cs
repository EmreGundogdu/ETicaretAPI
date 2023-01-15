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
            var result = await roleManager.CreateAsync(new() {Id=Guid.NewGuid().ToString(), Name = name });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var result = await roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public (object,int) GetAllRoles(int page,int pageSize)
        {
            var data = roleManager.Roles.Skip(page * pageSize).Take(pageSize).Select(x => new { x.Id, x.Name })
            return (data,roleManager.Roles.Count());
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            string role = await roleManager.GetRoleIdAsync(new() { Id = id });
            return (id, role);
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            var role = await roleManager.FindByIdAsync(id);
            role.Name = name;
            var result = await roleManager.UpdateAsync(role);
            return result.Succeeded;

        }
    }
}
