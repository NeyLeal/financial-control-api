using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.UseCases
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public CreateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserResponseDto> Execute(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if(existingUser != null) 
                throw new Exception("User with this email already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User(
                dto.Name,
                dto.Email,
                passwordHash
            );

            await _userRepository.AddAsync(user); 
            
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
