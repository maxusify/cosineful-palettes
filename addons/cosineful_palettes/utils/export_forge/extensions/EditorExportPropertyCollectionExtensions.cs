namespace CosinefulPalettes.Utils.ExportForge
{
    using System;

    using Godot;

    using GDC = Godot.Collections;

    /// <summary>
    /// Extensions for exported collection properties.
    /// </summary>
    public static class EditorExportPropertyCollectionExtensions
    {
        /// <summary>
        /// Sets the type of the collection. Example: "int", "float".
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="type">Type of the array items.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<GDC.Array> ArrayType(
            this IEditorExportProperty<GDC.Array> property,
            Type type
        )
        {
            property.SetPropertyHint(PropertyHint.ArrayType, type.Name);
            return property;
        }
        /// <summary>
        /// Sets the type of the collection. Example: "int", "float".
        /// </summary>
        /// <typeparam name="T">Type of array items</typeparam>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<GDC.Array> ArrayType<[MustBeVariant] T>(
            this IEditorExportProperty<GDC.Array> property
        )
        {
            property.SetPropertyHint(PropertyHint.ArrayType, typeof(T).Name);
            return property;
        }

        /// <summary>
        /// Sets the type of the collection. Example: "int", "float".
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<GDC.Dictionary> DictionaryType(
            this IEditorExportProperty<GDC.Dictionary> property,
            Type keyType,
            Type valueType
        )
        {
            property.SetPropertyHint(PropertyHint.DictionaryType, $"{keyType.Name},{valueType.Name}");
            return property;
        }

        /// <summary>
        /// Sets the type of the collection.
        /// </summary>
        /// <typeparam name="TKey">Type of the dictionary keys.</typeparam>
        /// <typeparam name="TValue">Type of the dictionary values.</typeparam>
        /// <param name="property">Property.</param>
        /// <returns>Property.</returns>
        public static IEditorExportProperty<GDC.Dictionary> DictionaryType<
            [MustBeVariant] TKey,
            [MustBeVariant] TValue>
        (this IEditorExportProperty<GDC.Dictionary> property)
        {
            property.SetPropertyHint(
                PropertyHint.ArrayType,
                $"{typeof(TKey).Name},{typeof(TValue).Name}"
            );

            return property;
        }
    }
}
