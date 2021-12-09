using System;
using System.Collections;
using System.IO;
using ZP.Net;

// Token: 0x02000594 RID: 1428
public class WeaponsCacheSpawnPersistent : PersistentItem
{
	// Token: 0x06002519 RID: 9497 RVA: 0x0001AA86 File Offset: 0x00018C86
	public override IEnumerator DoEvent(PersistentItemEventType type, BinaryReader reader)
	{
		int weaponsCacheRespawnTurns = GameManager.RulesetManager.ActiveRuleset.General.WeaponsCacheRespawnTurns;
		if (type == PersistentItemEventType.FirstTurn)
		{
			this.turnCount++;
			if (NetSystem.IsServer && this.turnCount >= weaponsCacheRespawnTurns)
			{
				GameManager.Board.SpawnNewWeaponGoal(0);
			}
		}
		this.Finish(type, this.turnCount >= weaponsCacheRespawnTurns);
		yield break;
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x00005550 File Offset: 0x00003750
	public override short PersistentItemID()
	{
		return 2;
	}

	// Token: 0x0400289C RID: 10396
	private int turnCount;
}
