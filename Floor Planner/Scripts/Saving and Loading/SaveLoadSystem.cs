using Godot;
using System.IO;
using System.Xml.Serialization;

public partial class SaveLoadSystem : Node
{
    /// <summary>
    /// Test function to save an individual furniture object.
    /// Later this will save one data structure that holds all of the information.
    /// </summary>
    public void Save(LayoutData _data, string _path)
    {
        using (var stream = File.Open(_path, FileMode.OpenOrCreate))
        {
            var formatter = new XmlSerializer(_data.GetType());

            formatter.Serialize(stream, _data);
            stream.Flush();
            stream.Close();
        }

        GD.Print("Saved!");
    }

    /// <summary>
    /// Test function to save an individual furniture object.
    /// Later this will save one data structure that holds all of the information.
    /// </summary>
    public LayoutData Load(string _path)
    {
        using (var stream = File.Open(_path, FileMode.OpenOrCreate))
        {
            var formatter = new XmlSerializer(typeof(LayoutData));

            if (formatter.Deserialize(stream) is not LayoutData _data)
            {
                GD.PrintErr("Selected file is not a valid file type");
                return null;
            }

            GD.Print("Loaded!");
            return _data;
        }

    }
}