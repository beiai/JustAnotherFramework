using System;
using System.Collections;
using System.Reflection;
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

public class AppMain : MonoBehaviour
{
    private Assembly _assembly;
    private Type _hotfixType;
    private MethodInfo _hotfixMain;
    private Delegate _hotfixUpdate;
    public YooAssets.EPlayMode playMode = YooAssets.EPlayMode.EditorSimulateMode;

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
        _hotfixUpdate?.Method.Invoke(null, null);
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
        Debug.Log("Hello Word");
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
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private IEnumerator InitYooAsset()
    {
        Debug.Log($"资源系统运行模式：{playMode}");

        // 编辑器模拟模式
        if (playMode == YooAssets.EPlayMode.EditorSimulateMode)
        {
            var createParameters = new YooAssets.EditorSimulateModeParameters();
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

        InitGameDlls();
    }

    private void InitGameDlls()
    {
        var handle = YooAssets.LoadAssetAsync<TextAsset>("Build/HotFix.dll");
        handle.Completed += LoadDllsInfo;
    }

    private void LoadDllsInfo(AssetOperationHandle handle)
    {
        if (handle.Status != EOperationStatus.Succeed) return;
#if !UNITY_EDITOR
        var dllBytes = handle.AssetObject as TextAsset;
        if (dllBytes != null) _assembly = Assembly.Load(dllBytes.bytes);
#else
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (assembly.GetName().Name == "HotFix")
            {
                _assembly = assembly;
            }
        }
#endif
        if (_assembly == null)
        {
            Debug.Log("HotFix Dll 加载失败！");
            return;
        }

        _hotfixType = _assembly.GetType("HotFix");
        if (_hotfixType == null)
        {
            Debug.Log("HotFix 类获取失败！");
            return;
        }
        
        _hotfixMain = _hotfixType.GetMethod("Main");
        if (_hotfixMain == null)
        {
            Debug.Log("HotFix Main 函数获取失败！");
            return;
        }
        
        // 如果是Update之类的函数，推荐先转成Delegate再调用，如
        var updateMethod = _hotfixType.GetMethod("Update");
        if (updateMethod == null)
        {
            Debug.Log("HotFix Update 函数获取失败！");
            return;
        }
        _hotfixUpdate = Delegate.CreateDelegate(typeof(Action), null, updateMethod);
        _hotfixMain.Invoke(null, null);
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