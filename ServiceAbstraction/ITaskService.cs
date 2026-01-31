using Shared.DTO.TaskDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAllAsync(string userId);
        Task<TaskDTO> GetByIdAsync(int taskId, string userId);
        Task<TaskDTO> CreateAsync(CreateTaskDto dto, string userId);
        Task<bool> UpdateAsync(int taskId, UpdateTaskDto dto, string userId);
        Task<bool> DeleteAsync(int taskId, string userId);
    }
}
