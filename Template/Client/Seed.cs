using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MathNet.Spatial.Euclidean;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Client
{
    public static class Seed
    {
        #region // storage

        private static readonly Vector3F[][] CubePolylines = new[]
        {
            // bottom
            new[]
            {
                new Vector3F(0, 0, 0),
                new Vector3F(1, 0, 0),
                new Vector3F(1, 1, 0),
                new Vector3F(0, 1, 0),
                new Vector3F(0, 0, 0),
            },
            // top
            new[]
            {
                new Vector3F(0, 0, 1),
                new Vector3F(1, 0, 1),
                new Vector3F(1, 1, 1),
                new Vector3F(0, 1, 1),
                new Vector3F(0, 0, 1),
            },
            // sides
            new[] { new Vector3F(0, 0, 0), new Vector3F(0, 0, 1), },
            new[] { new Vector3F(1, 0, 0), new Vector3F(1, 0, 1), },
            new[] { new Vector3F(1, 1, 0), new Vector3F(1, 1, 1), },
            new[] { new Vector3F(0, 1, 0), new Vector3F(0, 1, 1), },
        }.Select(polyline => Matrix4DEx.Translate(-0.5, -0.5, -0.5).Transform(polyline)).ToArray();

        private static readonly IPrimitive[] PointCloudBunny = new Func<IPrimitive[]>(() =>
        {
            var matrix = Matrix4DEx.Scale(10) * Matrix4DEx.Rotate(QuaternionEx.AroundAxis(UnitVector3D.XAxis, Math.PI * 0.5)) * Matrix4DEx.Translate(-1, -1, -0.5);

            // point cloud source: http://graphics.stanford.edu/data/3Dscanrep/
            var vertices = StreamPointCloud_XYZ(@"..\..\..\resources\bunny.xyz")
                .Select(vertex => new Materials.Position.Vertex(matrix.Transform(vertex)))
                .ToArray();

            return new IPrimitive[]
            {
                new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.PointList,
                    vertices,
                    Color.White
                )
            };
        })();

        #endregion

        private static double GetTimeSpanPeriodRatio(TimeSpan duration, TimeSpan periodDuration)
        {
            return duration.TotalMilliseconds % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        public static IEnumerable<IPrimitive> GetPrimitives()
        {
            return new IPrimitive[0]
                .Concat(GetPrimitivesTriangles())
                .Concat(GetPrimitivesWorldAxis())
                .Concat(GetPrimitivesScreenViewLines())
                .Concat(GetPrimitivesCubes())
                .Concat(GetPrimitivesPointCloud())
                ;
        }

        private static IEnumerable<IPrimitive> GetPrimitivesScreenViewLines()
        {
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.Screen),
                PrimitiveTopology.LineList,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(3, 20, 0)),
                    new Materials.Position.Vertex(new Vector3F(140, 20, 0)),
                },
                Color.Gray
            );

            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.View),
                PrimitiveTopology.LineList,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(-0.9f, -0.9f, 0)),
                    new Materials.Position.Vertex(new Vector3F(0.9f, -0.9f, 0)),
                },
                Color.Gray
            );
        }

        private static IEnumerable<IPrimitive> GetPrimitivesWorldAxis()
        {
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineList,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(1, 0, 0)),
                },
                Color.Red
            );

            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineList,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(0, 1, 0)),
                },
                Color.LawnGreen
            );

            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineList,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(0, 0, 1)),
                },
                Color.Blue
            );
        }

        private static IEnumerable<IPrimitive> GetPrimitivesCubes()
        {
            var duration = new TimeSpan(DateTime.UtcNow.Ticks);

            var angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                Matrix4DEx.Scale(0.5) *
                Matrix4DEx.Rotate(new UnitVector3D(1, 0, 0), angle) *
                Matrix4DEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).ToArray(),
                    Color.White
                );
            }

            angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                Matrix4DEx.Scale(0.5) *
                Matrix4DEx.Rotate(new UnitVector3D(0, 1, 0), angle) *
                Matrix4DEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).ToArray(),
                    Color.Yellow
                );
            }
        }

        private static IEnumerable<IPrimitive> GetPrimitivesPointCloud()
        {
            return PointCloudBunny;
        }

        private static IEnumerable<IPrimitive> GetPrimitivesTriangles()
        {
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.TriangleStrip,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(1, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(0, 1, 0)),
                    new Materials.Position.Vertex(new Vector3F(1, 1, 0)),
                },
                Color.Goldenrod
            );

            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.TriangleList,
                new[]
                {
                    new Materials.Position.Vertex(new Vector3F(-2, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(-2, 1, 0)),
                    new Materials.Position.Vertex(new Vector3F(-1, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(-4, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(-4, 1, 0)),
                    new Materials.Position.Vertex(new Vector3F(-3, 0, 0)),
                },
                Color.Cyan
            );
        }

        public static IEnumerable<Vector3F> StreamPointCloud_XYZ(string filePath)
        {
            using (var inputStream = new FileStream(filePath, FileMode.Open))
            {
                var pointCount = inputStream.Length / (4 * 3);
                using (var reader = new BinaryReader(inputStream))
                {
                    for (var i = 0L; i < pointCount; i++)
                    {
                        yield return new Vector3F(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                }
            }
        }
    }
}