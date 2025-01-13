using System.Collections;
using System.Collections.Generic;
using BodyPartSwap;
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

    public void ApplyConfiguration(Dictionary<Options, int> _configuration)
    {
        m_composition.ApplyConfiguration(_configuration);
    }

    public void RequestGoBack()
    {
        m_onBackRequested.Invoke();
    }

    public void TakePicture()
    {
        m_pictureTaker.TakePicture();
    }
}
