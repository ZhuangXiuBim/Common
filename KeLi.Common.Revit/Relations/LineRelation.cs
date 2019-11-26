﻿/*
 * MIT License
 *
 * Copyright(c) 2019 KeLi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
             ,---------------------------------------------------,              ,---------,
        ,----------------------------------------------------------,          ,"        ,"|
      ,"                                                         ,"|        ,"        ,"  |
     +----------------------------------------------------------+  |      ,"        ,"    |
     |  .----------------------------------------------------.  |  |     +---------+      |
     |  | C:\>FILE -INFO                                     |  |  |     | -==----'|      |
     |  |                                                    |  |  |     |         |      |
     |  |                                                    |  |  |/----|`---=    |      |
     |  |              Author: KeLi                          |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 10/30/2019 07:08:41 PM |  |  |     |         |      |
     |  | C:\>_                                              |  |  |     | -==----'|      |
     |  |                                                    |  |  |   ,/|==== ooo |      ;
     |  |                                                    |  |  |  // |(((( [66]|    ,"
     |  `----------------------------------------------------'  |," .;'| |((((     |  ,"
     +----------------------------------------------------------+  ;;  | |         |,"
        /_)_________________________________________________(_/  //'   | +---------+
           ___________________________/___  `,
          /  oooooooooooooooo  .o.  oooo /,   \,"-----------
         / ==ooooooooooooooo==.o.  ooo= //   ,`\--{)B     ,"
        /_==__==========__==_ooo__ooo=_/'   /___________,"
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Relations
{
    /// <summary>
    /// About two lines relationship.
    /// </summary>
    public static class LineRelation
    {
        /// <summary>
        /// Gets the result of whether the line line1 and the line line2 is space vertical.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsSpaceVertical(this Line line1, Line line2, double tolerance = 2 * 10e-3)
        {
            if (line1 == null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 == null)
                throw new ArgumentNullException(nameof(line2));

            return Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI / 2) < tolerance;
        }

        /// <summary>
        /// Gets the result of whether the line line1 and the line line2 is space parallel.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsSpaceParallel(this Line line1, Line line2, double tolerance = 2 * 10e-3)
        {
            if (line1 == null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 == null)
                throw new ArgumentNullException(nameof(line2));

            if (Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI) < tolerance)
                return true;

            return line1.Direction.AngleTo(line2.Direction) < tolerance;
        }

        /// <summary>
        /// Converts to plane line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line ToPlaneLine(this Line line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            var p1 = line.GetEndPoint(0);
            var p2 = line.GetEndPoint(1);

            return Line.CreateBound(new XYZ(p1.X, p1.Y, p1.Z), new XYZ(p2.X, p2.Y, p1.Z));
        }

        /// <summary>
        /// Gets the result of whether the line line1 and the line line2 is plane parallel.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsPlaneParallel(this Line line1, Line line2, double tolerance = 2 * 10e-3)
        {
            if (line1 == null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 == null)
                throw new ArgumentNullException(nameof(line2));

            line1 = line1.ToPlaneLine();
            line2 = line2.ToPlaneLine();

            if (Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI) < tolerance)
                return true;

            return line1.Direction.AngleTo(line2.Direction) % Math.PI < tolerance;
        }

        /// <summary>
        /// Gets the result of whether the line line1 and the line line2 is plane vertical.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsPlaneVertical(this Line line1, Line line2, double tolerance = 2 * 10e-3)
        {
            if (line1 == null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 == null)
                throw new ArgumentNullException(nameof(line2));

            line1 = line1.ToPlaneLine();
            line2 = line2.ToPlaneLine();

            return Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI / 2) < tolerance;
        }

        /// <summary>
        /// Gets the intersection of the line line1 and the line line2 on plane.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="isTouch"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static XYZ GetPlaneCrossingPoint(this Line line1, Line line2, bool isTouch = true, double tolerance = 2 * 10e-3)
        {
            if (line1 == null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 == null)
                throw new ArgumentNullException(nameof(line2));

            if (line1.IsPlaneParallel(line2))
                return null;

            XYZ result;
            var pt1 = line1.GetEndPoint(0);
            var pt2 = line1.GetEndPoint(1);
            var pt3 = line2.GetEndPoint(0);
            var pt4 = line2.GetEndPoint(1);
            var x1 = pt1.X;
            var y1 = pt1.Y;
            var x2 = pt2.X;
            var y2 = pt2.Y;
            var x3 = pt3.X;
            var y3 = pt3.Y;
            var x4 = pt4.X;
            var y4 = pt4.Y;
            var f1 = Math.Abs(line1.Direction.AngleTo(XYZ.BasisX) - Math.PI / 2) < tolerance;
            var f2 = Math.Abs(line2.Direction.AngleTo(XYZ.BasisX) - Math.PI / 2) < tolerance;

            // Must quadrature.
            if (line1.IsPlaneVertical(line2) && f1 || f2)
                result = f1 ? new XYZ(x1, y3, pt1.Z) : new XYZ(x3, y1, pt1.Z);
            else
            {
                var dx12 = x2 - x1;
                var dx31 = x3 - x1;
                var dx34 = x3 - x4;
                var dy21 = y2 - y1;
                var dy31 = y3 - y1;
                var dy34 = y3 - y4;
                var k1 = dx12 * dx34 * dy31 - x3 * dx12 * dy34 + x1 * dy21 * dx34;
                var k2 = dy21 * dx34 - dx12 * dy34;
                var k3 = dy21 * dy34 * dx31 - y3 * dy21 * dx34 + y1 * dx12 * dy34;
                var k4 = dx12 * dy34 - dy21 * dx34;

                // Equations of the state, by the formula to calculate the intersection.
                result = new XYZ(k1 / k2, k3 / k4, pt1.Z);
            }

            // It be used to calc the result in line1 and line2.
            var flag1 = (result.Y - x1) * (result.X - x2) <= 0;
            var flag2 = (result.Y - y1) * (result.Y - y2) <= 0;
            var flag3 = (result.X - x3) * (result.X - x4) <= 0;
            var flag4 = (result.Y - y3) * (result.Y - y4) <= 0;

            // No touch or true cross returns the intersection pt, otherwise returns null.
            return !isTouch || flag1 && flag2 && flag3 && flag4 ? result : null;
        }

        /// <summary>
        /// Gets the intersections of the line and the lines on plane.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lines"></param>
        /// <param name="isTouch"></param>
        /// <returns></returns>
        public static List<XYZ> GetPlaneCrossingPointList(this Line line, List<Line> lines, bool isTouch = true)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var results = new List<XYZ>();

            lines.ForEach(f => results.Add(line.GetPlaneCrossingPoint(f, isTouch)));

            return results.Where(w => w != null).ToList();
        }

        /// <summary>
        /// Gets the distinct vectors of the lines.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<XYZ> GetDistinctPointList(this List<Line> lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var results = new List<XYZ>();

            foreach (var line in lines)
            {
                results.Add(line.GetEndPoint(0));
                results.Add(line.GetEndPoint(1));
            }

            for (var i = 0; i < results.Count; i++)
            {
                if (results[i] == null)
                    continue;

                for (var j = i + 1; j < results.Count; j++)
                {
                    if (results[j] == null)
                        continue;

                    if (results[i].IsAlmostEqualTo(results[j]))
                        results[j] = null;
                }
            }

            return results.Where(w => w != null).ToList();
        }

        /// <summary>
        /// Gets points of the boundary.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<XYZ> GetBoundaryPointList(this List<Line> lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var results = new List<XYZ>();

            foreach (var line in lines)
                results.Add(line.GetEndPoint(0));

            var endPoint = lines[lines.Count - 1].GetEndPoint(1);

            // If no closed, the last line's end point is different from the first line's start point.
            if (!lines[0].GetEndPoint(0).IsAlmostEqualTo(endPoint))
                results.Add(endPoint);

            return results;
        }

        /// <summary>
        /// Gets the max point of the line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static XYZ GetMaxPoint(this Line line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            var pt1 = line.GetEndPoint(0);
            var pt2 = line.GetEndPoint(1);

            return new XYZ(Math.Max(pt1.X, pt2.X), Math.Max(pt1.Y, pt2.Y), Math.Max(pt1.Z, pt2.Z));
        }

        /// <summary>
        /// Gets the min point of the lines.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static XYZ GetMaxPoint(this List<Line> lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var pts = new List<XYZ>();

            foreach (var line in lines)
            {
                var pt1 = line.GetEndPoint(0);
                var pt2 = line.GetEndPoint(1);

                pts.Add(pt1);
                pts.Add(pt2);
            }

            var x = pts.Select(s => s.X).Max();
            var y = pts.Select(s => s.Y).Max();
            var z = pts.Select(s => s.Z).Max();

            return new XYZ(x, y, z);
        }

        /// <summary>
        /// Gets the min point of the line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static XYZ GetMinPoint(this Line line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            var pt1 = line.GetEndPoint(0);
            var pt2 = line.GetEndPoint(1);

            return new XYZ(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y), Math.Min(pt1.Z, pt2.Z));
        }

        /// <summary>
        /// Gets the min point of the lines.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static XYZ GetMinPoint(this List<Line> lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var pts = new List<XYZ>();

            foreach (var line in lines)
            {
                var pt1 = line.GetEndPoint(0);
                var pt2 = line.GetEndPoint(1);

                pts.Add(pt1);
                pts.Add(pt2);
            }

            return pts.GetMinPoint();
        }

        /// <summary>
        /// Gets the middle point of the line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static XYZ GetMidPoint(this Line line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            return (line.GetEndPoint(0) + line.GetEndPoint(1)) * 0.5;
        }

        /// <summary>
        /// Gets the middle point of the two lines in nearest area.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static XYZ GetMidPoint(this Line line1, Line line2)
        {
            if (line1 == null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 == null)
                throw new ArgumentNullException(nameof(line2));

            var pt1 = line1.GetEndPoint(0);
            var pt2 = line1.GetEndPoint(1);
            var pt3 = line2.GetEndPoint(0);
            var pt4 = line2.GetEndPoint(1);
            var distance = double.MaxValue;
            XYZ result = null;

            if (pt1.DistanceTo(pt3) < distance)
            {
                distance = pt1.DistanceTo(pt3);
                result = (pt1 + pt3) * 0.5;
            }

            if (pt1.DistanceTo(pt4) < distance)
            {
                distance = pt1.DistanceTo(pt4);
                result = (pt1 + pt4) * 0.5;
            }

            if (pt2.DistanceTo(pt3) < distance)
            {
                distance = pt2.DistanceTo(pt3);
                result = (pt2 + pt3) * 0.5;
            }

            if (pt2.DistanceTo(pt4) < distance)
                result = (pt2 + pt4) * 0.5;

            return result;
        }
    }
}