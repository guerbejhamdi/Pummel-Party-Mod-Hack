using System;
using System.Collections;
using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace MalbersAnimations
{
	// Token: 0x020006FD RID: 1789
	[RequireComponent(typeof(Animal))]
	public class AnimalAIControl : MonoBehaviour
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060034B0 RID: 13488 RVA: 0x00023D4A File Offset: 0x00021F4A
		public NavMeshAgent Agent
		{
			get
			{
				if (this.agent == null)
				{
					this.agent = base.GetComponentInChildren<NavMeshAgent>();
				}
				return this.agent;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060034B1 RID: 13489 RVA: 0x00023D6C File Offset: 0x00021F6C
		// (set) Token: 0x060034B2 RID: 13490 RVA: 0x0010EE10 File Offset: 0x0010D010
		public float StoppingDistance
		{
			get
			{
				return this.stoppingDistance;
			}
			set
			{
				NavMeshAgent navMeshAgent = this.Agent;
				this.stoppingDistance = value;
				navMeshAgent.stoppingDistance = value;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060034B3 RID: 13491 RVA: 0x0010EE34 File Offset: 0x0010D034
		public bool TargetisMoving
		{
			get
			{
				if (this.target != null)
				{
					this.targetisMoving = ((this.target.position - this.TargetLastPosition).magnitude > 0.001f);
					return this.targetisMoving;
				}
				this.targetisMoving = false;
				return this.targetisMoving;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060034B4 RID: 13492 RVA: 0x00023D74 File Offset: 0x00021F74
		public bool AgentActive
		{
			get
			{
				return this.Agent.isOnNavMesh && this.Agent.enabled;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060034B5 RID: 13493 RVA: 0x00023D90 File Offset: 0x00021F90
		// (set) Token: 0x060034B6 RID: 13494 RVA: 0x00023D98 File Offset: 0x00021F98
		public bool IsWaiting { get; protected set; }

		// Token: 0x060034B7 RID: 13495 RVA: 0x00023DA1 File Offset: 0x00021FA1
		private void Start()
		{
			this.StartAgent();
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x0010EE90 File Offset: 0x0010D090
		protected virtual void StartAgent()
		{
			this.animal = base.GetComponent<Animal>();
			this.animal.OnAnimationChange.AddListener(new UnityAction<int>(this.OnAnimationChanged));
			this.DoingAnAction = (this.isFlyingOffMesh = false);
			this.Agent.updateRotation = false;
			this.Agent.updatePosition = false;
			this.DefaultStopDistance = this.StoppingDistance;
			this.Agent.stoppingDistance = this.StoppingDistance;
			this.SetTarget(this.target);
			this.IsWaiting = false;
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x00023DA9 File Offset: 0x00021FA9
		private void Update()
		{
			this.Updating();
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x0010EF20 File Offset: 0x0010D120
		protected virtual void Updating()
		{
			if (this.isFlyingOffMesh)
			{
				return;
			}
			if (this.Stopped)
			{
				if (this.TargetisMoving)
				{
					this.Stopped = false;
					this.SetTarget(this.target);
				}
			}
			else if (this.animal.Fly || this.animal.Swim)
			{
				this.FreeMovement();
			}
			else if (this.AgentActive)
			{
				if (this.IsWaiting)
				{
					return;
				}
				if (this.targetPosition == AnimalAIControl.NullVector)
				{
					this.StopAnimal();
				}
				else
				{
					this.UpdateAgent();
				}
			}
			if (this.target)
			{
				if (this.TargetisMoving)
				{
					this.UpdateTargetTransform();
				}
				this.TargetLastPosition = this.target.position;
			}
			this.Agent.nextPosition = this.agent.transform.position;
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x0010EFF8 File Offset: 0x0010D1F8
		private void FreeMovement()
		{
			if (this.IsWaiting)
			{
				return;
			}
			if (this.target == null || this.targetPosition == AnimalAIControl.NullVector)
			{
				return;
			}
			this.RemainingDistance = (this.target ? Vector3.Distance(this.animal.transform.position, this.target.position) : 0f);
			Vector3 move = this.target.position - this.animal.transform.position;
			this.animal.Move(move, true);
			Debug.DrawRay(this.animal.transform.position, move.normalized, Color.white);
			if (this.RemainingDistance < this.StoppingDistance)
			{
				if (this.NextWayPoint != null && this.NextWayPoint.PointType != WayPointType.Air && this.animal.Fly)
				{
					this.animal.SetFly(false);
				}
				this.CheckNextTarget();
			}
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x0010F0FC File Offset: 0x0010D2FC
		private void CheckNextTarget()
		{
			if (this.isActionZone && !this.DoingAnAction)
			{
				this.animal.Action = true;
				this.animal.Stop();
				if (this.isActionZone.MoveToExitAction)
				{
					float waitTime = this.isActionZone.WaitTime;
					this.Debuging(base.name + "is Waiting " + waitTime.ToString() + " seconds to finish a 'Move to Exit' Action");
					this.animal.Invoke("WakeAnimal", waitTime);
					return;
				}
			}
			else
			{
				this.SetNextTarget();
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x0010F188 File Offset: 0x0010D388
		protected virtual void OnAnimationChanged(int animTag)
		{
			bool flag = animTag == AnimTag.Action;
			if (flag != this.DoingAnAction)
			{
				this.DoingAnAction = flag;
				if (this.DoingAnAction)
				{
					this.OnActionStart.Invoke();
					this.Debuging(base.name + " has started an ACTION");
					this.IsWaiting = true;
				}
				else
				{
					this.OnActionEnd.Invoke();
					this.Debuging(base.name + " has ended an ACTION");
					if (!this.EnterOFFMESH)
					{
						this.SetNextTarget();
					}
					else
					{
						this.IsWaiting = false;
					}
				}
			}
			if (animTag == AnimTag.Jump)
			{
				this.animal.MovementRight = 0f;
			}
			if (animTag == AnimTag.Locomotion || animTag == AnimTag.Idle)
			{
				if (this.animal.canFly && this.flyPending && !this.animal.Fly && this.NextWayPoint.PointType == WayPointType.Air)
				{
					this.animal.SetFly(true);
					this.flyPending = false;
					return;
				}
				if (!this.Agent.enabled)
				{
					this.Agent.enabled = true;
					this.Agent.ResetPath();
					this.EnterOFFMESH = false;
					if (this.targetPosition != AnimalAIControl.NullVector)
					{
						this.Agent.SetDestination(this.targetPosition);
						this.Agent.isStopped = false;
						return;
					}
				}
			}
			else if (this.Agent.enabled)
			{
				this.Agent.enabled = false;
				string str = "not on Locomotion or Idle";
				if (animTag == AnimTag.Action)
				{
					str = "doing an Action";
				}
				if (animTag == AnimTag.Jump)
				{
					str = "Jumping";
				}
				if (animTag == AnimTag.Fall)
				{
					str = "Falling";
				}
				if (animTag == AnimTag.Recover)
				{
					str = "Recovering";
				}
				this.Debuging("Disable Agent. " + base.name + " is " + str);
			}
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x0010F35C File Offset: 0x0010D55C
		private void SetNextTarget()
		{
			if (this.WaitToNextTargetC != null)
			{
				base.StopCoroutine(this.WaitToNextTargetC);
			}
			if (this.NextWayPoint != null)
			{
				this.WaitToNextTargetC = this.WaitToNextTarget(this.NextWayPoint.WaitTime, this.NextWayPoint.NextTarget);
				base.StartCoroutine(this.WaitToNextTargetC);
			}
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x0010F3B4 File Offset: 0x0010D5B4
		protected virtual void UpdateAgent()
		{
			Vector3 move = Vector3.zero;
			this.RemainingDistance = this.Agent.remainingDistance;
			if (this.Agent.pathPending || Mathf.Abs(this.RemainingDistance) <= 0.1f)
			{
				this.RemainingDistance = float.PositiveInfinity;
				this.UpdateTargetTransform();
			}
			if (this.RemainingDistance > this.StoppingDistance)
			{
				move = this.Agent.desiredVelocity;
				this.DoingAnAction = false;
			}
			else
			{
				this.OnTargetPositionArrived.Invoke(this.targetPosition);
				if (this.target)
				{
					this.OnTargetArrived.Invoke(this.target);
					if (this.isWayPoint)
					{
						this.isWayPoint.TargetArrived(this);
					}
				}
				this.targetPosition = AnimalAIControl.NullVector;
				this.agent.isStopped = true;
				this.CheckNextTarget();
			}
			this.animal.Move(move, true);
			if (this.AutoSpeed)
			{
				this.AutomaticSpeed();
			}
			this.CheckOffMeshLinks();
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00023DB1 File Offset: 0x00021FB1
		protected virtual void WakeAnimal()
		{
			this.animal.WakeAnimal();
			this.IsWaiting = false;
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x0010F4B4 File Offset: 0x0010D6B4
		protected virtual void CheckOffMeshLinks()
		{
			if (this.Agent.isOnOffMeshLink && !this.EnterOFFMESH)
			{
				this.EnterOFFMESH = true;
				OffMeshLinkData currentOffMeshLinkData = this.Agent.currentOffMeshLinkData;
				if (currentOffMeshLinkData.linkType == OffMeshLinkType.LinkTypeManual)
				{
					OffMeshLink offMeshLink = currentOffMeshLinkData.offMeshLink;
					if (offMeshLink.GetComponentInParent<ActionZone>() && !this.DoingAnAction)
					{
						this.animal.Action = (this.DoingAnAction = true);
						return;
					}
					float sqrMagnitude = (base.transform.position - offMeshLink.endTransform.position).sqrMagnitude;
					float sqrMagnitude2 = (base.transform.position - offMeshLink.startTransform.position).sqrMagnitude;
					Transform transform = (sqrMagnitude < sqrMagnitude2) ? offMeshLink.endTransform : offMeshLink.startTransform;
					Transform transform2 = (sqrMagnitude > sqrMagnitude2) ? offMeshLink.endTransform : offMeshLink.startTransform;
					base.StartCoroutine(MalbersTools.AlignTransform_Rotation(base.transform, transform.rotation, 0.15f, null));
					if (this.animal.canFly && offMeshLink.CompareTag("Fly"))
					{
						this.Debuging(base.name + ": Fly OffMesh");
						base.StartCoroutine(this.CFlyOffMesh(transform2));
						return;
					}
					if (offMeshLink.area == 2)
					{
						this.animal.SetJump();
						return;
					}
				}
				else if (currentOffMeshLinkData.linkType == OffMeshLinkType.LinkTypeJumpAcross)
				{
					this.animal.SetJump();
				}
			}
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x00023DC5 File Offset: 0x00021FC5
		protected virtual IEnumerator WaitToNextTarget(float time, Transform NextTarget)
		{
			if (this.isActionZone && this.isActionZone.MoveToExitAction)
			{
				time = 0f;
			}
			if (time > 0f)
			{
				this.IsWaiting = true;
				this.Debuging(base.name + " is waiting " + time.ToString("F2") + " seconds");
				this.animal.Move(Vector3.zero, true);
				yield return new WaitForSeconds(time);
			}
			this.IsWaiting = false;
			this.SetTarget(NextTarget);
			yield return null;
			yield break;
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x0010F628 File Offset: 0x0010D828
		protected virtual void AutomaticSpeed()
		{
			if (this.RemainingDistance < this.ToTrot)
			{
				this.animal.Speed1 = true;
				return;
			}
			if (this.RemainingDistance < this.ToRun)
			{
				this.animal.Speed2 = true;
				return;
			}
			if (this.RemainingDistance > this.ToRun)
			{
				this.animal.Speed3 = true;
			}
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x0010F688 File Offset: 0x0010D888
		public virtual void SetTarget(Transform target)
		{
			if (target == null)
			{
				this.StopAnimal();
				return;
			}
			this.target = target;
			this.targetPosition = target.position;
			this.isActionZone = target.GetComponent<ActionZone>();
			this.isWayPoint = target.GetComponent<MWayPoint>();
			this.NextWayPoint = target.GetComponent<IWayPoint>();
			this.Stopped = false;
			this.StoppingDistance = ((this.NextWayPoint != null) ? this.NextWayPoint.StoppingDistance : this.DefaultStopDistance);
			this.CheckAirTarget();
			this.Debuging(base.name + " is travelling to : " + target.name);
			if (!this.Agent.isOnNavMesh)
			{
				return;
			}
			this.Agent.enabled = true;
			this.Agent.SetDestination(this.targetPosition);
			this.Agent.isStopped = false;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x0010F760 File Offset: 0x0010D960
		private void CheckAirTarget()
		{
			if (this.NextWayPoint != null && this.NextWayPoint.PointType == WayPointType.Air && this.animal.canFly)
			{
				int currentAnimState = this.animal.CurrentAnimState;
				if (currentAnimState == AnimTag.Locomotion || currentAnimState == AnimTag.Idle)
				{
					this.animal.SetFly(true);
					this.flyPending = false;
					return;
				}
				this.flyPending = true;
			}
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x0010F7C8 File Offset: 0x0010D9C8
		public virtual void UpdateTargetTransform()
		{
			if (!this.Agent.isOnNavMesh)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			this.targetPosition = this.target.position;
			this.Agent.SetDestination(this.targetPosition);
			if (this.Agent.isStopped)
			{
				this.Agent.isStopped = false;
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x0010F830 File Offset: 0x0010DA30
		public virtual void StopAnimal()
		{
			if (this.Agent && this.Agent.isOnNavMesh)
			{
				this.Agent.isStopped = true;
			}
			this.targetPosition = AnimalAIControl.NullVector;
			base.StopAllCoroutines();
			if (this.animal)
			{
				this.animal.Stop();
			}
			this.IsWaiting = (this.isFlyingOffMesh = false);
			this.Stopped = true;
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x0010F8A4 File Offset: 0x0010DAA4
		public virtual void SetDestination(Vector3 point)
		{
			this.targetPosition = point;
			this.target = null;
			this.StoppingDistance = this.DefaultStopDistance;
			if (!this.Agent.isOnNavMesh || !this.Agent.enabled)
			{
				return;
			}
			this.Agent.SetDestination(this.targetPosition);
			this.Agent.isStopped = false;
			this.Stopped = false;
			string name = base.name;
			string str = " is travelling to : ";
			Vector3 vector = point;
			this.Debuging(name + str + vector.ToString());
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x00023DE2 File Offset: 0x00021FE2
		protected void Debuging(string Log)
		{
			if (this.debug)
			{
				Debug.Log(Log);
			}
			this.OnDebug.Invoke(Log);
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x00023DFE File Offset: 0x00021FFE
		internal IEnumerator CFlyOffMesh(Transform target)
		{
			this.animal.SetFly(true);
			this.flyPending = false;
			this.isFlyingOffMesh = true;
			float distance = float.MaxValue;
			this.agent.enabled = false;
			while (distance > this.agent.stoppingDistance)
			{
				this.animal.Move(target.position - this.animal.transform.position, true);
				distance = Vector3.Distance(this.animal.transform.position, target.position);
				yield return null;
			}
			this.animal.Stop();
			this.animal.SetFly(false);
			this.isFlyingOffMesh = false;
			yield break;
		}

		// Token: 0x04003316 RID: 13078
		private NavMeshAgent agent;

		// Token: 0x04003317 RID: 13079
		protected Animal animal;

		// Token: 0x04003318 RID: 13080
		protected ActionZone isActionZone;

		// Token: 0x04003319 RID: 13081
		protected MWayPoint isWayPoint;

		// Token: 0x0400331A RID: 13082
		protected static Vector3 NullVector = MalbersTools.NullVector;

		// Token: 0x0400331B RID: 13083
		protected Vector3 targetPosition = AnimalAIControl.NullVector;

		// Token: 0x0400331C RID: 13084
		protected Vector3 TargetLastPosition = AnimalAIControl.NullVector;

		// Token: 0x0400331D RID: 13085
		protected float RemainingDistance;

		// Token: 0x0400331E RID: 13086
		protected float DefaultStopDistance;

		// Token: 0x0400331F RID: 13087
		protected bool EnterOFFMESH;

		// Token: 0x04003320 RID: 13088
		protected bool DoingAnAction;

		// Token: 0x04003321 RID: 13089
		protected bool EnterAction;

		// Token: 0x04003322 RID: 13090
		protected bool Stopped;

		// Token: 0x04003323 RID: 13091
		private bool isFlyingOffMesh;

		// Token: 0x04003324 RID: 13092
		internal IWayPoint NextWayPoint;

		// Token: 0x04003325 RID: 13093
		protected bool flyPending;

		// Token: 0x04003326 RID: 13094
		[SerializeField]
		protected float stoppingDistance = 0.6f;

		// Token: 0x04003327 RID: 13095
		[SerializeField]
		protected Transform target;

		// Token: 0x04003328 RID: 13096
		public bool AutoSpeed = true;

		// Token: 0x04003329 RID: 13097
		public float ToTrot = 6f;

		// Token: 0x0400332A RID: 13098
		public float ToRun = 8f;

		// Token: 0x0400332B RID: 13099
		public bool debug;

		// Token: 0x0400332C RID: 13100
		[Space]
		public Vector3Event OnTargetPositionArrived = new Vector3Event();

		// Token: 0x0400332D RID: 13101
		public TransformEvent OnTargetArrived = new TransformEvent();

		// Token: 0x0400332E RID: 13102
		public UnityEvent OnActionStart = new UnityEvent();

		// Token: 0x0400332F RID: 13103
		public UnityEvent OnActionEnd = new UnityEvent();

		// Token: 0x04003330 RID: 13104
		public StringEvent OnDebug = new StringEvent();

		// Token: 0x04003331 RID: 13105
		protected bool targetisMoving;

		// Token: 0x04003333 RID: 13107
		private IEnumerator WaitToNextTargetC;

		// Token: 0x04003334 RID: 13108
		[HideInInspector]
		public bool showevents;
	}
}
