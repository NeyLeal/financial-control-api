using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.Exceptions;

namespace FinancialControl.Application.UseCases
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUseCase(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Execute(LoginDto dto)
        {
            var user  = await _userRepository.GetByEmailAsync(dto.Email);
            if(user == null)
            {
                throw new BusinessException("Invalid cendentials");
            }
            var passwordlValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordlValid)
            {
                throw new BusinessException("Invalid Credentials");
            }

            return _jwtTokenGenerator.Generate(user);
        }
    }
}
