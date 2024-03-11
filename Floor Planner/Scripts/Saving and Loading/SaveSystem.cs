using Godot;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public partial class SaveSystem : Node
{
    //  Temporary direct link to the furniture manager.
    [Export] private FileDialog m_fileDialog;
    [Export] private FurnitureManager m_furnitureManager;

    public void OnSaveButtonPressed()
    {
        SaveFurniture(m_furnitureManager.selectedFurniture.SaveTo());
    }

    /// <summary>
    /// Test function to save an individual furniture object.
    /// Later this will save one data structure that holds all of the information.
    /// </summary>
    public void SaveFurniture(FurnitureData _data)
    {
        using (var stream = File.Open(m_fileDialog.CurrentPath, FileMode.OpenOrCreate))
        {
            var formatter   = new XmlSerializer(_data.GetType());

            formatter.Serialize(stream, _data);
            stream.Flush();
            stream.Close();
        }

        GD.Print("Saved furniture's position");
    }
}