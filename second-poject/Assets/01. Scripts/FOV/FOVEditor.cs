using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR

using UnityEditor;

#endif
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//CustomEditor�� �����ؾ� FieldOfView�� ���� editor�� ��ġ �� �� �ִ� 
#if UNITY_EDITOR
[CustomEditor(typeof(FOV))]
public class FOVEditor : Editor
{
    void OnSceneGUI()
    {
        FOV fov = (FOV)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.eyeRadius);

        Vector3 viewAngleA = fov.findTargetAngle(-fov.eyeAngle / 2, false);
        Vector3 viewAngleB = fov.findTargetAngle(fov.eyeAngle / 2, false);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.eyeRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.eyeRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.TargetLists)
        {
            if (fov.FirstTarget != visibleTarget)
            {
                Handles.DrawLine(fov.transform.position, visibleTarget.position);
            }
        }
        Handles.color = Color.green;
        if (fov.FirstTarget)
        {
            Handles.DrawLine(fov.transform.position, fov.FirstTarget.position);
        }
    }
}

#endif