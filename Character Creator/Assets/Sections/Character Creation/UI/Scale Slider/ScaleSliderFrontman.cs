using BodyPartSwap;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScaleSliderFrontman : MonoBehaviour
{
    [Header("Reference:")]
    [SerializeField] private Slider m_widthSlider;
    [SerializeField] private Slider m_heightSlider;
    [SerializeField] private Slider m_depthSlider;

    //  Events:
    private Action<int, float> m_onScaleUpdated = null;

    public void Subscribe(Action<int, float> _action)
    {
        m_onScaleUpdated += _action;
    }

    public void UnSubscribe(Action<int, float> _action)
    {
        m_onScaleUpdated -= _action;
    }

    public void OnWidthUpdated(float _value)    => m_onScaleUpdated?.Invoke(0, _value);
    public void OnHeightUpdated(float _value)   => m_onScaleUpdated?.Invoke(1, _value);
    public void OnDepthUpdated(float _value)    => m_onScaleUpdated?.Invoke(2, _value);

    public void ApplySettings(Vector3 _scale)
    {
        m_widthSlider.SetValueWithoutNotify(_scale.x);
        m_heightSlider.SetValueWithoutNotify(_scale.y);
        m_depthSlider.SetValueWithoutNotify(_scale.z);
    }
}
