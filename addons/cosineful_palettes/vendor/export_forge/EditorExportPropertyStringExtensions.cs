namespace CosinefulPalettes.Vendor.ExportForge
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
        public static void Multiline(this EditorExportProperty<string> property)
        {
            property.SetPropertyHint(PropertyHint.MultilineText);
        }

        /// <summary>
        /// Makes the property a password input. Useful for secrets.
        /// </summary>
        /// <param name="property">Property.</param>
        public static void Password(this EditorExportProperty<string> property)
        {
            property.SetPropertyHint(PropertyHint.Password);
        }

        /// <summary>
        /// Applies a placeholder to the text field. Useful for providing guidance or examples.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="placeholder">Text to display as a placeholder.</param>
        public static void Placeholder(
            this EditorExportProperty<string> property,
            string placeholder
        )
        {
            property.SetPropertyHint(PropertyHint.PlaceholderText, placeholder);
        }

        /// <summary>
        /// Property will be treated as enum.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="hint">Hint that describes enum. Example: "Egg,Hen,Chicken".</param>
        public static void AsEnum(this EditorExportProperty<string> property, string hint)
        {
            property.SetPropertyHint(PropertyHint.Enum, hint);
        }
    }
}
