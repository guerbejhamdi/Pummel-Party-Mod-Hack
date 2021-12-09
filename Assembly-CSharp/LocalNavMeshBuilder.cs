using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002B2 RID: 690
[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder : MonoBehaviour
{
	// Token: 0x060013FE RID: 5118 RVA: 0x0000FBF2 File Offset: 0x0000DDF2
	private IEnumerator Start()
	{
		for (;;)
		{
			this.UpdateNavMesh(true);
			yield return this.m_Operation;
		}
		yield break;
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x0000FC01 File Offset: 0x0000DE01
	private void OnEnable()
	{
		this.m_NavMesh = new NavMeshData();
		this.m_Instance = NavMesh.AddNavMeshData(this.m_NavMesh);
		if (this.m_Tracked == null)
		{
			this.m_Tracked = base.transform;
		}
		this.UpdateNavMesh(false);
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x0000FC40 File Offset: 0x0000DE40
	private void OnDisable()
	{
		this.m_Instance.Remove();
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x0009739C File Offset: 0x0009559C
	private void UpdateNavMesh(bool asyncUpdate = false)
	{
		NavMeshSourceTag.Collect(ref this.m_Sources);
		NavMeshBuildSettings settingsByID = NavMesh.GetSettingsByID(0);
		Bounds localBounds = this.QuantizedBounds();
		if (asyncUpdate)
		{
			this.m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(this.m_NavMesh, settingsByID, this.m_Sources, localBounds);
			return;
		}
		NavMeshBuilder.UpdateNavMeshData(this.m_NavMesh, settingsByID, this.m_Sources, localBounds);
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x000973F4 File Offset: 0x000955F4
	private static Vector3 Quantize(Vector3 v, Vector3 quant)
	{
		float x = quant.x * Mathf.Floor(v.x / quant.x);
		float y = quant.y * Mathf.Floor(v.y / quant.y);
		float z = quant.z * Mathf.Floor(v.z / quant.z);
		return new Vector3(x, y, z);
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x00097458 File Offset: 0x00095658
	private Bounds QuantizedBounds()
	{
		return new Bounds(LocalNavMeshBuilder.Quantize(this.m_Tracked ? this.m_Tracked.position : base.transform.position, 0.1f * this.m_Size), this.m_Size);
	}

	// Token: 0x06001404 RID: 5124 RVA: 0x000974AC File Offset: 0x000956AC
	private void OnDrawGizmosSelected()
	{
		if (this.m_NavMesh)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(this.m_NavMesh.sourceBounds.center, this.m_NavMesh.sourceBounds.size);
		}
		Gizmos.color = Color.yellow;
		Bounds bounds = this.QuantizedBounds();
		Gizmos.DrawWireCube(bounds.center, bounds.size);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.m_Tracked ? this.m_Tracked.position : base.transform.position, this.m_Size);
	}

	// Token: 0x0400153A RID: 5434
	public Transform m_Tracked;

	// Token: 0x0400153B RID: 5435
	public Vector3 m_Size = new Vector3(80f, 20f, 80f);

	// Token: 0x0400153C RID: 5436
	private NavMeshData m_NavMesh;

	// Token: 0x0400153D RID: 5437
	private AsyncOperation m_Operation;

	// Token: 0x0400153E RID: 5438
	private NavMeshDataInstance m_Instance;

	// Token: 0x0400153F RID: 5439
	private List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();
}
