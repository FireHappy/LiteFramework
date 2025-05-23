
using LFramework.Core.Utility;

[AutoRegister(VContainer.Lifetime.Singleton)]
public class UserModel : IModel
{
    public string userName;
    public string password;
}
