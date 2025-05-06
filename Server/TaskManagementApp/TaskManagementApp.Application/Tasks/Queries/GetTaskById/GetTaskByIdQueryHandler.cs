using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.Dto;
using TaskManagementApp.Application.Interfaces;

namespace TaskManagementApp.Application.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTaskByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        { 
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(request.Id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
            }
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                IsCompleted = task.IsCompleted
            };  
        }
    }
}
