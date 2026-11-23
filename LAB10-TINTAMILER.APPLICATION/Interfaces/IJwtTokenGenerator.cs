using LAB10_TINTAMILER.DOMAIN.Entities;

namespace LAB10_TINTAMILER.APPLICATION.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime Expiration) GenerateToken(User user, IEnumerable<string> roles);
}
