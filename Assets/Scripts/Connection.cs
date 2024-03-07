using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class Connection : MonoBehaviour
{
    public static List<Connection> connections = new List<Connection>();

    // public StopPoint owner;

    public StopPoint a, b;

    public Vector2 aDir, bDir;

    public SplineContainer spline;
    public SplineInstantiate instantiator;

    public float width = 1;

    private void OnEnable()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall = () =>
        {
#endif
            spline = GetComponent<SplineContainer>();
            instantiator = GetComponent<SplineInstantiate>();
            connections.Add(this);
#if UNITY_EDITOR
        };
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall = () =>
        {
#endif
            connections.Remove(this);
#if UNITY_EDITOR
        };
#endif
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall = () =>
        {

            //if (a == null)
            {
                foreach (var pp in StopPoint.stopPoints)
                {
                    if (Vector3.Distance(pp.transform.position, spline.EvaluatePosition(0, 0f)) <= 10f)
                    {
                        a = pp;
                    }
                }
            }

            //if (b == null)
            {
                foreach (var pp in StopPoint.stopPoints)
                {
                    if (Vector3.Distance(pp.transform.position, spline.EvaluatePosition(0, 1f)) <= 10f)
                    {
                        b = pp;
                    }
                }
            }


            if (a != null && b != null)
            {
                if (!a.connections.Contains(this)) a.connections.Add(this);
                if (!b.connections.Contains(this)) b.connections.Add(this);
                gameObject.name = (a.name + " - " + b.name).Replace("Path point - ", "", System.StringComparison.Ordinal);

                if (instantiator) instantiator.UpdateInstances();
            }
        };
#endif
    }

    // c'est a lui de creer
}
