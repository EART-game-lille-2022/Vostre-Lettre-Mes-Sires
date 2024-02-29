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
        spline = GetComponent<SplineContainer>();
        instantiator = GetComponent<SplineInstantiate>();
        connections.Add(this);
    }

    private void OnDisable()
    {
        connections.Remove(this);
    }

    private void OnValidate()
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
    }

    // c'est a lui de creer
}
