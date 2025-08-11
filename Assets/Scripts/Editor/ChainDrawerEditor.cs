using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChainPathDrawer))]
public class ChainPathDrawerEditor : Editor
{
    ChainPathDrawer drawer;

    void OnSceneGUI()
    {
        drawer = (ChainPathDrawer)target;

        Handles.color = Color.green;
        for (int i = 0; i < drawer.pathPoints.Count; i++)
        {
            Vector3 worldPos = drawer.transform.TransformPoint(drawer.pathPoints[i]);
            Handles.DrawSolidDisc(worldPos, Vector3.forward, 0.05f);
        }

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.shift)
        {
            Vector2 mousePos = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            Vector3 worldPoint = ray.origin;
            Vector2 localPoint = drawer.transform.InverseTransformPoint(worldPoint);

            Undo.RecordObject(drawer, "Add Path Point");
            drawer.pathPoints.Add(localPoint);
            EditorUtility.SetDirty(drawer);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear Path"))
        {
            drawer.pathPoints.Clear();
        }
    }
}
