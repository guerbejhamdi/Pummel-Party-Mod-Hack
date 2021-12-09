using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class TemporaryEffect : MonoBehaviour
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060017ED RID: 6125 RVA: 0x00011C6C File Offset: 0x0000FE6C
	public float LifeTime
	{
		get
		{
			return this.m_lifeTime;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x060017EE RID: 6126 RVA: 0x00011C74 File Offset: 0x0000FE74
	public float SpawnTime
	{
		get
		{
			return this.m_spawnTime;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060017EF RID: 6127 RVA: 0x00011C7C File Offset: 0x0000FE7C
	// (set) Token: 0x060017F0 RID: 6128 RVA: 0x00011C84 File Offset: 0x0000FE84
	public bool IsDestroying { get; set; }

	// Token: 0x060017F1 RID: 6129 RVA: 0x00011C8D File Offset: 0x0000FE8D
	public void Awake()
	{
		this.m_startScale = base.transform.localScale;
		this.m_spawnTime = Time.time;
		TemporaryEffect.AddEffect(this);
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x00011CB1 File Offset: 0x0000FEB1
	public void OnDestroy()
	{
		TemporaryEffect.RemoveEffect(this);
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x00011CB9 File Offset: 0x0000FEB9
	public void BeginDestroy()
	{
		this.m_isDestroying = true;
		if (!this.m_animateDestroy)
		{
			TemporaryEffect.RemoveEffect(this);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.StartCoroutine(this.DestroyAnimation());
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x00011CE9 File Offset: 0x0000FEE9
	private IEnumerator DestroyAnimation()
	{
		float destroyTime = 0.5f;
		float startTime = Time.time;
		while (Time.time - startTime < destroyTime)
		{
			float num = (Time.time - startTime) / destroyTime;
			num = Easing.SineEaseIn(1f - num);
			base.transform.localScale = this.m_startScale * num;
			yield return null;
		}
		TemporaryEffect.RemoveEffect(this);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x00011CF8 File Offset: 0x0000FEF8
	public static void AddEffect(TemporaryEffect effect)
	{
		if (effect == null)
		{
			return;
		}
		TemporaryEffect.m_effects.Add(effect);
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x00011D0F File Offset: 0x0000FF0F
	public static void RemoveEffect(TemporaryEffect effect)
	{
		if (effect == null)
		{
			return;
		}
		TemporaryEffect.m_effects.Remove(effect);
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x000A5958 File Offset: 0x000A3B58
	public static void ClearEffects()
	{
		for (int i = TemporaryEffect.m_effects.Count - 1; i >= 0; i--)
		{
			if (!(TemporaryEffect.m_effects[i] == null))
			{
				UnityEngine.Object.Destroy(TemporaryEffect.m_effects[i].gameObject);
			}
		}
		TemporaryEffect.m_effects.Clear();
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x000A59B0 File Offset: 0x000A3BB0
	public static void UpdateEffects()
	{
		for (int i = TemporaryEffect.m_effects.Count - 1; i >= 0; i--)
		{
			if (!(TemporaryEffect.m_effects[i] == null))
			{
				TemporaryEffect temporaryEffect = TemporaryEffect.m_effects[i];
				if (temporaryEffect.gameObject.activeInHierarchy && !temporaryEffect.IsDestroying && Time.time - temporaryEffect.SpawnTime >= temporaryEffect.LifeTime)
				{
					temporaryEffect.BeginDestroy();
				}
			}
		}
	}

	// Token: 0x0400196F RID: 6511
	[SerializeField]
	private float m_lifeTime = 60f;

	// Token: 0x04001970 RID: 6512
	[SerializeField]
	private bool m_animateDestroy = true;

	// Token: 0x04001971 RID: 6513
	private Vector3 m_startScale;

	// Token: 0x04001972 RID: 6514
	private float m_spawnTime;

	// Token: 0x04001973 RID: 6515
	private bool m_isDestroying;

	// Token: 0x04001975 RID: 6517
	private static List<TemporaryEffect> m_effects = new List<TemporaryEffect>();
}
