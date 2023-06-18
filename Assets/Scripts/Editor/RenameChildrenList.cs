using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor script for renaming all selected GameObjects
/// </summary>
public class RenameAllSelectedEditor : EditorWindow
{
    private string newName = "Item #";

    [MenuItem("GameObject/Rename All Selected", false, 0)]
    private static void Init()
    {
        GetWindow<RenameAllSelectedEditor>("Rename all").Show();
    }

    private void OnGUI()
    {
        newName = EditorGUILayout.TextField("Name: ", newName);

        if (GUILayout.Button("Rename all"))
        {
            RenameChildren();
            Close();
        }

        EditorStyles.label.wordWrap = true;
        EditorGUILayout.LabelField("If the name contains '#', it will be replaced by the index of the GameObject in the selected list");
    }

    private void RenameChildren()
    {
        int i = 0;
        foreach (var selected in Selection.gameObjects)
        {
            i++;
            Undo.RecordObject(selected, "Rename All Selected");
            selected.name = newName.Replace("#", i.ToString());
            EditorUtility.SetDirty(selected);
        }
    }
}
