using UnityEngine;

namespace BodyPartSwap
{
    [CreateAssetMenu(fileName = "New Queue", menuName = "Part Swapping/Queue", order = 1)]

    public class OptionQueue : ScriptableObject
    {
        [SerializeField] private Options m_type;
        [SerializeField] private GameObject[] m_parts;

        public Options type     => m_type;
        public int queueLength  => m_parts.Length;

        public GameObject GetFromQueue(int _index)
        {
            if (_index < 0)
            {
                Debug.LogError($"Cannot find the negative index: {_index} in the queue.", this);
                return null;
            }

            if (_index >= m_parts.Length)
            {
                Debug.LogWarning($"Requested index: {_index} exceed queue amout. Returning last instead.", this);
                return m_parts[^1];
            }

            return m_parts[_index];
        }
    }
}
