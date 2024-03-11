using Godot;
using System.Collections.Generic;

public partial class FurnitureManager : Node
{
    [Export] public Node furnitureParent;
    [Export] public GizmoHandler gizmo;

    private List<Furniture> m_registeredFurnitures = new();
    private Furniture m_holdingFurniture = null;

    public Furniture selectedFurniture => m_holdingFurniture;

    public override void _Ready()
    {
        //  Register all furnitures already in the furniture parent.
        foreach (var _child in GetFurnitureFromNode(furnitureParent))
        {
            if (_child is Furniture _furniture) Register(_furniture);
        }
    }

    public override void _Process(double delta)
    {
        if (m_holdingFurniture != null)
        {
            m_holdingFurniture.IterateTo(gizmo.GlobalPosition, (float)delta);
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

    /// <summary>
    /// Called when a furniture is being selected. Caused by a click form the ClickHandler.
    /// </summary>
    private void OnFurnitureSelect(Furniture _furniture)
    {
        if (_furniture.GetHashCode() == m_holdingFurniture?.GetHashCode()) return;

        GD.Print($"Current selected object: {_furniture.Name}");
        gizmo.position = new Vector2(_furniture.GlobalPosition.X, _furniture.GlobalPosition.Z);
        m_holdingFurniture = _furniture;
    }

    /// <returns>All furniture classes from a tree of nodes.</returns>
    private List<Furniture> GetFurnitureFromNode(Node _node)
    {
        var _furnitures = new List<Furniture>();

        CheckChildren(_node);
        return _furnitures;

        void CheckChildren(Node _node)
        {
            foreach (var _child in _node.GetChildren())
            {
                if (_child is Furniture _furniture) _furnitures.Add(_furniture);
                CheckChildren(_child);
            }
        }
    }
}
