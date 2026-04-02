namespace CosinefulPalettes
{
    using System;
    using System.Collections.Generic;

    using CosinefulPalettes.Utils;
    using CosinefulPalettes.Utils.ExportForge;

    using Godot;

    using static Godot.Mathf;

    using GDC = Godot.Collections;

    /// <summary>
    /// Interface for cosineful palettes.
    /// </summary>
    public interface ICosinefulPalette
    {
        /// <summary>
        /// Emitted when a new color palette is generated.
        /// </summary>
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
        /// Current seed for randomization.
        /// </summary>
        int Seed { get; }
        /// <summary>
        /// Determines if the generation is seed based or not. If components are set manually, this should
        /// be set to <c>false</c>.
        /// </summary>
        bool UseSeed { get; }

        /// <summary>
        /// Randomizes the color palette.
        /// </summary>
        /// <param name="seed">Seed for randomization.</param>
        void Randomize(int seed = -1);
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
            set => _colorPalette = value;
        }

        private int OutputGradientColorCount
        {
            get => _colorPaletteColorCount;
            set {
                _colorPaletteColorCount = value;
                _GenerateForEditor();
            }
        }

        public Vector3 Brightness
        {
            get => _brightness;
            set {
                _brightness = value;
                UseSeed = false;
            }
        }

        public Vector3 Contrast
        {
            get => _contrast;
            set {
                _contrast = value;
                UseSeed = false;
            }
        }

        public Vector3 Frequency
        {
            get => _frequency;
            set {
                _frequency = value;
                UseSeed = false;
            }
        }

        public Vector3 Range
        {
            get => _range;
            set {
                _range = value;
                UseSeed = false;
            }
        }

        public int Seed
        {
            get => _seed;
            private set
            {
                _seed = value;
                UseSeed = true;
            }
        }

        public bool UseSeed
        {
            get => _useSeed;
            private set => _useSeed = value;
        }

        private Gradient _colorPalette = new();

        private int _colorPaletteColorCount = 100;

        private int _seed = Random.Shared.Next();
        private bool _useSeed = true;

        private Vector3 _brightness = new(0.5f, 0.5f, 0.5f);
        private Vector3 _contrast = new(0.5f, 0.5f, 0.5f);
        private Vector3 _frequency = new(1.0f, 1.0f, 1.0f);
        private Vector3 _range = new(0.0f, 0.33f, 0.67f);

        private int _lastSeed = -1;

        private readonly Stack<int> _prevSeeds = [];
        private readonly Stack<int> _nextSeeds = [];

        private readonly EditorExportForge _forge;

        #region Constructor

        public CosinefulPalette()
        {
            _forge = new EditorExportForge(this);

            // Produced output
            _ = _forge
                    .CreateProperty<Gradient>("Palette Preview")
                    .OnGet(() => OutputGradient)
                    .OnSet(value => OutputGradient = value)
                    .ReadOnly();

            _ = _forge
                    .CreateProperty<int>("Color Count")
                    .OnGet(() => OutputGradientColorCount)
                    .OnSet(value => OutputGradientColorCount = value)
                    .ReadOnly();

            // Seed
            _ = _forge
                    .CreateProperty<int>("Seed")
                    .When(() => UseSeed)
                    .OnGet(() => Seed)
                    .OnSet(value =>
                    {
                        _nextSeeds.Clear();

                        if (Seed != -1)
                        {
                            _prevSeeds.Push(Seed);
                        }

                        Seed = value;
                    });

            _ = _forge
                    .CreateProperty<bool>("Seed Based Generation")
                    .OnGet(() => UseSeed)
                    .OnSet(value => UseSeed = value);

            // Buttons
            _ = _forge
                    .CreateProperty<Callable>("Refresh Palette")
                    .OnGet(() => Callable.From(_GenerateForEditor))
                    .ToolButton("Refresh Palette", "Color");

            // Components
            const float min = 0f;
            const float max = 1f;
            const float step = 0.001f;
            const bool orGreater = true;
            const bool orLess = true;

            _ = _forge
                    .CreateProperty<Vector3>("Components/Brightness")
                    .OnGet(() => Brightness)
                    .OnSet(value => Brightness = value)
                    .Range(min, max, step, orGreater: orGreater, orLess: orLess);

            _ = _forge
                    .CreateProperty<Vector3>("Components/Contrast")
                    .OnGet(() => Contrast)
                    .OnSet(value => Contrast = value)
                    .Range(min, max, step, orGreater: orGreater, orLess: orLess);

            _ = _forge
                    .CreateProperty<Vector3>("Components/Frequency")
                    .OnGet(() => Frequency)
                    .OnSet(value => Frequency = value)
                    .Range(min, max, step, orGreater: orGreater, orLess: orLess);

            _ = _forge
                    .CreateProperty<Vector3>("Components/Range")
                    .OnGet(() => Range)
                    .OnSet(value => Range = value)
                    .Range(min, max, step, orGreater: orGreater, orLess: orLess);

            // Randomize

            _ = _forge
                    .CreateProperty<Callable>("Components/Randomize")
                    .When(() => UseSeed)
                    .OnGet(() => Callable.From(_ButtonRandomize))
                    .ToolButton("Randomize", "RandomNumberGenerator");

            _ = _forge
                    .CreateProperty<Callable>("Components/Previous")
                    .When(() => _prevSeeds.Count > 0 && UseSeed)
                    .OnGet(() => Callable.From(_ButtonPreviousSeed))
                    .ToolButton("Previous Seed", "GuiTreeArrowLeft");

            _ = _forge
                    .CreateProperty<Callable>("Components/Next")
                    .When(() => _nextSeeds.Count > 0 && UseSeed)
                    .OnGet(() => Callable.From(_ButtonNextSeed))
                    .ToolButton("Next Seed", "GuiTreeArrowRight");

            _GenerateForEditor();
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

        /// <summary>
        /// Creates new color palette based on the cosine formula with random values.
        /// </summary>
        /// <returns>Color palette.</returns>
        public static CosinefulPalette Create()
        {
            var palette = new CosinefulPalette();
            palette.Randomize();

            return palette;
        }

        #endregion Constructor
        #region Export Logic

        public override GDC.Array<GDC.Dictionary> _GetPropertyList()
        {
            return _forge.ForgeProperties();
        }

        public override Variant _Get(StringName property)
        {
            return _forge.HandleGetter(property);
        }

        public override bool _Set(StringName property, Variant value)
        {
            return _forge.HandleSetter(property, value);
        }

        #endregion Export Logic
        #region Public Methods

        public void Randomize(int seed = -1)
        {
            if (Engine.IsEditorHint())
            {
                _nextSeeds.Clear();

                if (Seed != -1)
                {
                    _prevSeeds.Push(Seed);
                }
            }

            Seed = seed;
            UseSeed = true;
            _RandomizeInternal();
        }

        public Gradient GetColorsGradient()
        {
            _Generate();

            return OutputGradient;
        }

        public Color[] GetColorsArray()
        {
            _Generate();

            return OutputGradient.Colors;
        }

        public Color GetColor(float offset)
        {
            return GetColorsGradient().Sample(offset);
        }

        public Color GetColor(int index)
        {
            var gradient = GetColorsGradient();

            return gradient.GetColor(Math.Clamp(index, 0, gradient.Colors.Length - 1));
        }

        #endregion Public Methods
        #region Private Methods

        private void _ButtonPreviousSeed()
        {
            if (_prevSeeds.Count == 0)
            {
                return;
            }

            if (Seed != -1)
            {
                _nextSeeds.Push(Seed);
            }

            Seed = _prevSeeds.Pop();

            _RandomizeInternal();
            _GenerateForEditor();
            NotifyPropertyListChanged();
        }

        private void _ButtonNextSeed()
        {
            if (_nextSeeds.Count == 0)
            {
                return;
            }

            if (Seed != -1)
            {
                _prevSeeds.Push(Seed);
            }

            Seed = _nextSeeds.Pop();

            _RandomizeInternal();
            _GenerateForEditor();
            NotifyPropertyListChanged();
        }

        private void _ButtonRandomize()
        {
            if (Seed != -1)
            {
                _prevSeeds.Push(Seed);
            }

            _nextSeeds.Clear();

            Seed = Random.Shared.Next();

            _RandomizeInternal();
            _GenerateForEditor();
            NotifyPropertyListChanged();
        }

        private void _RandomizeInternal()
        {
            if (Seed == _lastSeed)
            {
                return;
            }

            _lastSeed = Seed;

            var random = new Random(Seed);

            _brightness = getRandomizedVector();
            _contrast = getRandomizedVector();
            _frequency = getRandomizedVector();
            _range = getRandomizedVector();

            NotifyPropertyListChanged();

            return;

            Vector3 getRandomizedVector()
            {
                return new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle());
            }
        }

        private void _GenerateForEditor()
        {
            if (!Engine.IsEditorHint())
            {
                return;
            }

            _Generate();
        }

        private void _Generate()
        {
            var offsets = new List<float>(OutputGradientColorCount);

            for (var i = 0; i < OutputGradientColorCount; i++)
            {
                offsets.Add((float)i / (OutputGradientColorCount - 1));
            }

            if (UseSeed)
            {
                _RandomizeInternal();
            }

            OutputGradient = new Gradient {
                Colors = _Generate_CosineFormula(),
                Offsets = [.. offsets]
            };

            EmitSignal(SignalName.PaletteGenerated);
        }

        private Color[] _Generate_CosineFormula()
        {
            var colorCount = OutputGradientColorCount;
            var brightness = Brightness;
            var contrast = Contrast;
            var freq = Frequency;
            var range = Range;

            if (colorCount <= 0)
            {
                return [];
            }

            if (colorCount == 1)
            {
                return [genColor(1, 1)];
            }

            var colors = new Color[colorCount];

            for (var i = 0; i < colorCount; i++)
            {
                colors[i] = genColor(i, colorCount - 1);
            }

            return colors;

            Color genColor(int index, int maxIndex)
            {
                float idx = Max(index, 1);
                float nc = Max(maxIndex, 1);

                return new Color(
                    brightness.X + (contrast.X * Cos(Tau * ((freq.X * (idx / nc)) + range.X))),
                    brightness.Y + (contrast.Y * Cos(Tau * ((freq.Y * (idx / nc)) + range.Y))),
                    brightness.Z + (contrast.Z * Cos(Tau * ((freq.Z * (idx / nc)) + range.Z)))
                );
            }
        }

        #endregion Private Methods
    }
}
