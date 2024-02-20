using Godot;

public partial class Furniture : StaticBody3D, IClickable
{
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
}
