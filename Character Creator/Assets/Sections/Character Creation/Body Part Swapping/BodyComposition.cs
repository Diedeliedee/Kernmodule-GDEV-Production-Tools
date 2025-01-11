using System.Collections.Generic;
using BodyPartSwap;
using UnityEngine;

public class BodyComposition : MonoBehaviour
{
    [Header("Scene References:")]
    [SerializeField] private ActiveBodyPart m_head;
    [SerializeField] private ActiveBodyPart m_torso;
    [SerializeField] private ActiveBodyPart m_legs;

    private Dictionary<Options, IActiveElement> m_partCompilation = new();

    public Dictionary<Options, IActiveElement> compilation => m_partCompilation;

    public void Setup()
    {
        //  Compile the loose part references into a dictionary
        m_partCompilation.Add(Options.Head, m_head);
        m_partCompilation.Add(Options.Torso, m_torso);
        m_partCompilation.Add(Options.Legs, m_legs);
    }

    public void ApplyConfiguration(Dictionary<Options, int> _configuration)
    {
        if (m_partCompilation.Count <= 0)
        {
            Debug.LogError("Compilation in body composition class is null, when trying to apply a configuration. Likely the setup function has not been called yet.", this);
            return;
        }

        //  Iterate, and initialize every part in the compilation.
        foreach (var pair in m_partCompilation)
        {
            //  If the save file does not have information about a specific part, set it to the default index.
            if (!_configuration.ContainsKey(pair.Key))
            {
                _configuration.Add(pair.Key, 0);
            }

            //  Update every active part's index with that of the save file's corresponding option.
            pair.Value.ApplyIndex(_configuration[pair.Key]);
        }
    }

    public Dictionary<Options, int> ExtractConfiguration()
    {
        var configuration = new Dictionary<Options, int>();

        foreach (var pair in m_partCompilation)
        {
            configuration.Add(pair.Key, pair.Value.index);
        }
        return configuration;
    }

    public SwapCallbackResponse ProcessIncomingSwap(Options _type, int _offset)
    {
        if (!m_partCompilation.ContainsKey(_type))
        {
            Debug.LogError($"Warning! The swappable element of type {_type} is not found in the manager's dictionary!", this);
            return default;
        }

        return m_partCompilation[_type].ProcessSwap(_offset);
    }
}