using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CutsomSceneView : Editor
{
    [InitializeOnLoadMethod]
    static void Init()
    {
        SceneView.duringSceneGui += SceneView_duringSceneGui;
        Debug.Log("CustomSceneView initialized");
    }

    private static void SceneView_duringSceneGui(SceneView sceneView)
    {
        Handles.BeginGUI();

        GUILayout.Label("Custom Scene View Enabled");

        Handles.EndGUI();

        Vector3 mousePosition = Event.current.mousePosition;

        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        if (Physics.Raycast(
            ray: ray,
            hitInfo: out RaycastHit hitInfo,
            maxDistance: float.MaxValue
            ))
        {
            Vector3 targetPosition = hitInfo.point + hitInfo.normal / 2f;
            float targetPosX = targetPosition.x;
            float targetPosY = targetPosition.y + 0.4f;
            float targetPosZ = targetPosition.z;

            Handles.color = Color.red;
            Handles.DrawWireCube(new Vector3(
                    Mathf.RoundToInt(targetPosX),
                    Mathf.RoundToInt(targetPosY),
                    Mathf.RoundToInt(targetPosZ)), Vector3.one);

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            if (Event.current.GetTypeForControl(controlID) == EventType.MouseDown)
            {
                if (Event.current.button == 0)
                {
                    // left mouse click
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    
                    cube.transform.position = new Vector3(
                    Mathf.RoundToInt(targetPosX),
                    Mathf.RoundToInt(targetPosY),
                    Mathf.RoundToInt(targetPosZ));

                    Undo.RegisterCreatedObjectUndo(cube, "Place Cube");
                    //ADD THIS LINE FOR RANDOM COLORS WARNING: cube.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
                    // ADD THIS FOR CHANGING ALL CUBES COLOR AT RANDOM cube.GetComponent<Renderer>().sharedMaterial.color = UnityEngine.Random.ColorHSV();
                }
            }

            sceneView.Repaint();
        }
    }
}


