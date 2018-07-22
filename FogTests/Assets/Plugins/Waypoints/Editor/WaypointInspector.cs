using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint)), CanEditMultipleObjects]
public class WaypointInspector : Editor {

    protected virtual void OnSceneGUI()
    {
        var waypoint = target as Waypoint;

//        EditorGUI.BeginChangeCheck();
//        var newTargetPosition = Handles.PositionHandle(waypoint.targetPosition, Quaternion.identity);
//        if (EditorGUI.EndChangeCheck())
//        {
//            Undo.RecordObject(waypoint, "Change Look At Target Position");
//            waypoint.targetPosition = newTargetPosition;
//            waypoint.Update();
//        }
        
////        if (Event.current.shift)
////        {
//            EditorGUI.BeginChangeCheck();
//            var t = Handles.FreeMoveHandle(waypoint.transform.position, Quaternion.identity,
//                HandleUtility.GetHandleSize(Vector3.zero) * 0.1f, waypoint.transform.position, Handles.DotHandleCap);
////        Handles.ScaleHandle(HandleUtility.GetHandleSize(waypoint.transform.position));
//            if (EditorGUI.EndChangeCheck())
//            {
//                waypoint.transform.position = t;
//            }
////        }

        var wposition = waypoint.transform;

        var points = waypoint.points;
        for (var i = 0; i < points.Count; i++)
        {
            var point = wposition.TransformPoint(points[i]);
            if (waypoint.showNames)
                Handles.Label(point + waypoint.nameOffset, string.Format("{0}[{1}]", waypoint.name, i));
            EditorGUI.BeginChangeCheck();
            var newPointPosition = Handles.FreeMoveHandle(point, Quaternion.identity,
                HandleUtility.GetHandleSize(Vector3.zero) * 0.1f, point, Handles.DotHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                waypoint.points[i] = wposition.InverseTransformPoint(newPointPosition);
            }
        }

        for (var i = 0; i < points.Count; i++)
        {
            var p0 = wposition.TransformPoint(points[i]);
            var next = i + 1;
            
            if (next >= points.Count)
            {
                if (!waypoint.loop)
                    break;
                next = 0;
            }
            
            var p1 = wposition.TransformPoint(points[next]);
            
            Handles.DrawLine(p0, p1);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var waypoint = target as Waypoint;
//        waypoint.Update();

        if (GUILayout.Button("Add"))
        {
            var newPoint = new Vector3();
            if (waypoint.points.Count > 0)
            {
                newPoint = waypoint.points[waypoint.points.Count - 1];
            }
            waypoint.points.Add(newPoint + waypoint.newWaypointOffset);
        }
    }
}
