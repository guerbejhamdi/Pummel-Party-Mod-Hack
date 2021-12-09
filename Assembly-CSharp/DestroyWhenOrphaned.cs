using System;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class DestroyWhenOrphaned : MonoBehaviour
{
	// Token: 0x0600248B RID: 9355 RVA: 0x0001A44A File Offset: 0x0001864A
	private void Awake()
	{
		base.InvokeRepeating("DestroyGameObject", 0f, 0.1f);
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000DB6CC File Offset: 0x000D98CC
	private void DestroyGameObject()
	{
		if (base.transform.parent == null)
		{
			if (this.destroy_time > 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.destroy_time);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (this.remove_light)
			{
				UnityEngine.Object.Destroy(base.GetComponent<Light>());
			}
			base.CancelInvoke();
		}
	}

	// Token: 0x040027D1 RID: 10193
	public float destroy_time;

	// Token: 0x040027D2 RID: 10194
	public bool remove_light;
}
