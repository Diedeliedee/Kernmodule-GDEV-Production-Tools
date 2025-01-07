using UnityEngine;

namespace BodyPartSwap
{
    public partial class ActiveBodyPart : MonoBehaviour
    {
        [Header("Reference:")]
        [SerializeField] private OptionQueue m_queue;
        [SerializeField] private GameObject m_modelObject;

        public SwapCallbackResponse ProcessSwap(int _index)
        {
            var trueIndex = _index  % m_queue.queueLength;
            var pulled              = m_queue.GetFromQueue(trueIndex);
            var createdModelObject  = Instantiate(pulled.prefab, transform, false);

            Destroy(m_modelObject);

            m_modelObject       = createdModelObject;
            m_modelObject.name  = pulled.prefab.name;

            var callBack = new SwapCallbackResponse
            {
                responseIndex   = trueIndex,
                part            = pulled,
            };

            return callBack;
        }
    }
}