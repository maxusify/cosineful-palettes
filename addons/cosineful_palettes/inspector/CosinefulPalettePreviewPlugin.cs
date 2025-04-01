#if TOOLS

namespace CosinefulPalettes.Editor
{
    using Godot;

    public partial class CosinefulPalettePreviewPlugin : EditorInspectorPlugin
    {
        public override bool _CanHandle(GodotObject @object) => @object is CosinefulPalette;

        public override void _ParseBegin(GodotObject @object)
        {
            if (@object is not CosinefulPalette palette)
            {
                return;
            }

            AddCustomControl(new CosinefulPalettePreview(palette) {
                CustomMinimumSize = new Vector2(300f, 75f)
            });
        }
    }
}

#endif
