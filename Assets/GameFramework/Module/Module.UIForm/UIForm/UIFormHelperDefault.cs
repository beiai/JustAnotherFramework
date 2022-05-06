using GameFramework.Module.ObjectPool;
using UnityEngine;
using YooAsset;

namespace GameFramework.Module.UIForm
{
    /// <summary>
    /// 默认界面辅助器。
    /// </summary>
    public class UIFormHelperDefault : UIFormHelperBase
    {
        private IObjectPool<UIFormInstanceObject> _uiFormObjectPool;

        public override void LoadUIFormAsset(int serialId, string uiFormAssetName, UIGroup uiGroup,
            bool pauseCoveredUIForm, object userData)
        {
            var handle = YooAssets.LoadAssetAsync<GameObject>(uiFormAssetName);
            handle.Completed += assetOperationHandle =>
            {
                ModuleManager.GetModule<UIManager>().LoadAssetCallback(serialId, uiFormAssetName, uiGroup,
                    pauseCoveredUIForm, userData, handle);
            };
        }

        public override UIFormInstanceObject GetUIFormAsset(string uiFormAssetName)
        {
            if (_uiFormObjectPool == null)
            {
                _uiFormObjectPool = ModuleManager.GetModule<ObjectPoolManager>()
                    .CreateSingleSpawnObjectPool<UIFormInstanceObject>("UI Instance Pool");
                return null;
            }

            return _uiFormObjectPool.Spawn(uiFormAssetName);
        }

        /// <summary>
        /// 实例化界面。
        /// </summary>
        /// <param name="uiFormAssetName"></param>
        /// <param name="handle"></param>
        /// <returns>实例化后的界面。</returns>
        public override object AddUIFormAsset(string uiFormAssetName, AssetOperationHandle handle)
        {
            var uiFormInstance = Instantiate(handle.AssetObject);
            var uiFormInstanceObject = UIFormInstanceObject.Create(uiFormAssetName, uiFormInstance, this, handle);
            _uiFormObjectPool.Register(uiFormInstanceObject, true);
            return uiFormInstance;
        }

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        public override IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            var formInstance = uiFormInstance as GameObject;
            if (formInstance == null)
            {
                Log.Error("UI form instance is invalid.");
                return null;
            }

            var formInstanceTransform = formInstance.transform;
            formInstanceTransform.SetParent(((MonoBehaviour)uiGroup.Helper).transform);
            formInstanceTransform.localScale = Vector3.one;
            var rectTransform = formInstanceTransform.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            return formInstance.GetOrAddComponent<UIForm>();
        }

        /// <summary>
        /// 释放界面。
        /// </summary>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        public override void ReleaseUIForm(object uiFormInstance)
        {
            _uiFormObjectPool.UnSpawn(uiFormInstance);
        }
    }
}