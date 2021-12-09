using System;
using UnityEngine;

namespace DentedPixel.LTExamples
{
	// Token: 0x020007DB RID: 2011
	public class PathBezier : MonoBehaviour
	{
		// Token: 0x06003927 RID: 14631 RVA: 0x0011D590 File Offset: 0x0011B790
		private void OnEnable()
		{
			this.cr = new LTBezierPath(new Vector3[]
			{
				this.trans[0].position,
				this.trans[2].position,
				this.trans[1].position,
				this.trans[3].position,
				this.trans[3].position,
				this.trans[5].position,
				this.trans[4].position,
				this.trans[6].position
			});
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x0011D650 File Offset: 0x0011B850
		private void Start()
		{
			this.avatar1 = GameObject.Find("Avatar1");
			LTDescr ltdescr = LeanTween.move(this.avatar1, this.cr.pts, 6.5f).setOrientToPath(true).setRepeat(-1);
			Debug.Log("length of path 1:" + this.cr.length.ToString());
			Debug.Log("length of path 2:" + ltdescr.optional.path.length.ToString());
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x00026D65 File Offset: 0x00024F65
		private void Update()
		{
			this.iter += Time.deltaTime * 0.07f;
			if (this.iter > 1f)
			{
				this.iter = 0f;
			}
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x00026D97 File Offset: 0x00024F97
		private void OnDrawGizmos()
		{
			if (this.cr != null)
			{
				this.OnEnable();
			}
			Gizmos.color = Color.red;
			if (this.cr != null)
			{
				this.cr.gizmoDraw(-1f);
			}
		}

		// Token: 0x040037A6 RID: 14246
		public Transform[] trans;

		// Token: 0x040037A7 RID: 14247
		private LTBezierPath cr;

		// Token: 0x040037A8 RID: 14248
		private GameObject avatar1;

		// Token: 0x040037A9 RID: 14249
		private float iter;
	}
}
