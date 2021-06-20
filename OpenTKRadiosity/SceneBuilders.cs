using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using static OpenTKRadiosity.Objects;
using static OpenTKRadiosity.Utils;
using static OpenTKRadiosity.Transformation;

namespace OpenTKRadiosity
{
    public static class SceneBuilders
    {
        public static void LoadScene2()
        {
            LoadRoom();
            // Diffuse, Specular, Reflection, Refraction

            var t1 = new Mesh();
            LoadMeshByPath(ref t1, "../../models/cube.obj");
            t1.material = new Material();
            t1.material.color = Color.ForestGreen;
            t1.material.parameters = new List<double> { 0, 0, 0.8, 0 };
            //Transform(ref t1, Transformation.Scale(1.5, 1.5, 1.5));
            ApplyTransform(ref t1, Rotate(-130.0 / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref t1, Transformation.Move(-80, 75, 300));
            meshes.Add(t1);

            var t3 = new Sphere();
            t3.mat = new Material();
            t3.mat.color = Color.Black;
            t3.mat.specularHighlight = 100;
            t3.mat.parameters = new List<double> { 0.1, 5, 0.8, 0 };
            t3.pos = new Point3D(160, 100, 200);
            t3.radius = 50;
            spheres.Add(t3);

            var t6 = new Sphere();
            t6.mat = new Material();
            t6.mat.color = Color.White;
            t6.mat.specularHighlight = 30;
            t6.mat.refractionIndex = 1.5;
            t6.mat.parameters = new List<double> { 0, 0.1, 0.1, 0.8 };
            t6.pos = new Point3D(-140, 90, 0);
            t6.radius = 60;
            spheres.Add(t6);

            var t2 = new Mesh();
            LoadMeshByPath(ref t2, "../../models/cube.obj");
            t2.material = new Material();
            t2.material.color = Color.Red;
            t2.material.specularHighlight = 30;
            t2.material.refractionIndex = 1.5;
            t2.material.parameters = new List<double> { 0.15, 0.5, 0, 0.8 };
            ApplyTransform(ref t2, Transformation.Scale(0.5, 0.5, 0.5));
            ApplyTransform(ref t2, Rotate(-55.0 / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref t2, Transformation.Move(115, 100, 0));
            meshes.Add(t2);

        }

        public static void LoadRoom()
        {
            var floor = new Mesh();
            LoadMeshByPath(ref floor, "../../models/plane.obj");
            floor.material = new Material();
            floor.material.color = Color.White;
            floor.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref floor, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref floor, Transformation.Scale(2.5, 2.5, 5));
            ApplyTransform(ref floor, Transformation.Move(0, -100 + 250, -1));
            meshes.Add(floor);

            var ceiling = new Mesh();
            LoadMeshByPath(ref ceiling, "../../models/plane.obj");
            ceiling.material = new Material();
            ceiling.material.color = Color.DarkGreen;
            ceiling.material.parameters = new List<double> { 0, 0, 0, 0 };
            //AtheneTransform(ref ceiling, AtheneRotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref ceiling, Transformation.Scale(2.5, 2.5, 5));
            ApplyTransform(ref ceiling, Transformation.Move(0, -100 - 250, -1));
            meshes.Add(ceiling);

            var wall1 = new Mesh();
            LoadMeshByPath(ref wall1, "../../models/plane.obj");
            wall1.material = new Material();
            wall1.material.color = Color.LightYellow;
            wall1.material.parameters = new List<double> { 0, 0, 0.8, 0 };
            ApplyTransform(ref wall1, Transformation.Scale(2.5, 2.5, 5));
            ApplyTransform(ref wall1, Rotate((90) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref wall1, Transformation.Move(-250, -100, -1));
            meshes.Add(wall1);

            var wall2 = new Mesh();
            LoadMeshByPath(ref wall2, "../../models/plane.obj");
            wall2.material = new Material();
            wall2.material.color = Color.Purple;
            wall2.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall2, Transformation.Scale(2.5, 2.5, 2.5));
            ApplyTransform(ref wall2, Rotate((90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall2, Transformation.Move(0, -100, -1 + 500));
            meshes.Add(wall2);

            var wall3 = new Mesh();
            LoadMeshByPath(ref wall3, "../../models/plane.obj");
            wall3.material = new Material();
            wall3.material.color = Color.PaleTurquoise;
            wall3.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall3, Transformation.Scale(2.5, 2.5, 5));
            ApplyTransform(ref wall3, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref wall3, Transformation.Move(250, -100, -1));
            meshes.Add(wall3);

            var wall4 = new Mesh();
            LoadMeshByPath(ref wall4, "../../models/plane.obj");
            wall4.material = new Material();
            wall4.material.color = Color.Red;
            wall4.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall4, Transformation.Scale(2.5, 2.5, 2.5));
            ApplyTransform(ref wall4, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall4, Transformation.Move(0, -100, -500 - 1));
            meshes.Add(wall4);
        }

        public static void LoadCube()
        {
            var t1 = new Mesh();
            LoadMeshByPath(ref t1, "../../models/cube.obj");
            t1.material = new Material();
            t1.material.color = Color.Red;
            t1.material.specularHighlight = 30;
            t1.material.refractionIndex = 1.5;
            t1.material.parameters = new List<double> { 0.15, 0.5, 0, 0.8 };
            ApplyTransform(ref t1, Transformation.Scale(0.005, 0.005, 0.005));
            meshes.Add(t1);
        }

        public static void LoadCubes()
        {
            /*
            var t1 = new Mesh();
            LoadMeshByPath(ref t1, "../../models/cube.obj");
            t1.material = new Material();
            t1.material.color = Color.Yellow;
            t1.material.specularHighlight = 30;
            t1.material.refractionIndex = 1.5;
            t1.material.parameters = new List<double> { 0.15, 0.5, 0, 0.8 };
            ApplyTransform(ref t1, Transformation.Scale(0.005, 0.005, 0.005));
            ApplyTransform(ref t1, Transformation.Move(0, -1.4, -2));
            //ApplyTransform(ref t1, Transformation.Rotate(-0.3, 'y'));
            meshes.Add(t1);
            */

            var t1 = new Mesh();
            LoadMeshByPath(ref t1, "../../models/cube.obj");
            t1.material = new Material();
            t1.material.color = Color.White;
            t1.material.specularHighlight = 30;
            t1.material.refractionIndex = 1.5;
            t1.material.parameters = new List<double> { 0.15, 0.5, 0, 0.8 };
            ApplyTransform(ref t1, Transformation.Scale(0.005, 0.005, 0.005));
            ApplyTransform(ref t1, Transformation.Move(-0.4, -1.5, -0.9));
            ApplyTransform(ref t1, Transformation.Rotate(-0.45, 'y'));
            meshes.Add(t1);

            
            var t2 = new Mesh();
            LoadMeshByPath(ref t2, "../../models/cube.obj");
            t2.material = new Material();
            t2.material.color = Color.White;
            t2.material.specularHighlight = 30;
            t2.material.refractionIndex = 1.5;
            t2.material.parameters = new List<double> { 0.15, 0.5, 0, 0.8 };
            ApplyTransform(ref t2, Transformation.Scale(0.005, 0.01, 0.005));
            ApplyTransform(ref t2, Transformation.Rotate(2.8, 'y'));
            ApplyTransform(ref t2, Transformation.Move(0.6, -1, -1.9));
            meshes.Add(t2);
            

            var floor = new Mesh();
            LoadMeshByPath(ref floor, "../../models/plane.obj");
            floor.material = new Material();
            floor.material.color = Color.White;
            floor.material.parameters = new List<double> { 0, 0, 0, 0 };
            //ApplyTransform(ref floor, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref floor, Transformation.Scale(0.02, 0.02, 0.02));
            //ApplyTransform(ref floor, Rotate((-45) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref floor, Transformation.Move(0, -2, -1.8));
            meshes.Add(floor);

            var ceiling = new Mesh();
            LoadMeshByPath(ref ceiling, "../../models/plane.obj");
            ceiling.material = new Material();
            ceiling.material.color = Color.White;
            ceiling.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref ceiling, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref ceiling, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref ceiling, Transformation.Move(0, 2, -1.8));
            meshes.Add(ceiling);

            var wall1 = new Mesh();
            LoadMeshByPath(ref wall1, "../../models/plane.obj");
            wall1.material = new Material();
            wall1.material.color = Color.Red;
            wall1.material.parameters = new List<double> { 0, 0, 0.8, 0 };
            ApplyTransform(ref wall1, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall1, Rotate((90) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref wall1, Transformation.Move(-2, 0, -1.8));
            //Console.WriteLine("wall1 normal" + VectorToString(wall1.faces[0].normal));
            meshes.Add(wall1);

             var wall2 = new Mesh();
            LoadMeshByPath(ref wall2, "../../models/plane.obj");
            wall2.material = new Material();
            wall2.material.color = Color.White;
            wall2.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall2, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall2, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall2, Rotate((0) / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref wall2, Transformation.Move(0, 0, -3.8));
            //Console.WriteLine("wall2 normal" + VectorToString(wall2.faces[0].normal));
            meshes.Add(wall2);

            /*
            var wall2 = new Mesh();
            LoadMeshByPath(ref wall2, "../../models/cube.obj");
            wall2.material = new Material();
            wall2.material.color = Color.White;
            wall2.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall2, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall2, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall2, Rotate((0) / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref wall2, Transformation.Move(0, 0, -3.8));
            //Console.WriteLine("wall2 normal" + VectorToString(wall2.faces[0].normal));
            meshes.Add(wall2);
             */


            var wall3 = new Mesh();
            LoadMeshByPath(ref wall3, "../../models/plane.obj");
            wall3.material = new Material();
            wall3.material.color = Color.Green;
            wall3.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall3, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall3, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref wall3, Transformation.Move(2, 0, -1.8));
            //Console.WriteLine("wall3 normal" + VectorToString(wall3.faces[0].normal));
            meshes.Add(wall3);

            var light = new Mesh();
            LoadMeshByPath(ref light, "../../models/cube.obj");
            light.material = new Material();
            light.material.color = Color.LightYellow;
            light.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref light, Transformation.Scale(0.0005, 0.0005, 0.0005));
            ApplyTransform(ref light, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref light, Transformation.Move(0, 1.8, -1.8));
            lights.Add(light);

            /*
             
            var light = new Mesh();
            LoadMeshByPath(ref light, "../../models/plane.obj");
            light.material = new Material();
            light.material.color = Color.LightYellow;
            light.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref light, Transformation.Scale(0.008, 0.0005, 0.008));
            ApplyTransform(ref light, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref light, Transformation.Move(0, 1.99, -1.8));
            lights.Add(light);
             
            var light1 = new Mesh();
            LoadMeshByPath(ref light1, "../../models/plane.obj");
            light1.material = new Material();
            light1.material.color = Color.LightYellow;
            light1.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref light1, Transformation.Scale(0.008, 0.0005, 0.008));
            //ApplyTransform(ref light1, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref light1, Transformation.Move(0, 1.99, -1.9));
            lights.Add(light1); */


            var wall4 = new Mesh();
            LoadMeshByPath(ref wall4, "../../models/plane.obj");
            wall4.material = new Material();
            wall4.material.color = Color.White;
            wall4.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall4, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall4, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall4, Rotate((180) / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref wall4, Transformation.Move(0, 0, 0.2));
            meshes.Add(wall4);


        }

        public static void LoadWindow()
        {

            var t1 = new Mesh();
            LoadMeshByPath(ref t1, "../../models/cube.obj");
            t1.material = new Material();
            t1.material.color = Color.White;
            t1.material.specularHighlight = 30;
            t1.material.refractionIndex = 1.5;
            t1.material.parameters = new List<double> { 0.15, 0.5, 0, 0.8 };
            ApplyTransform(ref t1, Transformation.Scale(0.005, 0.005, 0.005));
            ApplyTransform(ref t1, Transformation.Move(-0.4, -1.5, -0.9));
            ApplyTransform(ref t1, Transformation.Rotate(-0.45, 'y'));
            meshes.Add(t1);


            var t2 = new Mesh();
            LoadMeshByPath(ref t2, "../../models/cube.obj");
            t2.material = new Material();
            t2.material.color = Color.White;
            t2.material.specularHighlight = 30;
            t2.material.refractionIndex = 1.5;
            t2.material.parameters = new List<double> { 0.15, 0.5, 0, 0.8 };
            ApplyTransform(ref t2, Transformation.Scale(0.005, 0.01, 0.005));
            ApplyTransform(ref t2, Transformation.Rotate(2.8, 'y'));
            ApplyTransform(ref t2, Transformation.Move(0.6, -1, -1.9));
            meshes.Add(t2);


            var floor = new Mesh();
            LoadMeshByPath(ref floor, "../../models/plane.obj");
            floor.material = new Material();
            floor.material.color = Color.White;
            floor.material.parameters = new List<double> { 0, 0, 0, 0 };
            //ApplyTransform(ref floor, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref floor, Transformation.Scale(0.02, 0.02, 0.02));
            //ApplyTransform(ref floor, Rotate((-45) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref floor, Transformation.Move(0, -2, -1.8));
            meshes.Add(floor);

            var ceiling = new Mesh();
            LoadMeshByPath(ref ceiling, "../../models/plane.obj");
            ceiling.material = new Material();
            ceiling.material.color = Color.White;
            ceiling.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref ceiling, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref ceiling, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref ceiling, Transformation.Move(0, 2, -1.8));
            meshes.Add(ceiling);

            var wall1 = new Mesh();
            LoadMeshByPath(ref wall1, "../../models/plane.obj");
            wall1.material = new Material();
            wall1.material.color = Color.Red;
            wall1.material.parameters = new List<double> { 0, 0, 0.8, 0 };
            ApplyTransform(ref wall1, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall1, Rotate((90) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref wall1, Transformation.Move(-2, 0, -1.8));
            //Console.WriteLine("wall1 normal" + VectorToString(wall1.faces[0].normal));
            meshes.Add(wall1);

            var wall21 = new Mesh();
            LoadMeshByPath(ref wall21, "../../models/cube.obj");
            wall21.material = new Material();
            wall21.material.color = Color.White;
            wall21.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall21, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall21, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall21, Rotate((0) / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref wall21, Transformation.Move(0, 0, -3.8));
            //Console.WriteLine("wall2 normal" + VectorToString(wall2.faces[0].normal));
            meshes.Add(wall21);


            var wall3 = new Mesh();
            LoadMeshByPath(ref wall3, "../../models/plane.obj");
            wall3.material = new Material();
            wall3.material.color = Color.Green;
            wall3.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall3, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall3, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref wall3, Transformation.Move(2, 0, -1.8));
            //Console.WriteLine("wall3 normal" + VectorToString(wall3.faces[0].normal));
            meshes.Add(wall3);

            var light = new Mesh();
            LoadMeshByPath(ref light, "../../models/cube.obj");
            light.material = new Material();
            light.material.color = Color.LightYellow;
            light.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref light, Transformation.Scale(0.0005, 0.0005, 0.0005));
            ApplyTransform(ref light, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref light, Transformation.Move(0, 1.8, -1.8));
            lights.Add(light);

            /*
             
            var light = new Mesh();
            LoadMeshByPath(ref light, "../../models/plane.obj");
            light.material = new Material();
            light.material.color = Color.LightYellow;
            light.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref light, Transformation.Scale(0.008, 0.0005, 0.008));
            ApplyTransform(ref light, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref light, Transformation.Move(0, 1.99, -1.8));
            lights.Add(light);
             
            var light1 = new Mesh();
            LoadMeshByPath(ref light1, "../../models/plane.obj");
            light1.material = new Material();
            light1.material.color = Color.LightYellow;
            light1.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref light1, Transformation.Scale(0.008, 0.0005, 0.008));
            //ApplyTransform(ref light1, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref light1, Transformation.Move(0, 1.99, -1.9));
            lights.Add(light1); */

            var wall4 = new Mesh();
            LoadMeshByPath(ref wall4, "../../models/plane.obj");
            wall4.material = new Material();
            wall4.material.color = Color.White;
            wall4.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall4, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall4, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall4, Rotate((180) / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref wall4, Transformation.Move(0, 0, 0.2));
            meshes.Add(wall4);


        }

        public static void LoadPlaneTesting()
        {
            var wall2 = new Mesh();
            LoadMeshByPath(ref wall2, "../../models/plane.obj");
            wall2.material = new Material();
            wall2.material.color = Color.Purple;
            wall2.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref wall2, Transformation.Scale(0.02, 0.02, 0.02));
            ApplyTransform(ref wall2, Rotate((-90) / 360.0 * (2.0 * Math.PI), 'x'), true);
            ApplyTransform(ref wall2, Rotate((0) / 360.0 * (2.0 * Math.PI), 'y'), true);
            ApplyTransform(ref wall2, Transformation.Move(0, 0, -3.8));
            Console.WriteLine("wall2 normal" + VectorToString(wall2.faces[0].normal));
            meshes.Add(wall2);

            var light = new Mesh();
            LoadMeshByPath(ref light, "../../models/plane.obj");
            light.material = new Material();
            light.material.color = Color.White;
            light.material.parameters = new List<double> { 0, 0, 0, 0 };
            ApplyTransform(ref light, Transformation.Scale(0.0005, 0.0005, 0.0005));
            ApplyTransform(ref light, Rotate((180) / 360.0 * (2.0 * Math.PI), 'z'), true);
            ApplyTransform(ref light, Transformation.Move(0, 1.99, -1.8));

            Console.WriteLine("light normal" + VectorToString(light.faces[0].normal));
            lights.Add(light);
        }

    }
}
