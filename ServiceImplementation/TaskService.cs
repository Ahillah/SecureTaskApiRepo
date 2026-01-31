using DomainLayer;
using DomainLayer.RepositoryInterface;
using ServiceAbstraction;
using Shared.DTO.TaskDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation
{
    public class TaskService:ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<IEnumerable<TaskDTO>> GetAllAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Invalid user");

            var tasks = await _taskRepository.GetAllAsync(userId);
            return tasks.Select(t => new TaskDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt
            });
        }
        public async Task<TaskDTO?> GetByIdAsync(int taskId, string userId)
        {
            if (taskId <= 0)
                throw new ArgumentException("Invalid Task Id");

            var task = await _taskRepository.GetByIdAsync(taskId, userId);

            if (task == null)
                return null;
            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }
        public async Task<TaskDTO> CreateAsync(CreateTaskDto dto, string userId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Invalid user");

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = userId,
                IsCompleted = false
            };

            await _taskRepository.AddAsync(task);

            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }
        public async Task<bool> UpdateAsync(int taskId, UpdateTaskDto dto, string userId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var task = await _taskRepository.GetByIdAsync(taskId, userId);

            if (task == null)
                return false;
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            await _taskRepository.Update(task);
            return true;
        }
        public async Task<bool> DeleteAsync(int taskId, string userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, userId);

            if (task == null)
                return false;

            await _taskRepository.DeleteAsync(task);
            return true;
        }
    }
}
