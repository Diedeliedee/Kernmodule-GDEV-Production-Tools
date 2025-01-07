using UnityEngine;
using BodyPartSwap;
using TMPro;

public class OptionHandler : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] private Options m_type = default;

    [Header("Reference:")]
    [SerializeField] private TextMeshProUGUI m_activeSelection;

    //  Run-time:
    private int m_index = 0;

    //  Events:
    private OptionFrontman.QueueRequest m_queueRequest = null;

    public void Setup(OptionFrontman.QueueRequest _queueRequest)
    {
        m_queueRequest = _queueRequest;

        //  Call the switch call-back, so that the model gets updated with the correct first option at the start of the scene.
        SendAndProcessSwapRequest(m_index);
    }

    public void PullNext()
    {
        SendAndProcessSwapRequest(++m_index);
    }

    public void PullPrevious()
    {
        SendAndProcessSwapRequest(--m_index);
    }

    /// <summary>
    /// Send a swap request to the frontman of the options menu, and receives a response from wherever it got it from to process.
    /// </summary>
    private void SendAndProcessSwapRequest(int _index)
    {
        var response = m_queueRequest.Invoke(m_type, _index);

        m_activeSelection.text  = response.part.name;
        m_index                 = response.responseIndex;
    }
}
