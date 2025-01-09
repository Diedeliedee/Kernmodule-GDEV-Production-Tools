using UnityEngine;
using BodyPartSwap;
using System.Collections.Generic;

public class OptionFrontman : MonoBehaviour, IOptionInterface
{
    [Header("Reference:")]
    [SerializeField] private Transform m_optionContentRoot;
    [SerializeField] private GameObject m_optionPrefab;
    
    //  Cache:
    private List<OptionHandler> m_options = new();

    //  Events:
    private IOptionInterface.SwapRequest m_swapBroadcast = null;

    public void Setup(Dictionary<Options, IActiveElement> _compilation)
    {
        //  Attach all the parts to the corresponding option handlers.
        foreach (var pair in _compilation)
        {
            //  Spawn option handler
            var spawnedOption = Instantiate(m_optionPrefab, m_optionContentRoot, false).GetComponent<OptionHandler>();

            //  Set-up, and add to the list.
            spawnedOption.Setup(pair.Key, pair.Value.typeName, pair.Value.selectedName, HandleSwapRequest);
            m_options.Add(spawnedOption);
        }
    }

    public void SubsribeToBroadcast(IOptionInterface.SwapRequest _request)
    {
        m_swapBroadcast += _request;
    }

    public void UnsubscribeFromBroadcast(IOptionInterface.SwapRequest _request)
    {
        m_swapBroadcast -= _request;
    }

    private SwapCallbackResponse HandleSwapRequest(Options _type, int _offset)
    {
        if (m_swapBroadcast == null)
        {
            Debug.LogError("No handler or manager is currently listening to the given swap broadcast.. Ignoring..", this);
            return default;
        }

        return m_swapBroadcast.Invoke(_type, _offset);
    }
}