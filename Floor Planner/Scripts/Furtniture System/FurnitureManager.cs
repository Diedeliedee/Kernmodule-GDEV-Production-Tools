using Godot;
using System.Collections.Generic;

public partial class FurnitureManager : Node, ISavable<LayoutData>
{
    [Export] public Node furnitureParent;
    [Export] public FurnitureMovementSystem movementSystem;

    private List<Furniture> m_registeredFurnitures = new();
    public Furniture selectedFurniture => movementSystem.selectedFurniture;

    public override void _Ready()
    {
        //  Register all furnitures already in the furniture parent.
        foreach (var _child in GetFurnitureFromNode(furnitureParent))
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

    /// <summary>
    /// Called when a furniture is being selected. Caused by a click form the ClickHandler.
    /// </summary>
    private void OnFurnitureSelect(Furniture _furniture)
    {
        movementSystem.OnFurnitureSelect(_furniture);
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

    public LayoutData SaveTo()
    {
        
    }

    public void LoadFrom(LayoutData _data)
    {
        throw new System.NotImplementedException();
    }
}
