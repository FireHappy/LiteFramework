using UnityEngine;

namespace LiteFramework.Configs
{
    [CreateAssetMenu(fileName = "UIRootConfig", menuName = "LiteFramework/UI Root Config")]
    public class UIRootConfig : ScriptableObject
    {
        public string UIPath = "UI";
        public string DefaultUIParentTag = "UIParent";
        public string DefaultUIDialogTag = "DialogParent";
        public GameObject RootUIPrefab; // 自动生成出来的 UICanvas 预制体
    }
}
