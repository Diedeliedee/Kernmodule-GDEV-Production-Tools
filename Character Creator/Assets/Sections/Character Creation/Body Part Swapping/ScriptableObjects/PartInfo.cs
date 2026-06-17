using UnityEngine;

namespace BodyPartSwap
{
	[CreateAssetMenu(fileName = "New Part Info", menuName = "Part Swapping/Part Info", order = 1)]

	public class PartInfo : ScriptableObject
	{
		[SerializeField] private Options m_type;
		[Space]
		[SerializeField] private Mesh m_mesh;
        [SerializeField] private Material m_material;

		public Options type			=> m_type;
		public Mesh mesh			=> m_mesh;
        public Material material	=> m_material;

		public void Setup(Options _type, Mesh _mesh, Material _material)
		{
			m_type		= _type;
			m_mesh		= _mesh;
			m_material	= _material;
		}
	}
}
