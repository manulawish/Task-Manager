using MediatR;

namespace TaskManagementApp.Application.Task.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
