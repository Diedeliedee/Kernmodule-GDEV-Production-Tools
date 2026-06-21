using UnityEngine;

namespace BodyPartSwap
{
    public partial class ActiveBodyPart : MonoBehaviour, IActiveElement
    {
        [Header("Reference:")]
        [SerializeField] private OptionQueue m_queue;
        [SerializeField] private SkinnedMeshRenderer m_meshRenderer;
        [Space]
        [SerializeField] private ActiveBodyPart m_ChildBodyPart = null;

        //  Run-time:
        private int m_index                 = 0;
        private PartInfo m_selectedPartInfo = null;
        private Vector3 m_scale             = Vector3.one;
        private Vector3 m_modifier          = Vector3.one;

        public Options type     => m_queue.type;
        public int index        => m_index;
        public Vector3 scale    => m_scale;

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
            switch (_axis)
            {
                case 0: m_scale.x = _scale; break;
                case 1: m_scale.y = _scale; break;
                case 2: m_scale.z = _scale; break;
            }

            m_ChildBodyPart?.ProcessScaleModifier(m_scale);
            transform.localScale = CalculatedScale();
        }

        public void ProcessScale(Vector3 _scale)
        {
            m_scale = _scale;
            m_ChildBodyPart?.ProcessScaleModifier(_scale);

            transform.localScale = CalculatedScale();
        }

        public void ProcessScaleModifier(Vector3 _parentScale)
        {
            var modifier = Vector3.one;

            if (_parentScale.x > 0) modifier.x /= _parentScale.x;
            if (_parentScale.y > 0) modifier.y /= _parentScale.y;
            if (_parentScale.z > 0) modifier.z /= _parentScale.z;

            m_modifier              = modifier;
            transform.localScale    = CalculatedScale();
        }


        public bool ContainsBodyPart(PartInfo _bodyPart)
        {
            return m_queue.Contains(_bodyPart);
        }

        public void AddToQueue(PartInfo _bodyPart)
        {
            m_queue.AddToQueue(_bodyPart);
        }

        private Vector3 CalculatedScale()
        {
            var scale = m_scale;

            scale.x *= m_modifier.x;
            scale.y *= m_modifier.y;
            scale.z *= m_modifier.z;

            return scale;
        }
    }
}