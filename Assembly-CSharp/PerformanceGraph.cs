using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002BF RID: 703
public class PerformanceGraph : MonoBehaviour
{
	// Token: 0x06001438 RID: 5176 RVA: 0x0000FDD9 File Offset: 0x0000DFD9
	private void Start()
	{
		base.StartCoroutine(this.UpdateMaxFrameTime());
		this.startTime = Time.time;
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000983C0 File Offset: 0x000965C0
	private void Update()
	{
		if (!this.initialClear && Time.time - this.startTime > 0.5f)
		{
			this.ClearAll();
			this.initialClear = true;
		}
		this.frameTimes[this.index] = (float)Math.Round((double)Time.deltaTime, 4);
		this.index++;
		if ((float)this.index == this.size)
		{
			this.index = 0;
		}
		this.curMaxFrameTime += (this.targetMaxFrameTime - this.curMaxFrameTime) * this.frameTimeUpdateSpeed;
		this.image.material.SetFloatArray(this.arrayName, this.frameTimes);
		this.image.material.SetInt("CurrentFrame", this.index);
		this.image.material.SetFloat("MaxFrameTime", this.curMaxFrameTime * 1f);
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.ClearAll();
		}
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x000984C0 File Offset: 0x000966C0
	private void ClearAll()
	{
		for (int i = 0; i < this.frameTimes.Length; i++)
		{
			this.frameTimes[i] = 0f;
		}
		this.index = 0;
		this.targetMaxFrameTime = 0f;
		this.curMaxFrameTime = 0f;
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x0000FDF3 File Offset: 0x0000DFF3
	private IEnumerator UpdateMaxFrameTime()
	{
		for (;;)
		{
			float num = float.MinValue;
			for (int i = 0; i < this.frameTimes.Length; i++)
			{
				if (this.frameTimes[i] > num)
				{
					num = this.frameTimes[i];
				}
			}
			this.targetMaxFrameTime = num;
			if (this.curMaxFrameTime == 0f)
			{
				this.curMaxFrameTime = this.targetMaxFrameTime;
			}
			float num2 = num * 1000f;
			if (num2 % 0.1f != this.lastFloat)
			{
				this.maxFps.text = num2.ToString(this.format);
				this.lastFloat = num2 % 0.1f;
			}
			yield return new WaitForSeconds(this.updateMaxFrameTimeRate);
		}
		yield break;
	}

	// Token: 0x04001586 RID: 5510
	public Image image;

	// Token: 0x04001587 RID: 5511
	public float updateMaxFrameTimeRate = 0.3f;

	// Token: 0x04001588 RID: 5512
	public Text maxFps;

	// Token: 0x04001589 RID: 5513
	public Text minFps;

	// Token: 0x0400158A RID: 5514
	public float frameTimeUpdateSpeed = 1f;

	// Token: 0x0400158B RID: 5515
	private float size = 512f;

	// Token: 0x0400158C RID: 5516
	private float[] frameTimes = new float[512];

	// Token: 0x0400158D RID: 5517
	private string arrayName = "GraphValues";

	// Token: 0x0400158E RID: 5518
	private int index;

	// Token: 0x0400158F RID: 5519
	private bool initialClear;

	// Token: 0x04001590 RID: 5520
	private float startTime;

	// Token: 0x04001591 RID: 5521
	private float targetMaxFrameTime;

	// Token: 0x04001592 RID: 5522
	private float curMaxFrameTime;

	// Token: 0x04001593 RID: 5523
	private readonly string format = "0.0";

	// Token: 0x04001594 RID: 5524
	private readonly string ms = "ms";

	// Token: 0x04001595 RID: 5525
	private float lastFloat;
}
