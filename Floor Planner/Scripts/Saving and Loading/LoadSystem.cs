using Godot;
using System.IO;
using System.Xml.Serialization;

public partial class LoadSystem : Node
{
    //  Temporary direct link to the furniture manager.
    [Export] private FileDialog m_fileDialog;
    [Export] private FurnitureManager m_furnitureManager;

    public void OnLoadButtonPressed()
    {
        LoadFurniture();
    }

    /// <summary>
    /// Test function to save an individual furniture object.
    /// Later this will save one data structure that holds all of the information.
    /// </summary>
    public void LoadFurniture()
    {
        using (var stream = File.Open(m_fileDialog.CurrentPath, FileMode.OpenOrCreate))
        {
            var formatter   = new XmlSerializer(typeof(FurnitureData));

            if (formatter.Deserialize(stream) is not FurnitureData _data)
            {
                GD.PrintErr("Selected file is not a valid file type");
                return;
            }
            m_furnitureManager.selectedFurniture.LoadFrom(_data);
        }

        GD.Print("Loaded furniture's position");
    }
}