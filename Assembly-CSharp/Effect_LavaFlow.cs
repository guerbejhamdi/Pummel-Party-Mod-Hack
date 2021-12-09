using System;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class Effect_LavaFlow : MonoBehaviour
{
	// Token: 0x0600132E RID: 4910 RVA: 0x00094474 File Offset: 0x00092674
	private void Update()
	{
		base.transform.localPosition += this.m_movementSpeed * Time.deltaTime;
		if (base.transform.position.z < this.m_endPosition.position.z)
		{
			float num = this.m_resetPosition.localPosition.x;
			float num2 = this.m_resetPosition.localPosition.y;
			float num3 = this.m_resetPosition.localPosition.z;
			num += UnityEngine.Random.Range(this.m_randomOffsetMin.x, this.m_randomOffsetMax.x);
			num2 += UnityEngine.Random.Range(this.m_randomOffsetMin.y, this.m_randomOffsetMax.y);
			num3 += UnityEngine.Random.Range(this.m_randomOffsetMin.z, this.m_randomOffsetMax.z);
			base.transform.localPosition = new Vector3(num, num2, num3);
		}
	}

	// Token: 0x0400147B RID: 5243
	[SerializeField]
	protected Vector3 m_movementSpeed;

	// Token: 0x0400147C RID: 5244
	[SerializeField]
	protected Transform m_resetPosition;

	// Token: 0x0400147D RID: 5245
	[SerializeField]
	protected Transform m_endPosition;

	// Token: 0x0400147E RID: 5246
	[SerializeField]
	protected Vector3 m_randomOffsetMin = Vector3.zero;

	// Token: 0x0400147F RID: 5247
	[SerializeField]
	protected Vector3 m_randomOffsetMax = Vector3.zero;
}
