public class LoginModel : IModel<UserData>
{
    UserData userData;
    public UserData GetData()
    {
        return userData;
    }
    public void SaveData(UserData data)
    {
        this.userData = data;
    }
}
