using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This uses UnityEditor
using UnityEditor;

[CustomEditor(typeof (FieldOfView))]

public class FieldOfViewEditor : Editor 
{
    void OnSceneGUI()
    {
        //Getting a Field of View Reference
        FieldOfView fov = (FieldOfView)target;

        //Drawing it's Sound Radius
        Handles.color = Color.blue;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.soundRadius);

        //Drawing it's View Radius
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

        //Drawing it's View Angle
        Vector3 viewAngleA = fov.DirectionFromAngle (-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirectionFromAngle (fov.viewAngle / 2, false);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

        //////////////////////////

        if(fov.visibleTargets.Count >= 1)
        {
             foreach (Transform visibleTarget in fov.visibleTargets)
            {
                Handles.color = Color.red; 
                Handles.DrawLine(fov.transform.position, visibleTarget.transform.position);
            }
        }
    }
}
