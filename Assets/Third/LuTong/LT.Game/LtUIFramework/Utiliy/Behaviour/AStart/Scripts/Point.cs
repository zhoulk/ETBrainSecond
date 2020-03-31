using System.Collections;
using System.Collections.Generic;

public class Point
{
    public int Id { get; set; }
    //父节点
    public Point Parent { get; set; }
    //当前节点路径的代价
    public float F { get; set; }
    //从起始点到当前节点的代价
    public float G { get; set; }
    //从当前节点到目标点的预估代价
    public float H { get; set; }

    //当前节点x坐标
    public int X { get; set; }
    //当前节点y坐标
    public int Y { get; set; }

    public bool IsWall { get; set; }

    public Point(int x, int y, Point parent = null)
    {
        Id = GetHashCode();
        X = x;
        Y = y;
        this.Parent = parent;
        IsWall = false;
    }

    public void UpdateParent(Point parent,float g)
    {
        Parent = parent;
        G = g;
        F = G + H;
    }
}
