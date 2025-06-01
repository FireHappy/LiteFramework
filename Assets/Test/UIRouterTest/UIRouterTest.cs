
using System.Diagnostics;
using LiteFramework.Module.UI;
using LiteFramework.Sample;
using UnityEngine;

public class UIRouterTest : MonoBehaviour
{
    public int TestIterations = 1_000;

    void Start()
    {
        var manager = new DummyUIManager();
        var router = new UIRouter(manager);     // 使用泛型委托缓存，逆天的牛逼方案

        UnityEngine.Debug.Log("---- UI Router Performance Test ----");

        Stopwatch sw = new Stopwatch();

        // 测试路由调用
        sw.Reset();
        sw.Start();
        for (int i = 0; i < TestIterations; i++)
        {
            router.Open<DummyView>();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"UIRouter Invoke: {sw.ElapsedMilliseconds} ms");

        //静态调用
        sw.Reset();
        sw.Start();
        for (int i = 0; i < TestIterations; i++)
        {
            manager.OpenUI<DummyPresenter, DummyView>();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"Static Invoke : {sw.ElapsedMilliseconds} ms");
    }
}
