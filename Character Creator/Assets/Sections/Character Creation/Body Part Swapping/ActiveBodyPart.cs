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
            int Modulo(int _a , int _b) => _a - _b * Mathf.FloorToInt((float)_a / (float)_b);

            var trueIndex = Modulo((m_index + _offset), m_queue.queueLength);

            ApplyIndex(trueIndex);
        }

        public void ApplyIndex(int _index)
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