using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MathNet.Spatial.Euclidean;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;
using EMBC.Utils;

namespace EMBC.Client
{
    public static class Seed
    {
        #region // storage

        private static readonly Vector3F[][] CubePolylines = new[]
        {
            new[]
            {
                new Vector3F(0, 0, 0),
                new Vector3F(1, 0, 0),
                new Vector3F(1, 1, 0),
                new Vector3F(0, 1, 0),
                new Vector3F(0, 0, 0),
            },
            new[]
            {
                new Vector3F(0, 0, 1),
                new Vector3F(1, 0, 1),
                new Vector3F(1, 1, 1),
                new Vector3F(0, 1, 1),
                new Vector3F(0, 0, 1),
            },
            new[] { new Vector3F(0, 0, 0), new Vector3F(0, 0, 1), },
            new[] { new Vector3F(1, 0, 0), new Vector3F(1, 0, 1), },
            new[] { new Vector3F(1, 1, 0), new Vector3F(1, 1, 1), },
            new[] { new Vector3F(0, 1, 0), new Vector3F(0, 1, 1), },
        }.Select(polyline => Matrix4DEx.Translate(-0.5, -0.5, -0.5).Transform(polyline)).ToArray();

        private static readonly IModel[] PointCloudBunny = new Func<IModel[]>(() =>
        {
            var matrix = Matrix4DEx.Scale(10) * Matrix4DEx.Rotate(QuaternionEx.AroundAxis(UnitVector3D.XAxis, Math.PI * 0.5)) * Matrix4DEx.Translate(-1, -1, -0.5);

            // point cloud source: http://graphics.stanford.edu/data/3Dscanrep/
            var vertices = StreamPointCloud_XYZ(@"..\..\..\resources\bunny.xyz")
                .Select(vertex => matrix.Transform(vertex))
                .ToArray();

            return new IModel[]
            {
                new Model
                {
                    ShaderType = ShaderType.Position,
                    Space = Space.World,
                    PrimitiveTopology = PrimitiveTopology.PointList,
                    Positions = vertices,
                    Color = Color.White.ToRgba(),
                }
            };
        })();

        #endregion

        private static double GetTimeSpanPeriodRatio(TimeSpan duration, TimeSpan periodDuration)
        {
            return duration.TotalMilliseconds % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        public static IEnumerable<IModel> GetModels()
        {
            return new IModel[0]
                .Concat(GetWorldAxis())
                //.Concat(GetScreenViewLines())
                //.Concat(GetTriangles())
                //.Concat(GetCubes())
                //.Concat(GetPointCloud())
                .Concat(GetPositionColorSamples())
                ;
        }

        private static IEnumerable<IModel> GetScreenViewLines()
        {
            yield return new Model
            {
                ShaderType = ShaderType.Position,
                Space = Space.Screen,
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(3, 20, 0),
                    new Vector3F(140, 20, 0),
                },
                Color = Color.Gray.ToRgba(),
            };

            yield return new Model
            {
                ShaderType = ShaderType.Position,
                Space = Space.View,
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(-0.9f, -0.9f, 0),
                    new Vector3F(0.9f, -0.9f, 0),
                },
                Color = Color.Gray.ToRgba(),
            };
        }

        private static IEnumerable<IModel> GetWorldAxis()
        {
            yield return new Model
            {
                ShaderType = ShaderType.Position,
                Space = Space.World,
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(0, 0, 0),
                    new Vector3F(1, 0, 0),
                },
                Color = Color.Red.ToRgba(),
            };

            yield return new Model
            {
                ShaderType = ShaderType.Position,
                Space = Space.World,
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(0, 0, 0),
                    new Vector3F(0, 1, 0),
                },
                Color = Color.FromArgb(255, 0, 255, 0).ToRgba(),
            };

            yield return new Model
            {
                ShaderType = ShaderType.Position,
                Space = Space.World,
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(0, 0, 0),
                    new Vector3F(0, 0, 1),
                },
                Color = Color.Blue.ToRgba(),
            };
        }

        private static IEnumerable<IModel> GetCubes()
        {
            var duration = new TimeSpan(DateTime.UtcNow.Ticks);

            var angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                Matrix4DEx.Scale(0.5) *
                Matrix4DEx.Rotate(new UnitVector3D(1, 0, 0), angle) *
                Matrix4DEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Model
                {
                    ShaderType = ShaderType.Position,
                    Space = Space.World,
                    PrimitiveTopology = PrimitiveTopology.LineStrip,
                    Positions = matrixModel.Transform(cubePolyline),
                    Color = Color.White.ToRgba(),
                };
            }

            angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                Matrix4DEx.Scale(0.5) *
                Matrix4DEx.Rotate(new UnitVector3D(0, 1, 0), angle) *
                Matrix4DEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Model
                {
                    ShaderType = ShaderType.Position,
                    Space = Space.World,
                    PrimitiveTopology = PrimitiveTopology.LineStrip,
                    Positions = matrixModel.Transform(cubePolyline),
                    Color = Color.Yellow.ToRgba(),
                };
            }
        }

        private static IEnumerable<IModel> GetPointCloud()
        {
            return PointCloudBunny;
        }

        private static IEnumerable<IModel> GetTriangles()
        {
            yield return new Model
            {
                ShaderType = ShaderType.Position,
                Space = Space.World,
                PrimitiveTopology = PrimitiveTopology.TriangleStrip,
                Positions = new[]
                {
                    new Vector3F(0, -1, 0),
                    new Vector3F(1, -1, 0),
                    new Vector3F(0, -2, 0),
                    new Vector3F(1, -2, 0),
                },
                Color = Color.Goldenrod.ToRgba(),
            };

            yield return new Model
            {
                ShaderType = ShaderType.Position,
                Space = Space.World,
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                Positions = new[]
                {
                    new Vector3F(-2, 0, 0),
                    new Vector3F(-2, 1, 0),
                    new Vector3F(-1, 0, 0),
                    new Vector3F(-4, 0, 0),
                    new Vector3F(-4, 1, 0),
                    new Vector3F(-3, 0, 0),
                },
                Color = Color.Cyan.ToRgba(),
            };
        }

        private static IEnumerable<IModel> GetPositionColorSamples()
        {
            /*
            yield return new Model
            {
                ShaderType = ShaderType.PositionColor,
                Space = Space.World,
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(1f, 0, 0),
                    new Vector3F(0, 1f, 0),
                },
                Colors = new[]
                {
                    Color.Cyan.ToRgba(),
                    Color.Magenta.ToRgba(),
                },
            };
            */
            yield return new Model
            {
                ShaderType = ShaderType.PositionColor,
                Space = Space.World,
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                Positions = new[]
                {
                    new Vector3F(1, 0, 0),
                    new Vector3F(0, 0, 1),
                    new Vector3F(0, 1, 0),
                },
                Colors = new[]
                {
                    Color.Red.ToRgba(),
                    Color.Blue.ToRgba(),
                    Color.LawnGreen.ToRgba(),
                },
            };
            
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