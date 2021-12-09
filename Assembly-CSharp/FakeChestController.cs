using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class FakeChestController : BoardGoalBase
{
	// Token: 0x060008CF RID: 2255 RVA: 0x000508E8 File Offset: 0x0004EAE8
	public void Awake()
	{
		this.m_customMaterial = new Material(this.m_baseMaterial);
		this.m_startColor = this.m_baseMaterial.GetColor("_ReplaceColor");
		MeshRenderer[] renderers = this.m_renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].sharedMaterial = this.m_customMaterial;
		}
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00009EFC File Offset: 0x000080FC
	public override void Open()
	{
		base.StartCoroutine(this.OpenChestEffect());
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x00009F0B File Offset: 0x0000810B
	private IEnumerator OpenChestEffect()
	{
		AudioSystem.PlayOneShot(this.fanfareClip, 0.75f, 0f, 1f);
		yield return new WaitForSeconds(0.5f);
		AudioSystem.PlayOneShot(this.unlockClip, 0.25f, 0f, 1f);
		yield return new WaitForSeconds(0.25f);
		this.anim.ResetTrigger("ChestFall");
		this.anim.SetTrigger("OpenChest");
		yield return new WaitForSeconds(1f);
		AudioSystem.PlayOneShot(this.confettiPop, 2f, 0f, 1f);
		this.confettiParticles[0].Emit(100);
		yield return new WaitForSeconds(0.4f);
		AudioSystem.PlayOneShot(this.confettiPop, 1f, 0f, 1f);
		this.confettiParticles[1].Emit(100);
		yield break;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00050940 File Offset: 0x0004EB40
	public override void Despawn()
	{
		if (this.m_isReal)
		{
			base.StartCoroutine(this.CloseChestEffect());
			return;
		}
		this.anim.ResetTrigger("ChestFall");
		this.anim.SetTrigger("CloseChest");
		UnityEngine.Object.Instantiate<GameObject>(this.despawnPuff, base.transform.position, Quaternion.identity, null);
		AudioSystem.PlayOneShot(this.despawnPuffClip, 0.75f, 0f, 1f);
		this.m_root.SetActive(false);
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00009F1A File Offset: 0x0000811A
	private IEnumerator CloseChestEffect()
	{
		this.anim.ResetTrigger("ChestFall");
		this.anim.SetTrigger("CloseChest");
		yield return new WaitForSeconds(0.15f);
		AudioSystem.PlayOneShot(this.despawnWoosh, 1f, 0f, 1f);
		yield break;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x00009F29 File Offset: 0x00008129
	public override void Spawn()
	{
		this.m_root.SetActive(true);
		this.chestFall = true;
		this.anim.ResetTrigger("ChestFall");
		this.anim.SetTrigger("ChestFall");
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00009F5E File Offset: 0x0000815E
	public void OnDropHitGround()
	{
		if (this.chestFall)
		{
			this.chestFall = false;
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00009F6F File Offset: 0x0000816F
	public void OnDropHitSOund()
	{
		if (this.chestFall)
		{
			AudioSystem.PlayOneShot(this.hitGroundSound, 0.5f, 0f, 1f);
		}
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00009F93 File Offset: 0x00008193
	public override bool IsFake()
	{
		return !this.m_isReal;
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00009F9E File Offset: 0x0000819E
	public void OpenAnim()
	{
		LeanTween.rotateLocal(this.m_lid, new Vector3(-90f, 0f, 0f), 0.25f).setEaseInBack();
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00009FCA File Offset: 0x000081CA
	public void CloseAnim()
	{
		LeanTween.rotateLocal(this.m_lid, new Vector3(0f, 0f, 0f), 0.25f).setEaseInQuint();
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00009FF6 File Offset: 0x000081F6
	public void DespawnAnim()
	{
		LeanTween.moveLocalY(base.gameObject, 20f, 0.5f).setEaseInQuint();
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x000509C8 File Offset: 0x0004EBC8
	public void SetRealChest(bool real, bool boardInteraction = false)
	{
		this.m_isReal = real;
		this.m_realChestRoot.SetActive(real);
		if (boardInteraction)
		{
			this.anim.enabled = true;
			if (this.interaction == null)
			{
				this.interaction = base.gameObject.AddComponent<TrophyInteraction>();
				this.interaction.title = "Treasure Chest";
				this.interaction.description = "TreasureChestDescription";
				this.interaction.buttonSettings = new InteractionButtonSettings[]
				{
					new InteractionButtonSettings("Open", true, 40),
					new InteractionButtonSettings("Ignore", true, 0)
				};
				this.interaction.icon = this.m_interactionSprite;
			}
			this.interaction.isFakeChest = !real;
		}
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00050A94 File Offset: 0x0004EC94
	public void SetChestIndex(int index)
	{
		this.m_chestDecals[this.m_chestIndex].SetActive(false);
		this.m_chestIndex = index;
		this.m_chestDecals[index].SetActive(true);
		Vector3 localScale = this.m_chestDecals[index].transform.localScale;
		this.m_chestDecals[index].transform.localScale = Vector3.zero;
		LeanTween.scale(this.m_chestDecals[index], localScale, 0.3f).setEaseOutBack();
		LeanTween.value(base.gameObject, new Action<float>(this.OnUpdateMaterial), 0f, 1f, 0.3f).setEaseOutBack();
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0000A013 File Offset: 0x00008213
	private void OnUpdateMaterial(float a)
	{
		this.m_customMaterial.SetColor("_ReplaceColor", Color.Lerp(this.m_startColor, this.m_chestColors[this.m_chestIndex], a));
	}

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private Color[] m_chestColors;

	// Token: 0x0400072E RID: 1838
	[SerializeField]
	private GameObject[] m_chestDecals;

	// Token: 0x0400072F RID: 1839
	[SerializeField]
	private GameObject m_lid;

	// Token: 0x04000730 RID: 1840
	[SerializeField]
	private MeshRenderer[] m_renderers;

	// Token: 0x04000731 RID: 1841
	[SerializeField]
	private Material m_baseMaterial;

	// Token: 0x04000732 RID: 1842
	[SerializeField]
	private GameObject m_root;

	// Token: 0x04000733 RID: 1843
	[SerializeField]
	private GameObject m_realChestRoot;

	// Token: 0x04000734 RID: 1844
	[SerializeField]
	private Sprite m_interactionSprite;

	// Token: 0x04000735 RID: 1845
	[Header("Goal")]
	public Animator anim;

	// Token: 0x04000736 RID: 1846
	public AudioClip hitGroundSound;

	// Token: 0x04000737 RID: 1847
	public AudioClip unlockClip;

	// Token: 0x04000738 RID: 1848
	public AudioClip fanfareClip;

	// Token: 0x04000739 RID: 1849
	public ParticleSystem[] confettiParticles;

	// Token: 0x0400073A RID: 1850
	public AudioClip confettiPop;

	// Token: 0x0400073B RID: 1851
	public AudioClip despawnWoosh;

	// Token: 0x0400073C RID: 1852
	public GameObject despawnPuff;

	// Token: 0x0400073D RID: 1853
	public AudioClip despawnPuffClip;

	// Token: 0x0400073E RID: 1854
	public bool chestFall = true;

	// Token: 0x0400073F RID: 1855
	private Material m_customMaterial;

	// Token: 0x04000740 RID: 1856
	private Color m_startColor;

	// Token: 0x04000741 RID: 1857
	private int m_chestIndex;

	// Token: 0x04000742 RID: 1858
	private bool m_isReal;

	// Token: 0x04000743 RID: 1859
	private TrophyInteraction interaction;
}
