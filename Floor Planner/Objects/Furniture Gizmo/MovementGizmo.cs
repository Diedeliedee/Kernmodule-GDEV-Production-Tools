using Godot;

public partial class MovementGizmo : StaticBody3D, IClickable
{
    //  Reference:
    private InputReader m_input = null;

    //  Run-time:
    private Vector3 m_offset = default;

    public void Setup(InputReader _input)
    {
        m_input = _input;
    }

    public void OnClick()
    {
        m_offset = GlobalPosition - m_input.planarMousePosition;
    }

    public void OnRelease()
    {
        m_offset = default;
    }

    public void WhileHold()
    {
        GD.Print(m_input.planarMousePosition + m_offset);
        GetParent<Node3D>().GlobalPosition = m_input.planarMousePosition + m_offset;
    }
}
