using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.User.Commands
{
    public record RegisterUserCommand(string Username, string Password) : IRequest<bool>;

}
