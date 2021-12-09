using System;
using UnityEngine;

// Token: 0x0200046A RID: 1130
public class RandomRotation : MonoBehaviour
{
	// Token: 0x06001E96 RID: 7830 RVA: 0x000C5CF0 File Offset: 0x000C3EF0
	private void Start()
	{
		float x = UnityEngine.Random.Range(this.m_minAngles.x, this.m_maxAngles.x);
		float y = UnityEngine.Random.Range(this.m_minAngles.y, this.m_maxAngles.y);
		float z = UnityEngine.Random.Range(this.m_minAngles.z, this.m_maxAngles.z);
		Quaternion quaternion = Quaternion.Euler(x, y, z);
		if (this.m_local)
		{
			base.transform.localRotation = quaternion;
			return;
		}
		base.transform.rotation = quaternion;
	}

	// Token: 0x040021A5 RID: 8613
	[SerializeField]
	protected bool m_local;

	// Token: 0x040021A6 RID: 8614
	[SerializeField]
	protected Vector3 m_minAngles;

	// Token: 0x040021A7 RID: 8615
	[SerializeField]
	protected Vector3 m_maxAngles;
}
