using Godot;
using System.Collections.Generic;

public partial class FurnitureManager : Node
{
    [Export] public Node furnitureParent;
    [Export] public GizmoHandler gizmo;

    private List<Furniture> m_registeredFurnitures = new();
    private Furniture m_holdingFurniture = null;

    public override void _Ready()
    {
        //  Register all furnitures already in the furniture parent.
        foreach (var _child in furnitureParent.GetChildren(true))
        {
            if (_child is Furniture _furniture) Register(_furniture);
        }
    }

    /// <summary>
    /// Registers the passed in furniture in the manager.
    /// </summary>
    public void Register(Furniture _furniture)
    {
        m_registeredFurnitures.Add(_furniture);
        _furniture.onSelected += OnFurnitureSelect;
    }

    private void OnFurnitureSelect(Furniture _furniture)
    {
        GD.Print($"Current selected object: {_furniture.Name}");
        gizmo.position = new Vector2(_furniture.GlobalPosition.X, _furniture.GlobalPosition.Z);
    }
}
