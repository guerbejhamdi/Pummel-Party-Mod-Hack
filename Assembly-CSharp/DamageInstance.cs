using System;
using UnityEngine;

// Token: 0x020003CC RID: 972
public struct DamageInstance
{
	// Token: 0x04001BAD RID: 7085
	public int damage;

	// Token: 0x04001BAE RID: 7086
	public bool hitAnim;

	// Token: 0x04001BAF RID: 7087
	public bool sound;

	// Token: 0x04001BB0 RID: 7088
	public float volume;

	// Token: 0x04001BB1 RID: 7089
	public Vector3 origin;

	// Token: 0x04001BB2 RID: 7090
	public bool ragdoll;

	// Token: 0x04001BB3 RID: 7091
	public float ragdollVel;

	// Token: 0x04001BB4 RID: 7092
	public bool blood;

	// Token: 0x04001BB5 RID: 7093
	public float bloodVel;

	// Token: 0x04001BB6 RID: 7094
	public float bloodAmount;

	// Token: 0x04001BB7 RID: 7095
	public string details;

	// Token: 0x04001BB8 RID: 7096
	public BoardPlayer killer;

	// Token: 0x04001BB9 RID: 7097
	public bool removeKeys;
}
