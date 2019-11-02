using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class CurveManager : MonoBehaviour
{
    public GameObject CreateCurve()
    {
        GameObject go = new GameObject();
        go.transform.SetParent(this.transform);

        go.AddComponent<BGCurve>();
        go.AddComponent<BGCcMath>();

        return go;
    }

    public void SetCurve(BGCurve curve, NavMeshPath path, Vector3 origin)
    {
        curve.Clear();
        foreach (Vector3 point in path.corners)
        {
            curve.AddPoint(new BGCurvePoint(curve, point, BGCurvePoint.ControlTypeEnum.Absent, true));
        }
    }
}
