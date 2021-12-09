using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class AnimationAdditiveTransform : MonoBehaviour
{
	// Token: 0x06000015 RID: 21 RVA: 0x00003A45 File Offset: 0x00001C45
	private void LateUpdate()
	{
		base.transform.position += this.m_position;
		base.transform.rotation *= Quaternion.Euler(this.m_rotation);
	}

	// Token: 0x0400001B RID: 27
	[SerializeField]
	private Vector3 m_position;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	private Vector3 m_rotation;
}
