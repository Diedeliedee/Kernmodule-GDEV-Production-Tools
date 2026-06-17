using System.Collections.Generic;
using UnityEngine;

namespace BodyPartSwap
{
    [CreateAssetMenu(fileName = "New Queue", menuName = "Part Swapping/Queue", order = 2)]

    public class OptionQueue : ScriptableObject
    {
        [SerializeField] private Options m_type;
        [Space]
        [SerializeField] private List<PartInfo> m_parts;

        public Options type     => m_type;
        public int queueLength  => m_parts.Count;

        public PartInfo GetFromQueue(int _index)
        {
            if (_index < 0)
            {
                Debug.LogError($"Cannot find the negative index: {_index} in the queue.", this);
                return null;
            }

            if (_index >= m_parts.Count)
            {
                Debug.LogWarning($"Requested index: {_index} exceed queue amout. Returning last instead.", this);
                return m_parts[^1];
            }

            return m_parts[_index];
        }

        public bool Contains(PartInfo _part)
        {
            return m_parts.Contains(_part);
        }

        public void AddToQueue(PartInfo _part)
        {
            if (_part == null || _part.mesh == null)
            {
                Debug.LogError($"It appears the {_part.type} model you want to add to the {m_type} queue is null or corrupt. Please try again!", this);
                return;
            }

            m_parts.Add(_part);
        }

        public void RemoveFromQueue(PartInfo _part)
        {
            if (_part == null || _part.mesh == null)
            {
                Debug.LogError($"It appears the {_part.type} model you want to remove from the {m_type} queue is null or corrupt. Please try again!", this);
                return;
            }

            m_parts.Remove(_part);
        }
    }
}
