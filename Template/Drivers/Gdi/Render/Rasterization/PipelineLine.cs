﻿using System;
using System.Collections.Generic;
using System.Linq;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Render.Rasterization
{
    public partial class Pipeline<TVsIn, TPsIn>
    {
        internal struct PrimitiveLine
        {
            public TPsIn PsIn0;
            public TPsIn PsIn1;
            public Vector4F PositionScreen0;
            public Vector4F PositionScreen1;
        }

        #region // routines

        /// Bresenham's line algorithm line rasterization algorithm.
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        public static IEnumerable<(int X, int Y)> LineBresenham(Vector2F point0, Vector2F point1)
        {
            var x0 = (int)Math.Round(point0.X);
            var y0 = (int)Math.Round(point0.Y);
            var x1 = (int)Math.Round(point1.X);
            var y1 = (int)Math.Round(point1.Y);
            var w = x1 - x0;
            var h = y1 - y0;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            var longest = Math.Abs(w);
            var shortest = Math.Abs(h);
            if (longest <= shortest)
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            var numerator = longest >> 1;
            for (var i = 0; i <= longest; i++)
            {
                yield return (x0, y0);
                numerator += shortest;
                if (numerator < longest)
                {
                    x0 += dx2;
                    y0 += dy2;
                }
                else
                {
                    numerator -= longest;
                    x0 += dx1;
                    y0 += dy1;
                }
            }
        }

        #endregion

        #region // vertex post-processing

        private void VertexPostProcessingLine(ref TPsIn psin0, ref TPsIn psin1)
        {
            PrimitiveLine primitive;
            primitive.PsIn0 = psin0;
            primitive.PsIn1 = psin1;
            VertexPostProcessing(ref primitive.PsIn0, out primitive.PositionScreen0);
            VertexPostProcessing(ref primitive.PsIn1, out primitive.PositionScreen1);

            RasterizeLine(primitive);
        }

        #endregion

        #region // rasterization

        private void RasterizeLine(in PrimitiveLine primitive)
        {
            var pixels = LineBresenham(
                    primitive.PositionScreen0.ToVector2F(),
                    primitive.PositionScreen1.ToVector2F())
                .ToArray();

            var deltaAlpha = 1f / pixels.Length;
            var alpha = deltaAlpha * 0.5f;

            foreach (var (x, y) in pixels)
            {
                var interpolant = primitive.PsIn0.InterpolateLinear(primitive.PsIn1, alpha);

                StagePixelShader(x, y, interpolant);

                alpha += deltaAlpha;
            }
        }

        #endregion
    }
}
