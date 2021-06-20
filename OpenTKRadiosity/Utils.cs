using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using static OpenTKRadiosity.Transformation;
using static OpenTKRadiosity.Objects;
using OpenTK;
using OpenTK.Graphics;
using System.Diagnostics;
using System.IO;

namespace OpenTKRadiosity
{
    public static class Utils
    {
        //Scene objects storage
        public static List<Mesh> meshes;
        public static List<Sphere> spheres;
        public static List<Mesh> lights;
        public static int subdivisions = 1;


        /// <summary>
        /// Get normal vectors for mesh object
        /// </summary>
        /// <param name="m"></param>
        /// <param name="clockwise"></param>
        /// <returns></returns>
        public static List<Point3D> GetVertexNormals(Mesh m, bool clockwise)
        {
            List<Point3D> ans = new List<Point3D>();
            foreach (Point3D point in m.points)
            {
                List<Point3D> faceNormals = new List<Point3D>();
                foreach (Polygon face in m.faces)
                {
                    if (face.points.Where((x) => x.index == point.index).Count() > 0) continue;

                    Point3D fnorm = GetNormal(face, clockwise);
                    faceNormals.Add(fnorm);
                }
                if (faceNormals.Count > 0)
                {
                    Point3D norm = new Point3D(0, 0, 0, point.index);
                    foreach (Point3D p in faceNormals)
                    {
                        norm.X += p.X;
                        norm.Y += p.Y;
                        norm.Z += p.Z;
                    }
                    double len = Distance(new Point3D(0, 0, 0), norm);
                    norm.X /= len;
                    norm.Y /= len;
                    norm.Z /= len;
                    ans.Add(norm);
                }
            }
            return ans;
        }

        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public static double Distance(PointF p1, PointF p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public static double Distance(Point3D p1, Point3D p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }
        public static double Distance(Polygon pol, Point3D p)
        {
            Point3D p1 = pol.points[0];
            Point3D p2 = pol.points[1];
            Point3D p3 = pol.points[2];

            double x1 = p1.X, y1 = p1.Y, z1 = p1.Z;
            double x2 = p2.X, y2 = p2.Y, z2 = p2.Z;
            double x3 = p3.X, y3 = p3.Y, z3 = p3.Z;

            double a = (y2 - y1) * (z3 - z1) - (z2 - z1) * (y3 - y1);
            double b = (z2 - z1) * (x3 - x1) - (x2 - x1) * (z3 - z1);
            double c = (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);
            double d = (-1) * (a * x1 + b * y1 + c * z1);

            return Math.Abs(a * p.X + b * p.Y + c * p.Z + d) / Math.Sqrt(a * a + b * b + c * c);
        }

        /// <summary>
        /// Dot product for two vectors
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double VectorDotProduct(Point3D p1, Point3D p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }

        /// <summary>
        /// Normalize vectors
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point3D Normalize(Point3D p)
        {
            double d = Distance(new Point3D(0, 0, 0), p);
            return new Point3D(p.X / d, p.Y / d, p.Z / d);
        }

        /// <summary>
        /// Angle between two vectors
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="inRadians">true => radians, false => degrees</param>
        /// <returns></returns>
        public static double AngleBetween(Point3D p1, Point3D p2, bool inRadians = false)
        {
            double a = Math.Sqrt(p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z) * Math.Sqrt(p2.X * p2.X + p2.Y * p2.Y + p2.Z * p2.Z);
            double b = p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
            double c = b / a;



            if (!inRadians)
                return Math.Acos(c) * 180 / Math.PI;
            else
                return Math.Acos(c);
        }

        /// <summary>
        /// Cos of angle between two vectors
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double CosBetween(Point3D p1, Point3D p2)
        {
            double p1X = p1.X;
            double p1Y = p1.Y;
            double p1Z = p1.Z;
            double p2X = p2.X;
            double p2Y = p2.Y;
            double p2Z = p2.Z;

            if (Math.Abs(p1.X) < 0.0001) p1X = 0;
            if (Math.Abs(p1.Y) < 0.0001) p1Y = 0;
            if (Math.Abs(p1.Z) < 0.0001) p1Z = 0;
            if (Math.Abs(p2.X) < 0.0001) p2X = 0;
            if (Math.Abs(p2.Y) < 0.0001) p2Y = 0;
            if (Math.Abs(p2.Z) < 0.0001) p2Z = 0;

            return (p1X * p2X + p1Y * p2Y + p1Z * p2Z) / (p1.Length() * p2.Length());

            //return (p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z) / (p1.Length() * p2.Length());
        }

        /// <summary>
        /// Reflect vector using normal of surface
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static Point3D RayReflect(Point3D vec, Point3D normal)
        {
            double dot1 = VectorDotProduct(vec, normal);
            return new Point3D(vec.X - normal.X * 2.0 * dot1, vec.Y - normal.Y * 2.0 * dot1, vec.Z - normal.Z * 2.0 * dot1);
        }

        /// <summary>
        /// Refract vector using surface normal and refraction index
        /// </summary>
        /// <param name="rayV"></param>
        /// <param name="normal"></param>
        /// <param name="refractionIndex"></param>
        /// <returns></returns>
        public static Point3D RayRefract(Point3D rayV, Point3D normal, double refractionIndex)
        {

            double oldInd = 1;
            double newInd = refractionIndex;

            double dp = -1 * (rayV * normal);

            Point3D n = normal;

            //normal vector facing wrong dir
            if (dp < 0)
            {

                n *= -1;
                dp *= -1;

                double t = oldInd;
                oldInd = newInd;
                newInd = t;
            }
            double eta = oldInd / newInd;

            double k = 1 - (1 - dp * dp) * eta * eta;

            //Does reflect?
            if (k < 0)
                return new Point3D(0, 0, 0);
            else
                return rayV * eta + n * (eta * dp - Math.Sqrt(k));
        }

        /// <summary>
        /// Get polygon normal directed outwards
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="clockwise">false to direct inwards</param>
        /// <returns></returns>
        public static Point3D GetNormal(Polygon polygon, bool clockwise)
        {
            Point3D normalVec = new Point3D();
            Point3D vec1 = new Point3D(polygon.points[0].X - polygon.points[1].X, polygon.points[0].Y - polygon.points[1].Y, polygon.points[0].Z - polygon.points[1].Z);
            Point3D vec2 = new Point3D(polygon.points[2].X - polygon.points[1].X, polygon.points[2].Y - polygon.points[1].Y, polygon.points[2].Z - polygon.points[1].Z);

            if (clockwise)
            {
                normalVec = new Point3D(vec1.Z * vec2.Y - vec1.Y * vec2.Z, vec1.X * vec2.Z - vec1.Z * vec2.X, vec1.Y * vec2.X - vec1.X * vec2.Y);
            }
            else
            {
                normalVec = new Point3D(vec1.Y * vec2.Z - vec1.Z * vec2.Y, vec1.Z * vec2.X - vec1.X * vec2.Z, vec1.X * vec2.Y - vec1.Y * vec2.X);
            }

            double dist = Distance(new Point3D(0, 0, 0), normalVec);
            return new Point3D(normalVec.X / dist, normalVec.Y / dist, normalVec.Z / dist);
        }

        public static double CalcAngleSum(Point3D q, Polygon p)
        {
            int n = p.points.Count;

            double m1, m2;

            double anglesum = 0, costheta = 0;
            double epsilon = 0.0000001;
            double twopi = Math.PI * 2;
            Point3D p1 = new Point3D();
            Point3D p2 = new Point3D();

            for (int i = 0; i < n; i++)
            {
                p1.X = p.points[i].X - q.X;
                p1.Y = p.points[i].Y - q.Y;
                p1.Z = p.points[i].Z - q.Z;

                p2.X = p.points[(i + 1) % n].X - q.X;
                p2.Y = p.points[(i + 1) % n].Y - q.Y;
                p2.Z = p.points[(i + 1) % n].Z - q.Z;

                m1 = Math.Sqrt(p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z);
                m2 = Math.Sqrt(p2.X * p2.X + p2.Y * p2.Y + p2.Z * p2.Z);
                if (m1 * m2 <= epsilon)
                    return (twopi);
                else
                    costheta = (p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z) / (m1 * m2);

                anglesum += Math.Acos(costheta);
            }
            return (anglesum);
        }

        /// <summary>
        /// Find intersection between ray and all of the scene objects
        /// </summary>
        /// <param name="origin">origin point</param>
        /// <param name="dir">directional vector</param>
        /// <param name="intersectionPoint">intersection point</param>
        /// <param name="normal">plane normal</param>
        /// <param name="material"></param>
        /// <returns></returns>
        public static bool FindIntersection(Point3D origin, Point3D dir, ref Point3D intersectionPoint,
            ref Point3D normal, ref Material material)
        {
            double dist = float.MaxValue;
            foreach (Sphere s in spheres)
            {
                double dist_i = 0;
                if (s.ray_intersection(origin, dir, ref dist_i) && dist_i < dist)
                {
                    dist = dist_i;
                    intersectionPoint = origin + dir * dist_i;
                    normal = Normalize(intersectionPoint - s.pos);
                    material = s.mat;
                }
            }
            foreach (Mesh msh in meshes)
            {
                foreach (Polygon pol in msh.faces)
                {
                    Point3D p = new Point3D();
                    double dist_i = 0;
                    if (pol.RayIntersection(origin, dir, ref p, ref dist_i) && dist_i < dist)
                    {
                        dist = dist_i;
                        intersectionPoint = origin + dir * dist_i;
                        normal = pol.normal;
                        material = msh.material;
                    }
                }
            }
            return dist < 1000000;
        }

        public static double LightFalloff(double dist, double maxDist)
        {
            double d = 1 - Math.Min(1, dist / maxDist);
            return d;
        }

        public static double[,] RotateAroundLineMatrix(Point3D p1, Point3D p2, double angle)
        {
            double l = (p2.X - p1.X) / Distance(p1, p2);
            double m = (p2.Y - p1.Y) / Distance(p1, p2);
            double n = (p2.Z - p1.Z) / Distance(p1, p2);

            return new double[4, 4]
                {{ l*l + Math.Cos(angle)*(1 - l*l), l*(1-Math.Cos(angle))*m + n*Math.Sin(angle), l*(1 - Math.Cos(angle))*n - m*Math.Sin(angle), 0 },
                { l*(1 - Math.Cos(angle))*m - n*Math.Sin(angle), m*m + Math.Cos(angle)*(1 - m*m), m*(1 - Math.Cos(angle))*n + l*Math.Sin(angle), 0 },
                { l*(1 - Math.Cos(angle))*n + m*Math.Sin(angle), m*(1 - Math.Cos(angle))*n - l*Math.Sin(angle), n*n + Math.Cos(angle)*(1 - n*n), 0 },
                { 0, 0, 0, 1}
                };
        }

        public static string VectorToString(Point3D p)
        {
            return "x: " + p.X + ", y: " + p.Y + ", z: " + p.Z;
        }

        /// <summary>
        /// Generate scattered diffuse rays
        /// </summary>
        /// <param name="p"></param>
        /// <param name="Norm"></param>
        /// <param name="ringsCount"></param>
        /// <param name="ParalCount"></param>
        /// <returns></returns>
        public static List<Point3D> MakeDiffuseRays(Point3D p, Point3D Norm, int ringsCount, int ParalCount)
        {
            List<Point3D> rays = new List<Point3D>();
            rays.Add(Norm);

            double ringAngle = 90 / (ringsCount + 1);
            double ParalAngle = 360 / ParalCount;
            Point3D curRot = new Point3D(Norm);
            for (int i = 0; i < ringsCount; i++)
            {
                curRot = TransformPoint(curRot, Rotate(ringAngle, 'x'));
                for (int j = 0; j < ParalCount; j++)
                {
                    rays.Add(curRot);
                    curRot = TransformPoint(curRot, RotateAroundLineMatrix(p, p + Norm, ParalAngle));
                }
            }
            return rays;
        }

        /// <summary>
        /// Convert Color to Color4
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color4 ColorToColor4(Color color)
        {
            return new Color4(
                color.R / 255.0f,
                color.G / 255.0f,
                color.B / 255.0f,
                255
                );
        }

        /// <summary>
        /// Convert Vector to Color4
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color4 VectorToColor4(Point3D color)
        {
            return new Color4(
                (float)color.X / 255.0f,
                (float)color.Y / 255.0f,
                (float)color.Z / 255.0f,
                255
                );
        }

        public static Color4 Color4Avg(Color4 c1, Color4 c2)
        {
            return new Color4(
                (c1.R + c2.R) / 2,
                (c1.G + c2.G) / 2,
                (c1.B + c2.B) / 2,
                255
                );
        }

        public static Color4 Color4Avg(Color4 c1, Color4 c2, Color4 c3)
        {
            return new Color4(
                (c1.R + c2.R + c3.R) / 3,
                (c1.G + c2.G + c3.G) / 3,
                (c1.B + c2.B + c3.B) / 3,
                255
                );
        }

        /// <summary>
        /// (TESTING) Convert Color to Color4 with slight randomization 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color4 ColorToColor4Randomized(Color color)
        {
            var rand = new Random();

            int delta = rand.Next(101) - 50;

            int R = color.R + delta - 100;
            int G = color.G + delta - 100;
            int B = color.B + delta - 100;

            if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;
            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;

            return new Color4(
                R / 255.0f,
                G / 255.0f,
                B / 255.0f,
                255
                );
        }

        /// <summary>
        /// Subdivide meshes
        /// </summary>
        /// <param name="i"></param>
        public static void SubdivideMeshes(int iterations)
        {
            for (int i = 0; i < meshes.Count(); i++)
            {
                if (meshes[i].hardcodeSubdivisions == 0)
                    meshes[i].Subdivide(iterations);
                else
                    meshes[i].Subdivide(meshes[i].hardcodeSubdivisions);
            }
        }

        /// <summary>
        /// Subdivide lightsource meshes
        /// </summary>
        /// <param name="i"></param>
        public static void SubdivideLights(int iterations)
        {
            lights.ForEach(x => x.Subdivide(iterations));
            lights.ForEach(x => x.MakeLight(x.material.color));
        }

        /// <summary>
        /// Subdivide meshes evenly, smallest polygons are granted the selected level of detail
        /// </summary>
        /// <param name="i"></param>
        public static void SubdivideMeshesEvenly(int iterations)
        {
            double min_sub_area = meshes
                .Where(x => x.hardcodeSubdivisions == 0)
                .Select(x => x.faces[0].Area()).Min() / Math.Pow(2, iterations);

            meshes.ForEach(x => Console.WriteLine("Level of detail: " +
                (int)Math.Round(Math.Log2(x.faces[0].Area() / min_sub_area))));

            for (int i = 0; i < meshes.Count(); i++)
            {
                if (meshes[i].hardcodeSubdivisions == 0)
                    meshes[i].Subdivide((int)Math.Round(Math.Log2(meshes[i].faces[0].Area() / min_sub_area)));
                else
                    meshes[i].Subdivide(meshes[i].hardcodeSubdivisions);
            }
        }

        /// <summary>
        /// Subdivide meshes evenly, smallest polygons are granted the selected level of detail
        /// </summary>
        /// <param name="i"></param>
        public static void SubdivideMeshesEvenlyByMax(int iterations)
        {
            double max_sub_area = meshes
                .Where(x => x.hardcodeSubdivisions == 0)
                .Select(x => x.faces[0].Area()).Max() / Math.Pow(2, iterations);

            meshes.ForEach(x => Console.WriteLine("Level of detail: " +
                (int)Math.Round(Math.Log2(x.faces[0].Area() / max_sub_area))));

            for (int i = 0; i < meshes.Count(); i++)
            {
                if (meshes[i].hardcodeSubdivisions == 0)
                    meshes[i].Subdivide((int)Math.Round(Math.Log2(meshes[i].faces[0].Area() / max_sub_area)));
                else
                    meshes[i].Subdivide(meshes[i].hardcodeSubdivisions);
            }
        }


        /// <summary>
        /// TODO: Subdivide lightsource meshes evenly
        /// </summary>
        /// <param name="i"></param>
        public static void SubdivideLightsEvenly(int iterations)
        {
            lights.ForEach(x => x.Subdivide(iterations));
            lights.ForEach(x => x.MakeLight(x.material.color));
        }

        public static Tuple<int, int> OrderedTuple(int a, int b) => (a > b) ? new Tuple<int, int>(b, a) : new Tuple<int, int>(a, b);

        public static Point3D MultiplyIllumination(Color c, double d)
        {
            return new Point3D(c.R * d, c.G * d, c.B * d);
        }

        public static Color AddToRadiance(Color radiance, Color illumination)
        {
            return Color.FromArgb(
                (radiance.R + illumination.R < 255) ? radiance.R + illumination.R : 255,
                (radiance.G + illumination.G < 255) ? radiance.G + illumination.G : 255,
                (radiance.B + illumination.B < 255) ? radiance.B + illumination.B : 255);
        }

        public static Color AvgToRadiance(Color radiance, Color illumination)
        {
            return Color.FromArgb(
                ((radiance.R + illumination.R) / 2 < 255) ? (radiance.R + illumination.R)/2 : 255,
                ((radiance.G + illumination.G) / 2 < 255) ? (radiance.G + illumination.G)/2 : 255,
                ((radiance.B + illumination.B) / 2 < 255) ? (radiance.B + illumination.B)/2 : 255);
        }

        public static Color VectorToColor(Point3D i)
        {
            return Color.FromArgb(
                (int)((i.X < 255) ? i.X : 255),
                (int)((i.Y < 255) ? i.Y : 255),
                (int)((i.Z < 255) ? i.Z : 255));
        }

        public static Point3D ColorToVector(Color c)
        {
            return new Point3D(c.R, c.G, c.B);
        }

        public static Point3D MultiplyVectors(Point3D c1, Point3D c2)
        {
            return new Point3D(c1.X * c1.X, c1.Y * c2.Y, c1.Z * c2.Z);
        }

        public static Color MultiplyColor(Color c1, Color c2)
        {
            return Color.FromArgb(
                c1.R * c2.R / 255,
                c1.G * c2.G / 255,
                c1.B * c2.B / 255);
        }

        public static Color MultiplyColor(Color c, double d)
        {
            return Color.FromArgb(
                (int)(c.R * d),
                (int)(c.G * d),
                (int)(c.B * d));
        }

        public static Point3D NormalizedVector(Point3D v)
        {
            double len = v.Length();
            return new Point3D(v.X / len, v.Y / len, v.Z / len);
        }

        /// <summary>
        /// Run Radiosity algorithm
        /// </summary>
        /// <param name="iterations"></param>
        public static void RunRadiosity(int iterations, double reflectivity, double lightIntensity, double treshold = 10.0, bool colorBleeding = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            int totalFaces = 0;

            for (int i = 0; i < meshes.Count(); i++)
                for (int j = 0; j < meshes[i].subdividedFaces.Count(); j++)
                {
                    totalFaces++;
                }

            if (iterations > 0)
            {
                //Inital light spreading
                foreach (Mesh light in lights)
                {
                    foreach (Polygon lightPoly in light.subdividedFaces)
                    {
                        for (int i = 0; i < meshes.Count(); i++)
                        {
                            for (int j = 0; j < meshes[i].subdividedFaces.Count(); j++)
                            {
                                Point3D pos = lightPoly.centerPoint;
                                Point3D dir = meshes[i].subdividedFaces[j].centerPoint - pos;
                                double len = dir.Length();

                                //Mutual visibility check
                                double cosphii = CosBetween(lightPoly.normal, dir);
                                double cosphij = -CosBetween(meshes[i].subdividedFaces[j].normal, dir);

                                if (cosphii >= 0 && cosphij >= 0)
                                {
                                    //Obstruction check
                                    double t = 0;
                                    Point3D P = new Point3D();
                                    bool visible = true;

                                    for (int k = 0; k < meshes.Count(); k++)
                                    {
                                        if (k != i)
                                        {
                                            for (int l = 0; l < meshes[k].faces.Count(); l++)
                                            {
                                                bool hit = meshes[k].faces[l].RayIntersection(pos, dir, ref P, ref t);
                                                if (hit)
                                                {
                                                    if ((P - pos).Length() < len)
                                                    {
                                                        visible = false;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!visible) break;
                                        }
                                    }


                                    //If totally visible
                                    if (visible)
                                    {
                                        double coef = 1 / (Math.PI * len * len);
                                        if (coef > 1) coef = 1;
                                        meshes[i].subdividedFaces[j].illumination += ColorToVector(light.material.color) * (lightIntensity * coef / 255.0);
                                    }
                                }
                            }
                        }
                    }
                }

                stopwatch.Stop();
                TimeSpan timeInit = stopwatch.Elapsed, timeff = stopwatch.Elapsed, timepass = stopwatch.Elapsed;

                stopwatch.Reset();
                stopwatch.Start();

                Console.WriteLine("Radiosity iteration 0 done");

                //bool[] faceDone = new bool[totalFaces];

                //Calculating formfactors and diffuse reflection
                if (iterations > 1)
                {

                    //Calculating formfactors
                    double[,] formfactors = new double[totalFaces, totalFaces];
                    Console.WriteLine(formfactors[0, 0]);
                    int ffi = 0, ffj;
                    for (int si = 0; si < meshes.Count(); si++)
                    {
                        for (int sj = 0; sj < meshes[si].subdividedFaces.Count(); sj++)
                        {
                            ffj = 0;
                            for (int i = 0; i < meshes.Count(); i++)
                            {
                                if (i != si)
                                {
                                    for (int j = 0; j < meshes[i].subdividedFaces.Count(); j++)
                                    {
                                        Point3D pos = meshes[si].subdividedFaces[sj].centerPoint;
                                        Point3D dir = meshes[i].subdividedFaces[j].centerPoint - pos;
                                        double len = dir.Length();

                                        //Mutual visibility check
                                        double cosphii = CosBetween(meshes[si].subdividedFaces[sj].normal, dir);
                                        double cosphij = -CosBetween(meshes[i].subdividedFaces[j].normal, dir);

                                        if (cosphii > 0 && cosphij > 0)
                                        {
                                            //Obstruction check
                                            double t = 0;
                                            Point3D P = new Point3D();
                                            bool visible = true;

                                            for (int k = 0; k < meshes.Count(); k++)
                                            {
                                                if (k != i && k != si)
                                                {
                                                    for (int l = 0; l < meshes[k].faces.Count(); l++)
                                                    {
                                                        bool hit = meshes[k].faces[l].RayIntersection(pos, dir, ref P, ref t);
                                                        if (hit)
                                                        {
                                                            if ((P - pos).Length() < len)
                                                            {
                                                                visible = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (!visible) break;
                                                }
                                            }


                                            //If totally visible
                                            if (visible)
                                            formfactors[ffi, ffj] =
                                                cosphii * cosphij * meshes[i].subdividedFaces[j].Area() / (Math.PI * len * len);
                                        }
                                        ffj++;
                                    }
                                }
                                else
                                    ffj += meshes[i].subdividedFaces.Count();
                            }

                            ffi++;
                        }

                    }
                    Console.WriteLine("Form factors calculated");

                    stopwatch.Stop();
                    timeff = stopwatch.Elapsed;

                    stopwatch.Reset();
                    stopwatch.Start();

                    //Radiosity passes
                    for (int iter = 1; iter < iterations; iter++)
                    {
                        ffi = 0;

                        for (int si = 0; si < meshes.Count(); si++)
                        {
                            for (int sj = 0; sj < meshes[si].subdividedFaces.Count(); sj++)
                            {
                                ffj = 0;

                                for (int i = 0; i < meshes.Count(); i++)
                                {
                                    if (i != si)
                                    {
                                        for (int j = 0; j < meshes[i].subdividedFaces.Count(); j++)
                                        {
                                            double coef = formfactors[ffi, ffj] * reflectivity;
                                            Point3D energyTransfer = (colorBleeding) ?
                                                MultiplyVectors(meshes[i].subdividedFaces[j].illumination * coef, ColorToVector(meshes[i].material.color))
                                                :meshes[i].subdividedFaces[j].illumination * coef;
                                            meshes[si].subdividedFaces[sj].illumination += energyTransfer;
                                            meshes[i].subdividedFaces[j].illumination -= energyTransfer;
                                            ffj++;
                                        }
                                    }
                                    else
                                        ffj += meshes[i].subdividedFaces.Count();
                                }

                                ffi++;
                            }
                        }

                        
                        Console.WriteLine("Radiosity iteration " + iter + " done");
                    }

                    stopwatch.Stop();
                    timepass = stopwatch.Elapsed / iterations;

                }

                string wr = "Initial: " + timeInit.ToString() + ", FormFactors: " + timeff.ToString() + ", One pass: " + timepass.ToString();

                File.WriteAllText("time.txt", wr);

                //Final light application
                for (int i = 0; i < meshes.Count(); i++)
                    for (int j = 0; j < meshes[i].subdividedFaces.Count(); j++)
                    {
                        meshes[i].subdividedFaces[j].radiance = MultiplyColor(
                            VectorToColor(meshes[i].subdividedFaces[j].illumination * 255.0),
                            meshes[i].material.color);
                    }


            }



            Console.WriteLine("Radiosity done");
        }



        /// <summary>
        /// Test direct visibility form light
        /// </summary>
        public static void VisibilityCheck()
        {

            foreach (Mesh light in lights)
            {
                foreach (Polygon lightPoly in light.subdividedFaces)
                {
                    for (int i = 0; i < meshes.Count(); i++)
                    {
                        for (int j = 0; j < meshes[i].subdividedFaces.Count(); j++)
                        {
                            //TODO: CHECK FOR VISIBILITY

                            Point3D centerRay = meshes[i].subdividedFaces[j].centerPoint - lightPoly.centerPoint;
                            Point3D centerRayRev = lightPoly.centerPoint - meshes[i].subdividedFaces[j].centerPoint ;
                            double r = centerRay.Length();

                            double cosphii = Math.Cos(AngleBetween(lightPoly.normal, centerRay, true));
                            double cosphij = Math.Cos(AngleBetween(meshes[i].subdividedFaces[j].normal, centerRayRev, true));

                            if (cosphii * cosphij > 0)
                            {
                                meshes[i].subdividedFaces[j].radiance = Color.Green;

                            }
                            else
                            {
                                meshes[i].subdividedFaces[j].radiance = Color.Red;
                                var norm = meshes[i].subdividedFaces[j].normal;

                                if (Math.Abs(lightPoly.normal.X - norm.X) <0.001)
                                {
                                    Console.WriteLine("Cos PhiI: " + cosphii + "\n" + "Cos PhiJ: " + cosphij + "\n");

                                    Console.WriteLine("Light Norm: " + "\n" + lightPoly.normal.X + "\n" + lightPoly.normal.Y + "\n" + lightPoly.normal.Z + "\n");
                                    Console.WriteLine("Poly Norm: " + "\n" + norm.X + "\n" + norm.Y + "\n" + norm.Z + "\n");
                                }
                            }

                        }
                    }


                }


            }
            Console.WriteLine("Visibility check done");
        }

        /// <summary>
        /// Test normal directions
        /// </summary>
        public static void NormalCheck()
        {
            for (int i = 0; i < lights.Count(); i++)
            {
                for (int j = 0; j < lights[i].subdividedFaces.Count(); j++)
                {
                    var norm = lights[i].subdividedFaces[j].normal;
                    var cp = lights[i].subdividedFaces[j].centerPoint;

                   // Console.WriteLine("Light Norm: " + "\n" + norm.X + "\n" + norm.Y + "\n" + norm.Z + "\n" + 
                    //    "Light CP: " + "\n" + cp.X + "\n" + cp.Y + "\n" + cp.Z + "\n");

                    double d = -norm.X * cp.X - norm.Y * cp.Y - norm.Z * cp.Z;

                    //Console.WriteLine("Light D: " + d);

                    if (d>0) lights[i].subdividedFaces[j].radiance = Color.Green;
                    else lights[i].subdividedFaces[j].radiance = Color.Red;
                }
            }


            for (int i = 0; i < meshes.Count(); i++)
            {
                for (int j = 0; j < meshes[i].subdividedFaces.Count(); j++)
                {
                    var norm = meshes[i].subdividedFaces[j].normal;
                    var cp = meshes[i].subdividedFaces[j].centerPoint;

                    //Console.WriteLine("Face Norm: " + "\n" + norm.X + "\n" + norm.Y + "\n" + norm.Z + "\n" +
                    //    "Face CP: " + "\n" + cp.X + "\n" + cp.Y + "\n" + cp.Z + "\n");

                    double d = -norm.X * cp.X - norm.Y * cp.Y - norm.Z * cp.Z;

                    //Console.WriteLine("Face D: " + d);


                    if (d > 0) meshes[i].subdividedFaces[j].radiance = Color.Green;
                    else meshes[i].subdividedFaces[j].radiance = Color.Red;
                }
            }


        }

        /// <summary>
        /// Convert scene data into rendering data
        /// </summary>
        /// <returns></returns>
        public static Vertex[] PreprocessScene()
        {

            //amount of individual objects
            int len = meshes.Count;

            //amount of individual light sources
            int llen = lights.Count;

            //TESTING - Randomize patch colors
            bool rand = false;

            //vertex index
            int vi = 0;

            //total face count
            int fcount = 0;

            //Counting total amount of faces
            for (int i = 0; i < len; i++)
            {
                if (meshes[i].subdividedFaces[0].points.Count == 3)
                {
                    fcount += meshes[i].subdividedFaces.Count * 3;
                }
                else
                {
                    fcount += meshes[i].subdividedFaces.Count * 6;
                }
            }

            //Adding total amount of lightsource faces
            for (int i = 0; i < llen; i++)
            {
                if (lights[i].subdividedFaces[0].points.Count == 3)
                {
                    fcount += lights[i].subdividedFaces.Count * 3;
                }
                else
                {
                    fcount += lights[i].subdividedFaces.Count * 6;
                }
            }

            Vertex[] res = new Vertex[fcount];

            int[] order = { 0, 3, 2, 2, 1, 0 };

            //Process an object
            for (int i = 0; i < len; i++)
            {
                //processing a face
                for (int j = 0; j < meshes[i].subdividedFaces.Count; j++)
                {
                    Color4 c1;
                    Color4 c2;

                    if (rand)
                    {
                        c1 = ColorToColor4Randomized(meshes[i].material.color);
                        c2 = ColorToColor4Randomized(meshes[i].material.color);
                    }
                    else
                    {
                        c1 = ColorToColor4(meshes[i].material.color);
                        c2 = ColorToColor4(meshes[i].material.color);
                    }

                    int faceShape = meshes[i].subdividedFaces[j].points.Count;

                    if (faceShape == 3)
                    { 
                        //processing a triangular vertex
                        for (int k = 0; k < 3; k++)
                        {
                            res[vi++] = new Vertex(new Vector4(
                                (float)meshes[i].subdividedFaces[j].points[k].X,
                                (float)meshes[i].subdividedFaces[j].points[k].Y,
                                (float)meshes[i].subdividedFaces[j].points[k].Z,
                                1.0f), ColorToColor4(meshes[i].subdividedFaces[j].radiance));
                        }
                    }
                    else
                    { 
                        //processing a rectangular vertex
                        for (int k = 0; k < 6; k++)
                        {
                            int l = order[k];
                            res[vi++] = new Vertex(new Vector4(
                                (float)meshes[i].subdividedFaces[j].points[l].X,
                                (float)meshes[i].subdividedFaces[j].points[l].Y,
                                (float)meshes[i].subdividedFaces[j].points[l].Z,
                                1.0f), ColorToColor4(meshes[i].subdividedFaces[j].radiance));
                        }
                    }


                    
                }
            }

            //Process a light source

            for (int i = 0; i < llen; i++)
            {
                //processing a face
                for (int j = 0; j < lights[i].subdividedFaces.Count; j++)
                {
                    Color4 c1;
                    Color4 c2;

                    if (rand)
                    {
                        c1 = ColorToColor4Randomized(lights[i].material.color);
                        c2 = ColorToColor4Randomized(lights[i].material.color);
                    }
                    else
                    {
                        c1 = ColorToColor4(lights[i].material.color);
                        c2 = ColorToColor4(lights[i].material.color);
                    }
                    int faceShape = lights[i].subdividedFaces[j].points.Count;

                    if (faceShape == 3)
                    {
                        //processing a triangular vertex
                        for (int k = 0; k < 3; k++)
                        {
                            res[vi++] = new Vertex(new Vector4(
                                (float)lights[i].subdividedFaces[j].points[k].X,
                                (float)lights[i].subdividedFaces[j].points[k].Y,
                                (float)lights[i].subdividedFaces[j].points[k].Z,
                                1.0f), ColorToColor4(lights[i].subdividedFaces[j].radiance));
                        }
                    }
                    else
                    {
                        //processing a rectangular vertex
                        for (int k = 0; k < 6; k++)
                        {
                            int l = order[k];
                            res[vi++] = new Vertex(new Vector4(
                                (float)lights[i].subdividedFaces[j].points[l].X,
                                (float)lights[i].subdividedFaces[j].points[l].Y,
                                (float)lights[i].subdividedFaces[j].points[l].Z,
                                1.0f), ColorToColor4(lights[i].subdividedFaces[j].radiance));
                        }
                    }



                }
            }


            return res;
        }


        //TODO: Interpolation

        /// <summary>
        /// Convert scene data into rendering data and interpolate
        /// </summary>
        /// <returns></returns>
        public static Vertex[] PreprocessSceneWithInterpolation()
        {

            //amount of individual objects
            int len = meshes.Count;

            //amount of individual light sources
            int llen = lights.Count;

            //vertex index
            int vi = 0;

            //total face count
            int fcount = 0;

            //Counting total amount of faces
            for (int i = 0; i < len; i++)
            {
                if (meshes[i].subdividedFaces[0].points.Count == 3)
                {
                    fcount += meshes[i].subdividedFaces.Count * 3;
                }
                else
                {
                    fcount += meshes[i].subdividedFaces.Count * 6;
                }
            }

            fcount *= 6;

            //Adding total amount of lightsource faces
            for (int i = 0; i < llen; i++)
            {
                if (lights[i].subdividedFaces[0].points.Count == 3)
                {
                    fcount += lights[i].subdividedFaces.Count * 3;
                }
                else
                {
                    fcount += lights[i].subdividedFaces.Count * 6;
                }
            }

            Vertex[] res = new Vertex[fcount];

            int[] order = { 0, 3, 2, 2, 1, 0 };

            //Process an object
            for (int i = 0; i < len; i++)
            {
                //Processing non-subdivided faces

                Dictionary<int, List<int>> faceDescendants = new Dictionary<int, List<int>>();

                for (int j = 0; j < meshes[i].subdividedFaces.Count; j++)
                {
                    if (!faceDescendants.ContainsKey(meshes[i].subdividedFaces[j].parent))
                        faceDescendants[meshes[i].subdividedFaces[j].parent] = new List<int>();

                    faceDescendants[meshes[i].subdividedFaces[j].parent].Add(j);
                }

                //processing a face
                for (int fi = 0; fi < meshes[i].faces.Count; fi++)
                {
                    //Interoplation

                    Dictionary<int, SortedSet<int>> pointNeighboringFaces = new Dictionary<int, SortedSet<int>>();
                    SortedSet<int> facePoints = new SortedSet<int>();

                    foreach (int j in faceDescendants[fi])
                    { 
                        for (int k = 0; k < meshes[i].subdividedFaces[j].points.Count(); k++)
                        {
                            if (!pointNeighboringFaces.ContainsKey(meshes[i].subdividedFaces[j].points[k].index))
                                pointNeighboringFaces[meshes[i].subdividedFaces[j].points[k].index] = new SortedSet<int>();

                            pointNeighboringFaces[meshes[i].subdividedFaces[j].points[k].index].Add(j);
                            facePoints.Add(meshes[i].subdividedFaces[j].points[k].index);
                        }
                    }

                    Dictionary<int, Color4> pointColors = new Dictionary<int, Color4>();

                    foreach (int pointInd in facePoints)
                    {

                        Color4 pointColor = VectorToColor4(
                            pointNeighboringFaces[pointInd]
                            .Select(x => meshes[i].subdividedFaces[x].radiance)
                            .Select(x => ColorToVector(x)) //1 or 255??
                            .Aggregate(new Point3D(0, 0, 0), (c1, c2) => c1 + c2) * (1.0 / pointNeighboringFaces[pointInd].Count()));
                        pointColors[pointInd] = pointColor;
                    }

                    foreach (int j in faceDescendants[fi])
                    {
                        Point3D center = meshes[i].subdividedFaces[j].centerPoint;
                        Color4 centerColor = Color4Avg(
                            pointColors[meshes[i].subdividedFaces[j].points[0].index],
                            pointColors[meshes[i].subdividedFaces[j].points[1].index],
                            pointColors[meshes[i].subdividedFaces[j].points[2].index]
                            );

                        for (int k = 0; k < meshes[i].subdividedFaces[j].points.Count(); k++)
                        {
                            Point3D p1 = meshes[i].subdividedFaces[j].points[k];
                            Point3D p2 = meshes[i].subdividedFaces[j].points[(k + 1) % 3];

                            Point3D sideCenter = (p1 + p2) * 0.5;
                            Color4 sideCenterColor = Color4Avg(pointColors[p1.index], pointColors[p2.index]);

                            res[vi++] = new Vertex(new Vector4(
                                    (float)p1.X,
                                    (float)p1.Y,
                                    (float)p1.Z,
                                    1.0f), pointColors[p1.index]);

                            res[vi++] = new Vertex(new Vector4(
                                    (float)center.X,
                                    (float)center.Y,
                                    (float)center.Z,
                                    1.0f), centerColor);

                            res[vi++] = new Vertex(new Vector4(
                                    (float)sideCenter.X,
                                    (float)sideCenter.Y,
                                    (float)sideCenter.Z,
                                    1.0f), sideCenterColor);

                            res[vi++] = new Vertex(new Vector4(
                                    (float)sideCenter.X,
                                    (float)sideCenter.Y,
                                    (float)sideCenter.Z,
                                    1.0f), sideCenterColor);

                            res[vi++] = new Vertex(new Vector4(
                                    (float)center.X,
                                    (float)center.Y,
                                    (float)center.Z,
                                    1.0f), centerColor);

                            res[vi++] = new Vertex(new Vector4(
                                    (float)p2.X,
                                    (float)p2.Y,
                                    (float)p2.Z,
                                    1.0f), pointColors[p2.index]);
                        }
                    }
                }
            }

            Console.WriteLine("Interpolation Done");

            //Process a light source

            for (int i = 0; i < llen; i++)
            {
                //processing a face
                for (int j = 0; j < lights[i].subdividedFaces.Count; j++)
                {
                    Color4 c1;
                    Color4 c2;


                    c1 = ColorToColor4(lights[i].material.color);
                    c2 = ColorToColor4(lights[i].material.color);
                    int faceShape = lights[i].subdividedFaces[j].points.Count;

                    if (faceShape == 3)
                    {
                        //processing a triangular vertex
                        for (int k = 0; k < 3; k++)
                        {
                            res[vi++] = new Vertex(new Vector4(
                                (float)lights[i].subdividedFaces[j].points[k].X,
                                (float)lights[i].subdividedFaces[j].points[k].Y,
                                (float)lights[i].subdividedFaces[j].points[k].Z,
                                1.0f), ColorToColor4(lights[i].subdividedFaces[j].radiance));
                        }
                    }
                    else
                    {
                        //processing a rectangular vertex
                        for (int k = 0; k < 6; k++)
                        {
                            int l = order[k];
                            res[vi++] = new Vertex(new Vector4(
                                (float)lights[i].subdividedFaces[j].points[l].X,
                                (float)lights[i].subdividedFaces[j].points[l].Y,
                                (float)lights[i].subdividedFaces[j].points[l].Z,
                                1.0f), ColorToColor4(lights[i].subdividedFaces[j].radiance));
                        }
                    }
                }
            }
            return res;
        }

    }
}
