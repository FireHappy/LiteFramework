
using System.Diagnostics;
using LiteFramework.Module.UI;
using LiteFramework.Sample;
using UnityEngine;

public class UIRouterTest : MonoBehaviour
{
    private const int TestIterations = 100_000;

    void Start()
    {
        var manager = new DummyUIManager();

        var routerInvoke = new UIRouterInvoke(manager);     // 使用 MethodInfo.Invoke
        var routerExpress = new UIRouter(manager);     // 使用 表达式树委托

        UnityEngine.Debug.Log("---- UI Router Performance Test ----");

        Stopwatch sw = new Stopwatch();

        // 测试慢的反射方案
        sw.Start();
        for (int i = 0; i < TestIterations; i++)
        {
            routerInvoke.Open<DummyView>();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"Reflection Invoke (Slow): {sw.ElapsedMilliseconds} ms");

        // 测试快的表达式树方案
        sw.Reset();
        sw.Start();
        for (int i = 0; i < TestIterations; i++)
        {
            routerExpress.Open<DummyView>();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"Expression Tree (Fast): {sw.ElapsedMilliseconds} ms");
    }
}
