using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PaintballStats
{
    public Vector3 position;
    public Color bg;
    public int sortingOrder;

    public PaintballStats(Vector3 _p, Color _bg, int sortingOrder)
    {
        position = _p;
        bg = _bg;
        this.sortingOrder = sortingOrder;

    }

}
