public interface IModel<T>
{
    public T GetData();
    public void SaveData(T data);
}
