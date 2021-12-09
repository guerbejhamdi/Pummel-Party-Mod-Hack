using System;
using UnityEngine;

// Token: 0x020002C3 RID: 707
[ExecuteInEditMode]
public class PlaneCutoffModifier : MonoBehaviour
{
	// Token: 0x06001447 RID: 5191 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x00098658 File Offset: 0x00096858
	private void Update()
	{
		this.mr.sharedMaterial.SetVector("_PlaneNormal", -base.transform.up);
		this.mr.sharedMaterial.SetVector("_StartPoint", base.transform.parent.position);
	}

	// Token: 0x040015A2 RID: 5538
	public MeshRenderer mr;
}
