using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class PathSplines : MonoBehaviour
{
	// Token: 0x06000475 RID: 1141 RVA: 0x000402C4 File Offset: 0x0003E4C4
	private void OnEnable()
	{
		this.cr = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00040348 File Offset: 0x0003E548
	private void Start()
	{
		this.avatar1 = GameObject.Find("Avatar1");
		LeanTween.move(this.avatar1, this.cr, 6.5f).setOrientToPath(true).setRepeat(1).setOnComplete(delegate()
		{
			Vector3[] to = new Vector3[]
			{
				this.trans[4].position,
				this.trans[3].position,
				this.trans[2].position,
				this.trans[1].position,
				this.trans[0].position
			};
			LeanTween.moveSpline(this.avatar1, to, 6.5f);
		}).setEase(LeanTweenType.easeOutQuad);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000067DE File Offset: 0x000049DE
	private void Update()
	{
		this.iter += Time.deltaTime * 0.07f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00006810 File Offset: 0x00004A10
	private void OnDrawGizmos()
	{
		if (this.cr == null)
		{
			this.OnEnable();
		}
		Gizmos.color = Color.red;
		if (this.cr != null)
		{
			this.cr.gizmoDraw(-1f);
		}
	}

	// Token: 0x040004E9 RID: 1257
	public Transform[] trans;

	// Token: 0x040004EA RID: 1258
	private LTSpline cr;

	// Token: 0x040004EB RID: 1259
	private GameObject avatar1;

	// Token: 0x040004EC RID: 1260
	private float iter;
}
