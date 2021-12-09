using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200079B RID: 1947
	public class LookAt : MonoBehaviour, IAnimatorListener
	{
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x00025A88 File Offset: 0x00023C88
		// (set) Token: 0x06003752 RID: 14162 RVA: 0x00025A7F File Offset: 0x00023C7F
		public Vector3 Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x00025A90 File Offset: 0x00023C90
		public bool IsAiming
		{
			get
			{
				return this.angle < this.LimitAngle && this.Active && this.AnimationActive && this.hasTarget;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x00025AB8 File Offset: 0x00023CB8
		// (set) Token: 0x06003756 RID: 14166 RVA: 0x00025AC0 File Offset: 0x00023CC0
		public RaycastHit AimHit
		{
			get
			{
				return this.aimHit;
			}
			set
			{
				this.aimHit = value;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003757 RID: 14167 RVA: 0x00025AC9 File Offset: 0x00023CC9
		// (set) Token: 0x06003758 RID: 14168 RVA: 0x00025AD1 File Offset: 0x00023CD1
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x00118A80 File Offset: 0x00116C80
		private void Awake()
		{
			if (Camera.main != null)
			{
				this.cam = Camera.main.transform;
			}
			Animator component = base.GetComponent<Animator>();
			this.AnimatorOnAnimatePhysics = (component && component.updateMode == AnimatorUpdateMode.AnimatePhysics);
			if (this.AnimatorOnAnimatePhysics)
			{
				return;
			}
			foreach (BoneRotation boneRotation in this.Bones)
			{
				boneRotation.initialRotation = boneRotation.bone.transform.localRotation;
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x00118B04 File Offset: 0x00116D04
		private void LateUpdate()
		{
			foreach (BoneRotation boneRotation in this.Bones)
			{
				boneRotation.initialRotation = boneRotation.bone.transform.localRotation;
			}
			this.LookAtBoneSet();
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x00025ADA File Offset: 0x00023CDA
		public void EnableLookAt(bool value)
		{
			this.AnimationActive = value;
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x00118B44 File Offset: 0x00116D44
		private void LookAtBoneSet()
		{
			if (!this.Target && !this.cam)
			{
				return;
			}
			this.hasTarget = false;
			if (this.UseCamera || this.Target)
			{
				this.hasTarget = true;
			}
			this.angle = Vector3.Angle(base.transform.forward, this.Direction);
			this.currentSmoothness = Mathf.Lerp(this.currentSmoothness, (float)(this.IsAiming ? 1 : 0), Time.deltaTime * this.Smoothness);
			if (this.currentSmoothness > 0.9999f)
			{
				this.currentSmoothness = 1f;
			}
			if (this.currentSmoothness < 0.0001f)
			{
				this.currentSmoothness = 0f;
			}
			for (int i = 0; i < this.Bones.Length; i++)
			{
				BoneRotation boneRotation = this.Bones[i];
				if (boneRotation.bone)
				{
					Vector3 b = base.transform.forward;
					if (this.UseCamera && this.cam)
					{
						Vector2 v = new Vector2(this.CameraCenter.x * (float)Screen.width, this.CameraCenter.y * (float)Screen.height);
						b = this.cam.forward;
						b = MalbersTools.DirectionFromCamera(boneRotation.bone, v, out this.aimHit, ~this.Ignore);
						if (this.aimHit.collider)
						{
							b = MalbersTools.DirectionTarget(boneRotation.bone.position, this.aimHit.point, true);
						}
					}
					if (this.Target)
					{
						b = MalbersTools.DirectionTarget(boneRotation.bone, this.Target, true);
					}
					this.Direction = Vector3.Lerp(this.Direction, b, Time.deltaTime * this.Smoothness);
					if (this.currentSmoothness == 0f)
					{
						return;
					}
					if (this.debug && i == this.Bones.Length - 1)
					{
						Debug.DrawRay(boneRotation.bone.position, this.Direction * 15f, Color.green);
					}
					Quaternion b2 = Quaternion.LookRotation(this.Direction, this.UpVector) * Quaternion.Euler(boneRotation.offset);
					Quaternion rotation = Quaternion.Lerp(boneRotation.bone.rotation, b2, boneRotation.weight * this.currentSmoothness);
					boneRotation.bone.rotation = rotation;
				}
			}
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x00025AE3 File Offset: 0x00023CE3
		public virtual void NoTarget()
		{
			this.Target = null;
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x0002339D File Offset: 0x0002159D
		public virtual void OnAnimatorBehaviourMessage(string message, object value)
		{
			this.InvokeWithParams(message, value);
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x00025AEC File Offset: 0x00023CEC
		private void OnValidate()
		{
			this.CameraCenter = new Vector2(Mathf.Clamp01(this.CameraCenter.x), Mathf.Clamp01(this.CameraCenter.y));
		}

		// Token: 0x04003665 RID: 13925
		[Tooltip("Global LookAt Activation")]
		[SerializeField]
		private bool active = true;

		// Token: 0x04003666 RID: 13926
		[Tooltip("The Animations allows the LookAt to be enable/disabled")]
		public bool AnimationActive = true;

		// Token: 0x04003667 RID: 13927
		[Space]
		[Tooltip("What layers the Look At Rays should ignore")]
		public LayerMask Ignore = 4;

		// Token: 0x04003668 RID: 13928
		public bool UseCamera;

		// Token: 0x04003669 RID: 13929
		[Tooltip("What point of the Camera it will cast the Ray")]
		public Vector2 CameraCenter = new Vector2(0.5f, 0.5f);

		// Token: 0x0400366A RID: 13930
		public Transform Target;

		// Token: 0x0400366B RID: 13931
		[Space]
		public float LimitAngle = 80f;

		// Token: 0x0400366C RID: 13932
		public float Smoothness = 5f;

		// Token: 0x0400366D RID: 13933
		public Vector3 UpVector = Vector3.up;

		// Token: 0x0400366E RID: 13934
		private float currentSmoothness;

		// Token: 0x0400366F RID: 13935
		[Space]
		public BoneRotation[] Bones;

		// Token: 0x04003670 RID: 13936
		private Transform cam;

		// Token: 0x04003671 RID: 13937
		protected float angle;

		// Token: 0x04003672 RID: 13938
		protected Vector3 direction;

		// Token: 0x04003673 RID: 13939
		public bool debug = true;

		// Token: 0x04003674 RID: 13940
		private bool hasTarget;

		// Token: 0x04003675 RID: 13941
		private RaycastHit aimHit;

		// Token: 0x04003676 RID: 13942
		private bool AnimatorOnAnimatePhysics;
	}
}
