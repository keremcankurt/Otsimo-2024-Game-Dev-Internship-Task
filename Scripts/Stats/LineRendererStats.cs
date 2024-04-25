using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineRendererStats
{
    public Color color;
    public List<Vector3> points;
    public float startWidth;
    public int sortingOrder;

    public LineRendererStats(Color _c, List<Vector3> _p, float _s, int _o)
    {
        color = _c;
        points = _p;
        startWidth = _s;
        sortingOrder = _o;
    }
}
