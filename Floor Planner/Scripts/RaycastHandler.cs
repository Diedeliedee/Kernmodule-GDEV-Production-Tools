using Godot;

public partial class RaycastHandler : RayCast3D
{
    private float m_range = 0f;
    private Camera3D m_cam = null;

    public override void _Ready()
    {
        m_range = Mathf.Abs(TargetPosition.Z);
        m_cam = GetParent<Camera3D>();
    }

    public bool Hit(out Result _result)
    {
        TargetPosition = ToLocal(m_cam.ProjectRayNormal(GetViewport().GetMousePosition()) * m_range);
        ForceRaycastUpdate();

        if (GetCollider() != null)
        {
            _result = new Result(GetCollisionPoint(), GetCollider());
            return true;
        }
        else
        {
            _result = default;
            return false;
        }
    }

    public struct Result
    {
        public readonly Vector3 point;
        public readonly GodotObject collider;

        public Result(Vector3 _point, GodotObject _collider)
        {
            point = _point;
            collider = _collider;
        }
    }
}
