using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000222 RID: 546
public class RollyRagdollAnim : MonoBehaviour
{
	// Token: 0x06000FE3 RID: 4067 RVA: 0x0000D8DE File Offset: 0x0000BADE
	public void Setup(System.Random rand)
	{
		this.speed = ZPMath.RandomFloat(rand, this.minSpeed, this.maxSpeed);
		if (this.randomNegative && rand.NextDouble() > 0.5)
		{
			this.speed = -this.speed;
		}
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x0000D91E File Offset: 0x0000BB1E
	private void Start()
	{
		this.startPosition = base.transform.localPosition;
		this.startRot = base.transform.localRotation;
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x0007DC64 File Offset: 0x0007BE64
	private void FixedUpdate()
	{
		this.minigameController = (RollyRagdollsController)GameManager.Minigame;
		if (this.minigameController == null)
		{
			return;
		}
		float currentTime = this.minigameController.CurrentTime;
		switch (this.animType)
		{
		case RollyRagdollAnim.AnimType.Rotation:
		case RollyRagdollAnim.AnimType.Wheel:
			base.transform.localRotation = Quaternion.Euler(this.startRot.eulerAngles + this.dirAxis * currentTime * this.speed);
			return;
		case RollyRagdollAnim.AnimType.Position:
		{
			float time = Mathf.PingPong(currentTime, this.speed) / this.speed;
			base.transform.localPosition = this.startPosition + this.dirAxis * this.offset * this.animCurve.Evaluate(time);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x04001011 RID: 4113
	public RollyRagdollAnim.AnimType animType = RollyRagdollAnim.AnimType.Position;

	// Token: 0x04001012 RID: 4114
	public Vector3 dirAxis = Vector3.up;

	// Token: 0x04001013 RID: 4115
	public float minSpeed = 1f;

	// Token: 0x04001014 RID: 4116
	public float maxSpeed = 1f;

	// Token: 0x04001015 RID: 4117
	public bool randomNegative;

	// Token: 0x04001016 RID: 4118
	private float speed = 1f;

	// Token: 0x04001017 RID: 4119
	[Header("Position")]
	public float offset = 1f;

	// Token: 0x04001018 RID: 4120
	public AnimationCurve animCurve;

	// Token: 0x04001019 RID: 4121
	private Vector3 startPosition;

	// Token: 0x0400101A RID: 4122
	private Quaternion startRot;

	// Token: 0x0400101B RID: 4123
	private RollyRagdollsController minigameController;

	// Token: 0x02000223 RID: 547
	public enum AnimType
	{
		// Token: 0x0400101D RID: 4125
		Rotation,
		// Token: 0x0400101E RID: 4126
		Position,
		// Token: 0x0400101F RID: 4127
		Wheel
	}
}
