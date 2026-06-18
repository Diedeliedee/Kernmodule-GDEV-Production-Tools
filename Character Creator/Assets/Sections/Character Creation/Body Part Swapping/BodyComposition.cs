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

        //  Initialize all the thingymabobs.
        foreach (var kvp in m_partCompilation)
        {
            kvp.Value.Setup();
        }
    }

    public void ApplyMemory(OverheadMemory _memory)
    {
        if (m_partCompilation.Count <= 0)
        {
            Debug.LogError("Compilation in body composition class is null, when trying to apply a configuration. Likely the setup function has not been called yet.", this);
            return;
        }

        //  Always import the registered external body parts first.
        foreach (var model in _memory.externalModels)
        {
            foreach (var bodyPart in model.bodyParts)
            {
                if (ContainsBodyPart(bodyPart)) continue;
                AddBodypartToQueue(bodyPart);
            }
        }

        //  Iterate, and initialize every part in the compilation.
        foreach (var pair in m_partCompilation)
        {
            //  If the save file does not have information about a specific part, set it to the default configuration.
            if (!_memory.activeConfiguration.ContainsKey(pair.Key))
            {
                var defaultConfiguration = new BodyPartConfiguration()
                {
                    index = 0,
                    scale = Vector3.one,
                };

                _memory.activeConfiguration.Add(pair.Key, defaultConfiguration);
            }

            //  Update every active part's index with that of the save file's corresponding option.
            pair.Value.ApplyIndex(_memory.activeConfiguration[pair.Key].index);

            //  Update the scale of every active part with that of the save file.
            pair.Value.ProcessScale(_memory.activeConfiguration[pair.Key].scale);
        }
    }

    public Dictionary<Options, BodyPartConfiguration> ExtractConfiguration()
    {
        var configurations = new Dictionary<Options, BodyPartConfiguration>();

        foreach (var pair in m_partCompilation)
        {
            var configuration = new BodyPartConfiguration
            {
                index = pair.Value.index,
                scale = pair.Value.scale,
            };

            configurations.Add(pair.Key, configuration);
        }
        return configurations;
    }

    public SwapCallback ProcessIncomingSwap(Options _type, int _offset)
    {
        if (!m_partCompilation.ContainsKey(_type))
        {
            Debug.LogError($"Warning! The swappable element of type {_type} is not found in the manager's dictionary!", this);
            return default;
        }

        return m_partCompilation[_type].ProcessSwap(_offset);
    }

    public void ProcessScale(Options _type, int _axis, float _scale)
    {
        if (!m_partCompilation.ContainsKey(_type))
        {
            Debug.LogError($"Warning! The scale-able element of type {_type} is not found in the manager's dictionary!", this);
            return;
        }

        m_partCompilation[_type].ProcessScale(_axis, _scale);
    }

    public bool ContainsBodyPart(PartInfo _bodypart)
    {
        return m_partCompilation[_bodypart.type].ContainsBodyPart(_bodypart);
    }

    public void AddBodypartToQueue(PartInfo _bodypart)
    {
        m_partCompilation[_bodypart.type].AddToQueue(_bodypart);
    }
}