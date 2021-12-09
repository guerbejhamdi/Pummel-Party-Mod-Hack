using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class MagnetFlyingItem : MonoBehaviour
{
	// Token: 0x0600088C RID: 2188 RVA: 0x0004F478 File Offset: 0x0004D678
	public void Setup(BoardPlayer targetPlayer, Transform target, MagnetItem.MagnetStealType type, byte itemID)
	{
		this.targetPlayer = targetPlayer;
		this.target = target;
		this.type = type;
		this.itemID = itemID;
		switch (type)
		{
		case MagnetItem.MagnetStealType.Gold:
			this.visual = UnityEngine.Object.Instantiate<GameObject>(this.goldFlyingPrefab, base.transform);
			break;
		case MagnetItem.MagnetStealType.Item:
		{
			ItemDetails itemDetails = GameManager.ItemList.items[(int)itemID];
			this.visual = UnityEngine.Object.Instantiate<GameObject>(itemDetails.recievePrefab, base.transform);
			break;
		}
		case MagnetItem.MagnetStealType.Trophy:
			this.visual = UnityEngine.Object.Instantiate<GameObject>(GameManager.Board.trophies[0], base.transform);
			break;
		}
		this.visual.transform.localPosition = Vector3.zero;
		base.StartCoroutine(this.DoFlyingItem());
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x00009D50 File Offset: 0x00007F50
	private IEnumerator DoFlyingItem()
	{
		for (;;)
		{
			Vector3 vector = this.target.position - base.transform.position;
			float magnitude = vector.magnitude;
			float num = this.velocity * Time.deltaTime;
			if (num >= magnitude)
			{
				break;
			}
			base.transform.position += vector.normalized * num;
			this.velocity *= 1.25f;
			yield return new WaitForFixedUpdate();
		}
		base.transform.position = this.target.position;
		switch (this.type)
		{
		case MagnetItem.MagnetStealType.Gold:
			this.targetPlayer.GiveGold(1, true);
			break;
		case MagnetItem.MagnetStealType.Item:
			this.targetPlayer.GiveItem(this.itemID, true);
			break;
		case MagnetItem.MagnetStealType.Trophy:
			this.targetPlayer.GiveTrophy(1, 0, false);
			break;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040006EA RID: 1770
	public GameObject goldFlyingPrefab;

	// Token: 0x040006EB RID: 1771
	private BoardPlayer targetPlayer;

	// Token: 0x040006EC RID: 1772
	private Transform target;

	// Token: 0x040006ED RID: 1773
	private float velocity = 0.01f;

	// Token: 0x040006EE RID: 1774
	private MagnetItem.MagnetStealType type;

	// Token: 0x040006EF RID: 1775
	private byte itemID;

	// Token: 0x040006F0 RID: 1776
	private GameObject visual;
}
