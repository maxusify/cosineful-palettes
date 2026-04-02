namespace CosinefulPalettes.Utils.ExportForge
{
    using System;
    using System.Text;

    using Godot;

    /// <summary>
    /// Extensions for numeric properties.
    /// </summary>
    public static class EditorExportPropertyNumericExtensions
    {
        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Vector2> Link(this IEditorExportProperty<Vector2> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
            return property;
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Vector2I> Link(this IEditorExportProperty<Vector2I> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
            return property;
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Vector3> Link(this IEditorExportProperty<Vector3> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
            return property;
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Vector3I> Link(this IEditorExportProperty<Vector3I> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
            return property;
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Vector4> Link(this IEditorExportProperty<Vector4> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
            return property;
        }

        /// <summary>
        /// Allows vector to have linked values when edited in the editor.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Vector4I> Link(this IEditorExportProperty<Vector4I> property)
        {
            property.SetPropertyHint(PropertyHint.Link);
            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<int> Range(
            this IEditorExportProperty<int> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<float> Range(
            this IEditorExportProperty<float> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<Vector2> Range(
            this IEditorExportProperty<Vector2> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<Vector2I> Range(
            this IEditorExportProperty<Vector2I> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<Vector3> Range(
            this IEditorExportProperty<Vector3> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<Vector3I> Range(
            this IEditorExportProperty<Vector3I> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<Vector4> Range(
            this IEditorExportProperty<Vector4> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
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
        /// <param name="radiansAsDegrees">Treats value as degrees and converts to radians.</param>
        /// <param name="degrees">Treats value as degrees.</param>
        /// <param name="hideSlider">Hides slider.</param>
        /// <param name="suffix">Optional suffix.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<Vector4I> Range(
            this IEditorExportProperty<Vector4I> property,
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
            property.ApplyRange(
                min, max, step, exponential, orGreater, orLess, radiansAsDegrees, degrees, hideSlider, suffix);

            return property;
        }

        /// <summary>
        /// Property is treated as bitmask. Useful for flags or options.
        /// </summary>
        /// <typeparam name="TFlags">Flags type.</typeparam>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<int> Flags<TFlags>(this IEditorExportProperty<int> property)
            where TFlags : struct, Enum
        {
            var hintStringBuilder = new StringBuilder();
            var flagNames = Enum.GetNames<TFlags>();

            for (var i = 0; i < flagNames.Length; i++)
            {
                hintStringBuilder.Append($"{flagNames[i]}").Append(i < flagNames.Length - 1 ? ',' : string.Empty);
            }

            property.SetPropertyHint(PropertyHint.Flags, hintStringBuilder.ToString());
            return property;
        }

        /// <summary>
        /// Property will be treated as enum.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="hint">Hint that describes enum. Example: "Egg,Hen,Chicken".</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<int> AsEnum(this IEditorExportProperty<int> property, string hint)
        {
            property.SetPropertyHint(PropertyHint.Enum, hint);
            return property;
        }

        #region Private Methods

        private static void ApplyRange<[MustBeVariant] TVariant>(
            this IEditorExportProperty<TVariant> property,
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
            var sb = new StringBuilder();

            sb.Append($"{min}, {max}");

            if (step != 0.1f)
            {
                sb.Append($", {step}");
            }

            if (exponential)
            {
                sb.Append(", exp");
            }

            if (orGreater)
            {
                sb.Append(", or_greater");
            }

            if (orLess)
            {
                sb.Append(", or_less");
            }

            if (radiansAsDegrees)
            {
                sb.Append(", radians_as_degrees");
            }

            if (degrees)
            {
                sb.Append(", degrees");
            }

            if (hideSlider)
            {
                sb.Append(", hide_slider");
            }

            if (!string.IsNullOrEmpty(suffix))
            {
                sb.Append($", suffix:{suffix}");
            }

            property.SetPropertyHint(PropertyHint.Range, sb.ToString());
        }

        #endregion Private Methods
    }
}
