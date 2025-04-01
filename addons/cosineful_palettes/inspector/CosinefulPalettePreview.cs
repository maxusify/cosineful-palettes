#if TOOLS && GODOT4_4_OR_GREATER

namespace CosinefulPalettes.Editor
{
    using Godot;

    public partial class CosinefulPalettePreview : Control
    {
        private TextureRect _textureRect = new() {
            CustomMinimumSize = new Vector2(0, 75f),
        };

        public CosinefulPalettePreview(CosinefulPalette cosinefulPalette)
        {
            _textureRect.Texture = new GradientTexture2D {
                Gradient = cosinefulPalette.GetColorsGradient()
            };

            AddChild(_textureRect);
            _textureRect.SetAnchorsPreset(LayoutPreset.FullRect);
        }

        public CosinefulPalettePreview()
        {
            var cosinefulPalette = new CosinefulPalette();

            _textureRect.Texture = new GradientTexture2D {
                Gradient = cosinefulPalette.GetColorsGradient()
            };

            AddChild(_textureRect);
            _textureRect.SetAnchorsPreset(LayoutPreset.FullRect);
        }
    }
}

#endif
