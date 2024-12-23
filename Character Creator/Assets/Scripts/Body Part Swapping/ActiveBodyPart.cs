using UnityEngine;

namespace BodyPartSwap
{
    public class ActiveBodyPart : MonoBehaviour
    {
        [SerializeField] private GameObject m_modelObject;

        public void SwapWith(GameObject _prefab)
        {
            var createdModelObject = Instantiate(_prefab, transform, false);

            Destroy(m_modelObject);

            m_modelObject       = createdModelObject;
            m_modelObject.name  = _prefab.name;
        }
    }
}