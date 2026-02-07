using CosinefulPalettes;

using Godot;

public partial class MyNode : Node
{
    [Export] public CosinefulPalette Palette { get; set; } = null!;

    public override void _Ready()
    {
        // Query the palette using offset value
        // 0.5f means middle of the gradient.
        var colorFromOffset = Palette.GetColor(0.5f);

        // Query the palette using index value.
        // Here we are querying color with index of 1.
        var colorFromIndex = Palette.GetColor(1);

        // Query all colors in the palette.
        var colors = Palette.GetColorsArray();

        // Query the palette as a gradient.
        var gradient = Palette.GetColorsGradient();

        // We can randomize the palette and get new colors.
        Palette.Randomize(1337);

        // .. or we can set the component values ourselves.
        Palette.Brightness = new Vector3(0.25f, 0.25f, 0.25f);
        Palette.Contrast = new Vector3(0.33f, 0.33f, 0.33f);
        Palette.Frequency = new Vector3(0.5f, 0.5f, 0.5f);
        Palette.Range = new Vector3(0.5f, 0.5f, 0.5f);

        var palette = CosinefulPalette.Create(
            brightness: new Vector3(0.25f, 0.25f, 0.25f),
            contrast: new Vector3(0.33f, 0.33f, 0.33f),
            frequency: new Vector3(0.5f, 0.5f, 0.5f),
            range: new Vector3(0.5f, 0.5f, 0.5f)
        );
    }
}
