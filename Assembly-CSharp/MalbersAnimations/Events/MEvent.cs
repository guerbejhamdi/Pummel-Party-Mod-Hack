using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Events
{
	// Token: 0x02000776 RID: 1910
	[CreateAssetMenu(menuName = "Malbers Animations/Event", fileName = "New Event Asset")]
	public class MEvent : ScriptableObject
	{
		// Token: 0x060036B1 RID: 14001 RVA: 0x00116FF8 File Offset: 0x001151F8
		public virtual void Invoke()
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked();
			}
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x00117030 File Offset: 0x00115230
		public virtual void Invoke(float value)
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked(value);
			}
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x00117068 File Offset: 0x00115268
		public virtual void Invoke(bool value)
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked(value);
			}
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x001170A0 File Offset: 0x001152A0
		public virtual void Invoke(string value)
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked(value);
			}
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x001170D8 File Offset: 0x001152D8
		public virtual void Invoke(int value)
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked(value);
			}
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x00117110 File Offset: 0x00115310
		public virtual void Invoke(GameObject value)
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked(value);
			}
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x00117148 File Offset: 0x00115348
		public virtual void Invoke(Transform value)
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked(value);
			}
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x00117180 File Offset: 0x00115380
		public virtual void Invoke(Vector3 value)
		{
			for (int i = this.eventListeners.Count - 1; i >= 0; i--)
			{
				this.eventListeners[i].OnEventInvoked(value);
			}
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x00025499 File Offset: 0x00023699
		public virtual void RegisterListener(MEventItemListener listener)
		{
			if (!this.eventListeners.Contains(listener))
			{
				this.eventListeners.Add(listener);
			}
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000254B5 File Offset: 0x000236B5
		public virtual void UnregisterListener(MEventItemListener listener)
		{
			if (this.eventListeners.Contains(listener))
			{
				this.eventListeners.Remove(listener);
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000254D2 File Offset: 0x000236D2
		public virtual void DebugLog(string text)
		{
			Debug.Log(text);
		}

		// Token: 0x040035ED RID: 13805
		private readonly List<MEventItemListener> eventListeners = new List<MEventItemListener>();
	}
}
