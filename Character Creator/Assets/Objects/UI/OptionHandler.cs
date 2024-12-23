using UnityEngine;
using BodyPartSwap;
using TMPro;

public class OptionHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_optionHeader;
    [SerializeField] private TextMeshProUGUI m_activeSelection;

    private PartQueue m_queue = null;
    private System.Action<GlobalInfo.Types, BodyPartInfo> m_onSwitchTo = null;

    public GlobalInfo.Types type => m_queue.type;

    public void Setup(PartQueue _queue, System.Action<GlobalInfo.Types, BodyPartInfo> _switchCallback)
    {
        m_queue         = _queue;
        m_onSwitchTo    = _switchCallback;

        m_optionHeader.text     = _queue.type.ToString();
        m_activeSelection.text  = _queue.active.name;

        //  Call the switch call-back, so that the model gets updated with the correct first option at the start of the scene.
        _switchCallback.Invoke(_queue.type, _queue.active);
    }

    public void PullNext()
    {
        var pulled = m_queue.PullNext();

        m_onSwitchTo.Invoke(type, pulled);
        m_activeSelection.text = pulled.name;
    }

    public void PullPrevious()
    {
        var pulled = m_queue.PullPrevious();

        m_onSwitchTo.Invoke(type, pulled);
        m_activeSelection.text = pulled.name;
    }
}
