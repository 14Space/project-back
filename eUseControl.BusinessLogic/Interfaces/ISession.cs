using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ISession
    {
        UserMinimal UserLogin(ULoginData data);
    }
}
