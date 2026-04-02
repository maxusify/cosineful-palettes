namespace CosinefulPalettes.Utils.ExportForge
{
    using System;
    using System.Collections.Generic;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Interface for export logic builder.
    /// </summary>
    public interface IEditorExportForge
    {
        /// <summary>
        /// Creates a new property with the specified name. The property is of type <typeparamref name="TVariant"/>.
        /// </summary>
        /// <typeparam name="TVariant">Type of property. Must be a variant type. </typeparam>
        /// <param name="name">Name of the property. </param>
        /// <returns>Editor property.</returns>
        IEditorExportProperty<TVariant> CreateProperty<[MustBeVariant] TVariant>(string name);
        /// <summary>
        /// Returns property list in format accepted by <see cref="GodotObject._GetPropertyList"/> method.
        /// </summary>
        /// <returns>Godot array of dictionaries.</returns>
        GDC.Array<GDC.Dictionary> HandleGetPropertyList();
        /// <summary>
        /// Alias for <see cref="HandleGetPropertyList"/>.
        /// Returns property list in format accepted by <see cref="GodotObject._GetPropertyList"/> method.
        /// </summary>
        /// <returns>Godot array of dictionaries.</returns>
        GDC.Array<GDC.Dictionary> ForgeProperties();
        /// <summary>
        /// Handles getter for the property with specified name.
        /// Should be called as return value of <see cref="GodotObject._Get"/> method.
        /// </summary>
        /// <param name="name">Name of the property. </param>
        /// <returns>Value of the property.</returns>
        Variant HandleGetter(string name);
        /// <summary>
        /// Handles setter for the property with specified name.
        /// Should be called as return value of <see cref="GodotObject._Set"/> method.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Result of the setter operation.</returns>
        bool HandleSetter(string name, Variant value);
    }

    /// <summary>
    /// Class that provides methods for creating editor properties
    /// provided through <see cref="GodotObject._GetPropertyList"/> method.
    /// </summary>
    public class EditorExportForge : IEditorExportForge
    {
        private static readonly Dictionary<Type, Variant.Type> TypeToVariantMap = new()
        {
            { typeof(int),              Variant.Type.Int },
            { typeof(float),            Variant.Type.Float },
            { typeof(string),           Variant.Type.String },
            { typeof(bool),             Variant.Type.Bool },
            { typeof(Vector2),          Variant.Type.Vector2 },
            { typeof(Vector2I),         Variant.Type.Vector2I },
            { typeof(Rect2),            Variant.Type.Rect2 },
            { typeof(Rect2I),           Variant.Type.Rect2I },
            { typeof(Vector3),          Variant.Type.Vector3 },
            { typeof(Vector3I),         Variant.Type.Vector3I },
            { typeof(Transform2D),      Variant.Type.Transform2D },
            { typeof(Vector4),          Variant.Type.Vector4 },
            { typeof(Vector4I),         Variant.Type.Vector4I },
            { typeof(Plane),            Variant.Type.Plane },
            { typeof(Quaternion),       Variant.Type.Quaternion },
            { typeof(Aabb),             Variant.Type.Aabb },
            { typeof(Basis),            Variant.Type.Basis },
            { typeof(Transform3D),      Variant.Type.Transform3D },
            { typeof(Projection),       Variant.Type.Projection },
            { typeof(Color),            Variant.Type.Color },
            { typeof(StringName),       Variant.Type.StringName },
            { typeof(NodePath),         Variant.Type.NodePath },
            { typeof(Rid),              Variant.Type.Rid },
            { typeof(GodotObject),      Variant.Type.Object },
            { typeof(Callable),         Variant.Type.Callable },
            { typeof(Signal),           Variant.Type.Signal },
            { typeof(GDC.Dictionary),   Variant.Type.Dictionary },
            { typeof(GDC.Array),        Variant.Type.Array },
            { typeof(byte[]),           Variant.Type.PackedByteArray },
            { typeof(int[]),            Variant.Type.PackedInt32Array },
            { typeof(long[]),           Variant.Type.PackedInt64Array },
            { typeof(float[]),          Variant.Type.PackedFloat32Array },
            { typeof(double[]),         Variant.Type.PackedFloat64Array },
            { typeof(string[]),         Variant.Type.PackedStringArray },
            { typeof(Vector2[]),        Variant.Type.PackedVector2Array },
            { typeof(Vector3[]),        Variant.Type.PackedVector3Array },
            { typeof(Color[]),          Variant.Type.PackedColorArray },
            { typeof(Vector4[]),        Variant.Type.PackedVector4Array }
        };

        private readonly Dictionary<string, IEditorExportProperty> _registered = [];
        private readonly GodotObject _godotObject;
        private GDC.Array<GDC.Dictionary>? _properties;

        public EditorExportForge(GodotObject godotObject)
        {
            _godotObject = godotObject;
        }

        public IEditorExportProperty<TVariant> CreateProperty<[MustBeVariant] TVariant>(string name)
        {
            if (_registered.ContainsKey(name))
            {
                throw new ArgumentException($"Property with name '{name}' already exists.", nameof(name));
            }

            var type = typeof(TVariant);

            var variantType = type.IsAssignableTo(typeof(GodotObject))
                ? Variant.Type.Object
                : GetVariantType<TVariant>();

            var property = new EditorExportProperty<TVariant>() {
                Name = name,
                Type = variantType,
                Target = _godotObject
            };

            _registered[name] = property;

            return property;
        }

        public GDC.Array<GDC.Dictionary> ForgeProperties() => HandleGetPropertyList();

        public GDC.Array<GDC.Dictionary> HandleGetPropertyList()
        {
            _properties = [];

            foreach (var (_, prop) in _registered)
            {
                var propData = prop.BuildPropertyData();

                if (propData.Count == 0)
                {
                    continue;
                }

                _properties.Add(propData);
            }

            return _properties;
        }

        public Variant HandleGetter(string name)
        {
            if (!_registered.TryGetValue(name, out var property))
            {
                return default;
            }

            return property.GetValue();
        }

        public bool HandleSetter(string name, Variant value)
        {
            if (!_registered.TryGetValue(name, out var property))
            {
                return false;
            }

            return property.SetValue(value);
        }

        private static Variant.Type GetVariantType<[MustBeVariant] TVariant>()
        {
            var type = typeof(TVariant);

            if (TypeToVariantMap.TryGetValue(type, out var variantType))
            {
                return variantType;
            }

            throw new InvalidOperationException($"Unsupported `Variant` type: {type}");
        }
    }
}
