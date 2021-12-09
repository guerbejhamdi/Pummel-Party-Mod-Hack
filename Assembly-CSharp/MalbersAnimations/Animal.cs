using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MalbersAnimations.Events;
using MalbersAnimations.Scriptables;
using MalbersAnimations.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations
{
	// Token: 0x020006EF RID: 1775
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody))]
	public class Animal : MonoBehaviour, IAnimatorListener, IMDamagable, IMCharacter, ICharacterMove
	{
		// Token: 0x060033B0 RID: 13232 RVA: 0x0002339D File Offset: 0x0002159D
		public virtual void OnAnimatorBehaviourMessage(string message, object value)
		{
			this.InvokeWithParams(message, value);
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000233A7 File Offset: 0x000215A7
		public virtual void WakeAnimal()
		{
			this.MovementAxis = Vector3.forward * 3f;
			this.ActionID = -2;
			this.iswakingUp = true;
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000233CD File Offset: 0x000215CD
		public virtual void ToggleStance(int NewStance)
		{
			this.Stance = ((this.Stance == NewStance) ? 0 : NewStance);
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x0010B9C4 File Offset: 0x00109BC4
		public virtual void ResetInputs()
		{
			this.Attack1 = false;
			this.Attack2 = false;
			this.Shift = false;
			this.Jump = false;
			this.Action = false;
			this.ActionID = 0;
			this.MovementAxis = Vector3.zero;
			this.RawDirection = Vector3.zero;
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x000233E2 File Offset: 0x000215E2
		public virtual void ToggleStance(IntVar NewStance)
		{
			this.Stance = ((this.Stance == NewStance) ? 0 : NewStance);
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x0010BA14 File Offset: 0x00109C14
		public virtual void ToggleTurnSpeed(float speeds)
		{
			if (this.walkSpeed.rotation != speeds)
			{
				this.runSpeed.rotation = speeds;
				this.trotSpeed.rotation = speeds;
				this.walkSpeed.rotation = speeds;
				return;
			}
			this.walkSpeed.rotation = (this.trotSpeed.rotation = (this.runSpeed.rotation = 0f));
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x00023401 File Offset: 0x00021601
		public virtual void InterruptAction()
		{
			this.MovementAxis = Vector3.forward * 3f;
			this.ActionID = -2;
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x0010BA84 File Offset: 0x00109C84
		public virtual void getDamaged(DamageValues DV)
		{
			if (this.Death)
			{
				return;
			}
			if (this.isTakingDamage)
			{
				return;
			}
			if (this.inmune)
			{
				return;
			}
			float num = DV.Amount - this.defense;
			this.OnGetDamaged.Invoke(num);
			this.life -= num;
			this.ActionID = -2;
			if (this.life > 0f)
			{
				this.damaged = true;
				base.StartCoroutine(this.IsTakingDamageTime(this.damageDelay));
				this._hitDirection = DV.Direction;
				return;
			}
			this.Death = true;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x00023420 File Offset: 0x00021620
		public virtual void Stop()
		{
			this.movementAxis = Vector3.zero;
			this.RawDirection = Vector3.zero;
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x0010BB18 File Offset: 0x00109D18
		public virtual void getDamaged(Vector3 Mycenter, Vector3 Theircenter, float Amount = 0f)
		{
			DamageValues dv = new DamageValues(Mycenter - Theircenter, Amount);
			this.getDamaged(dv);
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x00023438 File Offset: 0x00021638
		private IEnumerator IsTakingDamageTime(float time)
		{
			this.isTakingDamage = true;
			yield return new WaitForSeconds(time);
			this.isTakingDamage = false;
			yield break;
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x0010BB3C File Offset: 0x00109D3C
		public virtual void AttackTrigger(int triggerIndex)
		{
			if (triggerIndex == -1)
			{
				foreach (AttackTrigger attackTrigger in this.Attack_Triggers)
				{
					attackTrigger.Collider.enabled = true;
					attackTrigger.gameObject.SetActive(true);
				}
				return;
			}
			if (triggerIndex == 0)
			{
				foreach (AttackTrigger attackTrigger2 in this.Attack_Triggers)
				{
					attackTrigger2.Collider.enabled = false;
					attackTrigger2.gameObject.SetActive(false);
				}
				return;
			}
			List<AttackTrigger> list = this.Attack_Triggers.FindAll((AttackTrigger item) => item.index == triggerIndex);
			if (list != null)
			{
				foreach (AttackTrigger attackTrigger3 in list)
				{
					attackTrigger3.Collider.enabled = true;
					attackTrigger3.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x0002344E File Offset: 0x0002164E
		public virtual void SetAttack()
		{
			this.activeAttack = -1;
			this.Attack1 = true;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x0002345E File Offset: 0x0002165E
		public virtual void SetLoop(int cycles)
		{
			this.Loops = cycles;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x00023467 File Offset: 0x00021667
		public virtual void SetAttack(int attackID)
		{
			this.activeAttack = attackID;
			this.Attack1 = true;
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x00023477 File Offset: 0x00021677
		public virtual void SetAttack(bool value)
		{
			this.Attack1 = value;
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x00023480 File Offset: 0x00021680
		public virtual void SetSecondaryAttack()
		{
			if (this.hasAttack2)
			{
				base.StartCoroutine(this.ToogleAttack2());
			}
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x00023497 File Offset: 0x00021697
		public virtual void RigidDrag(float amount)
		{
			this._RigidBody.drag = amount;
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000234A5 File Offset: 0x000216A5
		private IEnumerator ToogleAttack2()
		{
			int num;
			for (int i = 0; i < this.ToogleAmount; i = num + 1)
			{
				this.Attack2 = true;
				yield return null;
				num = i;
			}
			this.Attack2 = false;
			yield break;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x0010BC78 File Offset: 0x00109E78
		public virtual bool CurrentAnimation(params int[] AnimsTags)
		{
			for (int i = 0; i < AnimsTags.Length; i++)
			{
				if (this.AnimState == AnimsTags[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000234B4 File Offset: 0x000216B4
		public void SetIntID(int value)
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.IDInt = value;
				this.Anim.SetInteger(this.hash_IDInt, this.IDInt);
			}
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x000234E1 File Offset: 0x000216E1
		public void SetFloatID(float value)
		{
			this.IDFloat = value;
			this.Anim.SetFloat(this.hash_IDFloat, this.IDFloat);
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x00023501 File Offset: 0x00021701
		protected void SetIntIDRandom(int range)
		{
			this.IDInt = UnityEngine.Random.Range(1, range + 1);
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x00023512 File Offset: 0x00021712
		public bool IsJumping
		{
			get
			{
				return this.AnimState == AnimTag.Jump;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x00023521 File Offset: 0x00021721
		// (set) Token: 0x060033C9 RID: 13257 RVA: 0x00023529 File Offset: 0x00021729
		public bool ActiveColliders { get; private set; }

		// Token: 0x060033CA RID: 13258 RVA: 0x0010BCA4 File Offset: 0x00109EA4
		public virtual void EnableColliders(bool active)
		{
			this.ActiveColliders = active;
			if (!active)
			{
				this._col_ = base.GetComponentsInChildren<Collider>(false).ToList<Collider>();
				List<Collider> list = new List<Collider>();
				foreach (Collider collider in this._col_)
				{
					if (!collider.isTrigger && collider.enabled)
					{
						list.Add(collider);
					}
				}
				this._col_ = list;
			}
			foreach (Collider collider2 in this._col_)
			{
				collider2.enabled = active;
			}
			if (active)
			{
				this._col_ = new List<Collider>();
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060033CC RID: 13260 RVA: 0x00023540 File Offset: 0x00021740
		// (set) Token: 0x060033CB RID: 13259 RVA: 0x00023532 File Offset: 0x00021732
		public virtual bool Gravity
		{
			get
			{
				return this._RigidBody.useGravity;
			}
			set
			{
				this._RigidBody.useGravity = value;
			}
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x0002354D File Offset: 0x0002174D
		public virtual void InAir(bool active)
		{
			this.IsInAir = active;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x00023556 File Offset: 0x00021756
		public virtual void SetJump()
		{
			base.StartCoroutine(this.ToggleJump());
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x00023565 File Offset: 0x00021765
		public virtual void SetAction(int ID)
		{
			this.ActionID = ID;
			this.Action = true;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x00023575 File Offset: 0x00021775
		public virtual void SetAction(Action ID)
		{
			this.ActionID = ID;
			this.Action = true;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x0010BD80 File Offset: 0x00109F80
		public virtual void SetAction(string actionName)
		{
			if (this.Anim.HasState(0, Animator.StringToHash(actionName)))
			{
				if (this.AnimState != AnimTag.Action && this.ActionID <= 0)
				{
					this.Anim.CrossFade(actionName, 0.1f, 0);
					return;
				}
			}
			else
			{
				Debug.LogWarning("The animal does not have an action called " + actionName);
			}
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x0010BDDC File Offset: 0x00109FDC
		public virtual void ResetAnimal()
		{
			this.fly = false;
			this.swim = false;
			this.fall = false;
			this.action = false;
			this.attack1 = false;
			this.damaged = false;
			this.attack2 = false;
			this.anim.Rebind();
			this.Platform = null;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x0010BE2C File Offset: 0x0010A02C
		public virtual void SetStun(float time)
		{
			if (this.AnimState == AnimTag.Jump || this.AnimState == AnimTag.JumpStart || this.AnimState == AnimTag.JumpEnd)
			{
				return;
			}
			if (this.StunC != null)
			{
				base.StopCoroutine(this.StunC);
			}
			this.StunC = null;
			this.StunC = this.ToggleStun(time);
			base.StartCoroutine(this.StunC);
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x0010BE98 File Offset: 0x0010A098
		public virtual void DisableAnimal()
		{
			base.enabled = false;
			MalbersInput component = base.GetComponent<MalbersInput>();
			if (component)
			{
				component.enabled = false;
			}
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x0002358A File Offset: 0x0002178A
		public virtual void SetFly(bool value)
		{
			if (this.canFly && this.hasFly)
			{
				this.fly = !value;
				this.Fly = true;
			}
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000235AD File Offset: 0x000217AD
		public virtual void SetToGlide(float value)
		{
			if (this.fly && this.fall)
			{
				base.StartCoroutine(this.GravityDrag(value));
			}
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x000235CD File Offset: 0x000217CD
		internal IEnumerator GravityDrag(float value)
		{
			while (this.AnimState != AnimTag.Fly)
			{
				yield return null;
			}
			this.groundSpeed = 2f;
			if (this._RigidBody)
			{
				this._RigidBody.useGravity = true;
				this._RigidBody.drag = value;
			}
			yield break;
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000235E3 File Offset: 0x000217E3
		internal IEnumerator ToggleJump()
		{
			int num;
			for (int i = 0; i < this.ToogleAmount; i = num + 1)
			{
				this.Jump = true;
				yield return null;
				num = i;
			}
			this.Jump = false;
			yield break;
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000235F2 File Offset: 0x000217F2
		internal IEnumerator ToggleAction()
		{
			int num;
			for (int i = 0; i < this.ToogleAmount; i = num + 1)
			{
				this.action = true;
				if (this.AnimState == AnimTag.Action)
				{
					this.OnAction.Invoke();
					this.SetFloatID(-1f);
					break;
				}
				yield return null;
				num = i;
			}
			this.action = false;
			if (this.AnimState != AnimTag.Action)
			{
				this.ActionID = -1;
				this.SetFloatID(0f);
			}
			yield break;
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x00023601 File Offset: 0x00021801
		internal IEnumerator ToggleStun(float time)
		{
			this.Stun = true;
			yield return new WaitForSeconds(time);
			this.stun = false;
			yield break;
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x00023617 File Offset: 0x00021817
		internal IEnumerator CDamageInterrupt()
		{
			yield return new WaitForSeconds(this.damageInterrupt);
			this.SetIntID(-1);
			yield break;
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x00023626 File Offset: 0x00021826
		public void InitializeInputs(Dictionary<string, BoolEvent> keys)
		{
			if (this.Inputs == null)
			{
				this.Inputs = new Dictionary<string, BoolEvent>();
			}
			this.Inputs = keys;
			this.CharacterConnect();
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x0010BEC4 File Offset: 0x0010A0C4
		public void SetInput(string key, bool value)
		{
			BoolEvent boolEvent;
			if (this.Inputs.TryGetValue(key, out boolEvent))
			{
				boolEvent.Invoke(value);
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x00023648 File Offset: 0x00021848
		public void AddInput(string key, BoolEvent NewBool)
		{
			if (!this.Inputs.ContainsKey(key))
			{
				this.Inputs.Add(key, NewBool);
			}
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x0010BEE8 File Offset: 0x0010A0E8
		private void CharacterConnect()
		{
			BoolEvent boolEvent;
			if (this.Inputs.TryGetValue("Attack1", out boolEvent))
			{
				boolEvent.AddListener(delegate(bool value)
				{
					this.Attack1 = value;
				});
			}
			BoolEvent boolEvent2;
			if (this.Inputs.TryGetValue("Attack2", out boolEvent2))
			{
				boolEvent2.AddListener(delegate(bool value)
				{
					this.Attack2 = value;
				});
			}
			BoolEvent boolEvent3;
			if (this.Inputs.TryGetValue("Action", out boolEvent3))
			{
				boolEvent3.AddListener(delegate(bool value)
				{
					this.Action = value;
				});
			}
			BoolEvent boolEvent4;
			if (this.Inputs.TryGetValue("Jump", out boolEvent4))
			{
				boolEvent4.AddListener(delegate(bool value)
				{
					this.Jump = value;
				});
			}
			BoolEvent boolEvent5;
			if (this.Inputs.TryGetValue("Shift", out boolEvent5))
			{
				boolEvent5.AddListener(delegate(bool value)
				{
					this.Shift = value;
				});
			}
			BoolEvent boolEvent6;
			if (this.Inputs.TryGetValue("Fly", out boolEvent6))
			{
				boolEvent6.AddListener(delegate(bool value)
				{
					this.Fly = value;
				});
			}
			BoolEvent boolEvent7;
			if (this.Inputs.TryGetValue("Down", out boolEvent7))
			{
				boolEvent7.AddListener(delegate(bool value)
				{
					this.Down = value;
				});
			}
			BoolEvent boolEvent8;
			if (this.Inputs.TryGetValue("Up", out boolEvent8))
			{
				boolEvent8.AddListener(delegate(bool value)
				{
					this.Up = value;
				});
			}
			BoolEvent boolEvent9;
			if (this.Inputs.TryGetValue("Dodge", out boolEvent9))
			{
				boolEvent9.AddListener(delegate(bool value)
				{
					this.Dodge = value;
				});
			}
			BoolEvent boolEvent10;
			if (this.Inputs.TryGetValue("Death", out boolEvent10))
			{
				boolEvent10.AddListener(delegate(bool value)
				{
					this.Death = value;
				});
			}
			BoolEvent boolEvent11;
			if (this.Inputs.TryGetValue("Stun", out boolEvent11))
			{
				boolEvent11.AddListener(delegate(bool value)
				{
					this.Stun = value;
				});
			}
			BoolEvent boolEvent12;
			if (this.Inputs.TryGetValue("Damaged", out boolEvent12))
			{
				boolEvent12.AddListener(delegate(bool value)
				{
					this.Damaged = value;
				});
			}
			BoolEvent boolEvent13;
			if (this.Inputs.TryGetValue("Speed1", out boolEvent13))
			{
				boolEvent13.AddListener(delegate(bool value)
				{
					this.Speed1 = value;
				});
			}
			BoolEvent boolEvent14;
			if (this.Inputs.TryGetValue("Speed2", out boolEvent14))
			{
				boolEvent14.AddListener(delegate(bool value)
				{
					this.Speed2 = value;
				});
			}
			BoolEvent boolEvent15;
			if (this.Inputs.TryGetValue("Speed3", out boolEvent15))
			{
				boolEvent15.AddListener(delegate(bool value)
				{
					this.Speed3 = value;
				});
			}
			BoolEvent boolEvent16;
			if (this.Inputs.TryGetValue("SpeedUp", out boolEvent16))
			{
				boolEvent16.AddListener(delegate(bool value)
				{
					this.SpeedUp = value;
				});
			}
			BoolEvent boolEvent17;
			if (this.Inputs.TryGetValue("SpeedDown", out boolEvent17))
			{
				boolEvent17.AddListener(delegate(bool value)
				{
					this.SpeedDown = value;
				});
			}
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x00023665 File Offset: 0x00021865
		private void Reset()
		{
			MalbersTools.SetLayer(base.transform, 20);
			base.gameObject.tag = "Animal";
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x0010C188 File Offset: 0x0010A388
		protected virtual void GetHashIDs()
		{
			this.hash_Vertical = Animator.StringToHash(this.m_Vertical);
			this.hash_Horizontal = Animator.StringToHash(this.m_Horizontal);
			this.hash_UpDown = Animator.StringToHash(this.m_UpDown);
			this.hash_Stand = Animator.StringToHash(this.m_Stand);
			this.hash_Jump = Animator.StringToHash(this.m_Jump);
			this.hash_Dodge = Animator.StringToHash(this.m_Dodge);
			this.hash_Fall = Animator.StringToHash(this.m_Fall);
			this.hash_Type = Animator.StringToHash(this.m_Type);
			this.hash_Slope = Animator.StringToHash(this.m_Slope);
			this.hash_Shift = Animator.StringToHash(this.m_Shift);
			this.hash_Fly = Animator.StringToHash(this.m_Fly);
			this.hash_Attack1 = Animator.StringToHash(this.m_Attack1);
			this.hash_Attack2 = Animator.StringToHash(this.m_Attack2);
			this.hash_Death = Animator.StringToHash(this.m_Death);
			this.hash_Damaged = Animator.StringToHash(this.m_Damaged);
			this.hash_Stunned = Animator.StringToHash(this.m_Stunned);
			this.hash_IDInt = Animator.StringToHash(this.m_IDInt);
			this.hash_IDFloat = Animator.StringToHash(this.m_IDFloat);
			this.hash_Swim = Animator.StringToHash(this.m_Swim);
			this.hash_Underwater = Animator.StringToHash(this.m_Underwater);
			this.hash_Action = Animator.StringToHash(this.m_Action);
			this.hash_IDAction = Animator.StringToHash(this.m_IDAction);
			this.hash_StateTime = Animator.StringToHash(this.m_StateTime);
			this.hash_Stance = Animator.StringToHash(this.m_Stance);
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x0010C330 File Offset: 0x0010A530
		private void Awake()
		{
			this.AnimalMesh = base.GetComponentInChildren<Renderer>();
			this.anim = base.GetComponent<Animator>();
			this.GetHashIDs();
			this._transform = base.transform;
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Int, this.hash_Type))
			{
				this.Anim.SetInteger(this.hash_Type, this.animalTypeID);
			}
			this.WaterLayer = LayerMask.GetMask(new string[]
			{
				"Water"
			});
			this.RootMotion = true;
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x00023684 File Offset: 0x00021884
		private void Start()
		{
			this.SetStart();
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x0010C3B4 File Offset: 0x0010A5B4
		protected virtual void SetStart()
		{
			this.DeltaPosition = Vector3.zero;
			this._RigidBody.isKinematic = false;
			this.Anim.updateMode = AnimatorUpdateMode.Normal;
			this.isInAir = false;
			this.scaleFactor = this._transform.localScale.y;
			this.Double_Jump = 0;
			this.MovementReleased = true;
			this.SetPivots();
			this.ActiveColliders = true;
			switch (this.StartSpeed)
			{
			case Animal.Ground.walk:
				this.Speed1 = true;
				break;
			case Animal.Ground.trot:
				this.Speed2 = true;
				break;
			case Animal.Ground.run:
				this.Speed3 = true;
				break;
			}
			this.Attack_Triggers = base.GetComponentsInChildren<AttackTrigger>(true).ToList<AttackTrigger>();
			this.OptionalAnimatorParameters();
			this.Start_Flying();
			this.FrameCounter = UnityEngine.Random.Range(0, 10000);
			this.OnAnimationChange.AddListener(new UnityAction<int>(this.OnAnimationStateEnter));
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x0010C49C File Offset: 0x0010A69C
		public virtual void SetPivots()
		{
			this.pivots = base.GetComponentsInChildren<Pivots>().ToList<Pivots>();
			if (this.pivots != null)
			{
				this.pivot_Hip = this.pivots.Find((Pivots p) => p.name.ToUpper().Contains("HIP"));
				this.pivot_Chest = this.pivots.Find((Pivots p) => p.name.ToUpper().Contains("CHEST"));
				this.pivot_Water = this.pivots.Find((Pivots p) => p.name.ToUpper().Contains("WATER"));
			}
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0010C558 File Offset: 0x0010A758
		protected virtual void Start_Flying()
		{
			if (this.hasFly && this.StartFlying && this.canFly)
			{
				this.stand = false;
				this.Fly = true;
				this.Anim.Play("Fly", 0);
				this.IsInAir = true;
				this._RigidBody.useGravity = false;
			}
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x0010C5B0 File Offset: 0x0010A7B0
		protected void OptionalAnimatorParameters()
		{
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Bool, this.hash_Swim))
			{
				this.hasSwim = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Bool, this.hash_Dodge))
			{
				this.hasDodge = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Bool, this.hash_Fly))
			{
				this.hasFly = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Bool, this.hash_Attack2))
			{
				this.hasAttack2 = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Bool, this.hash_Stunned))
			{
				this.hasStun = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Bool, this.hash_Underwater))
			{
				this.hasUnderwater = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Float, this.hash_UpDown))
			{
				this.hasUpDown = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Float, this.hash_Slope))
			{
				this.hasSlope = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Float, this.hash_StateTime))
			{
				this.hasStateTime = true;
			}
			if (MalbersTools.FindAnimatorParameter(this.Anim, AnimatorControllerParameterType.Int, this.hash_Stance))
			{
				this.hasStance = true;
			}
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0010C6CC File Offset: 0x0010A8CC
		public virtual void LinkingAnimator()
		{
			if (!this.Death)
			{
				this.Anim.SetFloat(this.hash_Vertical, this.vertical);
				this.Anim.SetFloat(this.hash_Horizontal, this.horizontal);
				this.Anim.SetBool(this.hash_Stand, this.stand);
				this.Anim.SetBool(this.hash_Shift, this.Shift);
				this.Anim.SetBool(this.hash_Jump, this.jump);
				this.Anim.SetBool(this.hash_Attack1, this.attack1);
				this.Anim.SetBool(this.hash_Damaged, this.damaged);
				this.Anim.SetBool(this.hash_Action, this.action);
				this.Anim.SetInteger(this.hash_IDAction, this.ActionID);
				this.Anim.SetInteger(this.hash_IDInt, this.IDInt);
				if (this.hasSlope)
				{
					this.Anim.SetFloat(this.hash_Slope, this.Slope);
				}
				if (this.hasStun)
				{
					this.Anim.SetBool(this.hash_Stunned, this.stun);
				}
				if (this.hasAttack2)
				{
					this.Anim.SetBool(this.hash_Attack2, this.attack2);
				}
				if (this.hasUpDown)
				{
					this.Anim.SetFloat(this.hash_UpDown, this.movementAxis.y);
				}
				if (this.hasStateTime)
				{
					this.Anim.SetFloat(this.hash_StateTime, this.StateTime);
				}
				if (this.hasDodge)
				{
					this.Anim.SetBool(this.hash_Dodge, this.dodge);
				}
				if (this.hasFly && this.canFly)
				{
					this.Anim.SetBool(this.hash_Fly, this.Fly);
				}
				if (this.hasSwim && this.canSwim)
				{
					this.Anim.SetBool(this.hash_Swim, this.swim);
				}
				if (this.hasUnderwater && this.CanGoUnderWater)
				{
					this.Anim.SetBool(this.hash_Underwater, this.underwater);
				}
			}
			this.Anim.SetBool(this.hash_Fall, this.fall);
			this.OnSyncAnimator.Invoke();
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x0010C920 File Offset: 0x0010AB20
		public virtual void Move(Vector3 move, bool active = true)
		{
			this.MovementReleased = (move.x == 0f && move.z == 0f);
			this.directionalMovement = active;
			float deltaTime = Time.deltaTime;
			this.RawDirection = move.normalized;
			if (this.LockUp && move.y > 0f)
			{
				move.y = 0f;
			}
			if (active)
			{
				if (move.magnitude > 1f)
				{
					move.Normalize();
				}
				this.RawDirection = Vector3.Lerp(this.RawDirection, move, deltaTime * this.upDownSmoothness * 5f);
				move = this._transform.InverseTransformDirection(move);
				if (!this.Fly && !this.underwater)
				{
					move = Vector3.ProjectOnPlane(move, this.SurfaceNormal).normalized;
				}
				float x = Mathf.Atan2(move.x, move.z);
				float num = move.z;
				if (!this.SmoothVertical)
				{
					if (num > 0f)
					{
						num = 1f;
					}
					if (num < 0f)
					{
						num = -1f;
					}
				}
				this.movementAxis = new Vector3(x, this.IgnoreYDir ? this.movementAxis.y : this.RawDirection.y, Mathf.Abs(num));
				if ((this.Fly || this.underwater) && !this.Up && !this.Down && this.IgnoreYDir)
				{
					this.movementAxis.y = Mathf.Lerp(this.movementAxis.y, 0f, deltaTime * this.upDownSmoothness * 3f);
				}
				if (!this.stand && this.AnimState != AnimTag.Action && this.AnimState != AnimTag.Sleep)
				{
					this.DeltaRotation *= Quaternion.Euler(0f, this.movementAxis.x * deltaTime * this.TurnMultiplier, 0f);
				}
				if (this.AnimState == AnimTag.Action)
				{
					this.movementAxis = Vector3.zero;
					return;
				}
			}
			else
			{
				this.movementAxis = new Vector3(move.x, this.movementAxis.y, move.z);
			}
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x0010CB50 File Offset: 0x0010AD50
		protected virtual void AdditionalTurn(float time)
		{
			float rotation = this.currentSpeed.rotation;
			float num = Mathf.Clamp(this.horizontal, -1f, 1f) * (float)((this.movementAxis.z >= 0f) ? 1 : -1);
			Vector3 euler = this._transform.InverseTransformDirection(0f, rotation * 2f * num * time, 0f);
			this.DeltaRotation *= Quaternion.Euler(euler);
			if (this.Fly || this.swim || this.stun || this.AnimState == AnimTag.Action)
			{
				return;
			}
			if (this.AnimState == AnimTag.Jump || this.AnimState == AnimTag.Fall)
			{
				float num2 = this.airRotation * this.horizontal * time * (float)((this.movementAxis.z >= 0f) ? 1 : -1);
				this.DeltaRotation *= Quaternion.Euler(this._transform.InverseTransformDirection(0f, num2, 0f));
				this.DeltaPosition += this._transform.DeltaPositionFromRotate(this.AnimalMesh.bounds.center, this.T_Up, num2);
			}
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0010CCA0 File Offset: 0x0010AEA0
		protected virtual void AdditionalSpeed(float time)
		{
			this.currentSpeed = new Speeds(1);
			if (this.hasUnderwater && this.underwater && this.CurrentAnimState == AnimTag.Underwater)
			{
				this.currentSpeed = this.underWaterSpeed;
			}
			else if (this.hasSwim && this.swim && this.CurrentAnimState == AnimTag.Swim)
			{
				this.currentSpeed = this.swimSpeed;
			}
			else if (this.hasFly && this.fly && this.CurrentAnimState == AnimTag.Fly)
			{
				this.currentSpeed = this.flySpeed;
			}
			else if (this.IsJumping || this.fall || this.CurrentAnimState == AnimTag.Fall)
			{
				this.currentSpeed = new Speeds(1);
			}
			else if (this.Speed3 || (this.Speed2 && this.Shift))
			{
				this.currentSpeed = this.runSpeed;
			}
			else if (this.Speed2 || (this.Speed1 && this.Shift))
			{
				this.currentSpeed = this.trotSpeed;
			}
			else if (this.Speed1)
			{
				this.currentSpeed = this.walkSpeed;
			}
			if (this.vertical < 0f)
			{
				this.currentSpeed.position = this.walkSpeed.position;
			}
			this.currentSpeed.position = this.currentSpeed.position * this.ScaleFactor;
			Vector3 a = this.T_Forward * this.vertical;
			if (a.magnitude > 1f)
			{
				a.Normalize();
			}
			this.DeltaPosition += a * this.currentSpeed.position / 5f * time;
			this.Anim.speed = Mathf.Lerp(this.Anim.speed, this.currentSpeed.animator * this.animatorSpeed, time * this.currentSpeed.lerpAnimator);
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x0010CEA4 File Offset: 0x0010B0A4
		public virtual void YAxisMovement(float smoothness, float time)
		{
			if (this.Up)
			{
				this.Down = false;
			}
			float num = this.MovementAxis.y;
			if (this.Up)
			{
				num = Mathf.Lerp(num, this.LockUp ? 0f : ((this.MovementForward > 0f) ? 0.7f : 1f), time * smoothness);
			}
			else if (this.Down)
			{
				num = Mathf.Lerp(num, (this.MovementForward > 0f) ? -0.7f : -1f, time * smoothness);
			}
			else if (!this.DirectionalMovement)
			{
				num = Mathf.Lerp(num, 0f, time * smoothness);
			}
			if (Mathf.Abs(num) < 0.001f)
			{
				num = 0f;
			}
			this.movementAxis.y = num;
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x0010CF6C File Offset: 0x0010B16C
		private void UpdatePlatformMovement(bool update)
		{
			if (this.Platform == null)
			{
				return;
			}
			if (this.AnimState == AnimTag.Jump || this.AnimState == AnimTag.NoAlign || this.underwater || this.fly)
			{
				this.Platform = null;
				return;
			}
			if (!update)
			{
				this.FixedDeltaPos = this.Platform.position - this.platform_Pos;
				this.platform_Pos = this.Platform.position;
				return;
			}
			float num = this.Platform.eulerAngles.y - this.platform_formAngle;
			if (num == 0f)
			{
				return;
			}
			this.DeltaRotation *= Quaternion.Euler(0f, num, 0f);
			this.DeltaPosition += this._transform.DeltaPositionFromRotate(this.Platform.position, Vector3.up, num);
			this.platform_formAngle = this.Platform.eulerAngles.y;
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0010D070 File Offset: 0x0010B270
		protected void RayCasting()
		{
			if (this.AnimState != AnimTag.Jump && this.AnimState != AnimTag.JumpEnd && this.AnimState != AnimTag.Recover && this.AnimState != AnimTag.Fall && this.FrameCounter % this.PivotsRayInterval != 0)
			{
				return;
			}
			if (this.underwater)
			{
				return;
			}
			this.UpVector = -Physics.gravity;
			this.scaleFactor = this._transform.localScale.y;
			this._Height = this.height * this.scaleFactor;
			this.backray = (this.frontray = false);
			this.hit_Chest = Animal.NULLRayCast;
			this.hit_Hip = Animal.NULLRayCast;
			this.hit_Chest.distance = (this.hit_Hip.distance = this._Height);
			if (this.Pivot_Hip != null)
			{
				if (Physics.Raycast(this.Pivot_Hip.GetPivot, -this.T_Up, out this.hit_Hip, this.scaleFactor * this.Pivot_Hip.multiplier, this.GroundLayer))
				{
					if (this.debug)
					{
						Debug.DrawRay(this.hit_Hip.point, this.hit_Hip.normal * 0.2f, Color.blue);
					}
					if (Vector3.Angle(this.hit_Hip.normal, Vector3.up) < this.maxAngleSlope)
					{
						this.backray = true;
						if (this.Platform == null && this.AnimState != AnimTag.Jump)
						{
							this.Platform = this.hit_Hip.transform;
							this.platform_Pos = this.Platform.position;
							this.platform_formAngle = this.Platform.eulerAngles.y;
						}
						this.CheckForLanding();
					}
				}
				else
				{
					this.Platform = null;
				}
			}
			if (Physics.Raycast(this.Main_Pivot_Point, -this.T_Up, out this.hit_Chest, this.Pivot_Multiplier, this.GroundLayer))
			{
				if (this.debug)
				{
					Debug.DrawRay(this.hit_Chest.point, this.hit_Chest.normal * 0.2f, Color.red);
				}
				if (Vector3.Angle(this.hit_Chest.normal, Vector3.up) < this.maxAngleSlope)
				{
					this.frontray = true;
				}
				this.CheckForLanding();
			}
			if (this.debug && this.frontray && this.backray)
			{
				Debug.DrawLine(this.hit_Hip.point, this.hit_Chest.point, Color.yellow);
			}
			if (!this.frontray && this.Stand)
			{
				this.fall = true;
				if (this.pivot_Hip && this.backray)
				{
					this.fall = false;
				}
			}
			this.FixDistance = this.hit_Hip.distance;
			if (!this.backray)
			{
				this.FixDistance = this.hit_Chest.distance;
			}
			if (!this.Pivot_Hip)
			{
				this.backray = this.frontray;
			}
			if (!this.Pivot_Chest)
			{
				this.frontray = this.backray;
			}
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0010D3A8 File Offset: 0x0010B5A8
		public virtual void AlignRotation(bool align, float time, float smoothness)
		{
			Quaternion rhs = Quaternion.FromToRotation(this.T_Up, this.SurfaceNormal) * this._transform.rotation;
			Quaternion lhs = Quaternion.Inverse(this._transform.rotation);
			Quaternion rhs2;
			if (align)
			{
				rhs2 = Quaternion.Inverse(this._transform.rotation) * rhs;
			}
			else
			{
				Quaternion rhs3 = Quaternion.FromToRotation(this.T_Up, this.UpVector) * this._transform.rotation;
				rhs2 = lhs * rhs3;
			}
			rhs2 = Quaternion.Slerp(this.DeltaRotation, this.DeltaRotation * rhs2, time * smoothness / 2f);
			this.DeltaRotation *= rhs2;
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x0010D464 File Offset: 0x0010B664
		protected virtual void FixRotation(float time)
		{
			if (this.swim || this.fly || this.underwater)
			{
				return;
			}
			if (this.IsInAir || this.slope < -1f || this.AnimState == AnimTag.NoAlign || !this.backray || (this.backray && !this.frontray))
			{
				if (this.slope < 0f || this.AnimState == AnimTag.Fall)
				{
					this.AlignRotation(false, time, this.AlingToGround);
					return;
				}
			}
			else
			{
				this.AlignRotation(true, time, this.AlingToGround);
			}
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x0010D4FC File Offset: 0x0010B6FC
		internal virtual void RaycastWater()
		{
			if (!this.pivot_Water)
			{
				return;
			}
			if (Physics.Raycast(this.pivot_Water.transform.position, -this.T_Up, out this.WaterHitCenter, this.scaleFactor * this.pivot_Water.multiplier * 1.5f, this.WaterLayer))
			{
				this.waterLevel = this.WaterHitCenter.point.y;
				this.isInWater = true;
				return;
			}
			if (this.isInWater && this.AnimState != AnimTag.SwimJump)
			{
				this.isInWater = false;
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x0010D598 File Offset: 0x0010B798
		protected virtual void Swimming(float time)
		{
			if (!this.hasSwim || !this.canSwim)
			{
				return;
			}
			if (this.underwater)
			{
				return;
			}
			if (this.Stand || !this.pivot_Water)
			{
				return;
			}
			if (this.FrameCounter % this.WaterRayInterval == 0)
			{
				this.RaycastWater();
			}
			if (this.isInWater)
			{
				if (((this.hit_Chest.distance < this._Height * 0.8f && this.movementAxis.z > 0f && this.hit_Chest.transform != null) || (this.hit_Hip.distance < this._Height * 0.8f && this.movementAxis.z < 0f && this.hit_Hip.transform != null)) && this.AnimState != AnimTag.Recover)
				{
					this.Swim = false;
					return;
				}
				if (!this.swim && this.Pivot_Chest.Y <= this.Waterlevel)
				{
					this.Swim = true;
				}
			}
			if (this.swim)
			{
				float num = Vector3.Angle(this.T_Up, this.WaterHitCenter.normal);
				Quaternion rhs = Quaternion.FromToRotation(this.T_Up, this.WaterHitCenter.normal) * this._transform.rotation;
				Quaternion rhs2 = Quaternion.Inverse(this._transform.rotation) * rhs;
				if (num > 0.5f)
				{
					rhs2 = Quaternion.Slerp(this.DeltaRotation, this.DeltaRotation * rhs2, time * 10f);
				}
				this.DeltaRotation *= rhs2;
				if (this.CanGoUnderWater && this.Down && !this.IsJumping && this.AnimState != AnimTag.SwimJump)
				{
					this.underwater = true;
				}
			}
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x0010D76C File Offset: 0x0010B96C
		protected virtual void FixPosition(float time)
		{
			if (this.swim)
			{
				return;
			}
			float num = this._Height - this.FixDistance;
			if (this.FixDistance > this._Height)
			{
				if (!this.isInAir && !this.swim)
				{
					this.YFix += (((this.AnimState == AnimTag.Locomotion || this.Stand) && num > 0.01f) ? num : (num * time * this.SnapToGround));
				}
			}
			else if (!this.fall && !this.IsInAir)
			{
				this.YFix += ((num < 0.01f || this.Stand) ? num : (num * time * this.SnapToGround));
			}
			this.FixDistance += this.YFix;
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x0010D834 File Offset: 0x0010BA34
		protected virtual void Falling()
		{
			this.fall_Point = this.Main_Pivot_Point + this.T_Forward * (this.Shift ? (this.GroundSpeed + 1f) : this.GroundSpeed) * this.FallRayDistance * this.ScaleFactor;
			if (this.FrameCounter % this.FallRayInterval != 0)
			{
				return;
			}
			if (this.AnimState == AnimTag.Sleep || this.AnimState == AnimTag.Action || this.AnimState == AnimTag.Swim || this.AnimState == AnimTag.Idle || this.swim || this.underwater)
			{
				return;
			}
			float num = this.Pivot_Multiplier;
			if (this.AnimState == AnimTag.Jump || this.AnimState == AnimTag.Fall || this.AnimState == AnimTag.Fly)
			{
				num *= this.FallRayMultiplier;
			}
			if (Physics.Raycast(this.fall_Point, -this.T_Up, out this.FallRayCast, num, this.GroundLayer))
			{
				if (this.debug)
				{
					Debug.DrawRay(this.fall_Point, -this.T_Up * num, Color.magenta);
					MalbersTools.DebugPlane(this.FallRayCast.point, 0.1f, Color.magenta, true);
				}
				if (Vector3.Angle(this.FallRayCast.normal, Vector3.up) * (float)((Vector3.Dot(this.T_ForwardNoY, this.FallRayCast.normal) > 0f) ? 1 : -1) > this.maxAngleSlope || (!this.frontray && !this.backray))
				{
					this.fall = true;
					return;
				}
				this.fall = false;
				this.CheckForLanding();
				if (this.AnimState == AnimTag.SwimJump)
				{
					this.Swim = false;
					return;
				}
			}
			else
			{
				this.fall = true;
				this.FallRayCast.normal = this.UpVector;
				if (this.debug)
				{
					MalbersTools.DebugPlane(this.fall_Point + -this.T_Up * num, 0.1f, Color.gray, true);
					Debug.DrawRay(this.fall_Point, -this.T_Up * num, Color.gray);
				}
			}
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x0002368C File Offset: 0x0002188C
		protected void CheckForLanding()
		{
			if (this.AnimState == AnimTag.Fly && this.Land)
			{
				this.SetFly(false);
				this.IsInAir = false;
				this.groundSpeed = this.LastGroundSpeed;
			}
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x0010DA74 File Offset: 0x0010BC74
		protected virtual bool IsFallingBackwards(float ammount)
		{
			if (this.FrameCounter % this.FallRayInterval != 0)
			{
				return false;
			}
			RaycastHit raycastHit = default(RaycastHit);
			Vector3 a = this.Pivot_Hip ? this.Pivot_Hip.transform.position : (this._transform.position + new Vector3(0f, this._Height, 0f));
			float num = this.Pivot_Hip ? (this.Pivot_Hip.multiplier * this.FallRayMultiplier) : this.FallRayMultiplier;
			Vector3 vector = a + this.T_Forward * -1f * ammount;
			if (this.debug)
			{
				Debug.DrawRay(vector, -this.T_Up * num * this.scaleFactor, Color.white);
			}
			if (Physics.Raycast(vector, -this.T_Up, out raycastHit, this.scaleFactor * num, this.GroundLayer))
			{
				return (double)raycastHit.normal.y < 0.6;
			}
			return !this.swim && this.movementAxis.z < 0f;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x0010DBB4 File Offset: 0x0010BDB4
		protected virtual void MovementSystem(float s1 = 1f, float s2 = 2f, float s3 = 3f)
		{
			float num = this.groundSpeed;
			float num2 = 1f + this.currentSpeed.lerpRotation;
			float num3 = 1f + this.currentSpeed.lerpPosition;
			num = ((this.swim || this.underwater) ? 1f : num);
			if (this.Shift && this.UseShift)
			{
				num += 1f;
			}
			if (!this.Fly && !this.Swim && !this.IsJumping)
			{
				if (this.SlowSlopes && (double)this.slope >= 0.5 && num > 1f)
				{
					num -= 1f;
				}
				if (this.slope >= 1f)
				{
					num = 0f;
					num3 = 10f;
				}
			}
			if (this.Fly || this.Underwater)
			{
				this.YAxisMovement(this.upDownSmoothness, Time.deltaTime);
			}
			if (this.movementAxis.z < 0f && !this.swim && !this.Fly && !this.fall && this.IsFallingBackwards(this.BackFallRayDistance))
			{
				num = 0f;
				num3 = 10f;
			}
			this.vertical = Mathf.Lerp(this.vertical, this.movementAxis.z * num, Time.deltaTime * num3);
			this.horizontal = Mathf.Lerp(this.horizontal, this.movementAxis.x * (float)((this.Shift && this.UseShift) ? 2 : 1), Time.deltaTime * num2);
			if (Mathf.Abs(this.horizontal) > 0.1f || Mathf.Abs(this.vertical) > 0.2f)
			{
				this.stand = false;
			}
			else
			{
				this.stand = true;
			}
			if (!this.MovementReleased)
			{
				this.stand = false;
			}
			if (this.jump || this.damaged || this.stun || this.fall || this.swim || this.fly || this.isInAir || (this.tired >= this.GotoSleep && this.GotoSleep != 0))
			{
				this.stand = false;
			}
			if (this.tired >= this.GotoSleep)
			{
				this.tired = 0;
			}
			if (!this.stand)
			{
				this.tired = 0;
			}
			if (!this.swim && !this.fly)
			{
				this.movementAxis.y = 0f;
			}
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x0010DE18 File Offset: 0x0010C018
		private void FixedUpdate()
		{
			if (this.fly || this.underwater)
			{
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			if (this.swim && this.AnimState != AnimTag.SwimJump)
			{
				this.YFix = (this.Waterlevel - this._Height + this.waterLine - this._transform.position.y) * fixedDeltaTime * 5f;
			}
			this.FixPosition(fixedDeltaTime);
			this.UpdatePlatformMovement(false);
			this.FixedDeltaPos.y = this.FixedDeltaPos.y + this.YFix;
			this._transform.position += this.FixedDeltaPos;
			this.YFix = 0f;
			this.FixedDeltaPos = Vector3.zero;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0010DEDC File Offset: 0x0010C0DC
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			this.UpdateSettings();
			this.AdditionalSpeed(deltaTime);
			this.AdditionalTurn(deltaTime);
			this.RayCasting();
			this.Swimming(deltaTime);
			this.FixRotation(deltaTime);
			this.UpdatePlatformMovement(true);
			this.Falling();
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x0010DF24 File Offset: 0x0010C124
		public virtual void UpdateSettings()
		{
			this.CurrentAnimState = this.Anim.GetCurrentAnimatorStateInfo(0).tagHash;
			this.NextAnimState = this.Anim.GetNextAnimatorStateInfo(0).tagHash;
			this.StateTime = this.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
			if (this.LastAnimationTag != this.AnimState)
			{
				this.LastAnimationTag = this.AnimState;
			}
			this.T_Up = this._transform.up;
			this.T_Right = this._transform.right;
			this.T_Forward = this._transform.forward;
			this.FrameCounter++;
			this.FrameCounter %= 100000;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000236BD File Offset: 0x000218BD
		private void LateUpdate()
		{
			this.MovementSystem(this.movementS1, this.movementS2, this.movementS3);
			this.LinkingAnimator();
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x0010DFEC File Offset: 0x0010C1EC
		private void OnAnimatorMove()
		{
			if (Time.timeScale <= 1E-45f)
			{
				return;
			}
			if (this.RootMotion && Time.deltaTime > 0f)
			{
				this._RigidBody.velocity = (this.Anim.deltaPosition + this.DeltaPosition) / Time.deltaTime;
			}
			this._transform.rotation *= this.Anim.deltaRotation * this.DeltaRotation;
			this._RigidBody.angularVelocity = Vector3.zero;
			this.DeltaPosition = Vector3.zero;
			this.DeltaRotation = Quaternion.identity;
			this.LastPosition = this._transform.position;
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x0010E0A8 File Offset: 0x0010C2A8
		protected virtual void OnAnimationStateEnter(int animTag)
		{
			if (animTag == AnimTag.Locomotion || animTag == AnimTag.Idle)
			{
				this.IsInAir = false;
				this.RootMotion = true;
				this.Double_Jump = 0;
				if (this.iswakingUp)
				{
					this.Move(Vector3.zero, true);
					this.iswakingUp = false;
				}
			}
			if (animTag == AnimTag.Swim)
			{
				this.RootMotion = true;
			}
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000236DD File Offset: 0x000218DD
		private void OnEnable()
		{
			if (Animal.Animals == null)
			{
				Animal.Animals = new List<Animal>();
			}
			Animal.Animals.Add(this);
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000236FB File Offset: 0x000218FB
		private void OnDisable()
		{
			Animal.Animals.Remove(this);
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003400 RID: 13312 RVA: 0x0010E104 File Offset: 0x0010C304
		public Vector3 T_ForwardNoY
		{
			get
			{
				return new Vector3(this.T_Forward.x, 0f, this.T_Forward.z).normalized;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x00023709 File Offset: 0x00021909
		public Rigidbody _RigidBody
		{
			get
			{
				if (this._rigidbody == null)
				{
					this._rigidbody = base.GetComponentInChildren<Rigidbody>();
				}
				return this._rigidbody;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x00023734 File Offset: 0x00021934
		// (set) Token: 0x06003402 RID: 13314 RVA: 0x0002372B File Offset: 0x0002192B
		public virtual float GroundSpeed
		{
			get
			{
				return this.groundSpeed;
			}
			set
			{
				this.groundSpeed = value;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x00023745 File Offset: 0x00021945
		// (set) Token: 0x06003404 RID: 13316 RVA: 0x0002373C File Offset: 0x0002193C
		public virtual float Speed
		{
			get
			{
				return this.vertical;
			}
			set
			{
				this.vertical = value;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x0010E13C File Offset: 0x0010C33C
		public float Slope
		{
			get
			{
				this.slope = 0f;
				if (this.pivot_Chest && this.pivot_Hip)
				{
					float num = Vector3.Angle(this.SurfaceNormal, this.UpVector);
					float num2 = (float)((this.pivot_Chest.Y > this.pivot_Hip.Y) ? 1 : -1);
					this.slope = num / this.maxAngleSlope * (float)((num2 <= 0f) ? -1 : 1);
					return this.slope;
				}
				return 0f;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003408 RID: 13320 RVA: 0x0002376B File Offset: 0x0002196B
		// (set) Token: 0x06003407 RID: 13319 RVA: 0x0002374D File Offset: 0x0002194D
		public virtual bool MovementReleased
		{
			get
			{
				return this.movementReleased;
			}
			private set
			{
				if (this.movementReleased != value)
				{
					this.movementReleased = value;
					this.OnMovementReleased.Invoke(value);
				}
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600340A RID: 13322 RVA: 0x00023773 File Offset: 0x00021973
		// (set) Token: 0x06003409 RID: 13321 RVA: 0x0010E1CC File Offset: 0x0010C3CC
		public virtual bool Swim
		{
			get
			{
				return this.swim;
			}
			set
			{
				if (this.swim != value && Time.time - this.swimChanged >= 0.8f)
				{
					this.swim = value;
					this.swimChanged = Time.time;
					this.currentSpeed = this.swimSpeed;
					if (this.swim)
					{
						this.fall = (this.isInAir = (this.fly = false));
						this.OnSwim.Invoke();
						this._RigidBody.constraints = Animal.Still_Constraints;
						this.currentSpeed = this.swimSpeed;
					}
				}
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600340B RID: 13323 RVA: 0x0002377B File Offset: 0x0002197B
		public float Direction
		{
			get
			{
				return this.horizontal;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x0002378C File Offset: 0x0002198C
		// (set) Token: 0x0600340C RID: 13324 RVA: 0x00023783 File Offset: 0x00021983
		public int Loops
		{
			get
			{
				return this.loops;
			}
			set
			{
				this.loops = value;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x0002379D File Offset: 0x0002199D
		// (set) Token: 0x0600340E RID: 13326 RVA: 0x00023794 File Offset: 0x00021994
		public int IDInt
		{
			get
			{
				return this.idInt;
			}
			set
			{
				this.idInt = value;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003411 RID: 13329 RVA: 0x000237AE File Offset: 0x000219AE
		// (set) Token: 0x06003410 RID: 13328 RVA: 0x000237A5 File Offset: 0x000219A5
		public float IDFloat
		{
			get
			{
				return this.idfloat;
			}
			set
			{
				this.idfloat = value;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003413 RID: 13331 RVA: 0x000237BF File Offset: 0x000219BF
		// (set) Token: 0x06003412 RID: 13330 RVA: 0x000237B6 File Offset: 0x000219B6
		public int Tired
		{
			get
			{
				return this.tired;
			}
			set
			{
				this.tired = value;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x000237C7 File Offset: 0x000219C7
		public bool IsInWater
		{
			get
			{
				return this.isInWater;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (set) Token: 0x06003415 RID: 13333 RVA: 0x000237CF File Offset: 0x000219CF
		public bool SpeedUp
		{
			set
			{
				if (value)
				{
					if (this.groundSpeed == this.movementS1)
					{
						this.Speed2 = true;
						return;
					}
					if (this.groundSpeed == this.movementS2)
					{
						this.Speed3 = true;
					}
				}
			}
		}

		// Token: 0x17000931 RID: 2353
		// (set) Token: 0x06003416 RID: 13334 RVA: 0x000237FF File Offset: 0x000219FF
		public bool SpeedDown
		{
			set
			{
				if (value)
				{
					if (this.groundSpeed == this.movementS3)
					{
						this.Speed2 = true;
						return;
					}
					if (this.groundSpeed == this.movementS2)
					{
						this.Speed1 = true;
					}
				}
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003417 RID: 13335 RVA: 0x0002382F File Offset: 0x00021A2F
		// (set) Token: 0x06003418 RID: 13336 RVA: 0x0010E25C File Offset: 0x0010C45C
		public bool Speed1
		{
			get
			{
				return this.speed1;
			}
			set
			{
				if (value)
				{
					this.speed1 = value;
					this.speed2 = (this.speed3 = false);
					this.groundSpeed = this.movementS1;
				}
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x00023837 File Offset: 0x00021A37
		// (set) Token: 0x0600341A RID: 13338 RVA: 0x0010E290 File Offset: 0x0010C490
		public bool Speed2
		{
			get
			{
				return this.speed2;
			}
			set
			{
				if (value)
				{
					this.speed2 = value;
					this.speed1 = (this.speed3 = false);
					this.groundSpeed = this.movementS2;
				}
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x0002383F File Offset: 0x00021A3F
		// (set) Token: 0x0600341C RID: 13340 RVA: 0x0010E2C4 File Offset: 0x0010C4C4
		public bool Speed3
		{
			get
			{
				return this.speed3;
			}
			set
			{
				if (value)
				{
					this.speed3 = value;
					this.speed2 = (this.speed1 = false);
					this.groundSpeed = this.movementS3;
				}
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x00023847 File Offset: 0x00021A47
		// (set) Token: 0x0600341E RID: 13342 RVA: 0x0002384F File Offset: 0x00021A4F
		public bool Jump
		{
			get
			{
				return this.jump;
			}
			set
			{
				this.jump = value;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600341F RID: 13343 RVA: 0x00023858 File Offset: 0x00021A58
		// (set) Token: 0x06003420 RID: 13344 RVA: 0x00023860 File Offset: 0x00021A60
		public bool Underwater
		{
			get
			{
				return this.underwater;
			}
			set
			{
				if (this.CanGoUnderWater)
				{
					this.underwater = value;
				}
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x00023871 File Offset: 0x00021A71
		// (set) Token: 0x06003422 RID: 13346 RVA: 0x00023879 File Offset: 0x00021A79
		public bool Shift
		{
			get
			{
				return this.shift;
			}
			set
			{
				this.shift = value;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x00023882 File Offset: 0x00021A82
		// (set) Token: 0x06003424 RID: 13348 RVA: 0x0002388A File Offset: 0x00021A8A
		public bool Down
		{
			get
			{
				return this.down;
			}
			set
			{
				this.down = value;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x00023893 File Offset: 0x00021A93
		// (set) Token: 0x06003426 RID: 13350 RVA: 0x0002389B File Offset: 0x00021A9B
		public bool Up
		{
			get
			{
				return this.up;
			}
			set
			{
				this.up = value;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x000238A4 File Offset: 0x00021AA4
		// (set) Token: 0x06003428 RID: 13352 RVA: 0x000238AC File Offset: 0x00021AAC
		public bool Dodge
		{
			get
			{
				return this.dodge;
			}
			set
			{
				this.dodge = value;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003429 RID: 13353 RVA: 0x000238B5 File Offset: 0x00021AB5
		// (set) Token: 0x0600342A RID: 13354 RVA: 0x000238BD File Offset: 0x00021ABD
		public bool Damaged
		{
			get
			{
				return this.damaged;
			}
			set
			{
				this.damaged = value;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x000238C6 File Offset: 0x00021AC6
		// (set) Token: 0x0600342C RID: 13356 RVA: 0x0010E2F8 File Offset: 0x0010C4F8
		public bool Fly
		{
			get
			{
				if (!this.canFly)
				{
					this.fly = false;
				}
				return this.fly;
			}
			set
			{
				if (!this.canFly)
				{
					return;
				}
				if (value)
				{
					this.fly = !this.fly;
					if (this.fly)
					{
						this._RigidBody.useGravity = false;
						this.LastGroundSpeed = (float)Mathf.RoundToInt(this.groundSpeed);
						this.groundSpeed = 1f;
						this.IsInAir = true;
						this.currentSpeed = this.flySpeed;
						Quaternion rotation = Quaternion.FromToRotation(this.T_Up, this.UpVector) * this._transform.rotation;
						base.StartCoroutine(MalbersTools.AlignTransformsC(this._transform, rotation, 0.3f, null));
					}
					else
					{
						this.groundSpeed = this.LastGroundSpeed;
					}
					this.OnFly.Invoke(this.fly);
				}
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x000238DD File Offset: 0x00021ADD
		// (set) Token: 0x0600342E RID: 13358 RVA: 0x0010E3C4 File Offset: 0x0010C5C4
		public bool Death
		{
			get
			{
				return this.death;
			}
			set
			{
				this.death = value;
				if (this.death)
				{
					this.Anim.SetTrigger(Hash.Death);
					this.Anim.SetBool(Hash.Attack1, false);
					if (this.hasAttack2)
					{
						this.Anim.SetBool(Hash.Attack2, false);
					}
					this.Anim.SetBool(Hash.Action, false);
					this.OnDeathE.Invoke();
					if (Animal.Animals.Count > 0)
					{
						Animal.Animals.Remove(this);
					}
				}
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600342F RID: 13359 RVA: 0x000238E5 File Offset: 0x00021AE5
		// (set) Token: 0x06003430 RID: 13360 RVA: 0x0010E450 File Offset: 0x0010C650
		public bool Attack1
		{
			get
			{
				return this.attack1;
			}
			set
			{
				if (!value)
				{
					this.attack1 = value;
				}
				if (this.death)
				{
					return;
				}
				if (this.AnimState == AnimTag.Action)
				{
					return;
				}
				if (!this.isAttacking && value)
				{
					this.attack1 = value;
					this.IDInt = this.activeAttack;
					if (this.IDInt <= 0)
					{
						this.SetIntIDRandom(this.TotalAttacks);
					}
					this.OnAttack.Invoke();
				}
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003431 RID: 13361 RVA: 0x000238ED File Offset: 0x00021AED
		// (set) Token: 0x06003432 RID: 13362 RVA: 0x000238F5 File Offset: 0x00021AF5
		public bool Attack2
		{
			get
			{
				return this.attack2;
			}
			set
			{
				if (this.death)
				{
					return;
				}
				if (value && this.AnimState == AnimTag.Action)
				{
					return;
				}
				this.attack2 = value;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003433 RID: 13363 RVA: 0x00023918 File Offset: 0x00021B18
		// (set) Token: 0x06003434 RID: 13364 RVA: 0x00023920 File Offset: 0x00021B20
		public bool Stun
		{
			get
			{
				return this.stun;
			}
			set
			{
				this.stun = value;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x00023929 File Offset: 0x00021B29
		// (set) Token: 0x06003436 RID: 13366 RVA: 0x00023931 File Offset: 0x00021B31
		public bool Action
		{
			get
			{
				return this.action;
			}
			set
			{
				if (this.ActionID == -1)
				{
					return;
				}
				if (this.death)
				{
					return;
				}
				if (this.action != value)
				{
					this.action = value;
					if (this.action)
					{
						base.StartCoroutine(this.ToggleAction());
					}
				}
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06003437 RID: 13367 RVA: 0x0002396B File Offset: 0x00021B6B
		// (set) Token: 0x06003438 RID: 13368 RVA: 0x00023973 File Offset: 0x00021B73
		public int ActionID
		{
			get
			{
				return this.actionID;
			}
			set
			{
				this.actionID = value;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003439 RID: 13369 RVA: 0x0002397C File Offset: 0x00021B7C
		// (set) Token: 0x0600343A RID: 13370 RVA: 0x00023984 File Offset: 0x00021B84
		public bool IsAttacking
		{
			get
			{
				return this.isAttacking;
			}
			set
			{
				this.isAttacking = value;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600343C RID: 13372 RVA: 0x0002399B File Offset: 0x00021B9B
		// (set) Token: 0x0600343B RID: 13371 RVA: 0x0002398D File Offset: 0x00021B8D
		public bool RootMotion
		{
			get
			{
				return this.Anim.applyRootMotion;
			}
			set
			{
				this.Anim.applyRootMotion = value;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x000239A8 File Offset: 0x00021BA8
		// (set) Token: 0x0600343E RID: 13374 RVA: 0x000239B0 File Offset: 0x00021BB0
		public bool IsInAir
		{
			get
			{
				return this.isInAir;
			}
			set
			{
				this.isInAir = value;
				this._RigidBody.constraints = (this.isInAir ? RigidbodyConstraints.FreezeRotation : Animal.Still_Constraints);
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x0600343F RID: 13375 RVA: 0x000239D5 File Offset: 0x00021BD5
		public bool Stand
		{
			get
			{
				return this.stand;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06003440 RID: 13376 RVA: 0x000239DD File Offset: 0x00021BDD
		// (set) Token: 0x06003441 RID: 13377 RVA: 0x000239E5 File Offset: 0x00021BE5
		public Vector3 HitDirection
		{
			get
			{
				return this._hitDirection;
			}
			set
			{
				this._hitDirection = value;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06003442 RID: 13378 RVA: 0x000239EE File Offset: 0x00021BEE
		public float ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06003443 RID: 13379 RVA: 0x000239F6 File Offset: 0x00021BF6
		public Pivots Pivot_Hip
		{
			get
			{
				return this.pivot_Hip;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x000239FE File Offset: 0x00021BFE
		public Pivots Pivot_Chest
		{
			get
			{
				return this.pivot_Chest;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06003445 RID: 13381 RVA: 0x00023A06 File Offset: 0x00021C06
		public int AnimState
		{
			get
			{
				if (this.NextAnimState == 0)
				{
					return this.CurrentAnimState;
				}
				return this.NextAnimState;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06003447 RID: 13383 RVA: 0x00023A32 File Offset: 0x00021C32
		// (set) Token: 0x06003446 RID: 13382 RVA: 0x00023A1D File Offset: 0x00021C1D
		public int LastAnimationTag
		{
			get
			{
				return this.lastAnimTag;
			}
			private set
			{
				this.lastAnimTag = value;
				this.OnAnimationChange.Invoke(value);
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x00023A3A File Offset: 0x00021C3A
		public Animator Anim
		{
			get
			{
				if (this.anim == null)
				{
					this.anim = base.GetComponent<Animator>();
				}
				return this.anim;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06003449 RID: 13385 RVA: 0x00023A5C File Offset: 0x00021C5C
		public Vector3 Pivot_fall
		{
			get
			{
				return this.fall_Point;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x0600344A RID: 13386 RVA: 0x0010E4BC File Offset: 0x0010C6BC
		public float Pivot_Multiplier
		{
			get
			{
				return (this.Pivot_Chest ? this.Pivot_Chest.multiplier : (this.Pivot_Hip ? this.Pivot_Hip.multiplier : 1f)) * this.scaleFactor;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x0600344B RID: 13387 RVA: 0x0010E50C File Offset: 0x0010C70C
		public Vector3 Main_Pivot_Point
		{
			get
			{
				if (this.pivot_Chest)
				{
					return this.pivot_Chest.GetPivot;
				}
				if (this.pivot_Hip)
				{
					return this.pivot_Hip.GetPivot;
				}
				Vector3 position = this._transform.position;
				position.y += this.height;
				return position;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x0600344C RID: 13388 RVA: 0x00023A64 File Offset: 0x00021C64
		public static RigidbodyConstraints Still_Constraints
		{
			get
			{
				return (RigidbodyConstraints)116;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x0600344D RID: 13389 RVA: 0x00023A68 File Offset: 0x00021C68
		// (set) Token: 0x0600344E RID: 13390 RVA: 0x00023A70 File Offset: 0x00021C70
		public Vector3 MovementAxis
		{
			get
			{
				return this.movementAxis;
			}
			set
			{
				this.movementAxis = value;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x00023A79 File Offset: 0x00021C79
		// (set) Token: 0x06003450 RID: 13392 RVA: 0x00023A86 File Offset: 0x00021C86
		public float MovementForward
		{
			get
			{
				return this.movementAxis.z;
			}
			set
			{
				this.movementAxis.z = value;
				this.MovementReleased = (value == 0f);
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x00023AA2 File Offset: 0x00021CA2
		// (set) Token: 0x06003452 RID: 13394 RVA: 0x00023AAF File Offset: 0x00021CAF
		public float MovementRight
		{
			get
			{
				return this.movementAxis.x;
			}
			set
			{
				this.movementAxis.x = value;
				this.MovementReleased = (value == 0f);
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x00023ACB File Offset: 0x00021CCB
		// (set) Token: 0x06003454 RID: 13396 RVA: 0x00023AD8 File Offset: 0x00021CD8
		public float MovementUp
		{
			get
			{
				return this.movementAxis.y;
			}
			set
			{
				this.movementAxis.y = value;
				this.MovementReleased = (value == 0f);
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x0010E56C File Offset: 0x0010C76C
		public Vector3 SurfaceNormal
		{
			get
			{
				if (!this.pivot_Hip || !(this.hit_Hip.transform != null))
				{
					return Vector3.up;
				}
				if (this.Pivot_Chest && this.hit_Chest.transform != null)
				{
					Vector3 normalized = (this.hit_Chest.point - this.hit_Hip.point).normalized;
					Vector3 normalized2 = Vector3.Cross(this.UpVector, normalized).normalized;
					return Vector3.Cross(normalized, normalized2).normalized;
				}
				return this.hit_Hip.normal;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x00023AF4 File Offset: 0x00021CF4
		// (set) Token: 0x06003457 RID: 13399 RVA: 0x00023AFC File Offset: 0x00021CFC
		public Renderer AnimalMesh
		{
			get
			{
				return this.animalMesh;
			}
			set
			{
				this.animalMesh = value;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x00023B05 File Offset: 0x00021D05
		// (set) Token: 0x06003459 RID: 13401 RVA: 0x00023B0D File Offset: 0x00021D0D
		public float Waterlevel
		{
			get
			{
				return this.waterLevel;
			}
			set
			{
				this.waterLevel = value;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x00023B16 File Offset: 0x00021D16
		public bool DirectionalMovement
		{
			get
			{
				return this.directionalMovement;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600345B RID: 13403 RVA: 0x00023B1E File Offset: 0x00021D1E
		// (set) Token: 0x0600345C RID: 13404 RVA: 0x00023B26 File Offset: 0x00021D26
		public Vector3 RawDirection
		{
			get
			{
				return this.rawDirection;
			}
			set
			{
				this.rawDirection = value;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x0600345D RID: 13405 RVA: 0x00023B2F File Offset: 0x00021D2F
		// (set) Token: 0x0600345E RID: 13406 RVA: 0x00023B37 File Offset: 0x00021D37
		public bool Land
		{
			get
			{
				return this.land;
			}
			set
			{
				this.land = value;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x00023B40 File Offset: 0x00021D40
		// (set) Token: 0x06003460 RID: 13408 RVA: 0x00023B48 File Offset: 0x00021D48
		public float StateTime
		{
			get
			{
				return this.stateTime;
			}
			set
			{
				this.stateTime = value;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003461 RID: 13409 RVA: 0x00023B51 File Offset: 0x00021D51
		// (set) Token: 0x06003462 RID: 13410 RVA: 0x0010E618 File Offset: 0x0010C818
		public int Stance
		{
			get
			{
				return this.stance;
			}
			set
			{
				if (this.stance != value)
				{
					this.lastStance = this.stance;
					this.stance = value;
					this.OnStanceChange.Invoke(value);
				}
				if (this.hasStance)
				{
					this.Anim.SetInteger(Hash.Stance, this.stance);
				}
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x00023B59 File Offset: 0x00021D59
		public int LastStance
		{
			get
			{
				return this.lastStance;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x00023B61 File Offset: 0x00021D61
		// (set) Token: 0x06003465 RID: 13413 RVA: 0x00023B69 File Offset: 0x00021D69
		public bool UseShift
		{
			get
			{
				return this.useShift;
			}
			set
			{
				this.useShift = value;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003466 RID: 13414 RVA: 0x00023B72 File Offset: 0x00021D72
		// (set) Token: 0x06003467 RID: 13415 RVA: 0x00023B7A File Offset: 0x00021D7A
		protected Transform Platform
		{
			get
			{
				return this.platform;
			}
			set
			{
				this.platform = value;
			}
		}

		// Token: 0x04003205 RID: 12805
		private int ToogleAmount = 4;

		// Token: 0x04003206 RID: 12806
		protected bool iswakingUp;

		// Token: 0x04003208 RID: 12808
		private IEnumerator StunC;

		// Token: 0x04003209 RID: 12809
		public Dictionary<string, BoolEvent> Inputs;

		// Token: 0x0400320A RID: 12810
		private float YFix;

		// Token: 0x0400320B RID: 12811
		protected float FixDistance;

		// Token: 0x0400320C RID: 12812
		public static List<Animal> Animals;

		// Token: 0x0400320D RID: 12813
		protected Animator anim;

		// Token: 0x0400320E RID: 12814
		protected Rigidbody _rigidbody;

		// Token: 0x0400320F RID: 12815
		private Renderer animalMesh;

		// Token: 0x04003210 RID: 12816
		protected Vector3 movementAxis;

		// Token: 0x04003211 RID: 12817
		protected Vector3 rawDirection;

		// Token: 0x04003212 RID: 12818
		internal Vector3 T_Up;

		// Token: 0x04003213 RID: 12819
		internal Vector3 T_Right;

		// Token: 0x04003214 RID: 12820
		internal Vector3 T_Forward;

		// Token: 0x04003215 RID: 12821
		public static readonly float LowWaterLevel = -1000f;

		// Token: 0x04003216 RID: 12822
		protected bool speed1;

		// Token: 0x04003217 RID: 12823
		protected bool speed2;

		// Token: 0x04003218 RID: 12824
		protected bool speed3;

		// Token: 0x04003219 RID: 12825
		protected bool movementReleased;

		// Token: 0x0400321A RID: 12826
		protected bool jump;

		// Token: 0x0400321B RID: 12827
		protected bool fly;

		// Token: 0x0400321C RID: 12828
		protected bool shift;

		// Token: 0x0400321D RID: 12829
		protected bool down;

		// Token: 0x0400321E RID: 12830
		protected bool up;

		// Token: 0x0400321F RID: 12831
		protected bool dodge;

		// Token: 0x04003220 RID: 12832
		internal bool fall;

		// Token: 0x04003221 RID: 12833
		internal bool fallback;

		// Token: 0x04003222 RID: 12834
		protected bool isInWater;

		// Token: 0x04003223 RID: 12835
		protected bool isInAir;

		// Token: 0x04003224 RID: 12836
		protected bool swim;

		// Token: 0x04003225 RID: 12837
		protected bool underwater;

		// Token: 0x04003226 RID: 12838
		protected bool stun;

		// Token: 0x04003227 RID: 12839
		protected bool action;

		// Token: 0x04003228 RID: 12840
		protected bool stand = true;

		// Token: 0x04003229 RID: 12841
		protected bool backray;

		// Token: 0x0400322A RID: 12842
		protected bool frontray;

		// Token: 0x0400322B RID: 12843
		private float waterLevel = -10f;

		// Token: 0x0400322C RID: 12844
		private bool directionalMovement;

		// Token: 0x0400322D RID: 12845
		protected float vertical;

		// Token: 0x0400322E RID: 12846
		protected float horizontal;

		// Token: 0x0400322F RID: 12847
		private float stateTime;

		// Token: 0x04003230 RID: 12848
		protected float groundSpeed = 1f;

		// Token: 0x04003231 RID: 12849
		protected float slope;

		// Token: 0x04003232 RID: 12850
		protected float idfloat;

		// Token: 0x04003233 RID: 12851
		protected float _Height;

		// Token: 0x04003234 RID: 12852
		protected int idInt;

		// Token: 0x04003235 RID: 12853
		protected int actionID = -1;

		// Token: 0x04003236 RID: 12854
		protected int tired;

		// Token: 0x04003237 RID: 12855
		protected int loops = 1;

		// Token: 0x04003238 RID: 12856
		public int animalTypeID;

		// Token: 0x04003239 RID: 12857
		[SerializeField]
		private int stance;

		// Token: 0x0400323A RID: 12858
		internal Vector3 FixedDeltaPos = Vector3.zero;

		// Token: 0x0400323B RID: 12859
		internal Vector3 DeltaPosition = Vector3.zero;

		// Token: 0x0400323C RID: 12860
		internal Vector3 LastPosition = Vector3.zero;

		// Token: 0x0400323D RID: 12861
		internal Quaternion DeltaRotation = Quaternion.identity;

		// Token: 0x0400323E RID: 12862
		public bool JumpPress;

		// Token: 0x0400323F RID: 12863
		public float JumpHeightMultiplier;

		// Token: 0x04003240 RID: 12864
		public float AirForwardMultiplier;

		// Token: 0x04003241 RID: 12865
		public bool CanDoubleJump;

		// Token: 0x04003242 RID: 12866
		internal int Double_Jump;

		// Token: 0x04003243 RID: 12867
		public LayerMask GroundLayer = 1;

		// Token: 0x04003244 RID: 12868
		public Animal.Ground StartSpeed = Animal.Ground.walk;

		// Token: 0x04003245 RID: 12869
		public float height = 1f;

		// Token: 0x04003246 RID: 12870
		internal Speeds currentSpeed;

		// Token: 0x04003247 RID: 12871
		public Speeds walkSpeed = new Speeds(8f, 4f, 6f);

		// Token: 0x04003248 RID: 12872
		public Speeds trotSpeed = new Speeds(4f, 4f, 6f);

		// Token: 0x04003249 RID: 12873
		public Speeds runSpeed = new Speeds(2f, 4f, 6f);

		// Token: 0x0400324A RID: 12874
		protected float CurrentAnimatorSpeed = 1f;

		// Token: 0x0400324B RID: 12875
		private Transform platform;

		// Token: 0x0400324C RID: 12876
		protected Vector3 platform_Pos;

		// Token: 0x0400324D RID: 12877
		protected float platform_formAngle;

		// Token: 0x0400324E RID: 12878
		public string m_Vertical = "Vertical";

		// Token: 0x0400324F RID: 12879
		public string m_Horizontal = "Horizontal";

		// Token: 0x04003250 RID: 12880
		public string m_UpDown = "UpDown";

		// Token: 0x04003251 RID: 12881
		public string m_Stand = "Stand";

		// Token: 0x04003252 RID: 12882
		public string m_Jump = "_Jump";

		// Token: 0x04003253 RID: 12883
		public string m_Fly = "Fly";

		// Token: 0x04003254 RID: 12884
		public string m_Fall = "Fall";

		// Token: 0x04003255 RID: 12885
		public string m_Attack1 = "Attack1";

		// Token: 0x04003256 RID: 12886
		public string m_Attack2 = "Attack2";

		// Token: 0x04003257 RID: 12887
		public string m_Stunned = "Stunned";

		// Token: 0x04003258 RID: 12888
		public string m_Damaged = "Damaged";

		// Token: 0x04003259 RID: 12889
		public string m_Shift = "Shift";

		// Token: 0x0400325A RID: 12890
		public string m_Death = "Death";

		// Token: 0x0400325B RID: 12891
		public string m_Dodge = "Dodge";

		// Token: 0x0400325C RID: 12892
		public string m_Underwater = "Underwater";

		// Token: 0x0400325D RID: 12893
		public string m_Swim = "Swim";

		// Token: 0x0400325E RID: 12894
		public string m_Action = "Action";

		// Token: 0x0400325F RID: 12895
		public string m_IDAction = "IDAction";

		// Token: 0x04003260 RID: 12896
		public string m_IDFloat = "IDFloat";

		// Token: 0x04003261 RID: 12897
		public string m_IDInt = "IDInt";

		// Token: 0x04003262 RID: 12898
		public string m_Slope = "Slope";

		// Token: 0x04003263 RID: 12899
		public string m_Type = "Type";

		// Token: 0x04003264 RID: 12900
		public string m_SpeedMultiplier = "SpeedMultiplier";

		// Token: 0x04003265 RID: 12901
		public string m_StateTime = "StateTime";

		// Token: 0x04003266 RID: 12902
		public string m_Stance = "Stance";

		// Token: 0x04003267 RID: 12903
		internal int hash_Vertical;

		// Token: 0x04003268 RID: 12904
		internal int hash_Horizontal;

		// Token: 0x04003269 RID: 12905
		internal int hash_UpDown;

		// Token: 0x0400326A RID: 12906
		internal int hash_Stand;

		// Token: 0x0400326B RID: 12907
		internal int hash_Jump;

		// Token: 0x0400326C RID: 12908
		internal int hash_Dodge;

		// Token: 0x0400326D RID: 12909
		internal int hash_Fall;

		// Token: 0x0400326E RID: 12910
		internal int hash_Type;

		// Token: 0x0400326F RID: 12911
		internal int hash_Slope;

		// Token: 0x04003270 RID: 12912
		internal int hash_Shift;

		// Token: 0x04003271 RID: 12913
		internal int hash_Fly;

		// Token: 0x04003272 RID: 12914
		internal int hash_Attack1;

		// Token: 0x04003273 RID: 12915
		internal int hash_Attack2;

		// Token: 0x04003274 RID: 12916
		internal int hash_Death;

		// Token: 0x04003275 RID: 12917
		internal int hash_Damaged;

		// Token: 0x04003276 RID: 12918
		internal int hash_Stunned;

		// Token: 0x04003277 RID: 12919
		internal int hash_IDInt;

		// Token: 0x04003278 RID: 12920
		internal int hash_IDFloat;

		// Token: 0x04003279 RID: 12921
		internal int hash_Swim;

		// Token: 0x0400327A RID: 12922
		internal int hash_Underwater;

		// Token: 0x0400327B RID: 12923
		internal int hash_IDAction;

		// Token: 0x0400327C RID: 12924
		internal int hash_Action;

		// Token: 0x0400327D RID: 12925
		internal int hash_StateTime;

		// Token: 0x0400327E RID: 12926
		internal int hash_Stance;

		// Token: 0x0400327F RID: 12927
		[HideInInspector]
		private bool hasFly;

		// Token: 0x04003280 RID: 12928
		[HideInInspector]
		private bool hasDodge;

		// Token: 0x04003281 RID: 12929
		[HideInInspector]
		private bool hasSlope;

		// Token: 0x04003282 RID: 12930
		[HideInInspector]
		private bool hasStun;

		// Token: 0x04003283 RID: 12931
		[HideInInspector]
		private bool hasAttack2;

		// Token: 0x04003284 RID: 12932
		[HideInInspector]
		private bool hasUpDown;

		// Token: 0x04003285 RID: 12933
		[HideInInspector]
		private bool hasUnderwater;

		// Token: 0x04003286 RID: 12934
		[HideInInspector]
		private bool hasSwim;

		// Token: 0x04003287 RID: 12935
		[HideInInspector]
		private bool hasStateTime;

		// Token: 0x04003288 RID: 12936
		[HideInInspector]
		private bool hasStance;

		// Token: 0x04003289 RID: 12937
		public float airRotation = 100f;

		// Token: 0x0400328A RID: 12938
		public bool AirControl;

		// Token: 0x0400328B RID: 12939
		public float airSmoothness = 2f;

		// Token: 0x0400328C RID: 12940
		internal Vector3 AirControlDir;

		// Token: 0x0400328D RID: 12941
		public float movementS1 = 1f;

		// Token: 0x0400328E RID: 12942
		public float movementS2 = 2f;

		// Token: 0x0400328F RID: 12943
		public float movementS3 = 3f;

		// Token: 0x04003290 RID: 12944
		[Range(0f, 90f)]
		public float maxAngleSlope = 45f;

		// Token: 0x04003291 RID: 12945
		public bool SlowSlopes = true;

		// Token: 0x04003292 RID: 12946
		[Range(0f, 100f)]
		public int GotoSleep;

		// Token: 0x04003293 RID: 12947
		public float SnapToGround = 20f;

		// Token: 0x04003294 RID: 12948
		public float AlingToGround = 30f;

		// Token: 0x04003295 RID: 12949
		public float FallRayDistance = 0.1f;

		// Token: 0x04003296 RID: 12950
		public float BackFallRayDistance = 0.5f;

		// Token: 0x04003297 RID: 12951
		public float FallRayMultiplier = 1f;

		// Token: 0x04003298 RID: 12952
		public bool SmoothVertical = true;

		// Token: 0x04003299 RID: 12953
		public bool IgnoreYDir;

		// Token: 0x0400329A RID: 12954
		public float TurnMultiplier = 100f;

		// Token: 0x0400329B RID: 12955
		public float waterLine;

		// Token: 0x0400329C RID: 12956
		public Speeds swimSpeed = new Speeds(8f, 4f, 6f);

		// Token: 0x0400329D RID: 12957
		internal int WaterLayer;

		// Token: 0x0400329E RID: 12958
		public bool canSwim = true;

		// Token: 0x0400329F RID: 12959
		public bool CanGoUnderWater;

		// Token: 0x040032A0 RID: 12960
		[Range(0f, 90f)]
		public float bank;

		// Token: 0x040032A1 RID: 12961
		public Speeds underWaterSpeed = new Speeds(8f, 4f, 6f);

		// Token: 0x040032A2 RID: 12962
		public Speeds flySpeed;

		// Token: 0x040032A3 RID: 12963
		public bool StartFlying;

		// Token: 0x040032A4 RID: 12964
		public bool canFly;

		// Token: 0x040032A5 RID: 12965
		public bool land = true;

		// Token: 0x040032A6 RID: 12966
		protected float LastGroundSpeed;

		// Token: 0x040032A7 RID: 12967
		public bool LockUp;

		// Token: 0x040032A8 RID: 12968
		public float life = 100f;

		// Token: 0x040032A9 RID: 12969
		public float defense;

		// Token: 0x040032AA RID: 12970
		public float damageDelay = 0.75f;

		// Token: 0x040032AB RID: 12971
		public float damageInterrupt = 0.5f;

		// Token: 0x040032AC RID: 12972
		public int TotalAttacks = 3;

		// Token: 0x040032AD RID: 12973
		public int activeAttack = -1;

		// Token: 0x040032AE RID: 12974
		public float attackStrength = 10f;

		// Token: 0x040032AF RID: 12975
		public float attackDelay = 0.5f;

		// Token: 0x040032B0 RID: 12976
		public bool inmune;

		// Token: 0x040032B1 RID: 12977
		protected bool attack1;

		// Token: 0x040032B2 RID: 12978
		protected bool attack2;

		// Token: 0x040032B3 RID: 12979
		protected bool isAttacking;

		// Token: 0x040032B4 RID: 12980
		protected bool isTakingDamage;

		// Token: 0x040032B5 RID: 12981
		protected bool damaged;

		// Token: 0x040032B6 RID: 12982
		protected bool death;

		// Token: 0x040032B7 RID: 12983
		protected List<AttackTrigger> Attack_Triggers;

		// Token: 0x040032B8 RID: 12984
		public float animatorSpeed = 1f;

		// Token: 0x040032B9 RID: 12985
		public float upDownSmoothness = 2f;

		// Token: 0x040032BA RID: 12986
		public bool debug = true;

		// Token: 0x040032BB RID: 12987
		internal RaycastHit hit_Hip;

		// Token: 0x040032BC RID: 12988
		internal RaycastHit hit_Chest;

		// Token: 0x040032BD RID: 12989
		internal RaycastHit WaterHitCenter;

		// Token: 0x040032BE RID: 12990
		internal RaycastHit FallRayCast;

		// Token: 0x040032BF RID: 12991
		protected Vector3 fall_Point;

		// Token: 0x040032C0 RID: 12992
		protected Vector3 _hitDirection;

		// Token: 0x040032C1 RID: 12993
		protected Vector3 UpVector = Vector3.up;

		// Token: 0x040032C2 RID: 12994
		protected float scaleFactor = 1f;

		// Token: 0x040032C3 RID: 12995
		protected List<Pivots> pivots = new List<Pivots>();

		// Token: 0x040032C4 RID: 12996
		protected Pivots pivot_Chest;

		// Token: 0x040032C5 RID: 12997
		protected Pivots pivot_Hip;

		// Token: 0x040032C6 RID: 12998
		protected Pivots pivot_Water;

		// Token: 0x040032C7 RID: 12999
		public int PivotsRayInterval = 1;

		// Token: 0x040032C8 RID: 13000
		public int FallRayInterval = 3;

		// Token: 0x040032C9 RID: 13001
		public int WaterRayInterval = 5;

		// Token: 0x040032CA RID: 13002
		public UnityEvent OnJump;

		// Token: 0x040032CB RID: 13003
		public UnityEvent OnAttack;

		// Token: 0x040032CC RID: 13004
		public FloatEvent OnGetDamaged;

		// Token: 0x040032CD RID: 13005
		public UnityEvent OnDeathE;

		// Token: 0x040032CE RID: 13006
		public UnityEvent OnAction;

		// Token: 0x040032CF RID: 13007
		public UnityEvent OnSwim;

		// Token: 0x040032D0 RID: 13008
		public BoolEvent OnFly;

		// Token: 0x040032D1 RID: 13009
		public UnityEvent OnUnderWater;

		// Token: 0x040032D2 RID: 13010
		public IntEvent OnAnimationChange;

		// Token: 0x040032D3 RID: 13011
		public IntEvent OnStanceChange;

		// Token: 0x040032D4 RID: 13012
		public UnityEvent OnSyncAnimator;

		// Token: 0x040032D5 RID: 13013
		private static RaycastHit NULLRayCast = default(RaycastHit);

		// Token: 0x040032D6 RID: 13014
		private List<Collider> _col_ = new List<Collider>();

		// Token: 0x040032D7 RID: 13015
		[HideInInspector]
		public int FrameCounter;

		// Token: 0x040032D8 RID: 13016
		public BoolEvent OnMovementReleased = new BoolEvent();

		// Token: 0x040032D9 RID: 13017
		private float swimChanged;

		// Token: 0x040032DA RID: 13018
		[SerializeField]
		private bool useShift = true;

		// Token: 0x040032DB RID: 13019
		public int CurrentAnimState;

		// Token: 0x040032DC RID: 13020
		private int lastAnimTag;

		// Token: 0x040032DD RID: 13021
		private Transform _transform;

		// Token: 0x040032DE RID: 13022
		public int NextAnimState;

		// Token: 0x040032DF RID: 13023
		private int lastStance;

		// Token: 0x040032E0 RID: 13024
		[HideInInspector]
		public bool EditorGeneral = true;

		// Token: 0x040032E1 RID: 13025
		[HideInInspector]
		public bool EditorGround = true;

		// Token: 0x040032E2 RID: 13026
		[HideInInspector]
		public bool EditorWater = true;

		// Token: 0x040032E3 RID: 13027
		[HideInInspector]
		public bool EditorAir = true;

		// Token: 0x040032E4 RID: 13028
		[HideInInspector]
		public bool EditorAdvanced = true;

		// Token: 0x040032E5 RID: 13029
		[HideInInspector]
		public bool EditorAirControl = true;

		// Token: 0x040032E6 RID: 13030
		[HideInInspector]
		public bool EditorAttributes = true;

		// Token: 0x040032E7 RID: 13031
		[HideInInspector]
		public bool EditorEvents;

		// Token: 0x040032E8 RID: 13032
		[HideInInspector]
		public bool EditorAnimatorParameters;

		// Token: 0x020006F0 RID: 1776
		public enum Ground
		{
			// Token: 0x040032EA RID: 13034
			walk = 1,
			// Token: 0x040032EB RID: 13035
			trot,
			// Token: 0x040032EC RID: 13036
			run
		}
	}
}
