namespace CosinefulPalettes.Vendor.ExportForge
{
    using System;

    using CosinefulPalettes.Vendor.ExportForge.Utils;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Extensions for exported collection properties.
    /// </summary>
    public static class EditorExportPropertyCollectionExtensions
    {
        /// <summary>
        /// Sets the type of the array items.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="type">Type of the array items. Must be Variant compatible.</param>
        public static void ArrayType(
            this EditorExportProperty<GDC.Array> property,
            Type type
        )
        {
            property.SetPropertyHint(PropertyHint.ArrayType, VariantTypes.GetTypeHintString(type));
        }

        /// <summary>
        /// Sets the type of the array items.
        /// </summary>
        /// <typeparam name="T">Type of the array items.</typeparam>
        /// <param name="property">Property.</param>
        public static void ArrayType<[MustBeVariant] T>(
            this EditorExportProperty<GDC.Array> property
        )
        {
            property.ArrayType(typeof(T));
        }

        /// <summary>
        /// Sets the types of the dictionary keys and values.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="keyType">Type of the keys. Must be Variant compatible.</param>
        /// <param name="valueType">Type of the values. Must be Variant compatible.</param>
        public static void DictionaryType(
            this EditorExportProperty<GDC.Dictionary> property,
            Type keyType,
            Type valueType
        )
        {
            property.SetPropertyHint(
                PropertyHint.DictionaryType,
                VariantTypes.GetDictionaryHintString(keyType, valueType)
            );
        }

        /// <summary>
        /// Sets the types of the dictionary keys and values.
        /// </summary>
        /// <typeparam name="TKey">Type of the dictionary keys.</typeparam>
        /// <typeparam name="TValue">Type of the dictionary values.</typeparam>
        /// <param name="property">Property.</param>
        public static void DictionaryType<
            [MustBeVariant] TKey,
            [MustBeVariant] TValue>
        (this EditorExportProperty<GDC.Dictionary> property)
        {
            property.DictionaryType(typeof(TKey), typeof(TValue));
        }
    }
}
