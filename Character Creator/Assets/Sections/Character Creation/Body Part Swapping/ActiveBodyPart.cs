using UnityEngine;

namespace BodyPartSwap
{
    public partial class ActiveBodyPart : MonoBehaviour
    {
        [Header("Reference:")]
        [SerializeField] private OptionQueue m_queue;
        [SerializeField] private GameObject m_modelObject;

        //  Run-time:
        private int m_index = 0;

        //  Events:
        private System.Action<SwapCallbackResponse> m_onChanged = null;

        /// <summary>
        /// What type of part this script is attached to.
        /// </summary>
        public Options type => m_queue.type;
        public int index    => m_index;

        public void SubscribeToCallback(System.Action<SwapCallbackResponse> _callback)
        {
            m_onChanged += _callback;
        }

        public void UnsubrscibeFromCallback(System.Action<SwapCallbackResponse> _callback)
        {
            m_onChanged -= _callback;
        }

        public void ProcessSwap(int _offset)
        {
            var trueIndex = (m_index + _offset) % m_queue.queueLength;

            UpdatePartWithIndex(trueIndex);
        }

        public void UpdatePartWithIndex(int _index)
        {
            var pulled              = m_queue.GetFromQueue(_index);
            var createdModelObject  = Instantiate(pulled, transform, false);

            Destroy(m_modelObject);

            m_modelObject       = createdModelObject;
            m_modelObject.name  = pulled.name;
            m_index             = _index;

            var callBack = new SwapCallbackResponse
            {
                part = pulled,
            };

            m_onChanged?.Invoke(callBack);
        }
    }
}