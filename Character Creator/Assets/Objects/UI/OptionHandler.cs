using UnityEngine;
using BodyPartSwap;
using TMPro;

public class OptionHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_optionHeader;
    [SerializeField] private TextMeshProUGUI m_activeSelection;

    private PartQueue m_queue = null;

    public void Setup(PartQueue _queue)
    {
        m_queue = _queue;

        m_optionHeader.text     = _queue.type.ToString();
        m_activeSelection.text  = _queue.active.name;
    }

    public void PullNext()
    {
        var pulled = m_queue.PullNext();

        //  TO-DO: Call to change model!!!
        m_activeSelection.text = pulled.name;
    }

    public void PullPrevious()
    {
        var pulled = m_queue.PullPrevious();

        //  TO-DO: Call to change model!!!
        m_activeSelection.text = pulled.name;
    }
}
