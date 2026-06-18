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

        public Options type     => m_queue.type;
        public int index        => m_index;
        public Vector3 scale    => transform.localScale;

        public string typeName      => type.ToString();
        public string selectedName  => m_selectedPartInfo ? m_selectedPartInfo.name : "Null :(";

        public void Setup()
        {
            //  Duplicate the queue and assign it. So that any run-time changes aren't saved.
            m_queue = Instantiate(m_queue);
        }

        public SwapCallback ProcessSwap(int _offset)
        {
            int Modulo(int _a , int _b) => _a - _b * Mathf.FloorToInt((float)_a / (float)_b);

            var trueIndex = Modulo((m_index + _offset), m_queue.queueLength);

            return ApplyIndex(trueIndex);
        }

        public SwapCallback ApplyIndex(int _index)
        {
            var pulled = m_queue.GetFromQueue(_index);

            m_meshRenderer.sharedMesh   = pulled.mesh;
            m_meshRenderer.material     = pulled.material;

            m_index             = _index;
            m_selectedPartInfo  = pulled;

            return new()
            {
                part    = pulled,
                name    = pulled.name,
                result  = SwapCallback.Result.Success,
            };
        }

        public void ProcessScale(int _axis, float _scale)
        {
            var scale = transform.localScale;

            switch (_axis)
            {
                case 0: scale.x = _scale; break;
                case 1: scale.y = _scale;  break;
                case 2: scale.z = _scale; break;
            }

            transform.localScale = scale;
        }

        public void ProcessScale(Vector3 _scale)
        {
            transform.localScale = _scale;
        }

        public bool ContainsBodyPart(PartInfo _bodyPart)
        {
            return m_queue.Contains(_bodyPart);
        }

        public void AddToQueue(PartInfo _bodyPart)
        {
            m_queue.AddToQueue(_bodyPart);
        }
    }
}