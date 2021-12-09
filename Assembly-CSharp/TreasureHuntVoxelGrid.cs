using System;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000282 RID: 642
public class TreasureHuntVoxelGrid : NetBehaviour
{
	// Token: 0x060012C8 RID: 4808 RVA: 0x0000F0E6 File Offset: 0x0000D2E6
	public override void OnNetInitialize()
	{
		if (!NetSystem.IsServer)
		{
			this.Setup();
		}
		base.OnNetInitialize();
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x00091650 File Offset: 0x0008F850
	public void Setup()
	{
		UnityEngine.Random.InitState(this.seed.Value);
		Vector3 vector = this.grid_size + new Vector3(3f, 3f, 3f);
		this.minigame_controller = (TreasureHuntController)GameManager.Minigame;
		this.voxelGrid = base.gameObject.AddComponent<VoxelGrid>();
		this.voxelGrid.isStatic = false;
		this.voxelGrid.colorGradient = this.colorGradient;
		this.voxelGrid.wireFrameMat = this.wireframeMat;
		this.voxelGrid.data = ArrayUtility.allocate_3d_array<short>((int)vector.x, (int)vector.y, (int)vector.z);
		float num = (float)UnityEngine.Random.Range(0, 10000);
		float num2 = (float)UnityEngine.Random.Range(0, 10000);
		for (int i = 0; i < (int)vector.x; i++)
		{
			for (int j = 0; j < (int)vector.z; j++)
			{
				float num3 = (float)i;
				float num4 = (float)j;
				float a = Mathf.Abs((vector.x - 1f) / 2f - (float)i);
				float b = Mathf.Abs((vector.z - 1f) / 2f - (float)j);
				float num5 = Mathf.Max(a, b);
				num5 /= (vector.x - 4f) / 2f;
				num5 = -num5 * (num5 / 1.8f) + 1f;
				float num6 = 15f / this.voxel_size;
				float num7 = 5f / this.voxel_size;
				float num8 = Mathf.PerlinNoise(num3 / num6 + num, num4 / num6 + num2);
				float num9 = Mathf.PerlinNoise(num3 / num7 + num2, num4 / num7 + num);
				float num10 = 0.9f * num8 + 0.1f * num9;
				num10 = Mathf.Clamp01(1f - num10);
				num10 = 0.51f + num10 * 0.49f;
				float num11 = vector.y * 0.8f;
				float num12 = vector.y - num11;
				float num13 = num10 / num11;
				float num14 = Mathf.Clamp01(num10 - 0.55f);
				for (int k = 0; k < (int)vector.y; k++)
				{
					float num15 = (float)k;
					float num16;
					if ((float)k >= num11)
					{
						float t = (vector.y - 2f - num15) / num12;
						num16 = Mathf.Lerp(num10, num14, t);
					}
					else
					{
						float t2 = (num11 - num15) / num11;
						num16 = Mathf.Lerp(num14, 0f, t2);
					}
					short num17 = (short)(-32768f + 65534f * num16);
					this.voxelGrid.data[i][k][j] = num17;
				}
			}
		}
		short num18 = 0;
		int num19 = 40;
		this.voxelGrid.Setup(this.grid_size, this.detail_per_chunk, this.voxel_size, this.chunk_mat, base.gameObject.layer);
		Vector3 randomUndergroundPosition = this.GetRandomUndergroundPosition(vector, 1f, Vector3i.one, vector / 2f);
		this.minigame_controller.Spawn(this.treasure, randomUndergroundPosition, Quaternion.identity).GetComponent<TreasureHuntTreasure>().ObjectID = num18;
		num18 += 1;
		for (int l = 0; l < num19; l++)
		{
			Vector3 randomUndergroundPosition2 = this.GetRandomUndergroundPosition(vector, 0.5f);
			this.minigame_controller.Spawn(this.collectible, randomUndergroundPosition2, Quaternion.identity).GetComponent<TreasureHuntCollectible>().ObjectID = num18;
			num18 += 1;
		}
		this.voxelGrid.BuildChunks();
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x0000F0FB File Offset: 0x0000D2FB
	private Vector3 GetRandomUndergroundPosition(Vector3 size, float check_sphere_radius)
	{
		return this.GetRandomUndergroundPosition(size, check_sphere_radius, Vector3.one, new Vector3i(size - Vector3.one));
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x000919CC File Offset: 0x0008FBCC
	private Vector3 GetRandomUndergroundPosition(Vector3 size, float checkSphereRadius, Vector3i min, Vector3i max)
	{
		min.x = Mathf.Clamp(min.x, 3, (int)size.x - 3);
		max.x = Mathf.Clamp(max.x, 3, (int)size.x - 1);
		min.y = Mathf.Clamp(min.y, 3, (int)(size.y * 0.85f) - 3);
		max.y = Mathf.Clamp(max.y, 3, (int)(size.y * 0.85f) - 3);
		min.z = Mathf.Clamp(min.z, 3, (int)size.z - 3);
		max.z = Mathf.Clamp(max.z, 3, (int)size.z - 3);
		int num = 1000;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = new Vector3((float)UnityEngine.Random.Range(min.x, max.x), (float)UnityEngine.Random.Range(min.y, max.y), (float)UnityEngine.Random.Range(min.z, max.z));
			if (this.voxelGrid.Filled(vector))
			{
				Vector3 vector2 = this.voxelGrid.GridPointToWorldSpace(vector);
				if (!Physics.CheckSphere(vector2, checkSphereRadius, 3072))
				{
					return vector2;
				}
			}
		}
		return Vector3.zero;
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0000F11F File Offset: 0x0000D31F
	private void Start()
	{
		this.minigame_controller.grid = this;
		this.voxelGrid.ChunkUpdatedEvent += this.minigame_controller.ChunkUpdated;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x0000F149 File Offset: 0x0000D349
	private void Update()
	{
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.F6))
		{
			this.voxelGrid.RenderMode = ((this.voxelGrid.RenderMode == VoxelGridRenderingMode.Normal) ? VoxelGridRenderingMode.Wireframe : VoxelGridRenderingMode.Normal);
		}
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x0000F17A File Offset: 0x0000D37A
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void VacuumRPC(NetPlayer sender, Vector3 pos)
	{
		this.Vacuum(pos, true);
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x00091B18 File Offset: 0x0008FD18
	public void Vacuum(Vector3 pos, bool remote_call = false)
	{
		float opacity = 0.045f;
		this.voxelGrid.Edit(new Edit(BrushShape.Sphere, BrushAction.IncreaseRelative, pos, 5f, opacity), false);
		if (!remote_call)
		{
			base.SendRPC("VacuumRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				pos
			});
		}
	}

	// Token: 0x040013F7 RID: 5111
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<int> seed = new NetVar<int>(0);

	// Token: 0x040013F8 RID: 5112
	public GameObject treasure;

	// Token: 0x040013F9 RID: 5113
	public GameObject collectible;

	// Token: 0x040013FA RID: 5114
	public Material chunk_mat;

	// Token: 0x040013FB RID: 5115
	public Material wireframeMat;

	// Token: 0x040013FC RID: 5116
	public Gradient colorGradient;

	// Token: 0x040013FD RID: 5117
	public Vector3 grid_size = new Vector3(20f, 20f, 20f);

	// Token: 0x040013FE RID: 5118
	public int detail_per_chunk = 20;

	// Token: 0x040013FF RID: 5119
	public float voxel_size = 0.1f;

	// Token: 0x04001400 RID: 5120
	public VoxelGrid voxelGrid;

	// Token: 0x04001401 RID: 5121
	private TreasureHuntController minigame_controller;
}
