
using LiteFramework.Core.MVP;
using LiteFramework.Core.Utility;
using UniRx;

[AutoRegister(VContainer.Lifetime.Singleton)]
public class UserModel : IModel
{
    //使用UniRx的数据绑定框架
    public ReactiveProperty<string> userName = new ReactiveProperty<string>("Guest");
    public ReactiveCommand OnExit = new ReactiveCommand();
    public string password;
}
