namespace CosinefulPalettes.Utils.ExportForge
{
    using Godot;

    /// <summary>
    /// Extensions for exported string properties.
    /// </summary>
    public static class EditorExportPropertyStringExtensions
    {
        /// <summary>
        /// Makes the property a multiline text field. Useful for long strings or multi-line descriptions.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<string> Multiline(this IEditorExportProperty<string> property)
        {
            property.SetPropertyHint(PropertyHint.MultilineText);
            return property;
        }

        /// <summary>
        /// Makes the property a password input. Useful for secrets.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<string> Password(this IEditorExportProperty<string> property)
        {
            property.SetPropertyHint(PropertyHint.Password);
            return property;
        }

        /// <summary>
        /// Applies a placeholder to the text field. Useful for providing guidance or examples.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="placeholder">Text to display as a placeholder.</param>
        /// <returns>Self.</returns>
        public static IEditorExportProperty<string> Placeholder(
            this IEditorExportProperty<string> property,
            string placeholder
        )
        {
            property.SetPropertyHint(PropertyHint.PlaceholderText, placeholder);
            return property;
        }

        /// <summary>
        /// Property will be treated as enum.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="hint">Hint that describes enum. Example: "Egg,Hen,Chicken".</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<string> AsEnum(this IEditorExportProperty<string> property, string hint)
        {
            property.SetPropertyHint(PropertyHint.Enum, hint);
            return property;
        }
    }
}
