using SFB;
using UnityEngine;

public static class ExplorerWrapper
{
    public static readonly ExtensionFilter jsonFilter   = new ExtensionFilter("Json Files", "json");
    public static readonly ExtensionFilter pngFilter    = new ExtensionFilter("Image Files", "png");

    public static string GetSaveLocation(string _header, string _defaultName, params ExtensionFilter[] _extensions)
    {
        //  Caching location.
        var path = StandaloneFileBrowser.SaveFilePanel(_header, Application.persistentDataPath, _defaultName, _extensions);

        //  Error handling.
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Something went wrong during file selection! Beware..");
            return string.Empty;
        }

        return path;
    }

    public static string GetLoadLocation(string _header, params ExtensionFilter[] _extensions)
    {
        //  Caching location.
        var paths = StandaloneFileBrowser.OpenFilePanel(_header, Application.persistentDataPath, _extensions, false);

        //  Error handling.
        if (paths == null || paths.Length <= 0)
        {
            Debug.LogError("Something went wrong during file selection! Beware..");
            return string.Empty;
        }

        return paths[0];
    }
}