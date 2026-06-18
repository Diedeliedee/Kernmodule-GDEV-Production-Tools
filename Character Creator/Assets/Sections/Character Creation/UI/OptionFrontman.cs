using UnityEngine;
using BodyPartSwap;
using System.Collections.Generic;
using static BodyPartSwap.IOptionInterface;
using System;

public class OptionFrontman : MonoBehaviour, IOptionInterface
{
    [Header("Reference:")]
    [SerializeField] private Transform m_optionContentRoot;
    [SerializeField] private GameObject m_optionPrefab;
    
    //  Cache:
    private List<OptionHandler> m_options = new();

    //  Events:
    private SwapRequest m_swapBroadcast                     = null;
    private Action<Options, int, float > m_scaleBroadcast   = null;

    public void Setup(Dictionary<Options, IActiveElement> _compilation)
    {
        //  Attach all the parts to the corresponding option handlers.
        foreach (var pair in _compilation)
        {
            //  Spawn option handler
            var spawnedOption = Instantiate(m_optionPrefab, m_optionContentRoot, false).GetComponent<OptionHandler>();

            //  Set-up, and add to the list.
            spawnedOption.Setup(pair.Key, pair.Value.typeName, HandleSwapRequest, HandleScaleRequest);
            m_options.Add(spawnedOption);
        }
    }

    public void ApplyCompilation(Dictionary<Options, IActiveElement> _compilation)
    {
        foreach (var option in m_options)
        {
            if (!_compilation.ContainsKey(option.type)) continue;

            option.ApplyStatus(_compilation[option.type].selectedName);
            option.ApplyScaleValues(_compilation[option.type].scale);
        }
    }

    public void Subscribe(SwapRequest _swapRequest, Action<Options, int, float> _scaleRequest)
    {
        m_swapBroadcast     += _swapRequest;
        m_scaleBroadcast    += _scaleRequest;
    }

    public void Unsubscribe(SwapRequest _swapRequest, Action<Options, int, float> _scaleRequest)
    {
        m_swapBroadcast     -= _swapRequest;
        m_scaleBroadcast    -= _scaleRequest;
    }

    private SwapCallback HandleSwapRequest(Options _type, int _offset)
    {
        if (m_swapBroadcast == null)
        {
            Debug.LogError("No handler or manager is currently listening to the given swap broadcast.. Ignoring..", this);
            return default;
        }

        return m_swapBroadcast.Invoke(_type, _offset);
    }

    private void HandleScaleRequest(Options _type, int _axis, float _scale)
    {
        if (m_scaleBroadcast == null)
        {
            Debug.LogError("No handler or manager is currently listening to the given scale broadcast.. Ignoring..", this);
            return;
        }

        m_scaleBroadcast.Invoke(_type, _axis, _scale);
    }
}