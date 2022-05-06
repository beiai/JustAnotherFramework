using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Module.UIForm
{
    /// <summary>
    /// 默认界面组辅助器。
    /// </summary>
    public class UIGroupHelperDefault : UIGroupHelperBase
    {
        public const int DepthFactor = 100;
        private int _depth;
        private Canvas _cachedCanvas;

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            _depth = depth;
            _cachedCanvas.overrideSorting = true;
            _cachedCanvas.sortingOrder = DepthFactor * depth;
        }

        private void Awake()
        {
            _cachedCanvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            _cachedCanvas.overrideSorting = true;
            _cachedCanvas.sortingOrder = _depth;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}