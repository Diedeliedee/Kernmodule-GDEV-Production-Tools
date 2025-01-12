using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Events;

public class SetupManager : MonoBehaviour
{
    [SerializeField] private UnityEvent m_onStartRequested;
    [SerializeField] private UnityEvent m_onQuitRequested;

    private RecentSaveDisplay m_recentSaveDisplay = null;

    public void Setup()
    {
        m_recentSaveDisplay= GetComponentInChildren<RecentSaveDisplay>(true);

        m_recentSaveDisplay.Setup();
    }

    public void NewButtonPressed()
    {
        m_onStartRequested.Invoke();
    }

    public void LoadButtonPressed()
    {
        var path = ExplorerWrapper.GetLoadLocation("Open Saved Character", ExplorerWrapper.jsonFilter);

        Blackboard.instance.loadedSave = FileBridge.LoadFrom<CharacterSetupMemory>(path);

        m_onStartRequested.Invoke();
    }

    public void RecentLoadButtonPressed(CharacterSetupMemory _save)
    {
        Blackboard.instance.loadedSave = _save;

        m_onStartRequested.Invoke();
    }

    public void QuitButtonPressed()
    {
        m_onQuitRequested.Invoke();
    }
}
