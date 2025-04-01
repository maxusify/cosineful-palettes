#if TOOLS

namespace CosinefulPalettes.Editor
{
    using Godot;

    [Tool]
    public partial class InspectorPlugin : EditorPlugin
    {
        private CosinefulPalettePreviewPlugin _previewPlugin;

        public override void _EnterTree()
        {
            _previewPlugin = new CosinefulPalettePreviewPlugin();

            AddInspectorPlugin(_previewPlugin);
        }

        public override void _ExitTree() => RemoveInspectorPlugin(_previewPlugin);
    }
}

#endif
