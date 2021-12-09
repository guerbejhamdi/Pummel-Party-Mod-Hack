using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200077D RID: 1917
	public class EffectManager : MonoBehaviour, IAnimatorListener
	{
		// Token: 0x060036D3 RID: 14035 RVA: 0x00117664 File Offset: 0x00115864
		private void Awake()
		{
			foreach (Effect effect in this.Effects)
			{
				effect.Owner = base.transform;
				if (!effect.instantiate && effect.effect)
				{
					effect.Instance = effect.effect;
				}
			}
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x001176E0 File Offset: 0x001158E0
		public virtual void PlayEffect(int ID)
		{
			List<Effect> list = this.Effects.FindAll((Effect effect) => effect.ID == ID && effect.active);
			if (list != null)
			{
				foreach (Effect effect2 in list)
				{
					this.Play(effect2);
				}
			}
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x000255CC File Offset: 0x000237CC
		private IEnumerator IPlayEffect(Effect e)
		{
			if (e.delay > 0f)
			{
				yield return new WaitForSeconds(e.delay);
			}
			yield return new WaitForEndOfFrame();
			if (e.instantiate)
			{
				e.Instance = UnityEngine.Object.Instantiate<GameObject>(e.effect);
				e.effect.gameObject.SetActive(false);
			}
			else
			{
				e.Instance = e.effect;
			}
			if (e.Instance && e.root)
			{
				e.Instance.transform.position = e.root.position;
				e.Instance.gameObject.SetActive(true);
			}
			TrailRenderer componentInChildren = e.Instance.GetComponentInChildren<TrailRenderer>();
			if (componentInChildren)
			{
				componentInChildren.Clear();
			}
			e.Instance.transform.localScale = Vector3.Scale(e.Instance.transform.localScale, e.ScaleMultiplier);
			e.OnPlay.Invoke();
			if (e.root)
			{
				if (e.isChild)
				{
					e.Instance.transform.parent = e.root;
				}
				if (e.useRootRotation)
				{
					e.Instance.transform.rotation = e.root.rotation;
				}
			}
			e.Instance.transform.localPosition += e.PositionOffset;
			e.Instance.transform.localRotation *= Quaternion.Euler(e.RotationOffset);
			if (e.Modifier)
			{
				e.Modifier.StartEffect(e);
			}
			base.StartCoroutine(this.Life(e));
			yield return null;
			yield break;
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x00117758 File Offset: 0x00115958
		public virtual void StopEffect(int ID)
		{
			List<Effect> list = this.Effects.FindAll((Effect effect) => effect.ID == ID && effect.active);
			if (list != null)
			{
				foreach (Effect effect2 in list)
				{
					if (effect2.Modifier)
					{
						effect2.Modifier.StopEffect(effect2);
					}
					effect2.OnStop.Invoke();
					effect2.On = false;
				}
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000255E2 File Offset: 0x000237E2
		private IEnumerator Life(Effect e)
		{
			if (e.life > 0f)
			{
				yield return new WaitForSeconds(e.life);
				if (e.Modifier)
				{
					e.Modifier.StopEffect(e);
				}
				e.OnStop.Invoke();
				if (e.instantiate)
				{
					UnityEngine.Object.Destroy(e.Instance);
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x001177F4 File Offset: 0x001159F4
		protected virtual void Play(Effect effect)
		{
			if (effect.effect == null)
			{
				return;
			}
			if (effect.Modifier)
			{
				effect.Modifier.AwakeEffect(effect);
			}
			if (!effect.toggleable)
			{
				base.StartCoroutine(this.IPlayEffect(effect));
				return;
			}
			effect.On = !effect.On;
			if (effect.On)
			{
				base.StartCoroutine(this.IPlayEffect(effect));
				return;
			}
			effect.OnStop.Invoke();
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x0002339D File Offset: 0x0002159D
		public virtual void OnAnimatorBehaviourMessage(string message, object value)
		{
			this.InvokeWithParams(message, value);
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x00117874 File Offset: 0x00115A74
		public virtual void _DisableEffect(string name)
		{
			List<Effect> list = this.Effects.FindAll((Effect effect) => effect.Name.ToUpper() == name.ToUpper());
			if (list != null)
			{
				using (List<Effect>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Effect effect2 = enumerator.Current;
						effect2.active = false;
					}
					return;
				}
			}
			Debug.LogWarning("No effect with the name: " + name + " was found");
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x00117904 File Offset: 0x00115B04
		public virtual void _DisableEffect(int ID)
		{
			List<Effect> list = this.Effects.FindAll((Effect effect) => effect.ID == ID);
			if (list != null)
			{
				using (List<Effect>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Effect effect2 = enumerator.Current;
						effect2.active = false;
					}
					return;
				}
			}
			Debug.LogWarning("No effect with the ID: " + ID.ToString() + " was found");
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x00117998 File Offset: 0x00115B98
		public virtual void _EnableEffect(string name)
		{
			List<Effect> list = this.Effects.FindAll((Effect effect) => effect.Name.ToUpper() == name.ToUpper());
			if (list != null)
			{
				using (List<Effect>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Effect effect2 = enumerator.Current;
						effect2.active = true;
					}
					return;
				}
			}
			Debug.LogWarning("No effect with the name: " + name + " was found");
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x00117A28 File Offset: 0x00115C28
		public virtual void _EnableEffect(int ID)
		{
			List<Effect> list = this.Effects.FindAll((Effect effect) => effect.ID == ID);
			if (list != null)
			{
				using (List<Effect>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Effect effect2 = enumerator.Current;
						effect2.active = true;
					}
					return;
				}
			}
			Debug.LogWarning("No effect with the ID: " + ID.ToString() + " was found");
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x00117ABC File Offset: 0x00115CBC
		public virtual void _EnableEffectPrefab(int ID)
		{
			Effect effect = this.Effects.Find((Effect item) => item.ID == ID);
			if (effect != null)
			{
				effect.Instance.SetActive(true);
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x00117B00 File Offset: 0x00115D00
		public virtual void _DisableEffectPrefab(int ID)
		{
			Effect effect = this.Effects.Find((Effect item) => item.ID == ID);
			if (effect != null)
			{
				effect.Instance.SetActive(false);
			}
		}

		// Token: 0x04003611 RID: 13841
		public List<Effect> Effects;
	}
}
