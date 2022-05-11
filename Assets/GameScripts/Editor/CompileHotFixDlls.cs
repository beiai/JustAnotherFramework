using System.IO;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;

namespace GameScripts.Editor
{
    public static class CompileHotFixDlls
    {
        [MenuItem("框架/编译热更新Dll/当前打包平台")]
        public static void CompileDllActiveBuildTarget()
        {
            CompileDlls(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("框架/编译热更新Dll/Win64")]
        public static void CompileDllWin64()
        {
            CompileDlls(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("框架/编译热更新Dll/Android")]
        public static void CompileDllAndroid()
        {
            CompileDlls(BuildTarget.Android);
        }

        [MenuItem("框架/编译热更新Dll/IOS")]
        public static void CompileDllIOS()
        {
            CompileDlls(BuildTarget.iOS);
        }

        /// <summary>
        /// 编译热更新Dll文件
        /// </summary>
        /// <param name="target">指定的Dll编译平台</param>
        private static void CompileDlls(BuildTarget target)
        {
            // 临时生成Dll的目录，放在Assets目录下会导致自动导入Dll
            var outputDir = Path.GetFullPath($"{Application.dataPath}/../Temp/GameBuild/{target}");
            CreateDirIfNotExists(outputDir);
            
            var group = BuildPipeline.GetBuildTargetGroup(target);
            var scriptCompilationSettings = new ScriptCompilationSettings
            {
                group = group,
                target = target
            };
            var scriptCompilationResult =
                PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, outputDir);
            foreach (var ass in scriptCompilationResult.assemblies)
            {
                Debug.LogFormat("compile assemblies:{0}", ass);
            }
            
            // 更重命名Dll热更文件，并拷贝到Assets目录下，走资源打包流程
            var copyDir = Path.GetFullPath($"{Application.dataPath}/GameRes/Build");
            CreateDirIfNotExists(copyDir);
            
            
            var hotfixDlls = new string[]
            {
                "HotFix.dll"
            };

            foreach(var dll in hotfixDlls)
            {
                var dllPath = $"{outputDir}/{dll}";
                var dllBytesPath = $"{copyDir}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
            }
            
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private static void CreateDirIfNotExists(string dirName)
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }
    }
}