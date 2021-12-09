using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
[CreateAssetMenu(fileName = "CharacterSkin", menuName = "Character/Create Skin", order = 1)]
public class CharacterSkin : ScriptableObject
{
	// Token: 0x04000121 RID: 289
	public PlayerSkin male;

	// Token: 0x04000122 RID: 290
	public PlayerSkin female;
}
