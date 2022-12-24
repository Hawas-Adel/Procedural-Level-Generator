using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralGeneration
{
	[SelectionBase]
	public class BuildingPiece : MonoBehaviour
	{
		[SerializeField] private List<AllowedNeighborsCollection> Borders;

		[Foldout("Gizmos")][SerializeField][Min(0f)] private float CellRadius = 1f;
		[Foldout("Gizmos")]
		[SerializeField]
		private Gradient BorderConnectionColor = new()
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.yellow, 0f),
				new GradientColorKey(Color.green, 0.5f),
				new GradientColorKey(Color.blue, 1f)
			},
			alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f,0f)
			}
		};

		private void OnDrawGizmosSelected()
		{
			foreach (var border in Borders)
			{
				Vector3 borderConnectionStart = transform.position + (border.BorderDirection.normalized * CellRadius);
				var borderColorLerpValue = (Borders.Count == 1) ? 0f : (float)Borders.IndexOf(border) / (Borders.Count - 1);
				Gizmos.color = BorderConnectionColor.Evaluate(borderColorLerpValue);
				foreach (var neigbor in border.AllowedNeighbors)
				{
					if (!neigbor)
					{
						continue;
					}

					Vector3 borderConnectionEnd = neigbor.transform.position - (border.BorderDirection.normalized * CellRadius);
					DrawBorderConnection(borderConnectionStart, borderConnectionEnd);
				}
			}
		}

		private void DrawBorderConnection(Vector3 borderConnectionStart, Vector3 borderConnectionEnd) => Gizmos.DrawLine(borderConnectionStart, borderConnectionEnd);

		[Button]
		private void RemoveDuplicateConnections()
		{
			for (int i = 0; i < Borders.Count; i++)
			{
				AllowedNeighborsCollection allowedNeighborsCollection = Borders[i];
				allowedNeighborsCollection.AllowedNeighbors = allowedNeighborsCollection.AllowedNeighbors.Distinct().ToList();
				Borders[i] = allowedNeighborsCollection;
			}
		}

		[Button]
		private void AddReverseConnections()
		{
			foreach (var border in Borders)
			{
				foreach (var neigbor in border.AllowedNeighbors)
				{
					var neigborBorders = neigbor.Borders.FirstOrDefault(B => B.BorderDirection == -border.BorderDirection);
					if (neigborBorders.AllowedNeighbors is null)
					{
						neigborBorders = new(-border.BorderDirection, new());
						neigbor.Borders.Add(neigborBorders);
					}

					neigborBorders.AllowedNeighbors.Add(this);
				}
			}
		}
	}

	[System.Serializable]
	internal struct AllowedNeighborsCollection
	{
		public Vector3 BorderDirection;
		public List<BuildingPiece> AllowedNeighbors;

		public AllowedNeighborsCollection(Vector3 borderDirection, List<BuildingPiece> allowedNeighbors)
		{
			BorderDirection = borderDirection;
			AllowedNeighbors = allowedNeighbors;
		}
	}
}
