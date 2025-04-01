#if GODOT4_4_OR_GREATER
#nullable enable

namespace CosinefulPalettes.Utils
{
    using System;
    using System.Text;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Interface for exported properties for the Godot editor.
    /// </summary>
    public interface IEditorExportProperty
    {
        /// <summary>
        /// Builds the property data dictionary for use with <see cref="GodotObject._GetPropertyList()"/>.
        /// </summary>
        /// <returns></returns>
        GDC.Dictionary BuildPropertyData();

        /// <summary>
        /// Returns the current value of the property as a <see cref="Variant"/>.
        /// </summary>
        Variant GetValue();

        /// <summary>
        /// Sets the value of the property from a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>True if successful, false otherwise.</returns>
        bool SetValue(Variant value);
    }

    /// <summary>
    /// Interface for typed editor export properties.
    /// </summary>
    /// <typeparam name="TVariant">Type of the property value.</typeparam>
    public interface IEditorExportProperty<[MustBeVariant] TVariant> : IEditorExportProperty
    {
        /// <summary>
        /// Sets callback for getting the current value of the property as a <see cref="TVariant"/>.
        /// </summary>
        /// <param name="getter">Function to get the value.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> OnGet(Func<TVariant> getter);

        /// <summary>
        /// Sets callback for setting the value of the property from a <see cref="TVariant"/>.
        /// </summary>
        /// <param name="setter">Function to set the value.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> OnSet(Action<TVariant> setter);

        /// <summary>
        /// Makes the property read-only.
        /// </summary>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> ReadOnly();

        /// <summary>
        /// Makes the vector types link their component values.
        /// </summary>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> LinkVectorComponents();

        /// <summary>
        /// Makes numerical and vector values ranged.
        /// </summary>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> Range(
            float min,
            float max,
            float step = 0.01f,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = ""
        );

        /// <summary>
        /// Makes the property a multiline text field. Useful for long strings or multi-line descriptions.
        /// </summary>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> Multiline();

        /// <summary>
        /// Applies a placeholder to the text field. Useful for providing guidance or examples.
        /// </summary>
        /// <param name="placeholder">Text to display as a placeholder.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> Placeholder(string placeholder);

        /// <summary>
        /// Indicates that the set color value should not include an alpha channel.
        /// Useful for color properties that only require RGB values.
        /// </summary>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> NoAlpha();

        /// <summary>
        /// Marks this property as array of values of specified type.
        /// </summary>
        /// <typeparam name="T">Type of array values.</typeparam>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> Array<[MustBeVariant] T>();

        /// <summary>
        /// Makes the property a password input. Useful for secrets.
        /// </summary>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> Password();

        /// <summary>
        /// Indicates that this property should be <see cref="Callable"/> that is used
        /// as a button in editor inspector.
        /// </summary>
        /// <param name="label">Label of the button.</param>
        /// <param name="icon">Optional icon name.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> ToolButton(string label, string icon = "");

        /// <summary>
        /// Adds conditional requirement for this property to be visible or not. Useful
        /// for properties that depend on other values.
        /// </summary>
        /// <param name="requirement">Specified requirement.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> When(Func<bool> requirement);
    }

    public partial class EditorExportProperty<[MustBeVariant] TVariant>()
        : RefCounted, IEditorExportProperty<TVariant>
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public Variant.Type Type { get; set; }

        private Func<TVariant>? _getter;
        private Action<TVariant>? _setter;
        private Func<bool>? _requirement;

        private PropertyUsageFlags _usageFlags = PropertyUsageFlags.Default;
        private PropertyHint _hint = PropertyHint.None;
        private string? _hintString;

        private GDC.Dictionary? _propertyData;
        private bool _refreshNeeded;

        #endregion Properties
        #region Public Methods - Accessors

        public GDC.Dictionary BuildPropertyData()
        {
            if (_requirement is { } req && !req())
            {
                return [];
            }

            if (_propertyData is { } data)
            {
                return data;
            }

            _propertyData = new GDC.Dictionary {
                ["name"] = Name,
                ["type"] = (int)Type,
                ["hint"] = (int)_hint,
                ["hint_string"] = _hintString ?? string.Empty,
                ["usage"] = (int)_usageFlags
            };

            if (Type == Variant.Type.Object)
            {
                _propertyData["hint"] = (int)PropertyHint.ResourceType;
                _propertyData["hint_string"] = typeof(TVariant).Name;
            }

            return _propertyData;
        }

        public Variant GetValue()
        {
            return _getter is { } getter
            ? Variant.From(getter())
            : default;
        }

        public bool SetValue(Variant value)
        {
            if (_setter is { } setter)
            {
                setter(value.As<TVariant>());
                return true;
            }

            return false;
        }

        #endregion Public Methods - Accessors
        #region Public Methods - Setup

        public IEditorExportProperty<TVariant> When(Func<bool> requirement)
        {
            _requirement = requirement;
            return this;
        }

        public IEditorExportProperty<TVariant> OnGet(Func<TVariant> getter)
        {
            _getter = getter;
            return this;
        }

        public IEditorExportProperty<TVariant> OnSet(Action<TVariant> setter)
        {
            _setter = setter;
            return this;
        }

        public IEditorExportProperty<TVariant> ReadOnly()
        {
            _usageFlags |= PropertyUsageFlags.ReadOnly;
            return this;
        }

        public IEditorExportProperty<TVariant> Range(
            float min,
            float max,
            float step = 0.01f,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = ""
        )
        {
            _hint = PropertyHint.Range;

            StringBuilder sb = new();
            _ = sb.Append($"{min}, {max}");

            if (step != 0.1f)
            {
                _ = sb.Append($", {step}");
            }

            if (exponential)
            {
                _ = sb.Append(", exp");
            }

            if (orGreater)
            {
                _ = sb.Append(", or_greater");
            }

            if (orLess)
            {
                _ = sb.Append(", or_less");
            }

            if (radiansAsDegrees)
            {
                _ = sb.Append(", radians_as_degrees");
            }

            if (degrees)
            {
                _ = sb.Append(", degrees");
            }

            if (hideSlider)
            {
                _ = sb.Append(", hide_slider");
            }

            if (!string.IsNullOrEmpty(suffix))
            {
                _ = sb.Append($", suffix:{suffix}");
            }

            _hintString = sb.ToString();

            return this;
        }

        public IEditorExportProperty<TVariant> LinkVectorComponents()
        {
            _hint = PropertyHint.Link;
            return this;
        }

        public IEditorExportProperty<TVariant> Multiline()
        {
            _hint = PropertyHint.MultilineText;
            return this;
        }

        public IEditorExportProperty<TVariant> Placeholder(string placeholder)
        {
            _hint = PropertyHint.PlaceholderText;
            _hintString = placeholder;
            return this;
        }

        public IEditorExportProperty<TVariant> NoAlpha()
        {
            _hint = PropertyHint.ColorNoAlpha;
            return this;
        }

        public IEditorExportProperty<TVariant> Array<[MustBeVariant] T>()
        {
            _hint = PropertyHint.ArrayType;
            _hintString = typeof(T).Name;
            return this;
        }

        public IEditorExportProperty<TVariant> Password()
        {
            _hint = PropertyHint.Password;
            return this;
        }

        public IEditorExportProperty<TVariant> ToolButton(string label, string icon = "")
        {
            _hint = PropertyHint.ToolButton;
            _hintString = string.IsNullOrEmpty(icon) ? label : $"{label},{icon}";
            return this;
        }

        #endregion Public Methods - Setup
    }
}

#endif
