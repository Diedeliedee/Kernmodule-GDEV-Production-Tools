using Godot;
using System;

public partial class GizmoHandler : Node3D
{
	private MovementGizmo m_movementGizmo = null;

    public Vector2 position
    {
        get => new Vector2(GlobalPosition.X, GlobalPosition.Z);
        set => GlobalPosition = new Vector3(value.X, GlobalPosition.Y, value.Y);
    }
    public float rotation
    {
        get => GlobalRotationDegrees.Y;
        set => GlobalRotationDegrees = new Vector3(GlobalRotationDegrees.X, value, GlobalRotationDegrees.Z);
    }

    public override void _Ready()
    {
        m_movementGizmo = GetChild<MovementGizmo>(0);
    }
}
