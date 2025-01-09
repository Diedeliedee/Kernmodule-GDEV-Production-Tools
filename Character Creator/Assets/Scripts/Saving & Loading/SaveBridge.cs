using System.IO;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public static class SaveBridge
{
    public static void SaveTo(CharacterSetupMemory _objectToSave, string _path)
    {
        try
        {
            var json            = JsonConvert.SerializeObject(_objectToSave);
            using var stream    = File.Create(_path);

            stream.Write(Encoding.UTF8.GetBytes(json));

            RecentSaveTracker.RegisterRecentSave(_path);

            Debug.Log($"File succesfully saved character to:\n{_path}");
        }
        catch
        {
            Debug.Log($"Something went wrong trying to save character to: {_path}");
            return;
        }
    }

    public static void SaveRawTo(byte[] _data, string _path)
    {
        try
        {
            using var stream = File.Create(_path);

            stream.Write(_data);
            Debug.Log($"File succesfully saved to:\n{_path}");
        }
        catch
        {
            Debug.Log($"Something went wrong trying to save raw byte array to: {_path}");
            return;
        }
    }

    public static CharacterSetupMemory LoadFrom(string _path)
    {
        try
        {
            if (!File.Exists(_path))
            {
                Debug.LogError($"No character file found at:\n{_path}");
                return default;
            }

            var json            = File.ReadAllText(_path);
            var objectToLoad    = JsonConvert.DeserializeObject<CharacterSetupMemory>(json);

            return objectToLoad;
        }
        catch
        {
            Debug.Log($"Something went wrong trying to load character from: {_path}");
            return default;
        }
    }
}