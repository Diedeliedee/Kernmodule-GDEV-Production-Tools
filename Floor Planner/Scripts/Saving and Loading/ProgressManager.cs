using Godot;

public partial class ProgressManager : Node
{
    //  Temporary direct link to the furniture manager.
    [Export] private FurnitureManager m_furnitureManager;
    [Export] private SaveLoadSystem m_saveLoadSystem;
    [Export] private FileDialog m_saveDialog;
    [Export] private FileDialog m_loadDialog;

    public void OnSaveConfirmed()
    {
        m_saveLoadSystem.Save(m_furnitureManager.SaveTo(), m_saveDialog.CurrentPath);
    }

    public void OnLoadConfirmed()
    {
        m_furnitureManager.LoadFrom(m_saveLoadSystem.Load(m_loadDialog.CurrentPath));
    }
}