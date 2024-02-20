using Godot;

public partial class MovementGizmo : StaticBody3D, IClickable
{
    [Export] public InputReader input;

    private Vector3 m_offset = default;

    public void OnClick()
    {
        m_offset = GlobalPosition - input.planarMousePosition;
    }

    public void OnRelease()
    {
        m_offset = default;
    }

    public void WhileHold()
    {
        GD.Print(input.planarMousePosition + m_offset);
        GetParent<Node3D>().GlobalPosition = input.planarMousePosition + m_offset;
    }
}
