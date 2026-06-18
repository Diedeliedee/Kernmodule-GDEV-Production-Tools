using System.IO;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public static class FileBridge
{
    /// <summary>
    /// Saves a file to the computer to the given path.
    /// </summary>
    /// <returns>If the save was successful or not.</returns>
    public static bool SaveTo<T>(T _objectToSave, string _path) where T : class
    {
        //  Construct a class for the settings.
        var settings = new JsonSerializerSettings();

        //  Ignore reference loops. Certain properties in the Vector3 struct, like "normalized" will self-reference.
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        try
        {
            var json            = JsonConvert.SerializeObject(_objectToSave, settings);
            using var stream    = File.Create(_path);

            stream.Write(Encoding.UTF8.GetBytes(json));

            Debug.Log($"File succesfully saved {typeof(T)} to: {_path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Something went wrong trying to save {typeof(T)} to: {_path}. Exception goes as follows:\n{e.Message}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Saves a raw byte array to the computer to the given path.
    /// </summary>
    /// <returns>If the save was successful or not.</returns>
    public static bool SaveRawTo(byte[] _data, string _path)
    {
        try
        {
            using var stream = File.Create(_path);

            stream.Write(_data);
            Debug.Log($"File succesfully saved to: {_path}");
        }
        catch
        {
            Debug.Log($"Something went wrong trying to save raw byte array to: {_path}");
            return false;
        }

        return false;
    }

    /// <summary>
    /// Loads a file from the given location through the given path.
    /// </summary>
    /// <returns>The loaded and deserialized object.</returns>
    public static T LoadFrom<T>(string _path) where T : class
    {
        try
        {
            if (!File.Exists(_path))
            {
                Debug.LogError($"No file found at: {_path}");
                return null;
            }

            var json            = File.ReadAllText(_path);
            var objectToLoad    = JsonConvert.DeserializeObject<T>(json);

            return objectToLoad;
        }
        catch
        {
            Debug.Log($"Something went wrong trying to load {typeof(T)} from: {_path}");
            return null;
        }
    }
}