using System;
using UnityEngine;

// Token: 0x02000164 RID: 356
public class RandomMesh : MonoBehaviour
{
	// Token: 0x06000A3B RID: 2619 RVA: 0x0000AA8E File Offset: 0x00008C8E
	private void Start()
	{
		if (this.m_meshes.Length != 0)
		{
			base.GetComponent<MeshFilter>().sharedMesh = this.m_meshes[UnityEngine.Random.Range(0, this.m_meshes.Length - 1)];
		}
	}

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	protected Mesh[] m_meshes;
}
