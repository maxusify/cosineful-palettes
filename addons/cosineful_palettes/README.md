# Cosineful Palettes

![Godot Engine](https://img.shields.io/badge/GODOT-%23FFFFFF.svg?style=for-the-badge&logo=godot-engine) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)


<p align="center">
    <img src="icon.svg" alt="Cosineful Palette" width="150" />
</p>

Addon for Godot Engine 4.4+ (Mono/.NET enabled) that allows you to create beautiful palettes with a simple and intuitive custom resource. Color generation is based on the cosine formula greatly inspired by [Inigo Quilez article](https://iquilezles.org/articles/palettes/).

## Installation

Recommended way to install Cosineful Palettes is through [GodotEnv](https://github.com/chickensoft-games/GodotEnv). In your `addons.jsonc` file add:

```json
{
    "addons": {
        // ... other addons ...

        "cosineful_palettes": {
            "url": "https://github.com/maxusify/cosineful-palettes",
            "subfolder": "addons/cosineful_palettes"
        }
    }
}
```

After that you can install the addon by running:

```sh
godotenv addons install
dotnet build
```

Make sure you have **BUILT** the project and **ENABLED** addon in your Godot project settings!

<p align="center">
    <img 
        src="assets/images//installation-enable-addon.jpg"
        alt="Enable addon in Godot project settings"
    />
</p>

## Usage

You can create `CosinefulPalette` resource through editor inspector:

<p align="center">
    <img 
        src="assets/images/showcase-editor-inspector-01.jpg"
        alt="Create CosinefulPalette resource through editor inspector"
    />
</p>

If you want to create `CosinefulPalette` resource programmatically, you can use the `CosinefulPalette.Create` method:


```csharp
CosinefulPalette palette = CosinefulPalette.Create(
    brightness: new Vector3(0.25f, 0.25f, 0.25f),
    contrast: new Vector3(0.33f, 0.33f, 0.33f),
    frequency: new Vector3(0.5f, 0.5f, 0.5f),
    range: new Vector3(0.5f, 0.5f, 0.5f)
);
```

If random outcome is desired, simply use empty constructor and call `Randomize` method:

```csharp
CosinefulPalette palette = new CosinefulPalette();
palette.Randomize();
```
 
You can also query the palette as follows:

```csharp
using CosinefulPalettes;

using Godot;

public partial class MyNode : Node
{
    [Export] public CosinefulPalette Palette { get; set; } = default!;

    public override void _Ready()
    {
        // Query the palette using offset value
        // 0.5f means middle of the gradient.
        Color colorFromOffset = Palette.GetColor(0.5f);

        // Query the palette using index value.
        // Here we are querying color with index of 1.
        Color colorFromIndex = Palette.GetColor(1);

        // Query all colors in the palette.
        Color[] colors = Palette.GetColorsArray();

        // Query the palette as a gradient.
        Gradient gradient = Palette.GetColorsGradient();

        // We can randomize the palette and get new colors.
        Palette.Randomize();

        // .. or we can set the component values ourselves.
        Palette.Brightness = new Vector3(0.25f, 0.25f, 0.25f);
        Palette.Contrast = new Vector3(0.33f, 0.33f, 0.33f);
        Palette.Frequency = new Vector3(0.5f, 0.5f, 0.5f);
        Palette.Range = new Vector3(0.5f, 0.5f, 0.5f);

        // ...
    }
}
```
