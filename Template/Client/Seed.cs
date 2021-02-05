using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathNet.Spatial.Euclidean;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;
using EMBC.Materials;

namespace EMBC.Client
{
    public static class Seed
    {
        private static readonly Point3D[][] CubePolylines = new[]
        {
            new[]
            {
                new Point3D(0, 0, 0),
                new Point3D(1, 0, 0),
                new Point3D(1, 1, 0),
                new Point3D(0, 1, 0),
                new Point3D(0, 0, 0),
            },
            new[]
            {
                new Point3D(0, 0, 1),
                new Point3D(1, 0, 1),
                new Point3D(1, 1, 1),
                new Point3D(0, 1, 1),
                new Point3D(0, 0, 1),
            },
            new[] { new Point3D(0, 0, 0), new Point3D(0, 0, 1), },
            new[] { new Point3D(1, 0, 0), new Point3D(1, 0, 1), },
            new[] { new Point3D(1, 1, 0), new Point3D(1, 1, 1), },
            new[] { new Point3D(0, 1, 0), new Point3D(0, 1, 1), },
        }.Select(polyline => MatrixEx.Translate(-0.5, -0.5, -0.5).Transform(polyline).ToArray()).ToArray();

        private static double GetTimeSpanPeriodRatio(TimeSpan duration, TimeSpan periodDuration)
        {
            return duration.TotalMilliseconds % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        public static IEnumerable<IPrimitive> GetPrimitives()
        {
            return GetPrimitivesScreenViewLines()
                .Concat(GetPrimitivesWorldAxis())
                .Concat(GetPrimitivesCubes())
                ;
        }

        private static IEnumerable<IPrimitive> GetPrimitivesScreenViewLines()
        {
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.Screen),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Point3D(3, 20, 0)),
                    new Materials.Position.Vertex(new Point3D(140, 20, 0)),
                },
                Color.Gray
            );

            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.View),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Point3D(-0.9, -0.9, 0)),
                    new Materials.Position.Vertex(new Point3D(0.9, -0.9, 0)),
                },
                Color.Gray
            );
        }
        private static IEnumerable<IPrimitive> GetPrimitivesWorldAxis()
        {
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Point3D(0, 0, 0)),
                    new Materials.Position.Vertex(new Point3D(1, 0, 0)),
                },
                Color.Red
            );

            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Point3D(0, 0, 0)),
                    new Materials.Position.Vertex(new Point3D(0, 1, 0)),
                },
                Color.LawnGreen
            );

            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Point3D(0, 0, 0)),
                    new Materials.Position.Vertex(new Point3D(0, 0, 1)),
                },
                Color.Blue
            );
        }

        private static IEnumerable<IPrimitive> GetPrimitivesCubes()
        {
            var duration = new TimeSpan(DateTime.UtcNow.Ticks);

            var angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(1, 0, 0), angle) *
                MatrixEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).Cast<Materials.Position.IVertex>(),
                    Color.White
                );
            }

            angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(0, 1, 0), angle) *
                MatrixEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).Cast<Materials.Position.IVertex>(),
                    Color.Yellow
                );
            }
        }
    }
}