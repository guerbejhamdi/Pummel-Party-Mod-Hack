using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033C RID: 828
public class DecalDestroyer : MonoBehaviour
{
	// Token: 0x0600167A RID: 5754 RVA: 0x00010F62 File Offset: 0x0000F162
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.lifeTime);
		MeshRenderer componentInChildren = base.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			this.m = componentInChildren.sharedMaterial;
			if (this.m.GetFloat("_Mode") == 1f)
			{
				Transform t = componentInChildren.gameObject.transform;
				while (t.transform.localScale.sqrMagnitude > 0f)
				{
					t.localScale = Vector3.MoveTowards(t.localScale, Vector3.zero, 3f * Time.deltaTime);
					yield return 0;
				}
				t = null;
			}
			else
			{
				while (this.m.color.a > 0f)
				{
					Color color = this.m.color;
					color.a -= 3f * Time.deltaTime;
					this.m.color = color;
					yield return 0;
				}
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040017A9 RID: 6057
	public float lifeTime = 5f;

	// Token: 0x040017AA RID: 6058
	private Material m;
}
