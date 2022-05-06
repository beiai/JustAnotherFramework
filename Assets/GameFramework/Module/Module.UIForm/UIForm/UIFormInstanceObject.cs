using System;
using GameFramework.Module.ObjectPool;
using UnityEngine;
using YooAsset;

namespace GameFramework.Module.UIForm
{
    /// <summary>
    /// 界面实例对象。
    /// </summary>
    public sealed class UIFormInstanceObject : ObjectBase
    {
        private object _uiFormInstance;
        private AssetOperationHandle _uiFormAssetHandle;
        private IUIFormHelper _uiFormHelper;

        public UIFormInstanceObject()
        {
            _uiFormAssetHandle = null;
            _uiFormHelper = null;
        }

        public static UIFormInstanceObject Create(string name, object uiFormInstance,
            IUIFormHelper uiFormHelper, AssetOperationHandle handle)
        {
            if (uiFormHelper == null)
            {
                throw new Exception("UI form helper is invalid.");
            }

            var uiFormInstanceObject = ReferencePool.Acquire<UIFormInstanceObject>();
            uiFormInstanceObject.Initialize(name, uiFormInstance);
            uiFormInstanceObject._uiFormInstance = uiFormInstance;
            uiFormInstanceObject._uiFormAssetHandle = handle;
            uiFormInstanceObject._uiFormHelper = uiFormHelper;
            return uiFormInstanceObject;
        }

        public override void Clear()
        {
            base.Clear();
            _uiFormAssetHandle = null;
            _uiFormHelper = null;
        }

        protected internal override void Release(bool isShutdown)
        {
            _uiFormAssetHandle.Release();
            GameObject.Destroy((GameObject)_uiFormInstance);
            _uiFormHelper.ReleaseUIForm(Target);
        }
    }
}