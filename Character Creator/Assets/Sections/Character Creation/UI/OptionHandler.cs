using UnityEngine;
using BodyPartSwap;
using TMPro;

public class OptionHandler : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private TextMeshProUGUI m_header;
    [SerializeField] private TextMeshProUGUI m_activeSelection;

    //  Cache:
    private Options m_type = default;

    //  Events:
    private IOptionInterface.SwapRequest m_swapRequest = null;

    //  Properties:
    public Options type => m_type;

    public void Setup(Options _type, string _headerName, IOptionInterface.SwapRequest _request)
    {
        m_type                  = _type;
        m_header.text           = _headerName;
        m_swapRequest           = _request;
    }

    public void ApplyStatus(string _selectedName)
    {
        m_activeSelection.text  = _selectedName;
    }

    public void PullNext()
    {
        SendAndProcessSwapRequest(1);
    }

    public void PullPrevious()
    {
        SendAndProcessSwapRequest(-1);
    }

    private void SendAndProcessSwapRequest(int _offset)
    {
        if (m_swapRequest == null)
        {
            Debug.LogWarning("No listener has been attached to this options' swap request! Be advised..", this);
            return;
        }

        var response = m_swapRequest.Invoke(m_type, _offset);

        if (response.result == SwapCallbackResponse.Result.Failure)
        {
            Debug.LogWarning("Something went wrong while trying to swap a body part! Be advised..", this);
            return;
        }

        ApplyStatus(response.name);
    }
}
