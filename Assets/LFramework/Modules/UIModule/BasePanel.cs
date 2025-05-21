using LFramework.Utility;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    protected virtual void Awake()
    {
        AutoInjectComponent.AutoInject(transform, this);
    }
}
