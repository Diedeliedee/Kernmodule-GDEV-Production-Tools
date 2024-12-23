using System.Collections.Generic;
using UnityEngine;

namespace BodyPartSwap
{
    public class PartQueue
    {
        public readonly GlobalInfo.Types type = default;

        private BodyPartInfo m_activePart           = null;
        private List<BodyPartInfo> m_inactiveParts  = new();

        /// <summary>
        /// The currently active and selected part.
        /// </summary>
        public BodyPartInfo active => m_activePart;

        public PartQueue(GlobalInfo.Types _type)
        {
            type = _type;
        }

        /// <summary>
        /// Inserts one part into the queue.
        /// </summary>
        public void Insert(BodyPartInfo _part)
        {
            if (m_activePart == null)
            {
                m_activePart = _part;
            }
            else
            {
                m_inactiveParts.Add(_part);
            }
        }

        /// <summary>
        /// Insert a given amount of parts into the queue.
        /// </summary>
        public void Insert(params BodyPartInfo[] _parts)
        {
            for (int i = 0; i < _parts.Length; i++) Insert(_parts[i]);
        }

        /// <summary>
        /// Pull and cycle a part from the start of the list.
        /// </summary>
        public BodyPartInfo PullNext()
        {
            m_activePart = SwapAt(0);
            return m_activePart;
        }

        /// <summary>
        /// Pull and cycle a part from the end of the list.
        /// </summary>
        public BodyPartInfo PullPrevious()
        {
            m_activePart = SwapAt(m_inactiveParts.Count - 1);
            return m_activePart;
        }

        /// <summary>
        /// Swaps whatever part is currently active with a given inactive part in the list.
        /// </summary>
        private BodyPartInfo SwapAt(int _index)
        {
            if (m_inactiveParts.Count == 0 || m_inactiveParts == null)
            {
                Debug.LogError($"Attempting to pull from the {type} queue, but there are no parts in it.");
                return null;
            }

            var pulled = m_inactiveParts[_index];

            //  Placing the active part in the inactive list.
            if (m_activePart != null)
            {
                m_inactiveParts.Insert(_index, m_activePart);
                m_activePart = null;
            }

            //  Removing the inactive pulled part into the active slot.
            m_inactiveParts.Remove(pulled);
            return pulled;
        }
    }
}