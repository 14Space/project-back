using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Core
{
    public class UserApi
    {
        public UserMinimal UserLogin(ULoginData data)
        {
            // Пока заглушка: пускаем всех, у кого пароль "123456"
            if (data.Password == "123456")
            {
                return new UserMinimal { Status = true };
            }
            
            return new UserMinimal { Status = false, StatusMsg = "Неверный логин или пароль" };
        }
    }
}
