using Godot;
using System.Collections.Generic;

public partial class FurnitureMover : Node
{
    private List<Furniture> m_registeredFurnitures = new();
    private Furniture m_holdingFurniture = null;

    private void ProcessGlobalClick(IClickable _c)
    {
        if (_c is Furniture _furniture) OnFurnitureSelect(_furniture);
    }

    private void OnFurnitureSelect(Furniture _furniture)
    {
        GD.Print($"Current selected object: {_furniture.Name}");
    }
}
