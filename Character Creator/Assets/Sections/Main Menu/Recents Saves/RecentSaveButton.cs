using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RecentSaveButton : MonoBehaviour
{
    private UnityEvent<SaveRegistration> m_onSaveSelected = new();
    private SaveRegistration m_registration               = null;

    public UnityEvent<SaveRegistration> onSaveSelected => m_onSaveSelected;

    public void Setup(SaveRegistration _registration)
    {
        var textMesh = GetComponentInChildren<TextMeshProUGUI>(true);

        textMesh.text = $"{Path.GetFileName(_registration.location)}\n" +
                        $"{_registration.date}\n" +
                        $"Version: {_registration.version}";

        m_registration = _registration;
    }

    public void OnButtonPressed()
    {
        m_onSaveSelected.Invoke(m_registration);
    }
}
