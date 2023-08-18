using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem
{
    public int Point;

    public void SetPoint(int PointData)
    {
        Point = Mathf.Clamp(PointData, 0, 1000000);
    }
    public void PointPlus(int PointPlusData)
    {
        Point = Mathf.Clamp(Point + PointPlusData, 0, 1000000);
    }
    public void PointMinus(int PointMinusData)
    {
        Point = Mathf.Clamp(Point - PointMinusData, 0, 1000000);
    }
}
