#if TOOLS && GODOT4_4_OR_GREATER

namespace CosinefulPalettes
{
    using System;
    using System.Collections.Generic;

    using CosinefulPalettes.Utils;

    using Godot;

    using static Godot.Mathf;

    using GDC = Godot.Collections;

    /// <summary>
    /// Interface for cosineful palettes.
    /// </summary>
    public interface ICosinefulPalette
    {
        event CosinefulPalette.PaletteGeneratedEventHandler PaletteGenerated;

        /// <summary>
        /// Color palette brightness.
        /// </summary>
        Vector3 Brightness { get; set; }

        /// <summary>
        /// Color palette contrast.
        /// </summary>
        Vector3 Contrast { get; set; }

        /// <summary>
        /// Color palette frequency of changing colors.
        /// </summary>
        Vector3 Frequency { get; set; }

        /// <summary>
        /// Color palette range of picking colors.
        /// </summary>
        Vector3 Range { get; set; }

        /// <summary>
        /// Randomizes the color palette.
        /// </summary>
        void Randomize();

        /// <summary>
        /// Returns color palette as <see cref="Gradient"/>.
        /// </summary>
        /// <returns><see cref="Gradient"/>.</returns>
        Gradient GetColorsGradient();

        /// <summary>
        /// Returns color palette as array of colors.
        /// </summary>
        /// <returns>Array of colors.</returns>
        Color[] GetColorsArray();

        /// <summary>
        /// Returns color sample of the palette at given offset in range [0, 1].
        /// </summary>
        /// <param name="offset">Offset in range [0, 1].</param>
        /// <returns>Color found at given offset.</returns>
        Color GetColor(float offset);

        /// <summary>
        /// Returns color at given index. Index value is clamped from 0
        /// to gradient colors length.
        /// </summary>
        /// <param name="index">Color index.</param>
        /// <returns>Color at given index.</returns>
        Color GetColor(int index);
    }

    /// <summary>
    /// Resource for generating color palette with cosine formula.
    /// </summary>
    [Tool, GlobalClass, Icon("../icon.svg")]
    public partial class CosinefulPalette : Resource, ICosinefulPalette
    {
        [Signal] public delegate void PaletteGeneratedEventHandler();

        private Gradient OutputGradient
        {
            get => _colorPalette;
            set {
                _colorPalette = value;
                NotifyPropertyListChanged();
            }
        }

        private int OutputGradientColorCount
        {
            get => _colorPaletteColorCount;
            set {
                _colorPaletteColorCount = value;
                GenerateForEditor();
                NotifyPropertyListChanged();
            }
        }

        public Vector3 Brightness
        {
            get => _brightness;
            set {
                _brightness = value;
                NotifyPropertyListChanged();
            }
        }

        public Vector3 Contrast
        {
            get => _constrast;
            set {
                _constrast = value;
                NotifyPropertyListChanged();
            }
        }

        public Vector3 Frequency
        {
            get => _frequency;
            set {
                _frequency = value;
                NotifyPropertyListChanged();
            }
        }

        public Vector3 Range
        {
            get => _range;
            set {
                _range = value;
                NotifyPropertyListChanged();
            }
        }

        private Gradient _colorPalette = new();
        private int _colorPaletteColorCount = 100;
        private Vector3 _brightness = new(0.5f, 0.5f, 0.5f);
        private Vector3 _constrast = new(0.5f, 0.5f, 0.5f);
        private Vector3 _frequency = new(1.0f, 1.0f, 1.0f);
        private Vector3 _range = new(0.0f, 0.33f, 0.67f);

        private readonly EditorExportBuilder _export;

        #region Constructor

        public CosinefulPalette()
        {
            _export = new EditorExportBuilder();

            // Produced output

            _ = _export
                .CreateProperty<Gradient>("Palette Preview")
                .OnGet(() => OutputGradient)
                .OnSet((value) => {
                    OutputGradient = value;
                    GenerateForEditor();
                })
                .ReadOnly();

            _ = _export
                .CreateProperty<int>("Color Count")
                .OnGet(() => _colorPaletteColorCount)
                .OnSet((value) => {
                    _colorPaletteColorCount = value;
                    GenerateForEditor();
                })
                .ReadOnly();

            // Buttons

            _ = _export
                .CreateProperty<Callable>("Refresh Palette")
                .OnGet(() => Callable.From(GenerateForEditor))
                .ToolButton("Refresh Palette", "Color");

            // Brightness

            _ = _export
                .CreateProperty<Vector3>("Components/Brightness")
                .OnGet(() => _brightness)
                .OnSet((value) => _brightness = value)
                .Range(0f, 1f, 0.001f);

            // Contrast

            _ = _export
                .CreateProperty<Vector3>("Components/Contrast")
                .OnGet(() => _constrast)
                .OnSet((value) => _constrast = value)
                .Range(0f, 1f, 0.001f);

            // Frequency

            _ = _export
                .CreateProperty<Vector3>("Components/Frequency")
                .OnGet(() => _frequency)
                .OnSet((value) => _frequency = value)
                .Range(0f, 1f, 0.001f);

            // Range

            _ = _export
                .CreateProperty<Vector3>("Components/Range")
                .OnGet(() => _range)
                .OnSet((value) => _range = value)
                .Range(0f, 1f, 0.001f);

            // Randomize

            _ = _export
                .CreateProperty<Callable>("Components/Randomize")
                .OnGet(() => Callable.From(Randomize))
                .ToolButton("Randomize", "RandomNumberGenerator");

            GenerateForEditor();
        }

        /// <summary>
        /// Creates new color palette based on the cosine formula.
        /// </summary>
        /// <param name="brightness">Palette brightness.</param>
        /// <param name="contrast">Palette contrast.</param>
        /// <param name="frequency">Frequency of the palette color change.</param>
        /// <param name="range">Range of the color palette color picks.</param>
        /// <returns>Color palette.</returns>
        public static CosinefulPalette Create(
            Vector3 brightness,
            Vector3 contrast,
            Vector3 frequency,
            Vector3 range)
        {
            return new CosinefulPalette {
                Brightness = brightness,
                Contrast = contrast,
                Frequency = frequency,
                Range = range
            };
        }

        #endregion Constructor
        #region Export Logic

        public override GDC.Array<GDC.Dictionary> _GetPropertyList()
            => _export.GetProperties();

        public override Variant _Get(StringName property)
            => _export.HandleGetter(property);

        public override bool _Set(StringName property, Variant value)
            => _export.HandleSetter(property, value);

        #endregion Export Logic
        #region Public Methods

        public void Randomize()
        {
            Brightness = GetRandomizedVector();
            Contrast = GetRandomizedVector();
            Frequency = GetRandomizedVector();
            Range = GetRandomizedVector();

            Vector3 GetRandomizedVector() => new(
                Random.Shared.NextSingle(),
                Random.Shared.NextSingle(),
                Random.Shared.NextSingle()
            );

            GenerateForEditor();
        }

        public Gradient GetColorsGradient()
        {
            Generate();
            return OutputGradient;
        }

        public Color[] GetColorsArray()
        {
            Generate();
            return OutputGradient.Colors;
        }

        public Color GetColor(float offset) => GetColorsGradient().Sample(offset);

        public Color GetColor(int index)
        {
            var gradient = GetColorsGradient();

            return gradient.GetColor(Math.Clamp(index, 0, gradient.Colors.Length - 1));
        }

        #endregion Public Methods
        #region Private Methods

        private void GenerateForEditor()
        {
            if (!Engine.IsEditorHint())
            {
                return;
            }

            Generate();
        }

        private void Generate()
        {
            List<float> offsets = [];

            for (var i = 0; i < OutputGradientColorCount; i++)
            {
                offsets.Add((float)i / (OutputGradientColorCount - 1));
            }

            OutputGradient = new Gradient() {
                Colors = CosineFormulaGeneratePalette(),
                Offsets = [.. offsets]
            };

            EmitSignal(SignalName.PaletteGenerated);
        }

        private Color[] CosineFormulaGeneratePalette()
        {
            var colorCount = OutputGradientColorCount;
            var brightness = Brightness;
            var contrast = Contrast;
            var freq = Frequency;
            var range = Range;

            switch (colorCount)
            {
                // If color count is zero or less, return empty array
                case <= 0:
                    return [];

                // If color count is one, return a single color
                case 1:
                    return [new Color(
                            brightness.X + (contrast.X * Cos(Tau * (freq.X + range.X))),
                            brightness.Y + (contrast.Y * Cos(Tau * (freq.Y + range.Y))),
                            brightness.Z + (contrast.Z * Cos(Tau * (freq.Z + range.Z)))
                        )];
                default:
                    break;
            }

            var colors = new Color[colorCount];
            var n = Max((float)colorCount - 1, 1f);

            for (var i = 0; i < colorCount; i++)
            {
                colors[i] = new Color(
                    brightness.X + (contrast.X * Cos(Tau * ((freq.X * (i / n)) + range.X))),
                    brightness.Y + (contrast.Y * Cos(Tau * ((freq.Y * (i / n)) + range.Y))),
                    brightness.Z + (contrast.Z * Cos(Tau * ((freq.Z * (i / n)) + range.Z)))
                );
            }

            return colors;
        }

        #endregion Private Methods
    }
}
#endif
