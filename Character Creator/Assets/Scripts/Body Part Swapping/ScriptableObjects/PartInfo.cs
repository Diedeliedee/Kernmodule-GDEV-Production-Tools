using UnityEngine;

namespace BodyPartSwap
{
    [CreateAssetMenu(fileName = "New Part", menuName = "Part Swapping/Part", order = 1)]
    public class PartInfo : ScriptableObject
    {
        [SerializeField] private Options m_type;
        [SerializeField] private GameObject m_prefab;

        public Options type         => m_type;
        public GameObject prefab    => m_prefab;

        public string typeName      => m_type.ToString();
        public string objectName    => m_prefab.name;
    }
}
