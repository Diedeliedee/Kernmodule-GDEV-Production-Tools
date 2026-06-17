using System.Collections;
using System.Collections.Generic;
using BodyPartSwap;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PhotoManager : MonoBehaviour
{
    [SerializeField] private UnityEvent m_onBackRequested;

    private BodyComposition m_composition   = null;
    private SceneSwitcher m_sceneSwitcher   = null;
    private PictureTaker m_pictureTaker     = null;

    public void Setup()
    {
        m_composition   = GetComponentInChildren<BodyComposition>(true);
        m_sceneSwitcher = GetComponentInChildren<SceneSwitcher>(true);
        m_pictureTaker  = GetComponentInChildren<PictureTaker>(true);

        m_composition.Setup();
        m_sceneSwitcher.Setup();
    }

    public void ApplyMemory(OverheadMemory _memory)
    {
        //  Always import external body parts first.
        foreach (var part in _memory.externalBodyParts)
        {
            if (m_composition.ContainsBodyPart(part.info)) continue;
            m_composition.AddBodypartToQueue(part.info);
        }

        //  Then configure the right indices.
        m_composition.ApplyConfiguration(_memory.activeConfiguration);

        m_sceneSwitcher.Begin();
    }

    public void RequestGoBack()
    {
        m_sceneSwitcher.ResetScene();
        m_onBackRequested.Invoke();
    }

    public void TakePicture()
    {
        m_pictureTaker.TakePicture();
    }
}
