using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteTaskCommand(int id)
        {
            Id = id;
        }
    }
}
