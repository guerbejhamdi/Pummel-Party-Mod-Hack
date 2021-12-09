using System;
using System.Collections;
using MalbersAnimations.Events;
using MalbersAnimations.Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations
{
	// Token: 0x0200073E RID: 1854
	[Serializable]
	public class Stat
	{
		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x000247A9 File Offset: 0x000229A9
		// (set) Token: 0x060035DE RID: 13790 RVA: 0x000247B1 File Offset: 0x000229B1
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
				if (value)
				{
					this.StartRegeneration();
					return;
				}
				this.StopRegeneration();
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060035DF RID: 13791 RVA: 0x000247CA File Offset: 0x000229CA
		// (set) Token: 0x060035E0 RID: 13792 RVA: 0x00115FC4 File Offset: 0x001141C4
		public float Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				if (!this.Active)
				{
					return;
				}
				if (value < 0f)
				{
					value = 0f;
				}
				if (this.value.Value != value)
				{
					this.value.Value = value;
					if (value == 0f)
					{
						this.OnStatEmpty.Invoke();
					}
					this.OnValueChangeNormalized.Invoke(value / this.MaxValue);
					this.OnValueChange.Invoke(value);
					if (value > this.Above && !this.isAbove)
					{
						this.OnStatAbove.Invoke();
						this.isAbove = true;
						this.isBelow = false;
						return;
					}
					if (value < this.Below && !this.isBelow)
					{
						this.OnStatBelow.Invoke();
						this.isBelow = true;
						this.isAbove = false;
					}
				}
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060035E1 RID: 13793 RVA: 0x000247D7 File Offset: 0x000229D7
		// (set) Token: 0x060035E2 RID: 13794 RVA: 0x000247E4 File Offset: 0x000229E4
		public float MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				this.maxValue.Value = value;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060035E3 RID: 13795 RVA: 0x000247F2 File Offset: 0x000229F2
		// (set) Token: 0x060035E4 RID: 13796 RVA: 0x000247FF File Offset: 0x000229FF
		public float MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				this.minValue.Value = value;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060035E5 RID: 13797 RVA: 0x0002480D File Offset: 0x00022A0D
		// (set) Token: 0x060035E6 RID: 13798 RVA: 0x00024815 File Offset: 0x00022A15
		public bool Regenerate
		{
			get
			{
				return this.regenerate;
			}
			set
			{
				this.regenerate = value;
				this.Regenerate_OldValue = this.regenerate;
				this.StartRegeneration();
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x060035E7 RID: 13799 RVA: 0x00024830 File Offset: 0x00022A30
		// (set) Token: 0x060035E8 RID: 13800 RVA: 0x00116090 File Offset: 0x00114290
		public bool Degenerate
		{
			get
			{
				return this.degenerate;
			}
			set
			{
				if (this.degenerate != value)
				{
					this.degenerate = value;
					this.OnDegenereate.Invoke(value);
					if (this.degenerate)
					{
						this.regenerate = false;
						this.StartDegeneration();
						this.StopRegeneration();
						return;
					}
					this.regenerate = this.Regenerate_OldValue;
					this.StopDegeneration();
					this.StartRegeneration();
				}
			}
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001160F0 File Offset: 0x001142F0
		internal void InitializeStat(MonoBehaviour holder)
		{
			this.isAbove = (this.isBelow = false);
			this.Coroutine = holder;
			if (this.value.Value > this.Above)
			{
				this.isAbove = true;
			}
			else if (this.value.Value < this.Below)
			{
				this.isBelow = true;
			}
			this.Regenerate_OldValue = this.Regenerate;
			if (this.MaxValue < this.Value)
			{
				this.MaxValue = this.Value;
			}
			this.Regeneration = null;
			this.Degeneration = null;
			this.ModifyPerTicks = null;
			this.StartRegeneration();
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x00024838 File Offset: 0x00022A38
		public virtual void Modify(float newValue)
		{
			if (!this.Active)
			{
				return;
			}
			this.Value += newValue;
			this.StartRegeneration();
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x00024857 File Offset: 0x00022A57
		public virtual void Modify(float newValue, float time)
		{
			if (!this.Active)
			{
				return;
			}
			this.StopSlowModification();
			this.ModifySlow = this.C_SmoothChangeValue(newValue, time);
			this.Coroutine.StartCoroutine(this.ModifySlow);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x0011618C File Offset: 0x0011438C
		public virtual void Modify(float newValue, int ticks, float timeBetweenTicks)
		{
			if (!this.Active)
			{
				return;
			}
			if (this.ModifyPerTicks != null)
			{
				this.Coroutine.StopCoroutine(this.ModifyPerTicks);
			}
			this.ModifyPerTicks = this.C_ModifyTicksValue(newValue, ticks, timeBetweenTicks);
			this.Coroutine.StartCoroutine(this.ModifyPerTicks);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x00024888 File Offset: 0x00022A88
		public virtual void ModifyMAX(float newValue)
		{
			if (!this.Active)
			{
				return;
			}
			this.MaxValue += newValue;
			this.StartRegeneration();
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000248A7 File Offset: 0x00022AA7
		public virtual void ModifyRegenerationRate(float newValue)
		{
			if (!this.Active)
			{
				return;
			}
			this.RegenRate.Value += newValue;
			this.StartRegeneration();
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000248CB File Offset: 0x00022ACB
		public virtual void ModifyRegenerationWait(float newValue)
		{
			if (!this.Active)
			{
				return;
			}
			this.RegenWaitTime.Value += newValue;
			if (this.RegenWaitTime < 0f)
			{
				this.RegenWaitTime.Value = 0f;
			}
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x0002490B File Offset: 0x00022B0B
		public virtual void SetRegenerationRate(float newValue)
		{
			if (!this.Active)
			{
				return;
			}
			this.RegenRate.Value = newValue;
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x00024922 File Offset: 0x00022B22
		public virtual void Reset()
		{
			this.Value = this.MaxValue;
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x00024930 File Offset: 0x00022B30
		public virtual void Clean()
		{
			this.StopDegeneration();
			this.StopRegeneration();
			this.StopTickDamage();
			this.StopSlowModification();
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001161DC File Offset: 0x001143DC
		protected virtual void StartRegeneration()
		{
			this.StopRegeneration();
			if (this.RegenRate == 0f || !this.Regenerate)
			{
				return;
			}
			this.Regeneration = this.C_Regenerate();
			this.Coroutine.StartCoroutine(this.Regeneration);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0002494A File Offset: 0x00022B4A
		protected virtual void StartDegeneration()
		{
			if (this.DegenRate == 0f)
			{
				return;
			}
			this.StopDegeneration();
			this.Degeneration = this.C_Degenerate();
			this.Coroutine.StartCoroutine(this.Degeneration);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00024983 File Offset: 0x00022B83
		protected virtual void StopRegeneration()
		{
			if (this.Regeneration != null)
			{
				this.Coroutine.StopCoroutine(this.Regeneration);
			}
			this.Regeneration = null;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x000249A5 File Offset: 0x00022BA5
		protected virtual void StopDegeneration()
		{
			if (this.Degeneration != null)
			{
				this.Coroutine.StopCoroutine(this.Degeneration);
			}
			this.Degeneration = null;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000249C7 File Offset: 0x00022BC7
		protected virtual void StopTickDamage()
		{
			if (this.ModifyPerTicks != null)
			{
				this.Coroutine.StopCoroutine(this.ModifyPerTicks);
			}
			this.ModifyPerTicks = null;
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x000249E9 File Offset: 0x00022BE9
		protected virtual void StopSlowModification()
		{
			if (this.ModifySlow != null)
			{
				this.Coroutine.StopCoroutine(this.ModifySlow);
			}
			this.ModifySlow = null;
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x00024A0B File Offset: 0x00022C0B
		protected virtual IEnumerator C_Regenerate()
		{
			if (this.RegenWaitTime > 0f)
			{
				yield return new WaitForSeconds(this.RegenWaitTime);
			}
			float ReachValue = (this.RegenRate > 0f) ? this.MaxValue : 0f;
			bool Positive = this.RegenRate > 0f;
			while (this.Value != ReachValue)
			{
				this.Value += this.RegenRate * Time.deltaTime;
				if (Positive && this.Value > this.MaxValue)
				{
					this.Reset();
					this.OnStatFull.Invoke();
				}
				else if (!Positive && this.Value < 0f)
				{
					this.Value = this.MinValue;
					this.OnStatEmpty.Invoke();
				}
				yield return null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x00024A1A File Offset: 0x00022C1A
		protected virtual IEnumerator C_Degenerate()
		{
			while (this.Degenerate || this.Value <= this.MinValue)
			{
				this.Value -= this.DegenRate * Time.deltaTime;
				yield return null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x00024A29 File Offset: 0x00022C29
		protected virtual IEnumerator C_ModifyTicksValue(float value, int Ticks, float time)
		{
			WaitForSeconds WaitForTicks = new WaitForSeconds(time);
			int num;
			for (int i = 0; i < Ticks; i = num + 1)
			{
				this.Value += value;
				if (this.Value <= this.MinValue)
				{
					this.Value = this.MinValue;
					break;
				}
				yield return WaitForTicks;
				num = i;
			}
			yield return null;
			this.StartRegeneration();
			yield break;
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x00024A4D File Offset: 0x00022C4D
		protected virtual IEnumerator C_SmoothChangeValue(float newvalue, float smoothChangeValueTime)
		{
			this.StopRegeneration();
			Debug.Log(newvalue);
			float currentTime = 0f;
			float currentValue = this.Value;
			newvalue = this.Value + newvalue;
			while (currentTime <= smoothChangeValueTime)
			{
				this.Value = Mathf.Lerp(currentValue, newvalue, currentTime / smoothChangeValueTime);
				currentTime += Time.deltaTime;
				yield return null;
			}
			this.Value = newvalue;
			yield return null;
			this.StartRegeneration();
			yield break;
		}

		// Token: 0x040034F7 RID: 13559
		public string name;

		// Token: 0x040034F8 RID: 13560
		[SerializeField]
		private bool active = true;

		// Token: 0x040034F9 RID: 13561
		public IntReference ID;

		// Token: 0x040034FA RID: 13562
		[SerializeField]
		private FloatReference value;

		// Token: 0x040034FB RID: 13563
		[SerializeField]
		private FloatReference maxValue;

		// Token: 0x040034FC RID: 13564
		[SerializeField]
		private FloatReference minValue;

		// Token: 0x040034FD RID: 13565
		[SerializeField]
		private bool regenerate;

		// Token: 0x040034FE RID: 13566
		public FloatReference RegenRate;

		// Token: 0x040034FF RID: 13567
		public FloatReference RegenWaitTime;

		// Token: 0x04003500 RID: 13568
		[SerializeField]
		private bool degenerate;

		// Token: 0x04003501 RID: 13569
		public FloatReference DegenRate;

		// Token: 0x04003502 RID: 13570
		private bool isBelow;

		// Token: 0x04003503 RID: 13571
		private bool isAbove;

		// Token: 0x04003504 RID: 13572
		public bool ShowEvents;

		// Token: 0x04003505 RID: 13573
		public UnityEvent OnStatFull = new UnityEvent();

		// Token: 0x04003506 RID: 13574
		public UnityEvent OnStatEmpty = new UnityEvent();

		// Token: 0x04003507 RID: 13575
		[SerializeField]
		public float Below;

		// Token: 0x04003508 RID: 13576
		[SerializeField]
		public float Above;

		// Token: 0x04003509 RID: 13577
		public UnityEvent OnStatBelow = new UnityEvent();

		// Token: 0x0400350A RID: 13578
		public UnityEvent OnStatAbove = new UnityEvent();

		// Token: 0x0400350B RID: 13579
		public FloatEvent OnValueChangeNormalized = new FloatEvent();

		// Token: 0x0400350C RID: 13580
		public FloatEvent OnValueChange = new FloatEvent();

		// Token: 0x0400350D RID: 13581
		public BoolEvent OnDegenereate = new BoolEvent();

		// Token: 0x0400350E RID: 13582
		private bool Regenerate_OldValue;

		// Token: 0x0400350F RID: 13583
		internal MonoBehaviour Coroutine;

		// Token: 0x04003510 RID: 13584
		public IEnumerator Regeneration;

		// Token: 0x04003511 RID: 13585
		public IEnumerator Degeneration;

		// Token: 0x04003512 RID: 13586
		public IEnumerator ModifyPerTicks;

		// Token: 0x04003513 RID: 13587
		public IEnumerator ModifySlow;
	}
}
