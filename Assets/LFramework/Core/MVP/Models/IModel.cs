public interface IModel<TData>
{
    public TData GetData();
    public void SaveData(TData data);
}
