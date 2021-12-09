using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
[CreateAssetMenu(fileName = "CharacterHat", menuName = "Character/Create Hat", order = 1)]
public class CharacterHat : ScriptableObject
{
	// Token: 0x04000119 RID: 281
	public string hatName;

	// Token: 0x0400011A RID: 282
	public string hatNameToken = "Name";

	// Token: 0x0400011B RID: 283
	public GameObject prefab;

	// Token: 0x0400011C RID: 284
	public string prefabName;

	// Token: 0x0400011D RID: 285
	public PlayerBone bone = PlayerBone.Head;

	// Token: 0x0400011E RID: 286
	public Vector3 position;

	// Token: 0x0400011F RID: 287
	public Vector3 rotation;

	// Token: 0x04000120 RID: 288
	public Vector3 scale = new Vector3(100f, 100f, 100f);
}
