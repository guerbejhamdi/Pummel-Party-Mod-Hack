using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
[Serializable]
public class LaserLeapLaserConfig
{
	// Token: 0x04000C67 RID: 3175
	[Header("Start Transform")]
	public Vector3 position;

	// Token: 0x04000C68 RID: 3176
	public Vector3 rotation;

	// Token: 0x04000C69 RID: 3177
	[Header("Time")]
	public float time;

	// Token: 0x04000C6A RID: 3178
	[Header("Movement")]
	public LaserLeapMovement movementType;

	// Token: 0x04000C6B RID: 3179
	public Vector3 totalMovement;

	// Token: 0x04000C6C RID: 3180
	public AnimationCurve movementCurve;

	// Token: 0x04000C6D RID: 3181
	public LaserLeapRotation rotationType;

	// Token: 0x04000C6E RID: 3182
	public Vector3 totalRotation;

	// Token: 0x04000C6F RID: 3183
	public AnimationCurve rotationCurve;

	// Token: 0x04000C70 RID: 3184
	[Header("Circle")]
	public float circularMovementDistance;

	// Token: 0x04000C71 RID: 3185
	public float circularTimeOffset;

	// Token: 0x04000C72 RID: 3186
	[Header("Orientation")]
	public LaserLeapTarget targetType;

	// Token: 0x04000C73 RID: 3187
	public Vector3 targetOffset;
}
