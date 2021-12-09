using System;

namespace MalbersAnimations
{
	// Token: 0x020006FB RID: 1787
	[Serializable]
	public struct Speeds
	{
		// Token: 0x060034AD RID: 13485 RVA: 0x0010ED64 File Offset: 0x0010CF64
		public Speeds(int defaultt)
		{
			this.position = 0f;
			this.animator = 1f;
			this.lerpPosition = 2f;
			this.lerpAnimator = 2f;
			this.rotation = 0f;
			this.lerpRotation = 2f;
			this.name = string.Empty;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x0010EDC0 File Offset: 0x0010CFC0
		public Speeds(float lerpPos, float lerpanim, float lerpTurn)
		{
			this.position = 0f;
			this.animator = 1f;
			this.rotation = 0f;
			this.lerpPosition = lerpPos;
			this.lerpAnimator = lerpanim;
			this.lerpRotation = lerpTurn;
			this.name = string.Empty;
		}

		// Token: 0x0400330F RID: 13071
		public string name;

		// Token: 0x04003310 RID: 13072
		public float position;

		// Token: 0x04003311 RID: 13073
		public float animator;

		// Token: 0x04003312 RID: 13074
		public float lerpPosition;

		// Token: 0x04003313 RID: 13075
		public float lerpAnimator;

		// Token: 0x04003314 RID: 13076
		public float rotation;

		// Token: 0x04003315 RID: 13077
		public float lerpRotation;
	}
}
