using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class Lab9Setup
{
    const string Root = "Assets/_2DAdventureGame/";
    const string Flag = "Lab9SetupDone_v1";
    const string OutDir = "C:/Artem/RobotRepairLab/Builds/RobotRepair-v1.0-windows";

    static Lab9Setup()
    {
        if (EditorPrefs.GetBool(Flag, false)) return;
        EditorApplication.delayCall += AutoRun;
    }

    static AudioClip Clip(string name)
    {
        return AssetDatabase.LoadAssetAtPath<AudioClip>(Root + "Audio/" + name);
    }

    static void AutoRun()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return;
        if (System.Type.GetType("Sfx, Assembly-CSharp") == null) return;
        EditorPrefs.SetBool(Flag, true);
        AssignClips();
    }

    [MenuItem("Tools/Lab9 Assign Clips")]
    static void AssignClips()
    {
        var pp = Root + "Prefabs/PlayerCharacter.prefab";
        var c = PrefabUtility.LoadPrefabContents(pp);
        var pc = c.GetComponent<PlayerController>();
        pc.hitClip = Clip("PlayerHit.wav");
        pc.collectClip = Clip("Collectible.wav");
        pc.launchClip = Clip("Projectile.wav");
        PrefabUtility.SaveAsPrefabAsset(c, pp);
        PrefabUtility.UnloadPrefabContents(c);

        var ep = Root + "Prefabs/Enemy.prefab";
        c = PrefabUtility.LoadPrefabContents(ep);
        c.GetComponent<EnemyController>().fixClip = Clip("EnemyFix.wav");
        PrefabUtility.SaveAsPrefabAsset(c, ep);
        PrefabUtility.UnloadPrefabContents(c);

        var scene = EditorSceneManager.OpenScene(Root + "Scenes/MyLevel.unity", OpenSceneMode.Single);
        var gm = Object.FindFirstObjectByType<GameManager>();
        gm.winClip = Clip("QuestComplete.wav");
        EditorUtility.SetDirty(gm);

        PlayerSettings.productName = "Robot Repair";
        PlayerSettings.companyName = "savchukapkn24";
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        PlayerSettings.defaultScreenWidth = 1280;
        PlayerSettings.defaultScreenHeight = 720;
        PlayerSettings.resizableWindow = true;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("Lab9Setup: звуки призначено, Player Settings готові");
    }

    [MenuItem("Tools/Lab9 Build")]
    static void Build()
    {
        var opts = new BuildPlayerOptions
        {
            scenes = new[] { Root + "Scenes/MyLevel.unity" },
            locationPathName = OutDir + "/RobotRepair.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };
        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log("Lab9Build: " + report.summary.result + " " + report.summary.totalSize + " байт, " + report.summary.totalErrors + " помилок");
    }
}
