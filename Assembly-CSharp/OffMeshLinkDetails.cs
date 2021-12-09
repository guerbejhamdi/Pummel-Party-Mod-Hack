using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000431 RID: 1073
[RequireComponent(typeof(OffMeshLink))]
[ExecuteInEditMode]
public class OffMeshLinkDetails : MonoBehaviour
{
	// Token: 0x04002073 RID: 8307
	public bool auto;

	// Token: 0x04002074 RID: 8308
	public OffMeshLink offMeshLink;

	// Token: 0x04002075 RID: 8309
	public OffMeshLinkTranslateType type;

	// Token: 0x04002076 RID: 8310
	public float height = 1f;

	// Token: 0x04002077 RID: 8311
	public float duration = 1f;

	// Token: 0x04002078 RID: 8312
	public Color handleColor = new Color32(147, 196, 125, 190);

	// Token: 0x04002079 RID: 8313
	public Color handleColor2 = new Color32(109, 158, 235, 190);

	// Token: 0x0400207A RID: 8314
	public Color handleColor3 = new Color32(246, 178, 107, 190);

	// Token: 0x0400207B RID: 8315
	public float jumpVelocity = 10f;

	// Token: 0x0400207C RID: 8316
	public float gravity = 10f;
}
