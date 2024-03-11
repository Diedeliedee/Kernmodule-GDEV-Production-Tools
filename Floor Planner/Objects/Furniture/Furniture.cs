using Godot;

public partial class Furniture : StaticBody3D, IClickable, ISavable<FurnitureData>
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

    public FurnitureData SaveTo()
    {
        var data = new FurnitureData();

        data.xPos       = GlobalPosition.X;
        data.zPos       = GlobalPosition.Z;
        data.rotation   = GlobalRotationDegrees.Y;
        data.width      = Scale.X;
        data.depth      = Scale.Z;
        data.height     = Scale.Y;

        return data;
    }

    public void LoadFrom(FurnitureData _data)
    {
        GlobalPosition          = new Vector3(_data.xPos, GlobalPosition.Y, _data.zPos);
        GlobalRotationDegrees   = new Vector3(GlobalRotationDegrees.X, _data.rotation, GlobalRotationDegrees.Z);
        Scale                   = new Vector3(_data.width, _data.height, _data.depth);
    }
}
