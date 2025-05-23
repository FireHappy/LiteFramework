
using LFramework.Core.Utility;
using UniRx;

[AutoRegister(VContainer.Lifetime.Singleton)]
public class UserModel : IModel
{
    public ReactiveProperty<string> userName = new ReactiveProperty<string>("Guest");
    public ReactiveCommand OnExit = new ReactiveCommand();
    public string password;
}
