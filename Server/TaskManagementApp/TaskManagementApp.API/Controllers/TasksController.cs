using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Application.Dto;
using TaskManagementApp.Application.Task.Commands.CreateTask;
using TaskManagementApp.Application.Tasks.Commands.DeleteTask;
using TaskManagementApp.Application.Tasks.Commands.UpdateTask;
using TaskManagementApp.Application.Tasks.Queries.GetAllTasks;
using TaskManagementApp.Application.Tasks.Queries.GetTaskById;

namespace TaskManagementApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllTasksQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           var query = new GetTaskByIdQuery() {
               Id = id
           };
            var task = await _mediator.Send(query);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> Update(int id, [FromBody] UpdateTaskCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var updated = await _mediator.Send(command);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteTaskCommand(id));
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
