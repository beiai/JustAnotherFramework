using GameFramework.Module;
using GameFramework.Module.Sound;
using GameFramework.Module.UIForm;
using UnityEngine;

public class HotFix
{
    public static void Main()
    {
        Debug.Log("HotFix Main Run");
        UIManager.Instance.OpenUIForm("UIForm/UIMain.prefab", "Default");
        Debug.Log("Open UI Form");
    }

    public static void Update()
    {
        if (Input.anyKeyDown)
        {
            SoundManager.Instance.PlaySound("Sound/UI/click", "UI");
        }
    }
}
