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
        private OpenUIFormInfo _openUIFormInfo;
        private IUIFormHelper _uiFormHelper;

        public UIFormInstanceObject()
        {
            _openUIFormInfo = null;
            _uiFormHelper = null;
        }

        public static UIFormInstanceObject Create(string name, object uiFormInstance,
            IUIFormHelper uiFormHelper, OpenUIFormInfo openUIFormInfo)
        {
            if (uiFormHelper == null)
            {
                throw new Exception("UI form helper is invalid.");
            }

            var uiFormInstanceObject = ReferencePool.Acquire<UIFormInstanceObject>();
            uiFormInstanceObject.Initialize(name, uiFormInstance);
            uiFormInstanceObject._uiFormInstance = uiFormInstance;
            uiFormInstanceObject._openUIFormInfo = openUIFormInfo;
            uiFormInstanceObject._uiFormHelper = uiFormHelper;
            return uiFormInstanceObject;
        }

        public override void Clear()
        {
            base.Clear();
            _openUIFormInfo = null;
            _uiFormHelper = null;
        }

        protected internal override void Release(bool isShutdown)
        {
            _openUIFormInfo.AssetHandle.Release();
            GameObject.Destroy((GameObject)_uiFormInstance);
            _uiFormHelper.ReleaseUIForm(Target);
        }
    }
}