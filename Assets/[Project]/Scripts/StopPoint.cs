using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class StopPoint : MonoBehaviour
{
    public static List<StopPoint> stopPoints = new List<StopPoint>();
    // public List<StopPoint> targets;

    /*[System.Serializable]
    public class Connection
    {
        public StopPoint owner;
        public StopPoint a, b;
        public Vector2 aDir, bDir;

        public Spline bezierKnots;
    }*/

    public List<Connection> connections;

    void Start() {
        /* foreach(var t in targets)
        {
            Connection c = FindOrCreate(t);
            connections.Add(c);
            t.connections.Add(c);
            if(!t.targets.Contains(this)) t.targets.Add(this);
        } 
        */
    }

    public Connection FindByDirection(Vector3 direction, float maxAngle = 60, float sampleTarget = .1f) {
        float foundangle = maxAngle;
        Connection foundConnection = null;

        foreach (var conn in connections)
        {
            float a = 0;
            if(conn.b == this) {
                // a = Vector3.Angle(direction, conn.b.transform.position - conn.a.transform.position);

                conn.spline.Evaluate(sampleTarget, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up);
                Debug.DrawRay(pos, Vector3.up, Color.red, 2);
                Debug.DrawRay(conn.b.transform.position, (Vector3)pos-conn.b.transform.position, Color.blue, 2);
                a = Vector3.Angle(direction, (Vector3)pos - conn.b.transform.position);
            }
            else {
                // a = Vector3.Angle(direction, conn.a.transform.position - conn.b.transform.position);
                conn.spline.Evaluate(1-sampleTarget, out Unity.Mathematics.float3 pos, out Unity.Mathematics.float3 tangeant, out Unity.Mathematics.float3 up);
                Debug.DrawRay(pos, Vector3.up, Color.red, 2);
                Debug.DrawRay(conn.a.transform.position, (Vector3)pos-conn.a.transform.position, Color.blue, 2);
                a = Vector3.Angle(direction, (Vector3)pos - conn.a.transform.position);
            }
            Debug.Log("dirAngle :" + a + " < ? " + foundangle + "\nconn:" + conn.a.name + "-" + conn.b.name, conn.gameObject);


            if (a < foundangle)
            {
                foundangle = a;
                foundConnection = conn;
            }
        }

        return foundConnection;
    }


    private void OnEnable()
    {
        stopPoints.Add(this);
    }

    private void OnDisable()
    {
        stopPoints.Remove(this);
    }

    Connection FindOrCreate(StopPoint target)
    {
        /*foreach(var t in connections)
        {
            if(t.a == target) return t;
            if (t.b == target) return t;
        }

        foreach (var t in target.connections)
        {
            if (t.a == target) return t;
            if (t.b == target) return t;
        }

        Connection conn = new Connection();
        conn.owner = target;
        conn.a = this; conn.b = target;
        conn.aDir = conn.b.transform.position - conn.a.transform.position;
        conn.bDir = -conn.aDir;

        return conn;
         */
        foreach(var conn in Connection.connections) {
            if (conn.a == target && conn.b == target) return conn;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        /* foreach (var t in targets)
        {
            Debug.DrawLine(transform.position, t.transform.position, Color.red);
        } */
    }
}
