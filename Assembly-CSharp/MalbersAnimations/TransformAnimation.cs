using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200075C RID: 1884
	[CreateAssetMenu(menuName = "Malbers Animations/Anim Transform")]
	public class TransformAnimation : ScriptableObject
	{
		// Token: 0x040035A7 RID: 13735
		public TransformAnimation.AnimTransType animTrans;

		// Token: 0x040035A8 RID: 13736
		private static Keyframe[] K = new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		};

		// Token: 0x040035A9 RID: 13737
		public float time = 1f;

		// Token: 0x040035AA RID: 13738
		public float delay = 1f;

		// Token: 0x040035AB RID: 13739
		public bool UsePosition;

		// Token: 0x040035AC RID: 13740
		public Vector3 Position;

		// Token: 0x040035AD RID: 13741
		public AnimationCurve PosCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035AE RID: 13742
		public bool SeparateAxisPos;

		// Token: 0x040035AF RID: 13743
		public AnimationCurve PosXCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035B0 RID: 13744
		public AnimationCurve PosYCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035B1 RID: 13745
		public AnimationCurve PosZCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035B2 RID: 13746
		public bool UseRotation;

		// Token: 0x040035B3 RID: 13747
		public Vector3 Rotation;

		// Token: 0x040035B4 RID: 13748
		public AnimationCurve RotCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035B5 RID: 13749
		public bool SeparateAxisRot;

		// Token: 0x040035B6 RID: 13750
		public AnimationCurve RotXCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035B7 RID: 13751
		public AnimationCurve RotYCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035B8 RID: 13752
		public AnimationCurve RotZCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x040035B9 RID: 13753
		public bool UseScale;

		// Token: 0x040035BA RID: 13754
		public Vector3 Scale = Vector3.one;

		// Token: 0x040035BB RID: 13755
		public AnimationCurve ScaleCurve = new AnimationCurve(TransformAnimation.K);

		// Token: 0x0200075D RID: 1885
		public enum AnimTransType
		{
			// Token: 0x040035BD RID: 13757
			TransformAnimation,
			// Token: 0x040035BE RID: 13758
			MountTriggerAdjustment
		}
	}
}
