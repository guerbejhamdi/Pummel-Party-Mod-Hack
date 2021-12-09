using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class LTSeq
{
	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x00008325 File Offset: 0x00006525
	public int id
	{
		get
		{
			return (int)(this._id | this.counter << 16);
		}
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00008337 File Offset: 0x00006537
	public void reset()
	{
		this.previous = null;
		this.tween = null;
		this.totalDelay = 0f;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00008352 File Offset: 0x00006552
	public void init(uint id, uint global_counter)
	{
		this.reset();
		this._id = id;
		this.counter = global_counter;
		this.current = this;
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00045AC0 File Offset: 0x00043CC0
	private LTSeq addOn()
	{
		this.current.toggle = true;
		LTSeq ltseq = this.current;
		this.current = LeanTween.sequence(true);
		this.current.previous = ltseq;
		ltseq.toggle = false;
		this.current.totalDelay = ltseq.totalDelay;
		this.current.debugIter = ltseq.debugIter + 1;
		return this.current;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00045B2C File Offset: 0x00043D2C
	private float addPreviousDelays()
	{
		LTSeq ltseq = this.current.previous;
		if (ltseq != null && ltseq.tween != null)
		{
			return this.current.totalDelay + ltseq.tween.time;
		}
		return this.current.totalDelay;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0000836F File Offset: 0x0000656F
	public LTSeq append(float delay)
	{
		this.current.totalDelay += delay;
		return this.current;
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00045B74 File Offset: 0x00043D74
	public LTSeq append(Action callback)
	{
		LTDescr ltdescr = LeanTween.delayedCall(0f, callback);
		return this.append(ltdescr);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0000838A File Offset: 0x0000658A
	public LTSeq append(Action<object> callback, object obj)
	{
		this.append(LeanTween.delayedCall(0f, callback).setOnCompleteParam(obj));
		return this.addOn();
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x000083AA File Offset: 0x000065AA
	public LTSeq append(GameObject gameObject, Action callback)
	{
		this.append(LeanTween.delayedCall(gameObject, 0f, callback));
		return this.addOn();
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x000083C5 File Offset: 0x000065C5
	public LTSeq append(GameObject gameObject, Action<object> callback, object obj)
	{
		this.append(LeanTween.delayedCall(gameObject, 0f, callback).setOnCompleteParam(obj));
		return this.addOn();
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x000083E6 File Offset: 0x000065E6
	public LTSeq append(LTDescr tween)
	{
		this.current.tween = tween;
		this.current.totalDelay = this.addPreviousDelays();
		tween.setDelay(this.current.totalDelay);
		return this.addOn();
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0000841D File Offset: 0x0000661D
	public LTSeq insert(LTDescr tween)
	{
		this.current.tween = tween;
		tween.setDelay(this.addPreviousDelays());
		return this.addOn();
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0000843E File Offset: 0x0000663E
	public LTSeq setScale(float timeScale)
	{
		this.setScaleRecursive(this.current, timeScale, 500);
		return this.addOn();
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00045B94 File Offset: 0x00043D94
	private void setScaleRecursive(LTSeq seq, float timeScale, int count)
	{
		if (count > 0)
		{
			this.timeScale = timeScale;
			seq.totalDelay *= timeScale;
			if (seq.tween != null)
			{
				if (seq.tween.time != 0f)
				{
					seq.tween.setTime(seq.tween.time * timeScale);
				}
				seq.tween.setDelay(seq.tween.delay * timeScale);
			}
			if (seq.previous != null)
			{
				this.setScaleRecursive(seq.previous, timeScale, count - 1);
			}
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00008458 File Offset: 0x00006658
	public LTSeq reverse()
	{
		return this.addOn();
	}

	// Token: 0x04000562 RID: 1378
	public LTSeq previous;

	// Token: 0x04000563 RID: 1379
	public LTSeq current;

	// Token: 0x04000564 RID: 1380
	public LTDescr tween;

	// Token: 0x04000565 RID: 1381
	public float totalDelay;

	// Token: 0x04000566 RID: 1382
	public float timeScale;

	// Token: 0x04000567 RID: 1383
	private int debugIter;

	// Token: 0x04000568 RID: 1384
	public uint counter;

	// Token: 0x04000569 RID: 1385
	public bool toggle;

	// Token: 0x0400056A RID: 1386
	private uint _id;
}
