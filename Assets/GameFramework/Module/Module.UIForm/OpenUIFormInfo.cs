﻿using YooAsset;

namespace GameFramework.Module.UIForm
{
    public sealed class OpenUIFormInfo : IReference
    {
        private int _serialId;
        private UIGroup _uiGroup;
        private bool _pauseCoveredUIForm;
        private object _userData;

        public OpenUIFormInfo()
        {
            _serialId = 0;
            _uiGroup = null;
            _pauseCoveredUIForm = false;
            _userData = null;
        }

        public int SerialId => _serialId;

        public UIGroup UIGroup => _uiGroup;

        public bool PauseCoveredUIForm => _pauseCoveredUIForm;

        public object UserData => _userData;
        
        public AssetOperationHandle AssetHandle { get; set; }

        public static OpenUIFormInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIForm, object userData)
        {
            OpenUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenUIFormInfo>();
            openUIFormInfo._serialId = serialId;
            openUIFormInfo._uiGroup = uiGroup;
            openUIFormInfo._pauseCoveredUIForm = pauseCoveredUIForm;
            openUIFormInfo._userData = userData;
            return openUIFormInfo;
        }

        public void Clear()
        {
            _serialId = 0;
            _uiGroup = null;
            _pauseCoveredUIForm = false;
            _userData = null;
        }
    }
}