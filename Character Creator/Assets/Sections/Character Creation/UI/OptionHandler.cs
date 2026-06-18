using UnityEngine;
using BodyPartSwap;
using TMPro;
using System;

public class OptionHandler : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private TextMeshProUGUI m_header;
    [SerializeField] private TextMeshProUGUI m_activeSelection;
    [SerializeField] private ScaleSliderFrontman m_sliderFrontman;

    //  Cache:
    private Options m_type = default;

    //  Events:
    private IOptionInterface.SwapRequest m_swapRequest  = null;
    private Action<Options, int, float> m_scaleRequest  = null;

    //  Properties:
    public Options type => m_type;

    public void Setup(Options _type, string _headerName, IOptionInterface.SwapRequest _swapRequest, Action<Options, int, float> _scaleRequest)
    {
        m_type          = _type;
        m_header.text   = _headerName;
        m_swapRequest   = _swapRequest;
        m_scaleRequest  = _scaleRequest;

        m_sliderFrontman.Subscribe(SendAndProcessScaleRequest);
    }

    public void ApplyStatus(string _selectedName)
    {
        m_activeSelection.text = _selectedName;
    }

    public void ApplyScaleValues(Vector3 _scale)
    {
        m_sliderFrontman.ApplySettings(_scale);
    }

    public void PullNext()      => SendAndProcessSwapRequest(1);
    public void PullPrevious()  => SendAndProcessSwapRequest(-1);

    private void SendAndProcessSwapRequest(int _offset)
    {
        if (m_swapRequest == null)
        {
            Debug.LogWarning("No listener has been attached to this options' swap request! Be advised..", this);
            return;
        }

        var response = m_swapRequest.Invoke(m_type, _offset);

        if (response.result == SwapCallback.Result.Failure)
        {
            Debug.LogWarning("Something went wrong while trying to swap a body part! Be advised..", this);
            return;
        }

        ApplyStatus(response.name);
    }

    private void SendAndProcessScaleRequest(int _axis, float _scale)
    {
        m_scaleRequest.Invoke(m_type, _axis, _scale);
    }

    private void OnDestroy()
    {
        m_sliderFrontman.UnSubscribe(SendAndProcessScaleRequest);
    }
}
