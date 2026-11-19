namespace CosinefulPalettes.Vendor.ExportForge
{
    using System;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Base class for exported properties for the Godot editor.
    /// Allows <see cref="EditorExportForge"/> to store properties of different value types together.
    /// </summary>
    public abstract class EditorExportProperty
    {
        /// <summary>
        /// Builds the property data dictionary for use with <see cref="GodotObject._GetPropertyList()"/>.
        /// </summary>
        /// <returns>Property data, or an empty dictionary if the property should not be listed.</returns>
        internal abstract GDC.Dictionary BuildPropertyData();

        /// <summary>
        /// Returns the current value of the property as a <see cref="Variant"/>.
        /// </summary>
        internal abstract Variant GetValue();

        /// <summary>
        /// Sets the value of the property from a <see cref="Variant"/>.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>True if successful, false otherwise.</returns>
        internal abstract bool SetValue(Variant value);
    }

    /// <summary>
    /// Editor export property for export forge.
    /// </summary>
    /// <typeparam name="TVariant">Property value type.</typeparam>
    public class EditorExportProperty<[MustBeVariant] TVariant> : EditorExportProperty
    {
        public string Name { get; }
        public Variant.Type Type { get; }
        public GodotObject Target => _forge.Target;
        public Func<TVariant>? Getter { get; private set; }
        public Action<TVariant>? Setter { get; private set; }
        public Func<bool>? CheckRequirement { get; private set; }
        public PropertyUsageFlags UsageFlags { get; private set; } = PropertyUsageFlags.Default;
        public PropertyHint PropertyHint { get; private set; } = PropertyHint.None;
        public string? HintString { get; private set; }

        private readonly EditorExportForge _forge;
        private bool _notifyWhenUpdated;
        private bool _shouldDebounce = true;
        private int _debounceMs = 250;

        internal EditorExportProperty(string name, Variant.Type type, EditorExportForge forge)
        {
            Name = name;
            Type = type;
            _forge = forge;
        }

        internal override GDC.Dictionary BuildPropertyData()
        {
            // If requirement is set, check if it is met.
            // If not, return an empty dictionary.
            if (CheckRequirement is { } check && !check())
            {
                return [];
            }

            // Rebuilt on every call so hint and usage changes are always picked up.
            var propertyData = new GDC.Dictionary {
                ["name"] = Name,
                ["type"] = (int)Type,
                ["hint"] = (int)PropertyHint,
                ["hint_string"] = HintString ?? string.Empty,
                ["usage"] = (int)UsageFlags
            };

            return propertyData;
        }

        internal override Variant GetValue() => Getter is { } getter ? Variant.From(getter()) : default;

        internal override bool SetValue(Variant value)
        {
            if (Setter is not { } setter)
            {
                return false;
            }

            setter(value.As<TVariant>());

            if (!_notifyWhenUpdated)
            {
                return true;
            }

            _forge.NotifyPropertyListChanged(_shouldDebounce ? _debounceMs : null);

            return true;
        }

        /// <summary>
        /// Sets the hint for the property.
        /// This can be used by the editor to provide additional information about the property.
        /// </summary>
        /// <param name="hint">Hint type.</param>
        /// <param name="hintString">Optional hint string.</param>
        /// <remarks>
        /// A property has a single hint, so setting it ends the method chain.
        /// Call other methods first.
        /// </remarks>
        public void SetPropertyHint(PropertyHint hint, string? hintString = null)
        {
            PropertyHint = hint;
            HintString = hintString ?? string.Empty;
        }

        /// <summary>
        /// Sets usage flags for the property.
        /// </summary>
        /// <param name="usageFlags">Usage flags.</param>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> SetUsageFlags(PropertyUsageFlags usageFlags)
        {
            UsageFlags = usageFlags;
            return this;
        }

        /// <summary>
        /// Add usage flag for the property.
        /// This can be used by the editor to provide additional information about the property.
        /// </summary>
        /// <param name="usageFlags">Flags to add.</param>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> AddUsageFlags(PropertyUsageFlags usageFlags)
        {
            UsageFlags |= usageFlags;
            return this;
        }

        /// <summary>
        /// Remove usage flag for the property.
        /// </summary>
        /// <param name="usageFlag">Usage flag to remove.</param>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> RemoveUsageFlags(PropertyUsageFlags usageFlag)
        {
            UsageFlags &= ~usageFlag;
            return this;
        }

        /// <summary>
        /// Sets callback for getting the current value of the property as a <typeparamref name="TVariant"/>.
        /// </summary>
        /// <param name="getter">Function to get the value.</param>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> OnGet(Func<TVariant> getter)
        {
            Getter = getter;
            return this;
        }

        /// <summary>
        /// Sets callback for setting the value of the property from a <typeparamref name="TVariant"/>.
        /// </summary>
        /// <param name="setter">Function to set the value.</param>
        /// <param name="notifyWhenUpdated">Whether to notify target when the value is updated.</param>
        /// <param name="debounceNotifyWhenUpdated">Debounce notify.</param>
        /// <param name="debounceNotifyWhenUpdatedMilliseconds">Debounce milliseconds.</param>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> OnSet(
            Action<TVariant> setter,
            bool notifyWhenUpdated = true,
            bool debounceNotifyWhenUpdated = true,
            int debounceNotifyWhenUpdatedMilliseconds = 250
        )
        {
            Setter = setter;
            _notifyWhenUpdated = notifyWhenUpdated;
            _shouldDebounce = debounceNotifyWhenUpdated;
            _debounceMs = debounceNotifyWhenUpdatedMilliseconds;
            return this;
        }

        /// <summary>
        /// Adds conditional requirement for this property to be visible or not. Useful
        /// for properties that depend on other values.
        /// </summary>
        /// <param name="checkCondition">Specified requirement.</param>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> When(Func<bool> checkCondition)
        {
            CheckRequirement = checkCondition;
            return this;
        }

        /// <summary>
        /// Makes the property read-only.
        /// </summary>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> ReadOnly()
        {
            UsageFlags |= PropertyUsageFlags.ReadOnly;
            return this;
        }

        /// <summary>
        /// Makes property serializable and stored in scene file.
        /// </summary>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> Store()
        {
            UsageFlags |= PropertyUsageFlags.Storage;
            return this;
        }

        /// <summary>
        /// Makes property invisible for editor.
        /// </summary>
        /// <returns>Self.</returns>
        public EditorExportProperty<TVariant> Internal()
        {
            UsageFlags &= ~PropertyUsageFlags.Editor;
            return this;
        }
    }
}
