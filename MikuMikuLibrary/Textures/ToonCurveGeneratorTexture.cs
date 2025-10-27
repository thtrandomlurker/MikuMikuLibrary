using MikuMikuLibrary.Objects.Extra.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuLibrary.Textures
{
    public class ColorPoint
    {
        public int Offset;
        public Vector3 Color;
        public bool Active = false;
    }
    public enum CurveType
    {
        Diffuse,
        Specular,
        Fresnel
    }
    public class ToonCurveGeneratorTexture : Texture
    {
        public List<ColorPoint> DiffuseCurvePoints { get; }
        public List<ColorPoint> SpecularCurvePoints { get; }
        public List<ColorPoint> FresnelCurvePoints { get; }

        public void UpdateCurve(CurveType curve)
        {
            List<ColorPoint> curvePointList;
            switch (curve)
            {
                case CurveType.Diffuse:
                    curvePointList = DiffuseCurvePoints;
                    break;
                case CurveType.Specular:
                    curvePointList = SpecularCurvePoints;
                    break;
                case CurveType.Fresnel:
                    curvePointList = FresnelCurvePoints;
                    break;
                default:
                    throw new ArgumentException("Invalid curve type");
            }

            int yOffset = 8 - (2 * ( (int)curve + 1));
            // pre-sort
            curvePointList.Sort((x, y) => x.Offset.CompareTo(y.Offset));

            // handle lead-in
            if (curvePointList[0].Offset > 0)
            {
                for (int i = 0; i < curvePointList[0].Offset; i++)
                {
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4)))] = (byte)(curvePointList[0].Color.X * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 1] = (byte)(curvePointList[0].Color.Y * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 2] = (byte)(curvePointList[0].Color.Z * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 3] = 255;
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4)))] = (byte)(curvePointList[0].Color.X * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 1] = (byte)(curvePointList[0].Color.Y * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 2] = (byte)(curvePointList[0].Color.Z * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 3] = 255;
                }
            }
            // then the middle parts.
            for (int dci = 0; dci < (curvePointList.Count - 1); dci++)
            {
                var cur = curvePointList[dci];
                var next = curvePointList[dci + 1];
                int distance = next.Offset - cur.Offset;

                for (int i = cur.Offset; i < next.Offset; i++)
                {
                    Vector3 color = cur.Color + ((next.Color - cur.Color) * Math.Clamp((((float)i - (float)cur.Offset) / (float)distance), 0.0f, 1.0f));

                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4)))] = (byte)(color.X * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 1] = (byte)(color.Y * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 2] = (byte)(color.Z * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 3] = 255;
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4)))] = (byte)(color.X * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 1] = (byte)(color.Y * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 2] = (byte)(color.Z * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 3] = 255;
                }
            }

            // then the end
            if (curvePointList.Last().Offset < 256)
            {
                for (int i = curvePointList.Last().Offset; i < 256; i++)
                {
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4)))] = (byte)(curvePointList.Last().Color.X * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 1] = (byte)(curvePointList.Last().Color.Y * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 2] = (byte)(curvePointList.Last().Color.Z * 255.0f);
                    this[0, 0].Data[((i) * 4 + (yOffset * (256 * 4))) + 3] = 255;
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4)))] = (byte)(curvePointList.Last().Color.X * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 1] = (byte)(curvePointList.Last().Color.Y * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 2] = (byte)(curvePointList.Last().Color.Z * 255.0f);
                    this[0, 0].Data[((i) * 4 + ((yOffset + 1) * (256 * 4))) + 3] = 255;
                }
            }
        }

        public ToonCurveGeneratorTexture() : base(256, 8, TextureFormat.RGBA8, 1, 1)
        {
            DiffuseCurvePoints = new List<ColorPoint>();
            DiffuseCurvePoints.Add(new ColorPoint() { Offset = 0, Color = new Vector3(0.0f, 0.0f, 0.0f) });
            DiffuseCurvePoints.Add(new ColorPoint() { Offset = 255, Color = new Vector3(1.0f, 0.0f, 0.0f) });
            SpecularCurvePoints = new List<ColorPoint>();
            SpecularCurvePoints.Add(new ColorPoint() { Offset = 0, Color = new Vector3(0.0f, 0.0f, 0.0f) });
            SpecularCurvePoints.Add(new ColorPoint() { Offset = 255, Color = new Vector3(0.0f, 1.0f, 0.0f) });
            FresnelCurvePoints = new List<ColorPoint>();
            FresnelCurvePoints.Add(new ColorPoint() { Offset = 0, Color = new Vector3(0.0f, 0.0f, 0.0f) });
            FresnelCurvePoints.Add(new ColorPoint() { Offset = 255, Color = new Vector3(0.0f, 0.0f, 1.0f) });
            // unused
            for (int i = 0; i < 256; i++)
            {
                this[0, 0].Data[((i) * 4)] = 0;
                this[0, 0].Data[((i) * 4) + 1] = 0;
                this[0, 0].Data[((i) * 4) + 2] = 0;
                this[0, 0].Data[((i) * 4) + 3] = 255;
                this[0, 0].Data[((i) * 4 + (1 * (256 * 4)))] = 0;
                this[0, 0].Data[((i) * 4 + (1 * (256 * 4))) + 1] = 0;
                this[0, 0].Data[((i) * 4 + (1 * (256 * 4))) + 2] = 0;
                this[0, 0].Data[((i) * 4 + (1 * (256 * 4))) + 3] = 255;
            }

            UpdateCurve(CurveType.Diffuse);
            UpdateCurve(CurveType.Specular);
            UpdateCurve(CurveType.Fresnel);
        }
    }
}
