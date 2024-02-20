using Godot;

public partial class InputReader : Node
{
    [Export] private RaycastHandler floorRaycast;

    public bool leftClickPressed => Input.IsActionJustPressed("Click");
    public bool leftClickHolding => Input.IsActionPressed("Click");
    public bool leftClickReleased => Input.IsActionJustReleased("Click");

    public Vector2 mousePosition => GetViewport().GetMousePosition();

    /// <returns>A position representing where the mouse intersects with a theoretical plane at Y = 0.</returns>
    public Vector3 planarMousePosition
    {
        get 
        { 
            floorRaycast.Hit(out RaycastHandler.Result _result);
            return _result.point;
        }
    }
}