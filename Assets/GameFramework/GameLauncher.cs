using System.Collections;
using GameFramework.Module;
using GameFramework.Module.Event;
using GameFramework.Module.Fsm;
using GameFramework.Module.ObjectPool;
using GameFramework.Module.Procedure;
using GameFramework.Module.Setting;
using GameFramework.Module.Sound;
using GameFramework.Module.UIForm;
using UnityEngine;
using YooAsset;

public class GameLauncher : MonoBehaviour
{
    public YooAssets.EPlayMode playMode = YooAssets.EPlayMode.EditorPlayMode;
    
    private void Awake()
    {
        InitApplication();
        Initialize();
    }

    private void Start()
    {
        StartCoroutine(InitYooAsset());
    }

    private void Update()
    {
        ModuleManager.Update();
        if (Input.anyKeyDown)
        {
            ModuleManager.GetModule<SoundManager>().PlaySound("Sound/UI/Click1", "UI");
        }
    }

    /// <summary>
    /// 创建框架模块
    /// </summary>
    private void CreateGameModules()
    {
        ModuleManager.GetModule<EventManager>();
        ModuleManager.GetModule<FsmManager>();
        ModuleManager.GetModule<ProcedureManager>();
        ModuleManager.GetModule<ObjectPoolManager>();
        ModuleManager.GetModule<SettingManager>();
        ModuleManager.GetModule<UIManager>();
        ModuleManager.GetModule<SoundManager>();
    }

    /// <summary>
    /// 初始化框架
    /// </summary>
    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);

        InitLogHelper();
        CreateGameModules();
        Log.Debug("Hello Word");
    }
    
    /// <summary>
    /// 初始化框架日志组件
    /// </summary>
    private void InitLogHelper()
    {
        var logHelper = new LogHelperDefault();
        LogManager.SetLogHelper(logHelper);
    }
    
    private void InitApplication()
    {
        Application.runInBackground = true;
        Application.backgroundLoadingPriority = ThreadPriority.High;

        // 设置最大帧数
        Application.targetFrameRate = 60;

        // 屏幕不休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
    private IEnumerator InitYooAsset()
    {
        Debug.Log($"资源系统运行模式：{playMode}");

        // 编辑器模拟模式
        if(playMode == YooAssets.EPlayMode.EditorPlayMode)
        {
            var createParameters = new YooAssets.EditorPlayModeParameters();
            createParameters.LocationServices = new DefaultLocationServices("Assets/GameRes");
            yield return YooAssets.InitializeAsync(createParameters);
        }

        // 单机模式
        if (playMode == YooAssets.EPlayMode.OfflinePlayMode)
        {
            var createParameters = new YooAssets.OfflinePlayModeParameters();
            createParameters.LocationServices = new DefaultLocationServices("Assets/GameRes");
            yield return YooAssets.InitializeAsync(createParameters);
        }

        // 联机模式
        if (playMode == YooAssets.EPlayMode.HostPlayMode)
        {
            var createParameters = new YooAssets.HostPlayModeParameters();
            createParameters.LocationServices = new DefaultLocationServices("Assets/GameRes");
            createParameters.DecryptionServices = null;
            createParameters.ClearCacheWhenDirty = false;
            createParameters.BreakpointResumeFileSize = 0;
            createParameters.DefaultHostServer = GetHostServerURL();
            createParameters.FallbackHostServer = GetHostServerURL();
            yield return YooAssets.InitializeAsync(createParameters);
        }
        
        ModuleManager.GetModule<UIManager>().OpenUIForm("UIForm/UIMain.prefab", "Default");
    }
    
    private string GetHostServerURL()
    {
        const string hostServerIP = "http://127.0.0.1";
        const string gameVersion = "100";

        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{gameVersion}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{gameVersion}";
    }
}