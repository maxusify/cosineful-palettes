namespace CosinefulPalettes.Vendor.ExportForge
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
        public static void NoAlpha(this EditorExportProperty<Color> property)
        {
            property.SetPropertyHint(PropertyHint.ColorNoAlpha);
        }
    }
}
