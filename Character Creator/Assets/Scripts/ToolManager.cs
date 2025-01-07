using System.Collections;
using System.Collections.Generic;
using BodyPartSwap;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    private SetupManager m_setup        = null;
    private CreatorManager m_creator    = null;

    private void Awake()
    {
        m_setup     = GetComponentInChildren<SetupManager>(true);
        m_creator   = GetComponentInChildren<CreatorManager>(true);
    }

    public void RequestMoveToCreator(CharacterSetupMemory _saveFile)
    {
        Blackboard.loadedSave = _saveFile;

        m_setup.gameObject.SetActive(false);
        m_creator.gameObject.SetActive(true);

        m_creator.Begin();
    }
}
