using System;
using LlockhamIndustries.Decals;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class ProjectionFader : MonoBehaviour
{
	// Token: 0x0600158B RID: 5515 RVA: 0x0009BA24 File Offset: 0x00099C24
	public void Start()
	{
		this.renderers = base.GetComponentsInChildren<ProjectionRenderer>();
		this.state = new bool[this.renderers.Length];
		this.times = new float[this.renderers.Length];
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].Properties[0].enabled = true;
		}
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x0001055E File Offset: 0x0000E75E
	private void FixedUpdate()
	{
		this.Do();
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x00010566 File Offset: 0x0000E766
	private void Do()
	{
		this.Check();
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x0009BA90 File Offset: 0x00099C90
	private void Check()
	{
		this.time += Time.deltaTime;
		if (this.num >= this.renderers.Length)
		{
			this.num = 0;
		}
		for (int i = this.num; i < this.renderers.Length; i++)
		{
			float sqrMagnitude = (this.renderers[i].transform.position - this.cam.position).sqrMagnitude;
			bool val = sqrMagnitude > this.maxSqrDist;
			if (val != this.state[i])
			{
				ProjectionRenderer pr = this.renderers[i];
				if (this.state[i])
				{
					pr.enabled = true;
				}
				LeanTween.cancel(pr.gameObject);
				LeanTween.value(pr.gameObject, pr.Properties[0].color, val ? this.offColor : this.startColor, 0.25f).setOnUpdate(delegate(Color c)
				{
					pr.SetColor(0, c);
					pr.UpdateProperties();
				}).setOnComplete(delegate()
				{
					if (val)
					{
						pr.enabled = false;
					}
				});
				this.state[i] = val;
			}
			if (this.time < this.interval)
			{
				this.num = i;
				return;
			}
			this.time -= this.interval;
		}
		this.num = 0;
	}

	// Token: 0x04001686 RID: 5766
	public Transform cam;

	// Token: 0x04001687 RID: 5767
	public float maxSqrDist = 625f;

	// Token: 0x04001688 RID: 5768
	public Color startColor;

	// Token: 0x04001689 RID: 5769
	public Color offColor;

	// Token: 0x0400168A RID: 5770
	private ProjectionRenderer[] renderers;

	// Token: 0x0400168B RID: 5771
	private bool[] state;

	// Token: 0x0400168C RID: 5772
	private float[] times;

	// Token: 0x0400168D RID: 5773
	private int num;

	// Token: 0x0400168E RID: 5774
	private float time;

	// Token: 0x0400168F RID: 5775
	private float interval = 0.005f;
}
