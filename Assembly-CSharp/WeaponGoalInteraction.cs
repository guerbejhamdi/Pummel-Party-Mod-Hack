using System;
using System.Collections;
using UnityEngine;
using ZP.Net;

// Token: 0x02000592 RID: 1426
public class WeaponGoalInteraction : Interaction
{
	// Token: 0x0600250F RID: 9487 RVA: 0x000E0910 File Offset: 0x000DEB10
	public override void Setup(BoardPlayer player)
	{
		base.Setup(player);
		Vector3 normalized = (player.transform.position - base.transform.position).normalized;
		player.PlayerAnimation.SetPlayerRotation(Mathf.Atan2(normalized.x, normalized.z) * 57.29578f - 180f);
		this.OnInteractionChoice(0, 0);
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x0001AA55 File Offset: 0x00018C55
	public override IEnumerator DoInteraction(byte choice)
	{
		if (choice == 100)
		{
			yield break;
		}
		yield return new WaitForSeconds(0.5f);
		int index = GameManager.Board.GetWeaponGoalIndex(this.player.CurrentNode);
		GameManager.Board.OpenWeaponSpace(index);
		yield return new WaitForSeconds(1.25f);
		Debug.Log("Doing Weapon Goal Interaction: " + choice.ToString());
		this.player.GiveItem(base.GetComponent<WeaponGoal>().itemID, true);
		yield return new WaitForSeconds(2f);
		UnityEngine.Object.Instantiate<GameObject>(this.persisitentWeaponChoice).GetComponent<WeaponsCacheSpawnPersistent>().Initialize(null, null);
		GameManager.Board.DespawnWeaponSpace(index);
		bool isServer = NetSystem.IsServer;
		base.Finished = true;
		GameManager.Board.EndInteraction();
		yield break;
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x0001AA6B File Offset: 0x00018C6B
	public override int GetAIChoice()
	{
		return 100;
	}

	// Token: 0x04002896 RID: 10390
	public GameObject persisitentWeaponChoice;
}
