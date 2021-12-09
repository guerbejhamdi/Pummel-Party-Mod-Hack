using System;
using System.Collections;
using System.Collections.Generic;
using MalbersAnimations.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations
{
	// Token: 0x020006E9 RID: 1769
	[RequireComponent(typeof(BoxCollider))]
	public class ActionZone : MonoBehaviour, IWayPoint
	{
		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003387 RID: 13191 RVA: 0x00023205 File Offset: 0x00021405
		// (set) Token: 0x06003388 RID: 13192 RVA: 0x0002320D File Offset: 0x0002140D
		public List<Transform> NextTargets
		{
			get
			{
				return this.nextTargets;
			}
			set
			{
				this.nextTargets = value;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003389 RID: 13193 RVA: 0x00023216 File Offset: 0x00021416
		public Transform NextTarget
		{
			get
			{
				if (this.NextTargets.Count > 0)
				{
					return this.NextTargets[UnityEngine.Random.Range(0, this.NextTargets.Count)];
				}
				return null;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x0600338A RID: 13194 RVA: 0x00023244 File Offset: 0x00021444
		public WayPointType PointType
		{
			get
			{
				return this.pointType;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x0600338B RID: 13195 RVA: 0x0002324C File Offset: 0x0002144C
		public float WaitTime
		{
			get
			{
				return this.waitTime.RandomValue;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x0600338C RID: 13196 RVA: 0x00023259 File Offset: 0x00021459
		// (set) Token: 0x0600338D RID: 13197 RVA: 0x00023261 File Offset: 0x00021461
		public float StoppingDistance
		{
			get
			{
				return this.stoppingDistance;
			}
			set
			{
				this.stoppingDistance = value;
			}
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x0010B334 File Offset: 0x00109534
		private void OnTriggerEnter(Collider other)
		{
			if (!MalbersTools.CollidersLayer(other, LayerMask.GetMask(new string[]
			{
				"Animal"
			})))
			{
				return;
			}
			if (this.HeadOnly && !other.name.ToLower().Contains("head"))
			{
				return;
			}
			Animal componentInParent = other.GetComponentInParent<Animal>();
			if (!componentInParent)
			{
				return;
			}
			componentInParent.ActionID = this.ID;
			if (this.animal_Colliders.Find((Collider coll) => coll == other) == null)
			{
				this.animal_Colliders.Add(other);
			}
			if (componentInParent == this.oldAnimal)
			{
				return;
			}
			if (this.oldAnimal)
			{
				this.oldAnimal.ActionID = -1;
				this.animal_Colliders = new List<Collider>();
			}
			this.oldAnimal = componentInParent;
			componentInParent.OnAction.AddListener(new UnityAction(this.OnActionListener));
			this.OnEnter.Invoke(componentInParent);
			if (this.automatic)
			{
				if (componentInParent.AnimState == AnimTag.Jump && !this.ActiveOnJump)
				{
					return;
				}
				componentInParent.SetAction(this.ID);
				base.StartCoroutine(this.ReEnable(componentInParent));
			}
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x0010B484 File Offset: 0x00109684
		private void OnTriggerExit(Collider other)
		{
			if (this.HeadOnly && !other.name.ToLower().Contains("head"))
			{
				return;
			}
			Animal componentInParent = other.GetComponentInParent<Animal>();
			if (!componentInParent)
			{
				return;
			}
			if (componentInParent != this.oldAnimal)
			{
				return;
			}
			if (this.animal_Colliders.Find((Collider item) => item == other))
			{
				this.animal_Colliders.Remove(other);
			}
			if (this.animal_Colliders.Count == 0)
			{
				this.OnExit.Invoke(this.oldAnimal);
				if (this.oldAnimal.ActionID == this.ID)
				{
					this.oldAnimal.ActionID = -1;
				}
				this.oldAnimal = null;
			}
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x0002326A File Offset: 0x0002146A
		private IEnumerator ReEnable(Animal animal)
		{
			if (this.AutomaticDisabled > 0f)
			{
				this.ZoneCollider.enabled = false;
				yield return null;
				yield return null;
				animal.ActionID = -1;
				yield return new WaitForSeconds(this.AutomaticDisabled);
				this.ZoneCollider.enabled = true;
			}
			this.oldAnimal = null;
			this.animal_Colliders = new List<Collider>();
			yield return null;
			yield break;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x00023280 File Offset: 0x00021480
		public virtual void _DestroyActionZone(float time)
		{
			UnityEngine.Object.Destroy(base.gameObject, time);
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0010B560 File Offset: 0x00109760
		private void OnActionListener()
		{
			if (!this.oldAnimal)
			{
				return;
			}
			base.StartCoroutine(this.OnActionDelay(this.ActionDelay, this.oldAnimal));
			if (this.Align && this.AlingPoint)
			{
				Vector3 newPosition = this.AlingPoint.position;
				if (this.AlingPoint2)
				{
					newPosition = MalbersTools.ClosestPointOnLine(this.AlingPoint.position, this.AlingPoint2.position, this.oldAnimal.transform.position);
				}
				if (this.AlignLookAt)
				{
					IEnumerator routine = MalbersTools.AlignLookAtTransform(this.oldAnimal.transform, this.AlingPoint, this.AlignTime, this.AlignCurve);
					base.StartCoroutine(routine);
				}
				else
				{
					if (this.AlignPos)
					{
						base.StartCoroutine(MalbersTools.AlignTransform_Position(this.oldAnimal.transform, newPosition, this.AlignTime, this.AlignCurve));
					}
					if (this.AlignRot)
					{
						base.StartCoroutine(MalbersTools.AlignTransform_Rotation(this.oldAnimal.transform, this.AlingPoint.rotation, this.AlignTime, this.AlignCurve));
					}
				}
			}
			base.StartCoroutine(this.CheckForCollidersOff());
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x0002328E File Offset: 0x0002148E
		private IEnumerator OnActionDelay(float time, Animal animal)
		{
			if (time > 0f)
			{
				yield return new WaitForSeconds(time);
			}
			this.OnAction.Invoke(animal);
			yield return null;
			yield break;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x000232AB File Offset: 0x000214AB
		private IEnumerator CheckForCollidersOff()
		{
			yield return null;
			yield return null;
			if (this.oldAnimal && !this.oldAnimal.ActiveColliders)
			{
				this.oldAnimal.OnAction.RemoveListener(new UnityAction(this.OnActionListener));
				this.oldAnimal.ActionID = -1;
				this.oldAnimal = null;
				this.animal_Colliders = new List<Collider>();
			}
			yield break;
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x000232BA File Offset: 0x000214BA
		public virtual void _WakeAnimal(Animal animal)
		{
			if (animal)
			{
				animal.MovementAxis = Vector3.forward * 3f;
			}
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000232D9 File Offset: 0x000214D9
		private void OnEnable()
		{
			if (ActionZone.ActionZones == null)
			{
				ActionZone.ActionZones = new List<ActionZone>();
			}
			this.ZoneCollider = base.GetComponent<Collider>();
			ActionZone.ActionZones.Add(this);
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x0010B69C File Offset: 0x0010989C
		private void OnDisable()
		{
			ActionZone.ActionZones.Remove(this);
			if (this.oldAnimal)
			{
				this.oldAnimal.OnAction.RemoveListener(new UnityAction(this.OnActionListener));
				this.oldAnimal.ActionID = -1;
			}
		}

		// Token: 0x040031D8 RID: 12760
		private static Keyframe[] K = new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		};

		// Token: 0x040031D9 RID: 12761
		public Action ID;

		// Token: 0x040031DA RID: 12762
		public bool automatic;

		// Token: 0x040031DB RID: 12763
		public int index;

		// Token: 0x040031DC RID: 12764
		public float AutomaticDisabled = 10f;

		// Token: 0x040031DD RID: 12765
		public bool HeadOnly;

		// Token: 0x040031DE RID: 12766
		public bool ActiveOnJump;

		// Token: 0x040031DF RID: 12767
		public bool MoveToExitAction;

		// Token: 0x040031E0 RID: 12768
		public bool Align;

		// Token: 0x040031E1 RID: 12769
		public bool AlignWithWidth;

		// Token: 0x040031E2 RID: 12770
		public Transform AlingPoint;

		// Token: 0x040031E3 RID: 12771
		public Transform AlingPoint2;

		// Token: 0x040031E4 RID: 12772
		public float AlignTime = 0.5f;

		// Token: 0x040031E5 RID: 12773
		public AnimationCurve AlignCurve = new AnimationCurve(ActionZone.K);

		// Token: 0x040031E6 RID: 12774
		public bool AlignPos = true;

		// Token: 0x040031E7 RID: 12775
		public bool AlignRot = true;

		// Token: 0x040031E8 RID: 12776
		public bool AlignLookAt;

		// Token: 0x040031E9 RID: 12777
		protected List<Collider> animal_Colliders = new List<Collider>();

		// Token: 0x040031EA RID: 12778
		protected Animal oldAnimal;

		// Token: 0x040031EB RID: 12779
		public float ActionDelay;

		// Token: 0x040031EC RID: 12780
		public AnimalEvent OnEnter = new AnimalEvent();

		// Token: 0x040031ED RID: 12781
		public AnimalEvent OnExit = new AnimalEvent();

		// Token: 0x040031EE RID: 12782
		public AnimalEvent OnAction = new AnimalEvent();

		// Token: 0x040031EF RID: 12783
		[MinMaxRange(0f, 60f)]
		[SerializeField]
		private RangedFloat waitTime = new RangedFloat(0f, 5f);

		// Token: 0x040031F0 RID: 12784
		public WayPointType pointType;

		// Token: 0x040031F1 RID: 12785
		public static List<ActionZone> ActionZones;

		// Token: 0x040031F2 RID: 12786
		[SerializeField]
		private List<Transform> nextTargets;

		// Token: 0x040031F3 RID: 12787
		[SerializeField]
		private float stoppingDistance = 0.5f;

		// Token: 0x040031F4 RID: 12788
		private Collider ZoneCollider;

		// Token: 0x040031F5 RID: 12789
		[HideInInspector]
		public bool EditorShowEvents = true;

		// Token: 0x040031F6 RID: 12790
		[HideInInspector]
		public bool EditorAI = true;
	}
}
