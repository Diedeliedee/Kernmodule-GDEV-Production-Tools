using BodyPartSwap;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolManager : MonoBehaviour
{
    private Blackboard m_blackboard = null;

    private SetupManager m_setup        = null;
    private CreatorManager m_creator    = null;
    private PhotoManager m_photo        = null;

    private void Awake()
    {
        m_blackboard = GetComponent<Blackboard>();

        m_setup     = GetComponentInChildren<SetupManager>(true);
        m_creator   = GetComponentInChildren<CreatorManager>(true);
        m_photo     = GetComponentInChildren<PhotoManager>(true);

        Blackboard.ResetBlackboard(m_blackboard);

        m_creator.Setup();
        m_photo.Setup();
    }

    public void RequestStartCharacterCreator()
    {
        RequestMoveToCreator();

        m_creator.ApplySaveFile(Blackboard.instance.loadedSave);
    }

    public void RequestMoveToCreator()
    {
        m_setup.gameObject.SetActive(false);
        m_photo.gameObject.SetActive(false);

        m_creator.gameObject.SetActive(true);
    }

    public void RequestMoveToPhotoMode()
    {
        m_setup.gameObject.SetActive(false);
        m_creator.gameObject.SetActive(false);

        m_photo.gameObject.SetActive(true);

        m_photo.ApplyConfiguration(Blackboard.instance.activeConfiguration);
    }

    public void RequestReloadTool()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RequestQuit()
    {
        Application.Quit();
    }
}
