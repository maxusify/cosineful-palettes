#if TOOLS

namespace CosinefulPalettes.Editor
{
    using Godot;

    /// <summary>
    /// Plots the raw R, G and B cosine curves of a palette over offset [0, 1].
    /// Dashed lines mark 0 and 1; anything outside them gets clipped in the final colors.
    /// </summary>
    [Tool]
    public partial class CosinefulPaletteCurves : Control
    {
        private const int SAMPLES = 128;
        private const float PADDING = 4f;

        private static readonly string[] ChannelColorNames = ["property_color_x", "property_color_y", "property_color_z"];

        private readonly CosinefulPalette _palette;
        private readonly Vector3[] _values = new Vector3[SAMPLES];
        private readonly Vector2[] _points = new Vector2[SAMPLES];

        public CosinefulPaletteCurves() : this(new CosinefulPalette())
        {
        }

        public CosinefulPaletteCurves(CosinefulPalette palette)
        {
            _palette = palette;
            CustomMinimumSize = new Vector2(0f, 96f);
            ClipContents = true;
        }

        public override void _Draw()
        {
            var size = Size;

            DrawRect(new Rect2(Vector2.Zero, size), new Color(0f, 0f, 0f, 0.25f));

            // Sample once and widen the plotted range if any channel leaves [0, 1].
            var lo = 0f;
            var hi = 1f;

            for (var s = 0; s < SAMPLES; s++)
            {
                var v = _palette.Evaluate((float)s / (SAMPLES - 1));
                _values[s] = v;
                lo = Mathf.Min(lo, Mathf.Min(v.X, Mathf.Min(v.Y, v.Z)));
                hi = Mathf.Max(hi, Mathf.Max(v.X, Mathf.Max(v.Y, v.Z)));
            }

            var top = PADDING;
            var bottom = size.Y - PADDING;
            var width = size.X;

            float toY(float value) => Mathf.Remap(value, lo, hi, bottom, top);

            var guide = new Color(1f, 1f, 1f, 0.25f);
            DrawDashedLine(new Vector2(0f, toY(0f)), new Vector2(width, toY(0f)), guide, 1f, 4f);
            DrawDashedLine(new Vector2(0f, toY(1f)), new Vector2(width, toY(1f)), guide, 1f, 4f);

            for (var channel = 0; channel < 3; channel++)
            {
                for (var s = 0; s < SAMPLES; s++)
                {
                    var x = width * s / (SAMPLES - 1);
                    _points[s] = new Vector2(x, toY(_values[s][channel]));
                }

                DrawPolyline(_points, GetThemeColor(ChannelColorNames[channel], "Editor"), 2f, true);
            }
        }
    }
}

#endif
