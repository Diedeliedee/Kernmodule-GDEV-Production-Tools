using Godot;
using System;

public partial class ClickHandler : RayCast3D
{
    [Export] private float m_castLength = 1000f;

    //  Events:
    public Action<IClickable> m_onClick;
    public Action<IClickable> m_onRelease;

    //  Cache:
    private IClickable m_currentSelected = null;

    //  Reference:
    private Camera3D m_cam;

    public override void _Ready()
    {
        m_cam = GetParent<Camera3D>();
    }

    public override void _Process(double delta)
    {
        m_currentSelected?.WhileHold();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == MouseButton.Left)
        {
            TargetPosition = ToLocal(m_cam.ProjectRayNormal(eventMouseButton.Position) * m_castLength);
            ForceRaycastUpdate();
            if (GetCollider() is not IClickable _clickable) return;

            if (eventMouseButton.Pressed)
            {
                _clickable.OnClick();
                m_currentSelected = _clickable;
                m_onClick?.Invoke(_clickable);
            }
            if (!eventMouseButton.Pressed)
            {
                _clickable.OnRelease();
                m_currentSelected = null;
                m_onRelease?.Invoke(_clickable);
            }
        }
    }
}
