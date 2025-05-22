using LFramework.Utility;
using UnityEngine;

public abstract class BaseModel<TData> : IModel<TData>
{
    TData data = default;
    public TData GetData()
    {
        return data;
    }

    public void SaveData(TData data)
    {
        this.data = data;
    }
}