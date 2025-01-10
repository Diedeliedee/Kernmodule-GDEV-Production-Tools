using UnityEngine;

namespace BodyPartSwap
{
    public partial class ActiveBodyPart : MonoBehaviour, IActiveElement
    {
        [Header("Reference:")]
        [SerializeField] private OptionQueue m_queue;
        [SerializeField] private SkinnedMeshRenderer m_meshRenderer;

        //  Run-time:
        private int m_index                 = 0;
        private PartInfo m_selectedPartInfo = null;

        public Options type => m_queue.type;
        public int index    => m_index;

        public string typeName      => type.ToString();
        public string selectedName  => m_selectedPartInfo ? m_selectedPartInfo.name : "Null :(";

        public SwapCallbackResponse ProcessSwap(int _offset)
        {
            int Modulo(int _a , int _b) => _a - _b * Mathf.FloorToInt((float)_a / (float)_b);

            var trueIndex = Modulo((m_index + _offset), m_queue.queueLength);

            return ApplyIndex(trueIndex);
        }

        public SwapCallbackResponse ApplyIndex(int _index)
        {
            var pulled = m_queue.GetFromQueue(_index);

            m_meshRenderer.sharedMesh   = pulled.mesh;
            m_meshRenderer.material     = pulled.material;

            m_index             = _index;
            m_selectedPartInfo  = pulled;

            var callBack = new SwapCallbackResponse
            {
                part    = pulled,
                name    = pulled.name,
                result  = SwapCallbackResponse.Result.Success,
            };

            return callBack;
        }
    }
}