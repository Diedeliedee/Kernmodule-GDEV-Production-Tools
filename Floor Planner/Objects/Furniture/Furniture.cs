using Godot;

public partial class Furniture : StaticBody3D, IClickable
{
    [Export] public float smoothenWeight = 1;

    public System.Action<Furniture> onSelected = null;

    public void OnRelease()
    {

    }

    public void OnClick()
    {
        onSelected?.Invoke(this);
    }

    public void WhileHold()
    {

    }

    public void IterateTo(Vector3 target, float deltaTime)
    {
        var current = GlobalPosition;

        current.X = Mathf.Lerp(current.X, target.X, deltaTime / smoothenWeight);
        current.Y = Mathf.Lerp(current.Y, target.Y, deltaTime / smoothenWeight);
        current.Z = Mathf.Lerp(current.Z, target.Z, deltaTime / smoothenWeight);
        GlobalPosition = current;
    }
}
