using System;
using UnityEngine;
using UnityEngine.Rendering;
using ZP.Net;

// Token: 0x02000289 RID: 649
public class VoxelTowerGrid : NetBehaviour
{
	// Token: 0x0600130B RID: 4875 RVA: 0x00092D24 File Offset: 0x00090F24
	public override void OnNetInitialize()
	{
		this.minigame_controller = (VoxelTowerController)GameManager.Minigame;
		this.voxel_grid = base.gameObject.AddComponent<VoxelGrid>();
		this.grid_size + new Vector3(3f, 3f, 3f);
		this.voxel_grid.gridBinaryFile = this.grid_binary_file;
		this.voxel_grid.colorGradient = this.voxelGradient;
		this.voxel_grid.UseNavMesh = true;
		this.voxel_grid.shadowCastingMode = ShadowCastingMode.Off;
		this.voxel_grid.Setup(this.grid_size, this.detail_per_chunk, this.voxel_size, this.chunk_mat, 10);
		this.voxel_grid.BuildChunks();
		base.OnNetInitialize();
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x00092DE4 File Offset: 0x00090FE4
	public void Start()
	{
		base.gameObject.name = "Tower" + base.OwnerSlot.ToString();
		this.minigame_controller.voxel_towers[(int)base.OwnerSlot] = this;
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x0000F389 File Offset: 0x0000D589
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void EditRPC(NetPlayer sender, Vector3 position)
	{
		this.Edit(position);
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x00092E28 File Offset: 0x00091028
	public void Edit(Vector3 pos)
	{
		bool flag = this.voxel_grid.Edit(new Edit(BrushShape.Sphere, BrushAction.Subtract, pos, 4.5f), false);
		if (NetSystem.IsServer && flag)
		{
			base.SendRPC("EditRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				pos
			});
		}
	}

	// Token: 0x0400144F RID: 5199
	[HideInInspector]
	public VoxelGrid voxel_grid;

	// Token: 0x04001450 RID: 5200
	private VoxelTowerController minigame_controller;

	// Token: 0x04001451 RID: 5201
	public TextAsset grid_binary_file;

	// Token: 0x04001452 RID: 5202
	public Material chunk_mat;

	// Token: 0x04001453 RID: 5203
	public Vector3 grid_size = new Vector3(20f, 20f, 20f);

	// Token: 0x04001454 RID: 5204
	public int detail_per_chunk = 20;

	// Token: 0x04001455 RID: 5205
	public float voxel_size = 0.1f;

	// Token: 0x04001456 RID: 5206
	public Gradient voxelGradient;
}
