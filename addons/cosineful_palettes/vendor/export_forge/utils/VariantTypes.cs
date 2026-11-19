namespace CosinefulPalettes.Vendor.ExportForge.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Maps C# types to Godot <see cref="Variant.Type"/> values and their default editor hints.
    /// </summary>
    internal static class VariantTypes
    {
        private static readonly Dictionary<Type, Variant.Type> _typeToVariantMap = new()
        {
            { typeof(int),              Variant.Type.Int },
            { typeof(long),             Variant.Type.Int },
            { typeof(sbyte),            Variant.Type.Int },
            { typeof(byte),             Variant.Type.Int },
            { typeof(short),            Variant.Type.Int },
            { typeof(ushort),           Variant.Type.Int },
            { typeof(uint),             Variant.Type.Int },
            { typeof(ulong),            Variant.Type.Int },
            { typeof(float),            Variant.Type.Float },
            { typeof(double),           Variant.Type.Float },
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
            { typeof(Vector4[]),        Variant.Type.PackedVector4Array },
            { typeof(StringName[]),     Variant.Type.Array },
            { typeof(NodePath[]),       Variant.Type.Array },
            { typeof(Rid[]),            Variant.Type.Array },
            { typeof(Variant),          Variant.Type.Nil }
        };

        /// <summary>
        /// Returns the <see cref="Variant.Type"/> used to store values of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">C# type. Must be Variant compatible.</param>
        /// <returns>Variant type.</returns>
        /// <exception cref="InvalidOperationException">The type is not Variant compatible.</exception>
        public static Variant.Type GetVariantType(Type type)
        {
            if (_typeToVariantMap.TryGetValue(type, out var variantType))
            {
                return variantType;
            }

            if (type.IsAssignableTo(typeof(GodotObject)))
            {
                return Variant.Type.Object;
            }

            if (type.IsEnum)
            {
                return Variant.Type.Int;
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();

                if (genericType == typeof(GDC.Array<>))
                {
                    return Variant.Type.Array;
                }

                if (genericType == typeof(GDC.Dictionary<,>))
                {
                    return Variant.Type.Dictionary;
                }
            }

            // Arrays of GodotObject derived types are marshaled as untyped arrays.
            if (type.IsArray && type.GetElementType()!.IsAssignableTo(typeof(GodotObject)))
            {
                return Variant.Type.Array;
            }

            throw new InvalidOperationException($"Unsupported `Variant` type: {type}");
        }

        /// <summary>
        /// Returns the hint Godot needs to edit values of <paramref name="type"/> properly:
        /// class name for objects, names for enums and element types for typed collections.
        /// </summary>
        /// <param name="type">C# type. Must be Variant compatible.</param>
        /// <returns>Hint and hint string, or <see cref="PropertyHint.None"/> if no hint is needed.</returns>
        public static (PropertyHint Hint, string HintString) GetDefaultHint(Type type)
        {
            if (type.IsAssignableTo(typeof(Resource)))
            {
                return (PropertyHint.ResourceType, type.Name);
            }

            if (type.IsAssignableTo(typeof(Node)))
            {
                return (PropertyHint.NodeType, type.Name);
            }

            if (type.IsEnum)
            {
                return type.IsDefined(typeof(FlagsAttribute), false)
                    ? (PropertyHint.Flags, GetFlagsHintString(type))
                    : (PropertyHint.Enum, GetEnumHintString(type));
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                var typeArguments = type.GetGenericArguments();

                if (genericType == typeof(GDC.Array<>) && typeArguments[0] != typeof(Variant))
                {
                    return (PropertyHint.ArrayType, GetTypeHintString(typeArguments[0]));
                }

                if (genericType == typeof(GDC.Dictionary<,>))
                {
                    return (PropertyHint.DictionaryType, GetDictionaryHintString(typeArguments[0], typeArguments[1]));
                }
            }

            // Arrays that are not packed arrays are marshaled as arrays of their element type.
            if (type.IsArray && GetVariantType(type) == Variant.Type.Array)
            {
                return (PropertyHint.ArrayType, GetTypeHintString(type.GetElementType()!));
            }

            return (PropertyHint.None, string.Empty);
        }

        /// <summary>
        /// Returns hint string describing a stored type in <c>"type/hint:hint_string"</c> format,
        /// used by <see cref="PropertyHint.ArrayType"/> and <see cref="PropertyHint.DictionaryType"/>.
        /// </summary>
        /// <param name="type">Stored type. Must be Variant compatible.</param>
        /// <returns>Hint string.</returns>
        public static string GetTypeHintString(Type type)
        {
            var variantType = GetVariantType(type);
            var (hint, hintString) = GetDefaultHint(type);

            return $"{(int)variantType}/{(int)hint}:{hintString}";
        }

        /// <summary>
        /// Returns hint string for <see cref="PropertyHint.DictionaryType"/>.
        /// </summary>
        /// <param name="keyType">Key type. Must be Variant compatible.</param>
        /// <param name="valueType">Value type. Must be Variant compatible.</param>
        /// <returns>Hint string.</returns>
        public static string GetDictionaryHintString(Type keyType, Type valueType)
        {
            return $"{GetTypeHintString(keyType)};{GetTypeHintString(valueType)}";
        }

        /// <summary>
        /// Returns hint string for <see cref="PropertyHint.Enum"/> with explicit values, e.g. <c>"A:0,B:4"</c>.
        /// </summary>
        /// <param name="enumType">Enum type.</param>
        /// <returns>Hint string.</returns>
        public static string GetEnumHintString(Type enumType)
        {
            return string.Join(',', GetEnumMembers(enumType).Select(member => $"{member.Name}:{member.Value}"));
        }

        /// <summary>
        /// Returns hint string for <see cref="PropertyHint.Flags"/> with explicit values, e.g. <c>"A:1,B:4,AB:5"</c>.
        /// Members Godot does not accept as flags (zero, negative or larger than 32 bits) are skipped.
        /// </summary>
        /// <param name="enumType">Enum type.</param>
        /// <returns>Hint string.</returns>
        public static string GetFlagsHintString(Type enumType)
        {
            return string.Join(
                ',',
                GetEnumMembers(enumType)
                    .Where(member => member.Value is >= 1 and <= uint.MaxValue)
                    .Select(member => $"{member.Name}:{member.Value}")
            );
        }

        private static IEnumerable<(string Name, long Value)> GetEnumMembers(Type enumType)
        {
            // Enum fields are listed in declaration order, which is the order shown in the editor.
            foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = field.GetRawConstantValue()!;

                yield return (field.Name, value is ulong unsignedValue ? unchecked((long)unsignedValue) : Convert.ToInt64(value));
            }
        }
    }
}
