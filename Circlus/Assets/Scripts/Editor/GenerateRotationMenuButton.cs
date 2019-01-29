using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RotationMenuManager))]
public class GenerateRotationMenuButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RotationMenuManager manager = (RotationMenuManager)target;
        if(manager != null)
        {
            if(GUILayout.Button("Generate Rotation Menu Items"))
            {
                manager.GenerateRotationMenuItems();
            }
        }
    }
}
