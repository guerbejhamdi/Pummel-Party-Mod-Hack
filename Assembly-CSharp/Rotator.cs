using System;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class Rotator : MonoBehaviour
{
	// Token: 0x06002497 RID: 9367 RVA: 0x0001A4A9 File Offset: 0x000186A9
	private void Start()
	{
		if (this.reset_rotation)
		{
			base.transform.rotation = Quaternion.identity;
		}
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x0001A4C3 File Offset: 0x000186C3
	private void Update()
	{
		if (!this.fixedUpdate)
		{
			this.DoRotation();
		}
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x0001A4D3 File Offset: 0x000186D3
	private void FixedUpdate()
	{
		if (this.fixedUpdate)
		{
			this.DoRotation();
		}
	}

	// Token: 0x0600249A RID: 9370 RVA: 0x000DB858 File Offset: 0x000D9A58
	private void DoRotation()
	{
		if (this.local_rotation)
		{
			base.transform.rotation *= Quaternion.AngleAxis(this.speed * Time.deltaTime, this.axis);
			return;
		}
		base.transform.localRotation *= Quaternion.AngleAxis(this.speed * Time.deltaTime, this.axis);
	}

	// Token: 0x040027D9 RID: 10201
	public Vector3 axis;

	// Token: 0x040027DA RID: 10202
	public float speed;

	// Token: 0x040027DB RID: 10203
	public bool local_rotation;

	// Token: 0x040027DC RID: 10204
	public bool reset_rotation;

	// Token: 0x040027DD RID: 10205
	public bool fixedUpdate;
}
