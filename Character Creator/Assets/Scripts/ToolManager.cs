using System.Collections;
using System.Collections.Generic;
using BodyPartSwap;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolManager : MonoBehaviour
{
    private Blackboard m_blackboard = null;

    private SetupManager m_setup        = null;
    private CreatorManager m_creator    = null;

    private void Awake()
    {
        m_blackboard = GetComponent<Blackboard>();

        m_setup     = GetComponentInChildren<SetupManager>(true);
        m_creator   = GetComponentInChildren<CreatorManager>(true);

        Blackboard.ResetBlackboard(m_blackboard);
    }

    public void RequestMoveToCreator(CharacterSetupMemory _saveFile)
    {
        Blackboard.instance.loadedSave = _saveFile;

        m_setup.gameObject.SetActive(false);
        m_creator.gameObject.SetActive(true);

        m_creator.Begin();
    }

    public void RequestReloadTool()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
