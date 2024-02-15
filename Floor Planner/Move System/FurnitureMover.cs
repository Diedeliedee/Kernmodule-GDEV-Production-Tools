using Godot;
using System;

public partial class FurnitureMover : Node
{
    [Export] private ClickHandler m_clickHandler;
    [Export] private Node3D 

    private Furniture m_holdingFurniture = null;

    public override void _Ready()
    {
        m_clickHandler.m_onClick += ProcessGlobalClick;
    }

    private void ProcessGlobalClick(IClickable _c)
    {
        if (_c is Furniture _furniture) OnFurnitureSelect(_furniture);
    }

    private void OnFurnitureSelect(Furniture _furniture)
    {
        GD.Print($"Current selected object: {_furniture.Name}");
    }

    //  Study this function carefully!
    public static Vector3? CalculateIntersectionWithYZero(Vector3 P0, Vector3 V)
    {
        // Check if the line is horizontal (no intersection with Y =  0)
        if (V.Y == 0)
        {
            return null; // No intersection
        }

        // Calculate the parameter t at the intersection point
        float t = -P0.Y / V.Y;

        // Calculate the intersection point coordinates
        float x = P0.X + V.X * t;
        float z = P0.Z + V.Z * t;

        return new Vector3(x, 0, z); // Return the intersection point
    }
}
