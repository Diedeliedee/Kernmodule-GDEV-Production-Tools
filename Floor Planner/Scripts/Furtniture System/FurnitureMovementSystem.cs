using Godot;

public partial class FurnitureMovementSystem : Node
{
    [Export] public GizmoHandler gizmo;

    private Furniture m_holdingFurniture = null;

    public Furniture selectedFurniture => m_holdingFurniture;

    public override void _Process(double delta)
    {
        if (m_holdingFurniture != null)
        {
            m_holdingFurniture.IterateTo(gizmo.GlobalPosition, (float)delta);
        }
    }

    /// <summary>
    /// Called when a furniture is being selected. Caused by a click form the ClickHandler.
    /// </summary>
    public void OnFurnitureSelect(Furniture _furniture)
    {
        if (_furniture.GetHashCode() == m_holdingFurniture?.GetHashCode()) return;

        GD.Print($"Current selected object: {_furniture.Name}");
        gizmo.position = new Vector2(_furniture.GlobalPosition.X, _furniture.GlobalPosition.Z);
        m_holdingFurniture = _furniture;
    }
}
