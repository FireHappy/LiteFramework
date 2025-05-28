using System.Collections;
using System.Collections.Generic;
using LiteFramework.Core.Utility;
using UnityEngine;

public class StaticBindVsAutoInjectTest : MonoBehaviour
{
    public Transform UIRoot;
    public MyUIView Target;

    public int LoopCount = 10000;

    void Start()
    {
        // Warmup
        AutoInjectComponent.AutoInject(UIRoot, Target);
        Target.StaticBind(UIRoot);

        // Test Reflection Inject
        var sw1 = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < LoopCount; i++)
        {
            AutoInjectComponent.AutoInject(UIRoot, Target);
        }
        sw1.Stop();
        Debug.Log($"Reflection Inject Time: {sw1.ElapsedMilliseconds}ms");

        // Test Static Binding
        var sw2 = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < LoopCount; i++)
        {
            Target.StaticBind(UIRoot);
        }
        sw2.Stop();
        Debug.Log($"Static Binding Time: {sw2.ElapsedMilliseconds}ms");
    }
}
