using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//http://members.chello.at/~easyfilter/bresenham.html

public static class Bresenham
{
    public static HashSet<Vector2Int> PlotLine(int x0, int y0, int x1, int y1)
    {
        var points = new HashSet<Vector2Int>();

        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;  /* error value e_xy */

        for (; ; )    /* loop */
        {
            TryAdd(points, x0, y0);
            e2 = 2 * err;
            if (e2 >= dy)   /* e_xy+e_x > 0 */
            {
                if (x0 == x1) break;
                err += dy;
                x0 += sx;
            }
            if (e2 <= dx)   /* e_xy+e_y < 0 */
            {
                if (y0 == y1) break;
                err += dx; y0 += sy;
            }
        }
        return points;
    }

    public static HashSet<Vector2Int> PlotCircle(int xm, int ym, int r)
    {
        var points = new HashSet<Vector2Int>();

        int x = -r, y = 0, err = 2 - 2 * r; /* II. Quadrant */
        do
        {
            TryAdd(points, xm - x, ym + y);  /*   I. Quadrant */
            TryAdd(points, xm - y, ym - x); /*  II. Quadrant */
            TryAdd(points, xm + x, ym - y); /* III. Quadrant */
            TryAdd(points, xm + y, ym + x); /*  IV. Quadrant */
            r = err;

            if (r <= y) err += ++y * 2 + 1; /* e_xy+e_y < 0 */
            if (r > x || err > y) err += ++x * 2 + 1; /* e_xy+e_x > 0 or no 2nd y-step */
        } while (x < 0);



        return points;
    }

    private static void TryAdd(HashSet<Vector2Int> set, int x, int y)
    {
        var point = new Vector2Int(x, y);
        if (set.Contains(point)) return;
        set.Add(point);
    }
}
