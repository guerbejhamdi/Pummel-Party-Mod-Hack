using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x0200058B RID: 1419
public class VoxelUndoManager
{
	// Token: 0x060024E6 RID: 9446 RVA: 0x0001A821 File Offset: 0x00018A21
	public VoxelUndoManager(VoxelGrid voxel_grid, int max_length)
	{
		this.max_length = max_length;
		this.voxel_grid = voxel_grid;
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x000E0054 File Offset: 0x000DE254
	public void StartEdit()
	{
		this.cur_array = ArrayUtility.copy_3d_array<short>(this.voxel_grid.data, (int)this.voxel_grid.gridSize.x, (int)this.voxel_grid.gridSize.y, (int)this.voxel_grid.gridSize.z);
		this.edit_started = true;
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x000E00B4 File Offset: 0x000DE2B4
	public void EndEdit()
	{
		if (this.edit_started)
		{
			this.edit_started = false;
			List<VoxelGridEdit> list = new List<VoxelGridEdit>();
			int num = 0;
			while ((float)num < this.voxel_grid.gridSize.x)
			{
				int num2 = 0;
				while ((float)num2 < this.voxel_grid.gridSize.y)
				{
					int num3 = 0;
					while ((float)num3 < this.voxel_grid.gridSize.z)
					{
						if (this.voxel_grid.data[num][num2][num3] != this.cur_array[num][num2][num3])
						{
							list.Add(new VoxelGridEdit((short)num, (short)num2, (short)num3, this.cur_array[num][num2][num3]));
						}
						num3++;
					}
					num2++;
				}
				num++;
			}
			if (list.Count != 0)
			{
				this.AddUndo(list);
				this.redos.Clear();
			}
		}
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000E0188 File Offset: 0x000DE388
	public void Undo()
	{
		if (this.undos.Count > 0)
		{
			this.AddRedo(this.DoEdit(this.undos[this.undos.Count - 1]));
			this.undos.RemoveAt(this.undos.Count - 1);
			return;
		}
		Debug.Log("Nothing to undo");
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x000E01EC File Offset: 0x000DE3EC
	public void Redo()
	{
		if (this.redos.Count > 0)
		{
			this.AddUndo(this.DoEdit(this.redos[this.redos.Count - 1]));
			this.redos.RemoveAt(this.redos.Count - 1);
			return;
		}
		Debug.Log("Nothing to redo");
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x0001A84D File Offset: 0x00018A4D
	private void AddUndo(List<VoxelGridEdit> u)
	{
		if (this.undos.Count > this.max_length)
		{
			this.undos.RemoveAt(0);
		}
		this.undos.Add(u);
	}

	// Token: 0x060024EC RID: 9452 RVA: 0x0001A87A File Offset: 0x00018A7A
	private void AddRedo(List<VoxelGridEdit> r)
	{
		if (this.redos.Count > this.max_length)
		{
			this.redos.RemoveAt(0);
		}
		this.redos.Add(r);
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x000E0250 File Offset: 0x000DE450
	private List<VoxelGridEdit> DoEdit(List<VoxelGridEdit> e)
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = int.MaxValue;
		int num4 = int.MinValue;
		int num5 = int.MaxValue;
		int num6 = int.MinValue;
		for (int i = 0; i < e.Count; i++)
		{
			int x = (int)e[i].x;
			int y = (int)e[i].y;
			int z = (int)e[i].z;
			short val = this.voxel_grid.data[x][y][z];
			this.voxel_grid.data[x][y][z] = e[i].val;
			e[i].val = val;
			if (x < num)
			{
				num = x;
			}
			if (x > num2)
			{
				num2 = x;
			}
			if (y < num3)
			{
				num3 = y;
			}
			if (y > num4)
			{
				num4 = y;
			}
			if (z < num5)
			{
				num5 = z;
			}
			if (z > num6)
			{
				num6 = z;
			}
		}
		this.voxel_grid.UpdateChunks(num, num2, num3, num4, num5, num6, true);
		return e;
	}

	// Token: 0x04002879 RID: 10361
	public int max_length;

	// Token: 0x0400287A RID: 10362
	private short[][][] cur_array;

	// Token: 0x0400287B RID: 10363
	private bool edit_started;

	// Token: 0x0400287C RID: 10364
	public List<List<VoxelGridEdit>> undos = new List<List<VoxelGridEdit>>();

	// Token: 0x0400287D RID: 10365
	public List<List<VoxelGridEdit>> redos = new List<List<VoxelGridEdit>>();

	// Token: 0x0400287E RID: 10366
	private VoxelGrid voxel_grid;
}
