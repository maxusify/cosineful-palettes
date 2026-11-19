namespace CosinefulPalettes.Vendor.ExportForge
{
    using System;
    using System.Collections.Generic;

    using CosinefulPalettes.Vendor.ExportForge.Utils;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Class that provides methods for creating editor properties
    /// provided through <see cref="GodotObject._GetPropertyList"/> method.
    /// </summary>
    public class EditorExportForge(GodotObject target)
    {
        private readonly Dictionary<string, EditorExportProperty> _properties = [];
        private readonly Debouncer _notifyDebouncer = new();

        /// <summary>
        /// Object whose properties are provided by this forge.
        /// </summary>
        public GodotObject Target { get; } = target;

        /// <summary>
        /// Creates a new property with the specified name. The property is of type <typeparamref name="TVariant"/>.
        /// </summary>
        /// <typeparam name="TVariant">Type of property. Must be a variant type.</typeparam>
        /// <param name="name">Name of the property.</param>
        /// <returns>Editor property.</returns>
        public EditorExportProperty<TVariant> CreateProperty<[MustBeVariant] TVariant>(string name)
        {
            if (_properties.ContainsKey(name))
            {
                throw new ArgumentException($"Property with name '{name}' already exists.", nameof(name));
            }

            var type = typeof(TVariant);
            var variantType = VariantTypes.GetVariantType(type);

            var property = new EditorExportProperty<TVariant>(name, variantType, this);

            // Objects, enums and typed collections need a hint to be edited properly. Can be overridden.
            var (hint, hintString) = VariantTypes.GetDefaultHint(type);

            if (hint != PropertyHint.None)
            {
                property.SetPropertyHint(hint, hintString);
            }

            // Nil type means the property can hold any Variant.
            if (variantType == Variant.Type.Nil)
            {
                property.AddUsageFlags(PropertyUsageFlags.NilIsVariant);
            }

            _properties[name] = property;

            return property;
        }

        /// <summary>
        /// Alias for <see cref="HandleGetPropertyList"/>.
        /// Returns property list in format accepted by <see cref="GodotObject._GetPropertyList"/> method.
        /// </summary>
        /// <returns>Godot array of dictionaries.</returns>
        public GDC.Array<GDC.Dictionary> ForgeProperties()
        {
            return HandleGetPropertyList();
        }

        /// <summary>
        /// Returns property list in format accepted by <see cref="GodotObject._GetPropertyList"/> method.
        /// </summary>
        /// <returns>Godot array of dictionaries.</returns>
        public GDC.Array<GDC.Dictionary> HandleGetPropertyList()
        {
            GDC.Array<GDC.Dictionary> propertyData = [];

            foreach (var (_, prop) in _properties)
            {
                var propData = prop.BuildPropertyData();

                if (propData.Count == 0)
                {
                    continue;
                }

                propertyData.Add(propData);
            }

            return propertyData;
        }

        /// <summary>
        /// Handles getter for the property with specified name.
        /// Should be called as return value of <see cref="GodotObject._Get"/> method.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        public Variant HandleGetter(StringName name)
        {
            if (!_properties.TryGetValue(name, out var property))
            {
                return default;
            }

            return property.GetValue();
        }

        /// <summary>
        /// Handles setter for the property with specified name.
        /// Should be called as return value of <see cref="GodotObject._Set"/> method.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Result of the setter operation.</returns>
        public bool HandleSetter(StringName name, Variant value)
        {
            if (!_properties.TryGetValue(name, out var property))
            {
                return false;
            }

            return property.SetValue(value);
        }

        /// <summary>
        /// Notifies the editor that the property list of <see cref="Target"/> changed.
        /// Debounced notifications are shared by all properties of this forge.
        /// </summary>
        /// <param name="debounceMilliseconds">Debounce delay, or <c>null</c> to notify at the end of the frame.</param>
        internal void NotifyPropertyListChanged(int? debounceMilliseconds)
        {
            if (debounceMilliseconds is { } delay)
            {
                _ = _notifyDebouncer.Debounce(NotifyTargetPropertyListChanged, delay);
                return;
            }

            // Supersede any pending debounced notification.
            _notifyDebouncer.Cancel();
            NotifyTargetPropertyListChanged();
        }

        private void NotifyTargetPropertyListChanged()
        {
            if (!GodotObject.IsInstanceValid(Target))
            {
                return;
            }

            Target.CallDeferred(GodotObject.MethodName.NotifyPropertyListChanged);
        }
    }
}
