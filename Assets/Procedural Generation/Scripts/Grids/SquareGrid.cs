using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration
{
	public class SquareGridCell : GridCellInfo
	{
		public Vector3 Size { get; private set; }
		public virtual void SetValues(Vector3 center, CellState cellState, Color cellGizmoColor, Vector3 size)
		{
			base.SetValues(center, cellState, cellGizmoColor);
			Size = size;
		}

		public override void DrawCellGizmo()
		{
			base.DrawCellGizmo();
			Gizmos.DrawWireCube(Center, Size);
		}
	}

	public class SquareGrid : Grid<SquareGridCell>
	{
		[MinValue(1)] public Vector3Int GridSize = Vector3Int.one;
		[MinValue(0f)] public Vector3 CellSize = Vector3.one;

		public override bool CellExistsInGrid(int i, int j, int k)
		{
			if (i < 0 || i >= GridSize.x)
			{
				return false;
			}

			if (j < 0 || j >= GridSize.y)
			{
				return false;
			}

			return k >= 0 && k < GridSize.z;
		}

		public override Vector3Int[] GetAllCellsInGrid()
		{
			List<Vector3Int> cells = new();
			for (int i = 0; i < GridSize.x; i++)
			{
				for (int j = 0; j < GridSize.y; j++)
				{
					for (int k = 0; k < GridSize.z; k++)
					{
						cells.Add(new(i, j, k));
					}
				}
			}

			return cells.ToArray();
		}

		public override SquareGridCell GetCellInfo(Vector3Int cellIndex)
		{
			var cellInfo = new SquareGridCell();
			cellInfo.SetValues(Vector3.Scale(CellSize, cellIndex), default, DefaultCellColor, CellSize);
			return cellInfo;
		}
	}
}
