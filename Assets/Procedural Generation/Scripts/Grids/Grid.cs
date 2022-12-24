using System.Linq;
using UnityEngine;

namespace ProceduralGeneration
{
	public enum CellState
	{
		UnDefined,
		Empty,
		PartOfMap
	}

	public abstract class GridCellInfo
	{
		public Vector3 Center { get; private set; }
		public CellState CellState { get; private set; }
		public Color CellGizmoColor { get; private set; }

		public virtual void SetValues(Vector3 center, CellState cellState, Color cellGizmoColor)
		{
			Center = center;
			CellGizmoColor = cellGizmoColor;
			CellState = cellState;
		}

		public virtual void DrawCellGizmo() => Gizmos.color = CellGizmoColor;
	}

	[DisallowMultipleComponent]
	public abstract class Grid<T> : MonoBehaviour where T : GridCellInfo, new()
	{
		[SerializeField] protected Color DefaultCellColor = Color.cyan;

		public abstract T GetCellInfo(Vector3Int cellIndex);
		public T GetCellInfo(int i, int j, int k) => GetCellInfo(new(i, j, k));
		public abstract bool CellExistsInGrid(int i, int j, int k);
		public abstract Vector3Int[] GetAllCellsInGrid();

		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			GetAllCellsInGrid().ToList().ForEach(cell => GetCellInfo(cell).DrawCellGizmo());
		}
	}
}
