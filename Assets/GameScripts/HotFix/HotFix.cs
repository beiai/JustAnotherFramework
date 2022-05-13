using GameFramework.Module;
using GameFramework.Module.Sound;
using GameFramework.Module.Timer;
using GameFramework.Module.UIForm;
using UnityEngine;

public class HotFix
{
    public static void Main()
    {
        Debug.Log("HotFix Main Run");
        UIManager.Instance.OpenUIForm("UIForm/UIMain.prefab", "Default");
        Debug.Log("Open UI Form");

        TimerManager.Instance.AddRepeatedTimer(500, 1,
            (timeLeft, userData) => { Log.Debug($"执行！剩余时间:{timeLeft}"); }, null,
            timeLeft => { Log.Debug($"Update执行！剩余时间:{timeLeft}"); }
        );
    }

    public static void Update()
    {
        if (Input.anyKeyDown)
        {
            SoundManager.Instance.PlaySound("Sound/UI/click", "UI");
        }
    }
}