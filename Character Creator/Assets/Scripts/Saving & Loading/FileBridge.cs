using System.IO;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public static class FileBridge
{
    public static void SaveTo<T>(T _objectToSave, string _path)
    {
        try
        {
            var json            = JsonConvert.SerializeObject(_objectToSave);
            using var stream    = File.Create(_path);

            stream.Write(Encoding.UTF8.GetBytes(json));
            Debug.Log($"File succesfully saved {typeof(T)} to:\n{_path}");
        }
        catch
        {
            Debug.Log($"Something went wrong trying to save {typeof(T)} to:\n{_path}");
            return;
        }
    }

    public static T LoadFrom<T>(string _path)
    {
        try
        {
            if (!File.Exists(_path))
            {
                Debug.LogError($"No file found at:\n{_path}");
                return default;
            }

            var json            = File.ReadAllText(_path);
            var objectToLoad    = JsonConvert.DeserializeObject<T>(json);

            return objectToLoad;
        }
        catch
        {
            Debug.Log($"Something went wrong trying to load {typeof(T)} from:\n{_path}");
            return default;
        }
    }
}