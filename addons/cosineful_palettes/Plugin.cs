#if TOOLS

namespace CosinefulPalettes.Editor
{
    using Godot;

    [Tool]
    public partial class Plugin : EditorPlugin
    {
        private const string PLUGIN_NAME = "cosineful_palettes";
        private const string PLUGIN_NAME_INSPECTOR = "inspector";

        public override void _EnablePlugin()
        {
            EditorInterface.Singleton.SetPluginEnabled(
                $"{PLUGIN_NAME}/{PLUGIN_NAME_INSPECTOR}",
                true
            );
        }

        public override void _DisablePlugin()
        {
            EditorInterface.Singleton.SetPluginEnabled(
                $"{PLUGIN_NAME}/{PLUGIN_NAME_INSPECTOR}",
                false
            );
        }
    }
}

#endif
