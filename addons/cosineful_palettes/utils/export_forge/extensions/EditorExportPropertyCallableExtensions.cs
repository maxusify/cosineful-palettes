namespace CosinefulPalettes.Utils.ExportForge
{
    using Godot;

    /// <summary>
    /// Extensions for exported <see cref="Callable"/> properties.
    /// </summary>
    public static class EditorExportPropertyCallableExtensions
    {
        /// <summary>
        /// Treats the callable as a tool button.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="label">Button label.</param>
        /// <param name="icon">Button icon from the theme icons. Example: "Variant", "RandomNumberGenerator".</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<Callable> ToolButton(
            this IEditorExportProperty<Callable> property,
            string label,
            string? icon = null
        )
        {
            property.SetPropertyHint(
                PropertyHint.ToolButton,
                string.IsNullOrEmpty(icon) ? label : $"{label},{icon}"
            );

            return property;
        }
    }
}
