namespace CosinefulPalettes.Vendor.ExportForge
{
    using Godot;

    /// <summary>
    /// Extensions for exported <see cref="Callable"/> properties.
    /// </summary>
    public static class EditorExportPropertyCallableExtensions
    {
        /// <summary>
        /// Treats the callable as a tool button. The property is shown in the editor only and not stored.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="label">Button label.</param>
        /// <param name="icon">Button icon from the theme icons. Example: "Variant", "RandomNumberGenerator".</param>
        public static void ToolButton(
            this EditorExportProperty<Callable> property,
            string label,
            string? icon = null
        )
        {
            property.SetPropertyHint(
                PropertyHint.ToolButton,
                string.IsNullOrEmpty(icon) ? label : $"{label},{icon}"
            );

            // Callables cannot be saved, so the button must only be shown in the editor.
            property.RemoveUsageFlags(PropertyUsageFlags.Storage);
        }
    }
}
