using UnityEngine;

namespace BodyPartSwap
{
	[CreateAssetMenu(fileName = "New Part Info", menuName = "Part Swapping/Part Info", order = 1)]

	public class PartInfo : ScriptableObject
	{
		[SerializeField] private Mesh m_mesh;
		[SerializeField] private Material m_material;

		public Mesh mesh			=> m_mesh;
		public Material material	=> m_material;
	}
}
