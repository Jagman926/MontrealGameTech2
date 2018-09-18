using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof (PlayerMovement))]

public class SoundRadiusEditor : Editor
{
    void OnSceneGUI()
    {
        //Getting a Field of View Reference
        PlayerMovement playerMove = (PlayerMovement)target;

        //Drawing it's View Radius
        Handles.color = Color.blue;
        Handles.DrawWireArc(playerMove.transform.position, Vector3.up, Vector3.forward, 360, playerMove.soundRadius);
    }
}
