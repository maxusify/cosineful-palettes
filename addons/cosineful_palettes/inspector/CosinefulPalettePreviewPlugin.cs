#if TOOLS

namespace CosinefulPalettes.Editor
{
    using Godot;

    public partial class CosinefulPalettePreviewPlugin : EditorInspectorPlugin
    {
        private const string COMPONENTS_PREFIX = "Components/";

        public override bool _CanHandle(GodotObject @object)
        {
            return @object is CosinefulPalette;
        }

        public override void _ParseBegin(GodotObject @object)
        {
            if (@object is not CosinefulPalette palette)
            {
                return;
            }

            AddCustomControl(new CosinefulPalettePreview(palette) {
                CustomMinimumSize = new Vector2(300f, 0f)
            });
        }

        public override bool _ParseProperty(
            GodotObject @object,
            Variant.Type type,
            string name,
            PropertyHint hintType,
            string hintString,
            PropertyUsageFlags usageFlags,
            bool wide)
        {
            if (@object is not CosinefulPalette
                || type != Variant.Type.Vector3
                || !name.StartsWith(COMPONENTS_PREFIX))
            {
                return false;
            }

            AddPropertyEditor(name, new CosinefulComponentProperty());

            return true;
        }
    }
}

#endif
