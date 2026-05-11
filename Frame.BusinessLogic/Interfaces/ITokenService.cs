using Frame.Domain.Entities;

namespace Frame.BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
