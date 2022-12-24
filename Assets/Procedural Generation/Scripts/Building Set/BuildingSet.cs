using NaughtyAttributes;
using UnityEngine;

namespace ProceduralGeneration
{
	[SelectionBase]
	public class BuildingSet : MonoBehaviour
	{
		[SerializeField] private BuildingPiece[] BuildingPieces;

		[Button]
		private void GetChildBuildingPieces() => BuildingPieces = GetComponentsInChildren<BuildingPiece>();
	}
}
