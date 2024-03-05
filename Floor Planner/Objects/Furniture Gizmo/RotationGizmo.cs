using Godot;

public partial class RotationGizmo : StaticBody3D, IClickable
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
    }

    public void OnRelease()
    {
    }

    public void WhileHold()
    {
    }
}
