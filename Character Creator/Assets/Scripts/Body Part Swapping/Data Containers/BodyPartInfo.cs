using UnityEngine;

namespace BodyPartSwap
{
    [CreateAssetMenu(fileName = "New Body Part", menuName = "Body Part Swapping/Body Part", order = 1)]
    public class BodyPartInfo : ScriptableObject
    {
        [SerializeField] private GlobalInfo.Types m_type;
        [SerializeField] private GameObject m_prefab;

        public GlobalInfo.Types type    => m_type;
        public GameObject prefab        => m_prefab;

        public string typeName      => m_type.ToString();
        public string objectName    => m_prefab.name;
    }
}
