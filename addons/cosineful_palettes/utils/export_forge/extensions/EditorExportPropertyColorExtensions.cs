namespace CosinefulPalettes.Utils.ExportForge
{
    using Godot;

    /// <summary>
    /// Extensions for exported <see cref="Color"/> properties.
    /// </summary>
    public static class EditorExportPropertyColorExtensions
    {
        /// <summary>
        /// Disables alpha channel of the edited color.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Color> NoAlpha(this IEditorExportProperty<Color> property)
        {
            property.SetPropertyHint(PropertyHint.ColorNoAlpha);
            return property;
        }
    }
}
