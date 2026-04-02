namespace CosinefulPalettes.Utils.ExportForge
{
    using System;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Interface for exported properties for the Godot editor.
    /// </summary>
    public interface IEditorExportProperty
    {
        /// <summary>
        /// Builds the property data dictionary for use with <see cref="GodotObject._GetPropertyList()"/>.
        /// </summary>
        /// <returns></returns>
        GDC.Dictionary BuildPropertyData();
        /// <summary>
        /// Returns the current value of the property as a <see cref="Variant"/>.
        /// </summary>
        Variant GetValue();
        /// <summary>
        /// Sets the value of the property from a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>True if successful, false otherwise.</returns>
        bool SetValue(Variant value);
    }

    /// <summary>
    /// Interface for typed editor export properties.
    /// </summary>
    /// <typeparam name="TVariant">Type of the property value.</typeparam>
    public interface IEditorExportProperty<[MustBeVariant] TVariant> : IEditorExportProperty
    {
        /// <summary>
        /// Sets callback for getting the current value of the property as a <see cref="TVariant"/>.
        /// </summary>
        /// <param name="getter">Function to get the value.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> OnGet(Func<TVariant> getter);
        /// <summary>
        /// Sets callback for setting the value of the property from a <see cref="TVariant"/>.
        /// </summary>
        /// <param name="setter">Function to set the value.</param>
        /// <param name="notifyWhenUpdated">Whether to notify target when the value is updated.</param>
        /// <param name="debounceNotifyWhenUpdated">Debounce notify.</param>
        /// <param name="debounceNotifyWhenUpdatedMilliseconds">Debounce miliseconds.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> OnSet(
            Action<TVariant> setter,
            bool notifyWhenUpdated = true,
            bool debounceNotifyWhenUpdated = true,
            int debounceNotifyWhenUpdatedMilliseconds = 250
        );
        /// <summary>
        /// Sets the hint for the property.
        /// This can be used by the editor to provide additional information about the property.
        /// </summary>
        /// <param name="hint">Hint type.</param>
        /// <param name="hintString">Optional hint string.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> SetPropertyHint(PropertyHint hint, string? hintString = null);
        /// <summary>
        /// Add usage flag for the property.
        /// This can be used by the editor to provide additional information about the property.
        /// </summary>
        /// <param name="usageFlags">Flags to add.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> AddUsageFlags(PropertyUsageFlags usageFlags);
        /// <summary>
        /// Remove usage flag for the property.
        /// </summary>
        /// <param name="usageFlag">Usage flag to remove.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> RemoveUsageFlags(PropertyUsageFlags usageFlag);
        /// <summary>
        /// Makes the property read-only.
        /// </summary>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> ReadOnly();
        /// <summary>
        /// Adds conditional requirement for this property to be visible or not. Useful
        /// for properties that depend on other values.
        /// </summary>
        /// <param name="checkCondition">Specified requirement.</param>
        /// <returns>Self.</returns>
        IEditorExportProperty<TVariant> When(Func<bool> checkCondition);
    }

    /// <summary>
    /// Editor export property for export forge.
    /// </summary>
    /// <typeparam name="TVariant">Property value type.</typeparam>
    public partial class EditorExportProperty<[MustBeVariant] TVariant>()
        : IEditorExportProperty<TVariant>, IDisposable
    {
        public string Name { get; set; } = string.Empty;
        public Variant.Type Type { get; set; }
        public GodotObject Target { get; set; } = null!;
        public Func<TVariant>? Getter { get; private set; }
        public Action<TVariant>? Setter { get; private set; }
        public Func<bool>? CheckRequirement { get; private set; }
        public PropertyUsageFlags UsageFlags { get; private set; } = PropertyUsageFlags.Default;
        public PropertyHint PropertyHint { get; private set; } = PropertyHint.None;
        public string? HintString { get; private set; }

        private GDC.Dictionary? _propertyData;
        private bool _notifyWhenUpdated;
        private bool _debounceNotifyWhenUpdated = true;
        private int _debounceNotifyWhenUpdatedMilliseconds = 250;
        private Debouncer? _debouncer;

        public GDC.Dictionary BuildPropertyData()
        {
            // If requirement is set, check if it is met.
            // If not, return an empty dictionary.
            if (CheckRequirement is { } check && !check())
            {
                return [];
            }

            // Check if data was already built. If so, return it.
            if (_propertyData is { } data)
            {
                return data;
            }

            _propertyData = new GDC.Dictionary {
                ["name"] = Name,
                ["type"] = (int)Type,
                ["hint"] = (int)PropertyHint,
                ["hint_string"] = HintString ?? string.Empty,
                ["usage"] = (int)UsageFlags
            };

            // If type is GodotObject, handle it differently.
            if (Type == Variant.Type.Object)
            {
                _propertyData["hint"] = (int)PropertyHint.ResourceType;
                _propertyData["hint_string"] = typeof(TVariant).Name;
            }

            return _propertyData;
        }

        public Variant GetValue() => Getter is { } getter
            ? Variant.From(getter())
            : default;

        public bool SetValue(Variant value)
        {
            if (Setter is { } setter)
            {
                setter(value.As<TVariant>());

                if (!_notifyWhenUpdated)
                {
                    return true;
                }

                if (_debounceNotifyWhenUpdated)
                {
                    _debouncer ??= new Debouncer();
                    _debouncer.DelayMilliseconds = _debounceNotifyWhenUpdatedMilliseconds;
                    _ = _debouncer.Debounce(_DebouncerHandler);
                }
                else
                {
                    Target.NotifyPropertyListChanged();
                }

                return true;
            }

            return false;
        }

        public IEditorExportProperty<TVariant> SetPropertyHint(PropertyHint hint, string? hintString = null)
        {
            PropertyHint = hint;
            HintString = hintString ?? string.Empty;
            return this;
        }

        public IEditorExportProperty<TVariant> AddUsageFlags(PropertyUsageFlags usageFlags)
        {
            UsageFlags |= usageFlags;
            return this;
        }

        public IEditorExportProperty<TVariant> RemoveUsageFlags(PropertyUsageFlags usageFlag)
        {
            UsageFlags &= ~usageFlag;
            return this;
        }

        public IEditorExportProperty<TVariant> OnGet(Func<TVariant> getter)
        {
            Getter = getter;
            return this;
        }

        public IEditorExportProperty<TVariant> OnSet(
            Action<TVariant> setter,
            bool notifyWhenUpdated = true,
            bool debounceNotifyWhenUpdated = true,
            int debounceNotifyWhenUpdatedMilliseconds = 250
        )
        {
            Setter = setter;
            _notifyWhenUpdated = notifyWhenUpdated;
            _debounceNotifyWhenUpdated = debounceNotifyWhenUpdated;
            _debounceNotifyWhenUpdatedMilliseconds = debounceNotifyWhenUpdatedMilliseconds;
            return this;
        }

        public IEditorExportProperty<TVariant> When(Func<bool> checkCondition)
        {
            CheckRequirement = checkCondition;
            return this;
        }

        public IEditorExportProperty<TVariant> ReadOnly()
        {
            UsageFlags |= PropertyUsageFlags.ReadOnly;
            return this;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _propertyData?.Dispose();
            _propertyData = null;
        }

        private void _DebouncerHandler()
        {
            if (!GodotObject.IsInstanceValid(Target))
            {
                return;
            }

            Target.CallDeferred(GodotObject.MethodName.NotifyPropertyListChanged);
        }
    }
}
