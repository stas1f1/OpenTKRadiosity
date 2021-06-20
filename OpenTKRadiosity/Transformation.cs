using System;
using System.Collections.Generic;
using System.Text;
using static OpenTKRadiosity.Objects;

namespace OpenTKRadiosity
{
    public static class Transformation
    {

        public static double scaleFactorX = 1;
        public static double scaleFactorY = 1;
        public static double scaleFactorZ = 1;

        public static double rotateAngleX = 0;
        public static double rotateAngleY = 0;
        public static double rotateAngleZ = 0;

        public static int translateX = 0;
        public static int translateY = 0;
        public static int translateZ = 0;

        public static double[,] MoveMatrix;
        public static double[,] RotateMatrix;
        public static double[,] ScaleMatrix;

        public static double[,] InitMatrix;
        public static double[,] CurMatrix;

        public static double[,] Rotate(double angle, char axis)
        {
            if (axis == 'x')
                return new double[4, 4]
                {   { 1, 0, 0, 0 },
                    { 0, Math.Cos(angle), -Math.Sin(angle), 0},
                    {0, Math.Sin(angle), Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 } };
            if (axis == 'y')
                return new double[4, 4]
                {   { Math.Cos(angle), 0, Math.Sin(angle), 0},
                    { 0, 1, 0, 0 },
                    {-Math.Sin(angle), 0, Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 } };
            if (axis == 'z')
                return new double[4, 4]
                {   { Math.Cos(angle), -Math.Sin(angle), 0, 0},
                    { Math.Sin(angle), Math.Cos(angle), 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 } };
            return new double[4, 4];
        }

        public static double[,] Move(double dx, double dy, double dz)
        {
            return new double[4, 4]
                {{ 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { dx, dy, dz, 1} };
        }

        public static double[,] Scale(double s1, double s2, double s3)
        {
            return new double[4, 4]
                {{ s1, 0, 0, 0 },
                { 0, s2, 0, 0 },
                { 0, 0, s3, 0 },
                { 0, 0, 0, 1} };
        }

        public static void ResetTransformations()
        {
            scaleFactorX = 1;
            scaleFactorY = 1;
            scaleFactorZ = 1;

            rotateAngleX = 0;
            rotateAngleY = 0;
            rotateAngleZ = 0;

            translateX = 0;
            translateY = 0;
            translateZ = 0;

            MoveMatrix = Move(0, 0, 0);
            RotateMatrix = Rotate(0, 'x');
            ScaleMatrix = Scale(1, 1, 1);

            InitMatrix = Move(0, 0, 0);
            CurMatrix = Move(0, 0, 0);
        }

        public static int RowCount(double[,] matrix) => matrix.GetUpperBound(0) + 1;
        public static int ColCount(double[,] matrix) => matrix.GetUpperBound(1) + 1;

        public static double[,] MultMatrix(double[,] m1, double[,] m2)
        {
            double[,] m = new double[RowCount(m1), ColCount(m2)];

            for (int k = 0; k < RowCount(m1); k++)
                for (int i = 0; i < RowCount(m2); i++)
                {
                    double t = 0;
                    for (int j = 0; j < ColCount(m1); j++)
                        t += m1[k, j] * m2[j, i];
                    m[k, i] = t;
                }
            return m;
        }

        public static Point3D TransformPoint(Point3D p, double[,] m2)
        {
            double[,] m1 = new double[1, 4] { { p.X, p.Y, p.Z, 1 } };
            double[,] m = MultMatrix(m1, m2);
            Point3D newp = new Point3D((float)m[0, 0], (float)m[0, 1], (float)m[0, 2]);
            return newp;
        }

        public static void ApplyTransform(ref Mesh mes, double[,] m2, bool rotation = false)
        {
            foreach (Point3D p in mes.points) p.ApplyMatrix(m2);
            foreach (Edge e in mes.edges)
            {
                e.p1.ApplyMatrix(m2);
                e.p2.ApplyMatrix(m2);
            }
            foreach (Polygon p in mes.faces)
            {
                foreach (Point3D p3d in p.points) p3d.ApplyMatrix(m2);
                if (rotation) p.normal.ApplyMatrix(m2);
            }
        }
    }
}
