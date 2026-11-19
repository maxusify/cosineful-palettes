#if TOOLS

namespace CosinefulPalettes.Editor
{
    using Godot;

    /// <summary>
    /// Inspector editor for a single palette component (brightness, contrast, frequency or range).
    /// Shows one full-width slider per color channel instead of the default compact Vector3 editor.
    /// </summary>
    [Tool]
    public partial class CosinefulComponentProperty : EditorProperty
    {
        private static readonly string[] ChannelNames = ["R", "G", "B"];
        private static readonly string[] ChannelColorNames = ["property_color_x", "property_color_y", "property_color_z"];

        private readonly Label[] _labels = new Label[3];
        private readonly EditorSpinSlider[] _sliders = new EditorSpinSlider[3];
        private readonly TextureButton _linkButton;

        private bool _updating;
        private bool _dragging;
        private bool _propertyListRefreshPending;

        public CosinefulComponentProperty() : this(0.0, 1.0, 0.001)
        {
        }

        public CosinefulComponentProperty(double min, double max, double step)
        {
            var root = new HBoxContainer();
            var rows = new VBoxContainer {
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };

            for (var i = 0; i < 3; i++)
            {
                var channel = i;
                var row = new HBoxContainer();

                _labels[i] = new Label {
                    Text = ChannelNames[i],
                    CustomMinimumSize = new Vector2(18f, 0f),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                _sliders[i] = new EditorSpinSlider {
                    MinValue = min,
                    MaxValue = max,
                    Step = step,
                    AllowGreater = true,
                    AllowLesser = true,
                    SizeFlagsHorizontal = SizeFlags.ExpandFill
                };

                _sliders[i].ValueChanged += value => OnSliderValueChanged(channel, value);
                _sliders[i].Grabbed += () => _dragging = true;
                _sliders[i].Ungrabbed += () =>
                {
                    _dragging = false;
                    FlushPropertyListRefresh();
                };

                row.AddChild(_labels[i]);
                row.AddChild(_sliders[i]);
                rows.AddChild(row);

                AddFocusable(_sliders[i]);
            }

            _linkButton = new TextureButton {
                ToggleMode = true,
                StretchMode = TextureButton.StretchModeEnum.KeepCentered,
                CustomMinimumSize = new Vector2(24f, 0f),
                TooltipText = "Link channels: dragging one slider sets R, G and B to the same value."
            };

            root.AddChild(rows);
            root.AddChild(_linkButton);

            AddChild(root);
            SetBottomEditor(root);
        }

        public override void _Notification(int what)
        {
            if (what != NotificationThemeChanged)
            {
                return;
            }

            for (var i = 0; i < 3; i++)
            {
                _labels[i].AddThemeColorOverride("font_color", GetThemeColor(ChannelColorNames[i], "Editor"));
            }

            _linkButton.TextureNormal = GetThemeIcon("Unlinked", "EditorIcons");
            _linkButton.TexturePressed = GetThemeIcon("Instance", "EditorIcons");
        }

        public override void _UpdateProperty()
        {
            var value = GetEditedObject().Get(GetEditedProperty()).AsVector3();

            _updating = true;

            for (var i = 0; i < 3; i++)
            {
                _sliders[i].SetValueNoSignal(value[i]);
            }

            _updating = false;
        }

        private void OnSliderValueChanged(int channel, double newValue)
        {
            if (_updating)
            {
                return;
            }

            var edited = GetEditedObject();
            var value = edited.Get(GetEditedProperty()).AsVector3();

            if (_linkButton.ButtonPressed)
            {
                value = new Vector3((float)newValue, (float)newValue, (float)newValue);
            }
            else
            {
                value[channel] = (float)newValue;
            }

            // Editing a component switches the palette from seed based to manual,
            // which hides seed related properties. Rebuilding the inspector mid-drag
            // would destroy this editor, so the rebuild is postponed until release.
            var wasSeedBased = edited is CosinefulPalette { UseSeed: true };

            EmitChanged(GetEditedProperty(), value);

            if (wasSeedBased && edited is CosinefulPalette { UseSeed: false })
            {
                _propertyListRefreshPending = true;
            }

            if (!_dragging)
            {
                FlushPropertyListRefresh();
            }
        }

        private void FlushPropertyListRefresh()
        {
            if (!_propertyListRefreshPending)
            {
                return;
            }

            _propertyListRefreshPending = false;

            if (GetEditedObject() is { } edited && IsInstanceValid(edited))
            {
                edited.CallDeferred(GodotObject.MethodName.NotifyPropertyListChanged);
            }
        }
    }
}

#endif
