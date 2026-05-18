using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string Generate(User user);
    }
}
