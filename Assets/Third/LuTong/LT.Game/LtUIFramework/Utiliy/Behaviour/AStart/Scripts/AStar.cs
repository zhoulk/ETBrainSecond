using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    private const int _mapWith = 15;
    private const int _mapHeight = 15;

    private Point[,] _map = new Point[_mapWith, _mapHeight];

    //开启集合
    Dictionary<int, Point> _openDic = new Dictionary<int, Point>();
    //关闭集合
    Dictionary<int, Point> _closeDic = new Dictionary<int, Point>();

    void Start()
    {
        InitMap();
        Point start = _map[2, 3];
        Point end = _map[13, 4];
        FindPath(start,end);

        ShowPath(start,end);
    }

    private void ShowPath(Point start ,Point end)
    {
        Point temp = end;
        while (true)
        {
            Debug.Log(temp.X + "-" + temp.Y);
            Color c = Color.gray;
            if (temp == start)
            {
                c = Color.green;
            }
            else if(temp == end)
            {
                c = Color.red;
            }
            CreateLine(temp.X,temp.Y,c);

            if(temp.Parent == null)
                break;
            temp = temp.Parent;
        }

        //创建障碍物
        for (int x = 0; x < _mapWith; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                if (_map[x, y].IsWall)
                {
                    CreateLine(x,y,Color.blue);
                }
            }
        }
    }

    private void CreateLine(int x,int y,Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(x,y,0);
        go.GetComponent<Renderer>().material.color = color;
    }

    //初始化地图
    private void InitMap()
    {
        for (int x = 0; x < _mapWith; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                _map[x, y] = new Point(x,y);
            }
        }

        //设置障碍物
        _map[4, 2].IsWall = true;
        _map[4, 3].IsWall = true;
        _map[4, 4].IsWall = true;
        _map[4, 5].IsWall = true;
        _map[4, 6].IsWall = true;
    }

    private void AddOpenPoint(Point point)
    {
        _openDic.Add(point.Id, point);
    }

    private void RemoveOpenPoint(Point point)
    {
        _openDic.Remove(point.Id);
    }

    private bool IsOpenPoint(Point point)
    {
        return _openDic.ContainsKey(point.Id);
    }

    private void AddClosePoint(Point point)
    {
        _closeDic.Add(point.Id, point);
    }

    private void RemoveClosePoint(Point point)
    {
        _closeDic.Remove(point.Id);
    }

    private bool IsClosePoint(Point point)
    {
        return _closeDic.ContainsKey(point.Id);
    }

    private void FindPath(Point start, Point end)
    {
        AddOpenPoint(start);

        while (_openDic.Count > 0)
        {
            Point point = FindMinFPoint();
            RemoveOpenPoint(point);
            AddClosePoint(point);

            //过滤找到的周围点
            List<Point> surroundPoints = GetSurroundPoints(point);
            PointFilter(surroundPoints);

            //周围点
            foreach (Point surroundPoint in surroundPoints)
            {

                if (IsOpenPoint(surroundPoint))
                {
                    //存在Openlist中
                    float nowG = CalcG(surroundPoint, point);
                    //判断nowG值是否比现有的G值小
                    if (nowG < surroundPoint.G)
                    {
                        //替换父对象
                        surroundPoint.UpdateParent(point, nowG);
                    }
                }
                else
                {
                    //不存在Openlist中
                    surroundPoint.Parent = point;
                    CalcF(surroundPoint,end);
                    AddOpenPoint(surroundPoint);
                }

            }
            //判断end是否在开启列表中
            if (IsOpenPoint(end))
            {
                break;
            }

        }



    }

    /// <summary>
    /// 过滤周围点
    /// </summary>
    /// <param name="src"></param>
    /// <param name="closePoint"></param>
    private void PointFilter(List<Point> src)
    {
        for (int i = src.Count -1; i >=0; i--)
        {
            if (IsClosePoint(src[i]))
            {
                src.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 找到周围所有的点
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private List<Point> GetSurroundPoints(Point point)
    {
        //先得到上下左右
        Point up = null, down = null, left = null, right = null;
        if (point.Y < _mapHeight - 1)
        {
            up = _map[point.X, point.Y + 1];
        }

        if (point.Y > 0)
        {
            down = _map[point.X, point.Y - 1];
        }

        if (point.X > 0)
        {
            left = _map[point.X - 1, point.Y];
        }

        if (point.X < _mapWith - 1)
        {
            right = _map[point.X + 1, point.Y];
        }

        //左上 右上 左下 右下
        Point lu = null, ru = null, ld = null, rd = null;

        if (up != null && left != null)
        {
            lu = _map[point.X - 1, point.Y + 1];
        }

        if (up != null && right != null)
        {
            ru = _map[point.X + 1, point.Y + 1];
        }

        if (down != null && left != null)
        {
            ld = _map[point.X - 1, point.Y - 1];
        }

        if (down != null && right != null)
        {
            rd = _map[point.X + 1, point.Y - 1];
        }

        List<Point> list = new List<Point>();

        if (up != null && up.IsWall == false)
        {
            list.Add(up);
        }

        if (down != null && down.IsWall == false)
        {
            list.Add(down);
        }

        if (left != null && left.IsWall == false)
        {
            list.Add(left);
        }

        if (right != null && right.IsWall == false)
        {
            list.Add(right);
        }

        //自身不能是障碍  左边和上边不能是障碍
        if (lu != null && lu.IsWall == false && left.IsWall == false && up.IsWall == false)
        {
            list.Add(lu);
        }

        if (ru != null && ru.IsWall == false && right.IsWall == false && up.IsWall == false)
        {
            list.Add(ru);
        }

        if (ld != null && ld.IsWall == false && left.IsWall == false && down.IsWall == false)
        {
            list.Add(ld);
        }

        if (rd != null && rd.IsWall == false && right.IsWall == false && down.IsWall == false)
        {
            list.Add(rd);
        }

        return list;
    }

    /// <summary>
    /// 找到最小的F值的点
    /// </summary>
    private Point FindMinFPoint()
    {
        float f = float.MaxValue;
        Point temp = null;
        foreach (Point p in _openDic.Values)
        {
            if (p.F < f)
            {
                temp = p;
                f = p.F;
            }
        }
        return temp;
    }

    //计数F值
    private void CalcF(Point now, Point end)
    {
        //H值计数
        float h = Mathf.Abs(end.X - now.X) + Mathf.Abs(end.Y - now.Y);

        //G值计数
        float g = 0;
        if (now.Parent == null)
        {
            //开始点没有父亲
            g = 0;
        }
        else
        {
            //计算和父亲的距离
            g = Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(now.Parent.X, now.Parent.Y)) + now.Parent.G;
        }

        float f = g + h;

        now.F = f;
        now.G = g;
        now.H = h;
    }

    //计数G值
    private float CalcG(Point now, Point parent)
    {
        //计算和父亲的距离  g值
        return Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(now.Parent.X, now.Parent.Y)) + now.Parent.G;
    }

}
