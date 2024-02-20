using Godot;

public partial class MovementGizmo : StaticBody3D, IClickable
{
    [Export] public InputReader input;

    public void OnClick()
    {

    }

    public void OnRelease()
    {

    }

    public void WhileHold()
    {
        GD.Print(input.planarMousePosition);
        GetParent<Node3D>().GlobalPosition = input.planarMousePosition;
    }
}
