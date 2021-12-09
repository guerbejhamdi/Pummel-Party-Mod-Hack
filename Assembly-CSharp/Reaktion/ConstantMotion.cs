using System;
using UnityEngine;

namespace Reaktion
{
	// Token: 0x020005A4 RID: 1444
	public class ConstantMotion : MonoBehaviour
	{
		// Token: 0x06002591 RID: 9617 RVA: 0x0001AE01 File Offset: 0x00019001
		private void Awake()
		{
			this.position.Initialize();
			this.rotation.Initialize();
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000E2FD4 File Offset: 0x000E11D4
		private void Update()
		{
			if (this.position.mode != ConstantMotion.TransformMode.Off)
			{
				if (this.useLocalCoordinate)
				{
					base.transform.localPosition += this.position.Vector * this.position.Delta;
				}
				else
				{
					base.transform.position += this.position.Vector * this.position.Delta;
				}
			}
			if (this.rotation.mode != ConstantMotion.TransformMode.Off)
			{
				Quaternion lhs = Quaternion.AngleAxis(this.rotation.Delta, this.rotation.Vector);
				if (this.useLocalCoordinate)
				{
					base.transform.localRotation = lhs * base.transform.localRotation;
					return;
				}
				base.transform.rotation = lhs * base.transform.rotation;
			}
		}

		// Token: 0x04002900 RID: 10496
		public ConstantMotion.TransformElement position = new ConstantMotion.TransformElement();

		// Token: 0x04002901 RID: 10497
		public ConstantMotion.TransformElement rotation = new ConstantMotion.TransformElement
		{
			velocity = 30f
		};

		// Token: 0x04002902 RID: 10498
		public bool useLocalCoordinate = true;

		// Token: 0x020005A5 RID: 1445
		public enum TransformMode
		{
			// Token: 0x04002904 RID: 10500
			Off,
			// Token: 0x04002905 RID: 10501
			XAxis,
			// Token: 0x04002906 RID: 10502
			YAxis,
			// Token: 0x04002907 RID: 10503
			ZAxis,
			// Token: 0x04002908 RID: 10504
			Arbitrary,
			// Token: 0x04002909 RID: 10505
			Random
		}

		// Token: 0x020005A6 RID: 1446
		[Serializable]
		public class TransformElement
		{
			// Token: 0x06002594 RID: 9620 RVA: 0x0001AE49 File Offset: 0x00019049
			public void Initialize()
			{
				this.randomVector = UnityEngine.Random.onUnitSphere;
				this.randomScalar = UnityEngine.Random.value;
			}

			// Token: 0x17000471 RID: 1137
			// (get) Token: 0x06002595 RID: 9621 RVA: 0x000E30C4 File Offset: 0x000E12C4
			public Vector3 Vector
			{
				get
				{
					switch (this.mode)
					{
					case ConstantMotion.TransformMode.XAxis:
						return Vector3.right;
					case ConstantMotion.TransformMode.YAxis:
						return Vector3.up;
					case ConstantMotion.TransformMode.ZAxis:
						return Vector3.forward;
					case ConstantMotion.TransformMode.Arbitrary:
						return this.arbitraryVector;
					case ConstantMotion.TransformMode.Random:
						return this.randomVector;
					default:
						return Vector3.zero;
					}
				}
			}

			// Token: 0x17000472 RID: 1138
			// (get) Token: 0x06002596 RID: 9622 RVA: 0x000E311C File Offset: 0x000E131C
			public float Delta
			{
				get
				{
					float num = 1f - this.randomness * this.randomScalar;
					return this.velocity * num * Time.deltaTime;
				}
			}

			// Token: 0x0400290A RID: 10506
			public ConstantMotion.TransformMode mode;

			// Token: 0x0400290B RID: 10507
			public float velocity = 1f;

			// Token: 0x0400290C RID: 10508
			public Vector3 arbitraryVector = Vector3.up;

			// Token: 0x0400290D RID: 10509
			public float randomness;

			// Token: 0x0400290E RID: 10510
			private Vector3 randomVector;

			// Token: 0x0400290F RID: 10511
			private float randomScalar;
		}
	}
}
