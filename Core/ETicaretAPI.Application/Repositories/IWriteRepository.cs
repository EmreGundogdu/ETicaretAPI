using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : class
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddAsync(List<T> model);
        Task<bool> RemoveAsync(T model);
        Task<bool> RemoveAsync(string id);
        Task<bool> UpdateAsync(T model);
    }
}
