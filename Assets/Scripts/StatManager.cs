using UnityEngine;
using UnityEditor;

public class StatManager : EditorWindow {

    [MenuItem("Window/Stat Manager")]
    public static void ShowWindow()
    {
        GetWindow(typeof(StatManager));
    }

    public int value;

    void OnGUI()
    {
        //All window functionality goes here
        value = EditorGUILayout.IntField(value);
    }

    public void CreateStatsFile(GameObject prefab)
    {
        //new stats text file that is made custom for their abilities

    }

    public void UpdateStatsFromFile(TextAsset stats)
    {

    }
}
