using System.Collections.Generic;
using System.Linq;
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

    public float GetLength()
    {
        return spline.CalculateLength();
    }

    private void OnEnable()
    {
            if(this == null) return;
            spline = GetComponent<SplineContainer>();
            instantiator = GetComponent<SplineInstantiate>();
            connections.Add(this);
#if UNITY_EDITOR
        if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            UnityEditor.EditorApplication.delayCall += MatchContent;
        else
#endif
            MatchContent();
    }

    private void OnDisable()
    {
        connections.Remove(this);
    }

    private void OnValidate()
    {
        
#if UNITY_EDITOR
        if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        UnityEditor.EditorApplication.delayCall += MatchContent;
#endif
    }

    void MatchContent() {
        if(spline == null) return;
        //if (a == null)
        {
            foreach (var pp in StopPoint.stopPoints)
            {
                if (Vector3.Distance(pp.transform.position, spline.EvaluatePosition(0, 0f)) <= 10f)
                {
                    a = pp;
                    // spline.SetLinkedKnotPosition()
                    
                    var knot = spline.Splines[0].First();
                    knot.Position = transform.InverseTransformPoint(pp.transform.position);
                    spline.Splines[0].SetKnot(0, knot);
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
                    var knot = spline.Splines[0].Last();
                    knot.Position = transform.InverseTransformPoint(pp.transform.position);
                    spline.Splines[0].SetKnot(spline.Splines[0].Count-1, knot);
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
