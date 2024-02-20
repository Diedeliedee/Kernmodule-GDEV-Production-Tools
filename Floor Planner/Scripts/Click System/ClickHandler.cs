using Godot;

public partial class ClickHandler : Node
{
    //  Reference:
    [Export] public RaycastHandler gizmoRaycast;
    [Export] public RaycastHandler furtnitureRaycast;
    [Export] public InputReader input;

    //  Cache:
    private IClickable m_currentSelected = null;

    public override void _Process(double delta)
    {
        if (input.leftClickPressed)
        {
            if (RegisterClick(gizmoRaycast)) return;
            if (RegisterClick(furtnitureRaycast)) return;
        }
        if (input.leftClickReleased && m_currentSelected != null)
        {
            m_currentSelected.OnRelease();
            m_currentSelected = null;
            return;
        }
        m_currentSelected?.WhileHold();
    }

    private bool RegisterClick(RaycastHandler _raycaster)
    {
        if (!_raycaster.Hit(out RaycastHandler.Result _result)) return false;
        if (_result.collider is not IClickable _clickable) return false;

        _clickable.OnClick();
        m_currentSelected = _clickable;
        return true;
    }
}
