using System.IO;
using SFB;
using UnityEngine;

public class PictureTaker : MonoBehaviour
{
    [SerializeField] private RenderTexture m_renderTexture;

    public void TakePicture()
    {
        var texture = ToTexture2D(m_renderTexture);

        //  Write the texture to a PNG byte array.
        byte[] bytes = ImageConversion.EncodeToPNG(texture);
        Destroy(texture);

        //  Testing purposes. :)
        //File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);

        //  Create extension filter for saving..
        var extenstions = new ExtensionFilter[]
        {
            new("Image Files", "png"),
        };

        //  Load a path via the file browser.
        var path = StandaloneFileBrowser.SaveFilePanel("Save Photo", Application.persistentDataPath, "Nice Picture", extenstions);

        //  Error handling.
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Something went wrong during file selection! Beware..", this);
            return;
        }

        //  Save the file to that path.
        FileBridge.SaveRawTo(bytes, path);
    }

    private Texture2D ToTexture2D(RenderTexture _renderTexture)
    {
        Debug.Log(_renderTexture.format);

        var tex = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.ARGB32, false);

        RenderTexture.active = _renderTexture;

        tex.ReadPixels(new(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        tex.Apply();

        return tex;
    }
}