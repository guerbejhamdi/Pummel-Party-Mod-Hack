using System;
using System.Collections.Generic;
using MalbersAnimations.Scriptables;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200073A RID: 1850
	public class Stats : MonoBehaviour
	{
		// Token: 0x060035C2 RID: 13762 RVA: 0x00115E54 File Offset: 0x00114054
		private void Start()
		{
			base.StopAllCoroutines();
			foreach (Stat stat in this.stats)
			{
				stat.InitializeStat(this);
			}
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x00115EAC File Offset: 0x001140AC
		private void OnDisable()
		{
			base.StopAllCoroutines();
			foreach (Stat stat in this.stats)
			{
				stat.Clean();
			}
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x00024594 File Offset: 0x00022794
		public virtual void _PinStat(string name)
		{
			this.PinnedStat = this.GetStat(name);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000245A3 File Offset: 0x000227A3
		public virtual void _PinStat(int ID)
		{
			this.PinnedStat = this.GetStat(ID);
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000245B2 File Offset: 0x000227B2
		public virtual void _PinStat(IntVar ID)
		{
			this.PinnedStat = this.GetStat(ID);
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x00115F04 File Offset: 0x00114104
		public virtual Stat GetStat(string name)
		{
			this.PinnedStat = this.stats.Find((Stat item) => item.name == name);
			return this.PinnedStat;
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x00115F44 File Offset: 0x00114144
		public virtual Stat GetStat(int ID)
		{
			this.PinnedStat = this.stats.Find((Stat item) => item.ID == ID);
			return this.PinnedStat;
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x00115F84 File Offset: 0x00114184
		public virtual Stat GetStat(IntVar ID)
		{
			this.PinnedStat = this.stats.Find((Stat item) => item.ID == ID);
			return this.PinnedStat;
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000245C1 File Offset: 0x000227C1
		public virtual void _PinStatModifyValue(float value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Modify(value);
				return;
			}
			Debug.Log("There's no Pinned Stat");
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000245E2 File Offset: 0x000227E2
		public virtual void _PinStatModifyValue(float value, float time)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Modify(value, time);
				return;
			}
			Debug.Log("There's no Pinned Stat");
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x00024604 File Offset: 0x00022804
		public virtual void _PinStatModifyValue1Sec(float value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Modify(value, 1f);
				return;
			}
			Debug.Log("There's no Pinned Stat");
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0002462A File Offset: 0x0002282A
		public virtual void _PinStatSetValue(float value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Value = value;
				return;
			}
			Debug.Log("There's no Pinned Stat");
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0002464B File Offset: 0x0002284B
		public virtual void _PinStatModifyMaxValue(float value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.ModifyMAX(value);
				return;
			}
			Debug.Log("There's no Pinned Stat");
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x0002466C File Offset: 0x0002286C
		public virtual void _PinStatSetMaxValue(float value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.MaxValue = value;
				return;
			}
			Debug.Log("There's no Pinned Stat");
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0002468D File Offset: 0x0002288D
		public virtual void _PinStatModifyRegenerationRate(float value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.ModifyRegenerationRate(value);
				return;
			}
			Debug.Log("There's no Active Stat or the Stat you are trying to modify does not exist");
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x000246AE File Offset: 0x000228AE
		public virtual void _PinStatDegenerate(bool value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Degenerate = value;
				return;
			}
			Debug.Log("There's no Active Stat or the Stat you are trying to modify does not exist");
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x000246CF File Offset: 0x000228CF
		public virtual void _PinStatRegenerate(bool value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Regenerate = value;
				return;
			}
			Debug.Log("There's no Active Stat or the Stat you are trying to modify does not exist");
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x000246F0 File Offset: 0x000228F0
		public virtual void _PinStatEnable(bool value)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Active = value;
				return;
			}
			Debug.Log("There's no Active Stat or the Stat you are trying to modify does not exist");
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x00024711 File Offset: 0x00022911
		public virtual void _PinStatModifyValue(float newValue, int ticks, float timeBetweenTicks)
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Modify(newValue, ticks, timeBetweenTicks);
				return;
			}
			Debug.Log("There's no Active Stat or the Stat you are trying to modify does not exist");
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x00024734 File Offset: 0x00022934
		public virtual void _PinStatCLEAN()
		{
			if (this.PinnedStat != null)
			{
				this.PinnedStat.Clean();
				return;
			}
			Debug.Log("There's no Active Stat or the Stat you are trying to modify does not exist");
		}

		// Token: 0x040034F2 RID: 13554
		public List<Stat> stats = new List<Stat>();

		// Token: 0x040034F3 RID: 13555
		[SerializeField]
		private Stat PinnedStat;
	}
}
