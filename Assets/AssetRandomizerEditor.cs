
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AssetRandomizer))]
public class AssetRandomizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AssetRandomizer assetRandomizer = (AssetRandomizer)target;
        if (GUILayout.Button("Fill The List"))
        {
            assetRandomizer.FillTheList();
        }

        if (GUILayout.Button("Randomize Size"))
        {
            assetRandomizer.RandomizeSize();
        }
        GUILayout.Space(15);
        if (GUILayout.Button("Reset To Original Size"))
        {
            assetRandomizer.ResetSize();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
