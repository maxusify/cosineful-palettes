namespace CosinefulPalettes.Vendor.ExportForge
{
    using System;
    using System.Globalization;
    using System.Text;

    using CosinefulPalettes.Vendor.ExportForge.Utils;

    using Godot;

    /// <summary>
    /// Extensions for numeric properties.
    /// </summary>
    public static class EditorExportPropertyNumericExtensions
    {
        public const double DEFAULT_STEP_DOUBLE = 0.01;
        public const float DEFAULT_STEP_FLOAT = 0.01f;
        public const int DEFAULT_STEP_INT = 1;
        public const long DEFAULT_STEP_LONG = 1;

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Link(this EditorExportProperty<Vector2> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Link(this EditorExportProperty<Vector2I> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Link(this EditorExportProperty<Vector3> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Link(this EditorExportProperty<Vector3I> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Link(this EditorExportProperty<Vector4> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Link(this EditorExportProperty<Vector4I> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
        }

        /// <summary>
        /// Makes integer value ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <param name="preferSlider">Shows the slider, which integer properties hide by default.</param>
        public static void Range(
            this EditorExportProperty<int> property,
            int min,
            int max,
            int step = DEFAULT_STEP_INT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = "",
            bool preferSlider = false
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, preferSlider);
        }

        /// <summary>
        /// Makes long integer value ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <param name="preferSlider">Shows the slider, which integer properties hide by default.</param>
        public static void Range(
            this EditorExportProperty<long> property,
            long min,
            long max,
            long step = DEFAULT_STEP_LONG,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = "",
            bool preferSlider = false
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, preferSlider);
        }

        /// <summary>
        /// Makes float value ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        public static void Range(
            this EditorExportProperty<float> property,
            float min,
            float max,
            float step = DEFAULT_STEP_FLOAT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = ""
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, false);
        }

        /// <summary>
        /// Makes double value ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        public static void Range(
            this EditorExportProperty<double> property,
            double min,
            double max,
            double step = DEFAULT_STEP_DOUBLE,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = ""
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, false);
        }

        /// <summary>
        /// Makes <see cref="Vector2"/> components values ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        public static void Range(
            this EditorExportProperty<Vector2> property,
            float min,
            float max,
            float step = DEFAULT_STEP_FLOAT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = ""
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, false);
        }

        /// <summary>
        /// Makes <see cref="Vector2I"/> components values ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <param name="preferSlider">Shows the slider, which integer properties hide by default.</param>
        public static void Range(
            this EditorExportProperty<Vector2I> property,
            int min,
            int max,
            int step = DEFAULT_STEP_INT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = "",
            bool preferSlider = false
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, preferSlider);
        }

        /// <summary>
        /// Makes <see cref="Vector3"/> components values ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        public static void Range(
            this EditorExportProperty<Vector3> property,
            float min,
            float max,
            float step = DEFAULT_STEP_FLOAT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = ""
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, false);
        }

        /// <summary>
        /// Makes <see cref="Vector3I"/> components values ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <param name="preferSlider">Shows the slider, which integer properties hide by default.</param>
        public static void Range(
            this EditorExportProperty<Vector3I> property,
            int min,
            int max,
            int step = DEFAULT_STEP_INT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = "",
            bool preferSlider = false
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, preferSlider);
        }

        /// <summary>
        /// Makes <see cref="Vector4"/> components values ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        public static void Range(
            this EditorExportProperty<Vector4> property,
            float min,
            float max,
            float step = DEFAULT_STEP_FLOAT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = ""
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, false);
        }

        /// <summary>
        /// Makes <see cref="Vector4I"/> components values ranged.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="step">Step value.</param>
        /// <param name="exponential">Editing in exponential scale.</param>
        /// <param name="orGreater">Greater values allowed.</param>
        /// <param name="orLess">Lesser values allowed.</param>
        /// <param name="radiansAsDegrees">Value is stored in radians, but edited in degrees. Range values are in degrees.</param>
        /// <param name="degrees">Hints that the value is an angle in degrees.</param>
        /// <param name="hideSlider">Hides the slider or up/down arrows.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <param name="preferSlider">Shows the slider, which integer properties hide by default.</param>
        public static void Range(
            this EditorExportProperty<Vector4I> property,
            int min,
            int max,
            int step = DEFAULT_STEP_INT,
            bool exponential = false,
            bool orGreater = false,
            bool orLess = false,
            bool radiansAsDegrees = false,
            bool degrees = false,
            bool hideSlider = false,
            string suffix = "",
            bool preferSlider = false
        )
        {
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix, preferSlider);
        }

        /// <summary>
        /// Property is treated as bitmask. Useful for flags or options.
        /// </summary>
        /// <typeparam name="TFlags">Flags type.</typeparam>
        /// <param name="property">Property.</param>
        public static void Flags<TFlags>(this EditorExportProperty<int> property)
            where TFlags : struct, Enum
        {
            property.SetPropertyHint(PropertyHint.Flags, VariantTypes.GetFlagsHintString(typeof(TFlags)));
        }

        /// <summary>
        /// Property is treated as bitmask. Useful for flags or options.
        /// </summary>
        /// <typeparam name="TFlags">Flags type.</typeparam>
        /// <param name="property">Property.</param>
        public static void Flags<TFlags>(this EditorExportProperty<long> property)
            where TFlags : struct, Enum
        {
            property.SetPropertyHint(PropertyHint.Flags, VariantTypes.GetFlagsHintString(typeof(TFlags)));
        }

        /// <summary>
        /// Property will be treated as enum.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="hint">Hint that describes enum. Example: "Egg,Hen,Chicken".</param>
        public static void AsEnum(this EditorExportProperty<int> property, string hint)
        {
            property.SetPropertyHint(PropertyHint.Enum, hint);
        }

        /// <summary>
        /// Property will be treated as enum.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="hint">Hint that describes enum. Example: "Egg,Hen,Chicken".</param>
        public static void AsEnum(this EditorExportProperty<long> property, string hint)
        {
            property.SetPropertyHint(PropertyHint.Enum, hint);
        }

        #region Private Methods

        private static void ApplyRange<[MustBeVariant] TVariant, TNumber>(
            this EditorExportProperty<TVariant> property,
            TNumber min,
            TNumber max,
            TNumber step,
            bool exponential,
            bool orGreater,
            bool orLess,
            bool radiansAsDegrees,
            bool degrees,
            bool hideSlider,
            string suffix,
            bool preferSlider
        )
            where TNumber : IFormattable
        {
            // Godot expects "." as decimal separator, regardless of the system culture.
            var sb = new StringBuilder()
                .Append(min.ToString(null, CultureInfo.InvariantCulture)).Append(',')
                .Append(max.ToString(null, CultureInfo.InvariantCulture)).Append(',')
                .Append(step.ToString(null, CultureInfo.InvariantCulture));

            if (exponential)
            {
                sb.Append(",exp");
            }

            if (orGreater)
            {
                sb.Append(",or_greater");
            }

            if (orLess)
            {
                sb.Append(",or_less");
            }

            if (radiansAsDegrees)
            {
                sb.Append(",radians_as_degrees");
            }

            if (degrees)
            {
                sb.Append(",degrees");
            }

            if (hideSlider)
            {
                sb.Append(",hide_control");
            }

            if (preferSlider)
            {
                sb.Append(",prefer_slider");
            }

            if (!string.IsNullOrEmpty(suffix))
            {
                sb.Append(",suffix:").Append(suffix);
            }

            property.SetPropertyHint(PropertyHint.Range, sb.ToString());
        }

        #endregion Private Methods
    }
}
