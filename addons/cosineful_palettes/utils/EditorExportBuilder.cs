#nullable enable

namespace CosinefulPalettes.Utils
{
    using System;
    using System.Collections.Generic;

    using Godot;

    using GDC = Godot.Collections;

    public partial class EditorExportBuilder : RefCounted
    {
        #region Static Properties

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

        #endregion Static Properties
        #region Properties

        private readonly Dictionary<string, IEditorExportProperty> _registered = [];
        private GDC.Array<GDC.Dictionary>? _properties;

        #endregion Properties
        #region Public Methods

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

            EditorExportProperty<TVariant> property = new() {
                Name = name,
                Type = variantType
            };

            _registered[name] = property;

            return property;
        }

        public GDC.Array<GDC.Dictionary> GetProperties()
        {
            _properties = [];

            foreach ((var _, var prop) in _registered)
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

        public Variant HandleGetter(string name) => !_registered.TryGetValue(name, out var property) ? default : property.GetValue();

        public bool HandleSetter(string name, Variant value) => _registered.TryGetValue(name, out var property) && property.SetValue(value);

        #endregion Public Methods

        private static Variant.Type GetVariantType<[MustBeVariant] TVariant>()
        {
            var type = typeof(TVariant);

            return TypeToVariantMap.TryGetValue(type, out var variantType)
                ? variantType
                : throw new InvalidOperationException($"Unsupported `Variant` type: {type}");
        }
    }
}
