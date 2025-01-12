using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class RecentSaveDisplay : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private WarningText m_warningText;

    [Header("Events:")]
    [SerializeField] private UnityEvent<CharacterSetupMemory> m_onSaveSelected;

    //  Cache:
    private List<RecentSaveButton> m_displays = new();

    public void Setup()
    {
        //  Load all registered saves.
        var saves = RecentSaveTracker.GetRegisteredSaves();

        foreach (var save in saves)
        {
            var spawnedDisplay = Instantiate(m_prefab, transform, false).GetComponent<RecentSaveButton>();

            spawnedDisplay.Setup(save);
            spawnedDisplay.onSaveSelected.AddListener(HandleSave);

            m_displays.Add(spawnedDisplay);
        }
    }

    public void HandleSave(SaveRegistration _registration)
    {
        var loadedSave = FileBridge.LoadFrom<CharacterSetupMemory>(_registration.location);

        if (loadedSave == null)
        {
            m_warningText.DisplayMessage(   $"File '{_registration.name}' was not found at the registered location! " +
                                            $"It probably got moved somewhere else or deleted..");
            
            return;
        }

        m_onSaveSelected.Invoke(loadedSave);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_displays.Count; i++)
        {
            m_displays[i].onSaveSelected.RemoveListener(HandleSave);
        }
    }
}
