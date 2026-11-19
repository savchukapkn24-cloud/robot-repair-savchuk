using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[InitializeOnLoad]
public static class Lab8Setup
{
    const string Root = "Assets/_2DAdventureGame/";
    const string Flag = "Lab8SetupDone_v1";

    static Lab8Setup()
    {
        if (EditorPrefs.GetBool(Flag, false)) return;
        EditorApplication.delayCall += Run;
    }

    [MenuItem("Tools/Lab8 Build")]
    static void Menu() { EditorPrefs.SetBool(Flag, true); Run(); }

    static void Run()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return;
        if (System.Type.GetType("RisingWater, Assembly-CSharp") == null) return;
        EditorPrefs.SetBool(Flag, true);

        var water = AssetDatabase.LoadAssetAtPath<Tile>(
            AssetDatabase.GUIDToAssetPath("d7cc763ce90e0c24da61d09e5981d5ab"));

        string basePath = Root + "Prefabs/RisingWater.prefab";
        var go = new GameObject("RisingWater");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = water.sprite;
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = new Vector2(11, 8);
        sr.sortingOrder = -5;
        sr.color = new Color(0.35f, 0.65f, 1f, 0.6f);
        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = new Vector2(11, 8);
        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        var rw = go.AddComponent<RisingWater>();
        rw.riseSpeed = 0.2f; rw.maxHeight = -4.5f; rw.damage = 1; rw.startX = 7f;
        var basePrefab = PrefabUtility.SaveAsPrefabAsset(go, basePath);
        Object.DestroyImmediate(go);

        string fastPath = Root + "Prefabs/RisingWaterFast.prefab";
        var tmp = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab);
        PrefabUtility.SaveAsPrefabAsset(tmp, fastPath);
        Object.DestroyImmediate(tmp);
        var c = PrefabUtility.LoadPrefabContents(fastPath);
        var f = c.GetComponent<RisingWater>();
        f.riseSpeed = 0.35f; f.maxHeight = -6f; f.startX = -5f;
        PrefabUtility.SaveAsPrefabAsset(c, fastPath);
        PrefabUtility.UnloadPrefabContents(c);
        var fastPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(fastPath);

        var scene = EditorSceneManager.OpenScene(Root + "Scenes/MyLevel.unity", OpenSceneMode.Single);
        var a = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab);
        a.transform.position = new Vector3(12.5f, -10f, 0);
        var b = (GameObject)PrefabUtility.InstantiatePrefab(fastPrefab);
        b.transform.position = new Vector3(0.5f, -10f, 0);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("Lab8Setup: вода додана");
    }
}
