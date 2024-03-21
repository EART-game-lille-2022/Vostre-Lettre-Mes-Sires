using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDefinitions : MonoBehaviour
{
    public static Vector2 Bezier(Vector2 a, Vector2 aAnchor, Vector2 b, Vector2 bAnchor, float t)
    {
        aAnchor = a + aAnchor;
        bAnchor = b + bAnchor;
        return Vector2.Lerp(Vector2.Lerp(Vector2.Lerp(a, aAnchor, t), Vector2.Lerp(aAnchor, b, t), t), Vector2.Lerp(Vector2.Lerp(aAnchor, b, t), Vector2.Lerp(b, bAnchor, t), t), t);
        // return (1 - t)(1 - t)(1 - t)a + 3(1 - t)(1 - t)taAnchor + 3(1 - t)ttbAnchor + tttb;
    }
    public static float ApproxLength(Vector2 a, Vector2 aAnchor, Vector2 b, Vector2 bAnchor, int quality = 100)
    {
        Vector2 last = a;
        float length = 0;
        for (int i = 1; i <= quality; i++)
        {
            Vector2 next = Bezier(a, aAnchor, b, bAnchor, i / (float)quality);
            length += Vector2.Distance(last, next);
            last = next;
        }
        return length;
    }

    // "Better" because it uses the length to determine the quality so it's alike a "per meter" quality
    public static float BetterApproxLength(Vector2 a, Vector2 aAnchor, Vector2 b, Vector2 bAnchor, int quality = 100)
    {
        float l = ApproxLength(a, aAnchor, b, bAnchor, quality);
        float l2 = ApproxLength(a, aAnchor, b, bAnchor, Mathf.CeilToInt(quality * l));
        return l2;
    }
}
