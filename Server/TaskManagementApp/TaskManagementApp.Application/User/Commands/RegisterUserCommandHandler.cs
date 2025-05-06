using MediatR;
using TaskManagementApp.DApplicationomain.Interfaces;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.User.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByUsernameAsync(request.Username);
            if (existing is not null) return false;

            var user = new Domain.Entities.User { Username = request.Username, Password = request.Password };
            await _userRepository.AddAsync(user);
            return true;
        }
    }
}
