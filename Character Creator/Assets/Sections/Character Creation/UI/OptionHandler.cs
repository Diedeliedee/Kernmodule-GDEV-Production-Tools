using UnityEngine;
using BodyPartSwap;
using TMPro;

public class OptionHandler : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private TextMeshProUGUI m_header;
    [SerializeField] private TextMeshProUGUI m_activeSelection;

    //  Cache:
    private IChangeablePart m_linkedPart;

    public void Setup(IChangeablePart _linkedPart)
    {
        m_linkedPart    = _linkedPart;
        m_header.text   = _linkedPart.type.ToString();

        _linkedPart.SubscribeToCallback(OnAttachedPartUpdated);
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
        m_linkedPart.ProcessSwap(_offset);
    }

    private void OnAttachedPartUpdated(SwapCallbackResponse _response)
    {
        m_activeSelection.text = _response.chosenPartName;
    }

    private void OnDestroy()
    {
        m_linkedPart.UnsubrscibeFromCallback(OnAttachedPartUpdated);
    }
}
