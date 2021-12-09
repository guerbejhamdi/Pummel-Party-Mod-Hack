using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000314 RID: 788
[ExecuteInEditMode]
public class ReplaceMeshLinks : MonoBehaviour
{
	// Token: 0x060015AC RID: 5548 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0009C244 File Offset: 0x0009A444
	private void Update()
	{
		if (this.replace)
		{
			this.Replace();
		}
		if (this.disable)
		{
			OffMeshLink[] componentsInChildren = base.GetComponentsInChildren<OffMeshLink>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			OffMeshLinkDetails[] componentsInChildren2 = base.GetComponentsInChildren<OffMeshLinkDetails>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
			NavMeshLink[] componentsInChildren3 = base.GetComponentsInChildren<NavMeshLink>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].bidirectional = false;
			}
			this.disable = false;
		}
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x0009C2D4 File Offset: 0x0009A4D4
	private void Replace()
	{
		this.replace = false;
		NavMeshLink[] componentsInChildren = base.GetComponentsInChildren<NavMeshLink>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
		}
		OffMeshLink[] componentsInChildren2 = base.GetComponentsInChildren<OffMeshLink>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			NavMeshLink navMeshLink = componentsInChildren2[j].gameObject.AddComponent<NavMeshLink>();
			if (!(componentsInChildren2[j].endTransform == null) && !(componentsInChildren2[j].startTransform == null))
			{
				navMeshLink.startPoint = navMeshLink.transform.worldToLocalMatrix.MultiplyPoint(componentsInChildren2[j].startTransform.position);
				navMeshLink.endPoint = navMeshLink.transform.worldToLocalMatrix.MultiplyPoint(componentsInChildren2[j].endTransform.position);
			}
		}
	}

	// Token: 0x040016AD RID: 5805
	public bool replace;

	// Token: 0x040016AE RID: 5806
	public bool disable;
}
