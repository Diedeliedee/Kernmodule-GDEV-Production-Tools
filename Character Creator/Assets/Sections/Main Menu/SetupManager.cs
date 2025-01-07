using SFB;
using UnityEngine;
using UnityEngine.Events;

public class SetupManager : MonoBehaviour
{
    [SerializeField] private UnityEvent<CharacterSetupMemory> m_onStartRequested;

    public void NewButtonPressed()
    {
        m_onStartRequested.Invoke(null);
    }

    public void LoadButtonPressed()
    {
        var extenstions = new ExtensionFilter[]
        {
            new("Json Files", "json", "JSON"),
            new("T Files", "json", "JSON"),
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Open Saved Character", Application.persistentDataPath, extenstions, false);

        if (paths == null || paths.Length <= 0)
        {
            Debug.LogError("Something went wrong during file selection! Beware..", this);
            return;
        }

        var loadedSave = FileBridge.LoadFrom<CharacterSetupMemory>(paths[0]);

        m_onStartRequested.Invoke(loadedSave);
    }
}
