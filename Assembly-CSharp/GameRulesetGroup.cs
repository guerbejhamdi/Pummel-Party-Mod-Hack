using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000078 RID: 120
public interface GameRulesetGroup
{
	// Token: 0x0600025D RID: 605
	void Serialize(ZPBitStream stream, ushort version);

	// Token: 0x0600025E RID: 606
	void Deserialize(ZPBitStream stream, ushort version);

	// Token: 0x0600025F RID: 607
	void Apply();

	// Token: 0x06000260 RID: 608
	string GetRulesetGroupName();

	// Token: 0x06000261 RID: 609
	Sprite GetRulesetGroupIcon();

	// Token: 0x06000262 RID: 610
	void ShowUIWindow(string header, RulesetUIWindow window);

	// Token: 0x06000263 RID: 611
	RulesetGroupType GetRulesetType();
}
