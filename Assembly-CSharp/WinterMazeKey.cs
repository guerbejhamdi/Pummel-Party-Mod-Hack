using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000598 RID: 1432
public class WinterMazeKey : MonoBehaviour
{
	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06002536 RID: 9526 RVA: 0x0001AB2A File Offset: 0x00018D2A
	// (set) Token: 0x06002537 RID: 9527 RVA: 0x0001AB32 File Offset: 0x00018D32
	public bool Collected { get; set; }

	// Token: 0x06002538 RID: 9528 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Start()
	{
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000E0F38 File Offset: 0x000DF138
	private void Update()
	{
		if (this.Collected)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
			this.keyLight.intensity = Mathf.Clamp01(this.keyLight.intensity - Time.deltaTime * 10f);
		}
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000E0FA0 File Offset: 0x000DF1A0
	public void Collect(CharacterBase player)
	{
		this.Collected = true;
		AudioSystem.PlayOneShot("RetroCoinPickup_CC0_Davidsraba", 1f, 0f);
		UnityEngine.Object.Instantiate<GameObject>(this.collectParticlePrefab, base.transform.position, Quaternion.identity);
		if (NetSystem.IsServer && player != null)
		{
			short score = player.Score;
			player.Score = score + 1;
		}
	}

	// Token: 0x040028AA RID: 10410
	public Light keyLight;

	// Token: 0x040028AB RID: 10411
	public GameObject particleObject;

	// Token: 0x040028AC RID: 10412
	public GameObject collectParticlePrefab;

	// Token: 0x040028AD RID: 10413
	public MeshRenderer meshRenderer;
}
