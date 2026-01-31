using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.RepositoryInterface
{
    public interface ITaskRepository
    {
        Task<TaskItem> GetByIdAsync(int id, string userId);
        Task<IEnumerable<TaskItem>> GetAllAsync(string userId);
        Task AddAsync(TaskItem task);
       Task Update(TaskItem task);
       Task DeleteAsync(TaskItem task);
    }
}
