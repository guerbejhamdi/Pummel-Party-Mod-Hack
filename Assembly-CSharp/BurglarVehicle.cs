using System;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class BurglarVehicle : MonoBehaviour
{
	// Token: 0x06000B3F RID: 2879 RVA: 0x00060BC0 File Offset: 0x0005EDC0
	public void Update()
	{
		float num = this.m_speed * Time.deltaTime;
		base.transform.position += base.transform.forward * num;
		float num2 = 6.2831855f;
		for (int i = 0; i < this.m_wheels.Length; i++)
		{
			this.m_wheels[i].transform.Rotate(new Vector3(num / num2 * 360f, 0f, 0f));
		}
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x0000B2B7 File Offset: 0x000094B7
	public void SetSpeed(float speed)
	{
		this.m_speed = speed;
	}

	// Token: 0x04000A5C RID: 2652
	[SerializeField]
	protected float m_speed = 1f;

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	protected GameObject[] m_wheels;
}
