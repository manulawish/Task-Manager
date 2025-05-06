using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.DApplicationomain.Interfaces;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.User.Commands
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Domain.Entities.User?>
    {
        private readonly IUserRepository _userRepository;
        public LoginUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Domain.Entities.User?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user is null || user.Password != request.Password) return null;
            return user;
        }
    }
}
