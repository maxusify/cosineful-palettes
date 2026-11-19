#if TOOLS

namespace CosinefulPalettes.Editor
{
    using Godot;

    /// <summary>
    /// Live preview of a palette: the generated gradient plus its per-channel curves.
    /// Refreshes whenever the palette emits <see cref="CosinefulPalette.PaletteGenerated"/>.
    /// </summary>
    [Tool]
    public partial class CosinefulPalettePreview : VBoxContainer
    {
        private readonly CosinefulPalette _palette;
        private readonly GradientTexture2D _texture = new() {
            Width = 256,
            Height = 1
        };
        private readonly CosinefulPaletteCurves _curves;

        public CosinefulPalettePreview() : this(new CosinefulPalette())
        {
        }

        public CosinefulPalettePreview(CosinefulPalette cosinefulPalette)
        {
            _palette = cosinefulPalette;

            AddChild(new TextureRect {
                Texture = _texture,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.Scale,
                CustomMinimumSize = new Vector2(0f, 40f)
            });

            _curves = new CosinefulPaletteCurves(cosinefulPalette);
            AddChild(_curves);
        }

        public override void _EnterTree()
        {
            _palette.PaletteGenerated += OnPaletteGenerated;
            OnPaletteGenerated();
        }

        public override void _ExitTree()
        {
            _palette.PaletteGenerated -= OnPaletteGenerated;
        }

        private void OnPaletteGenerated()
        {
            _texture.Gradient = _palette.CurrentGradient;
            _curves.QueueRedraw();
        }
    }
}

#endif
