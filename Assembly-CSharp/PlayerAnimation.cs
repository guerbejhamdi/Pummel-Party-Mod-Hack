using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class PlayerAnimation : MonoBehaviour
{
	// Token: 0x06001C45 RID: 7237 RVA: 0x00014B26 File Offset: 0x00012D26
	public void Slam()
	{
		if (this.anim.isActiveAndEnabled)
		{
			this.anim.SetTrigger("Slam");
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x00014B45 File Offset: 0x00012D45
	public void StartStaffCasting()
	{
		if (this.anim.isActiveAndEnabled)
		{
			this.anim.SetTrigger("StaffSpellCast");
		}
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x00014B64 File Offset: 0x00012D64
	public void FireStaffCast()
	{
		if (this.anim.isActiveAndEnabled)
		{
			this.anim.SetTrigger("StaffSpellFire");
		}
	}

	// Token: 0x1700033D RID: 829
	// (set) Token: 0x06001C48 RID: 7240 RVA: 0x00014B83 File Offset: 0x00012D83
	public float Velocity
	{
		set
		{
			this.velocity = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetFloat("Velocity", this.velocity);
			}
		}
	}

	// Token: 0x1700033E RID: 830
	// (set) Token: 0x06001C49 RID: 7241 RVA: 0x00014BAF File Offset: 0x00012DAF
	public float VelocityY
	{
		set
		{
			this.velocityY = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetFloat("VelocityY", this.velocityY);
			}
		}
	}

	// Token: 0x1700033F RID: 831
	// (set) Token: 0x06001C4A RID: 7242 RVA: 0x00014BDB File Offset: 0x00012DDB
	public bool Grounded
	{
		set
		{
			this.grounded = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("Grounded", this.grounded);
			}
		}
	}

	// Token: 0x17000340 RID: 832
	// (set) Token: 0x06001C4B RID: 7243 RVA: 0x00014C07 File Offset: 0x00012E07
	public bool Carrying
	{
		set
		{
			this.carrying = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("Carrying", this.carrying);
			}
		}
	}

	// Token: 0x17000341 RID: 833
	// (set) Token: 0x06001C4C RID: 7244 RVA: 0x00014C33 File Offset: 0x00012E33
	public bool PistolGripRight
	{
		set
		{
			this.pistolGripRight = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("PistolGripRight", this.pistolGripRight);
			}
		}
	}

	// Token: 0x17000342 RID: 834
	// (set) Token: 0x06001C4D RID: 7245 RVA: 0x00014C5F File Offset: 0x00012E5F
	public int CarryingSide
	{
		set
		{
			this.carryingSide = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetInteger("CarryingSide", this.carryingSide);
			}
		}
	}

	// Token: 0x17000343 RID: 835
	// (set) Token: 0x06001C4E RID: 7246 RVA: 0x00014C8B File Offset: 0x00012E8B
	public bool Surfing
	{
		set
		{
			this.surfing = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("Surfing", this.surfing);
			}
		}
	}

	// Token: 0x17000344 RID: 836
	// (set) Token: 0x06001C4F RID: 7247 RVA: 0x00014CB7 File Offset: 0x00012EB7
	public bool Driving
	{
		set
		{
			this.driving = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("Driving", this.driving);
			}
		}
	}

	// Token: 0x17000345 RID: 837
	// (set) Token: 0x06001C50 RID: 7248 RVA: 0x00014CE3 File Offset: 0x00012EE3
	public bool Stunned
	{
		set
		{
			this.stunned = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("Stunned", this.stunned);
			}
		}
	}

	// Token: 0x17000346 RID: 838
	// (set) Token: 0x06001C51 RID: 7249 RVA: 0x00014D0F File Offset: 0x00012F0F
	public bool HoldingRifle
	{
		set
		{
			this.holdingRifle = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("HoldingRifle", this.holdingRifle);
			}
		}
	}

	// Token: 0x17000347 RID: 839
	// (set) Token: 0x06001C52 RID: 7250 RVA: 0x00014D3B File Offset: 0x00012F3B
	public bool Crouching
	{
		set
		{
			this.crouching = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("Crouching", this.crouching);
			}
		}
	}

	// Token: 0x17000348 RID: 840
	// (set) Token: 0x06001C53 RID: 7251 RVA: 0x00014D67 File Offset: 0x00012F67
	public float ShotgunStrength
	{
		set
		{
			this.shotgunStrength = value;
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetFloat("ShotgunStrength", this.shotgunStrength);
			}
		}
	}

	// Token: 0x17000349 RID: 841
	// (set) Token: 0x06001C54 RID: 7252 RVA: 0x000BBAB4 File Offset: 0x000B9CB4
	public Vector2 MovementAxis
	{
		set
		{
			this.movementAxis = value;
			this.movementAxisZero = (this.movementAxis.sqrMagnitude < 0.01f);
			if (this.anim.isActiveAndEnabled)
			{
				this.anim.SetBool("MovementAxisZero", this.movementAxisZero);
			}
		}
	}

	// Token: 0x1700034A RID: 842
	// (set) Token: 0x06001C55 RID: 7253 RVA: 0x00014D93 File Offset: 0x00012F93
	public byte MovementAxisByte
	{
		set
		{
			this.MovementAxis = this.movementAxisList[(int)value];
		}
	}

	// Token: 0x1700034B RID: 843
	// (set) Token: 0x06001C56 RID: 7254 RVA: 0x00014DA7 File Offset: 0x00012FA7
	public float SpineRotation
	{
		set
		{
			this.spineRotation = value;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06001C57 RID: 7255 RVA: 0x00014DB0 File Offset: 0x00012FB0
	public float PlayerRotation
	{
		get
		{
			return this.playerRotation;
		}
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x000BBB04 File Offset: 0x000B9D04
	public PlayerAnimation()
	{
		this.listeners = new Dictionary<AnimationEventType, List<AnimationEventListener>>();
		int num = Enum.GetNames(typeof(AnimationEventType)).Length;
		for (int i = 0; i < num; i++)
		{
			this.listeners.Add((AnimationEventType)i, new List<AnimationEventListener>());
		}
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x00014DB8 File Offset: 0x00012FB8
	public void SetHeadScale(float scale)
	{
		this.playerBones[32].localScale = Vector3.one * scale;
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x000BBCA8 File Offset: 0x000B9EA8
	public void Setup()
	{
		if (this.setup)
		{
			return;
		}
		this.setup = true;
		this.anim = base.GetComponent<Animator>();
		this.playerBones[0] = base.transform.Find("Armature");
		this.playerBones[1] = this.playerBones[0].Find("mixamorig:Hips");
		this.playerBones[2] = this.playerBones[1].Find("mixamorig:LeftUpLeg");
		this.playerBones[3] = this.playerBones[2].Find("mixamorig:LeftLeg");
		this.playerBones[4] = this.playerBones[3].Find("mixamorig:LeftFoot");
		this.playerBones[5] = this.playerBones[4].Find("mixamorig:LeftToeBase");
		this.playerBones[6] = this.playerBones[5].Find("mixamorig:LeftToe_End");
		this.playerBones[7] = this.playerBones[6].Find("mixamorig:LeftToe_End_end");
		this.playerBones[8] = this.playerBones[1].Find("mixamorig:RightUpLeg");
		this.playerBones[9] = this.playerBones[8].Find("mixamorig:RightLeg");
		this.playerBones[10] = this.playerBones[9].Find("mixamorig:RightFoot");
		this.playerBones[11] = this.playerBones[10].Find("mixamorig:RightToeBase");
		this.playerBones[12] = this.playerBones[11].Find("mixamorig:RightToe_End");
		this.playerBones[13] = this.playerBones[12].Find("mixamorig:RightToe_End_end");
		this.playerBones[14] = this.playerBones[1].Find("mixamorig:Spine");
		this.playerBones[15] = this.playerBones[14].Find("mixamorig:Spine1");
		this.playerBones[16] = this.playerBones[15].Find("mixamorig:Spine2");
		this.playerBones[17] = this.playerBones[16].Find("mixamorig:LeftShoulder");
		this.playerBones[18] = this.playerBones[17].Find("mixamorig:LeftArm");
		this.playerBones[19] = this.playerBones[18].Find("mixamorig:LeftForeArm");
		this.playerBones[20] = this.playerBones[19].Find("mixamorig:LeftHand");
		this.playerBones[21] = this.playerBones[20].Find("mixamorig:LeftHandIndex1");
		this.playerBones[22] = this.playerBones[21].Find("mixamorig:LeftHandIndex2");
		this.playerBones[23] = this.playerBones[22].Find("mixamorig:LeftHandIndex3");
		this.playerBones[24] = this.playerBones[23].Find("mixamorig:LeftHandIndex4");
		this.playerBones[25] = this.playerBones[24].Find("mixamorig:LeftHandIndex4End");
		this.playerBones[31] = this.playerBones[16].Find("mixamorig:Neck");
		this.playerBones[32] = this.playerBones[31].Find("mixamorig:Head");
		this.playerBones[33] = this.playerBones[32].Find("mixamorig:HeadTop_End");
		this.playerBones[34] = this.playerBones[33].Find("mixamorig:HeadTop_End_end");
		this.playerBones[26] = this.playerBones[20].Find("mixamorig:LeftHandThumb1");
		this.playerBones[27] = this.playerBones[26].Find("mixamorig:LeftHandThumb2");
		this.playerBones[28] = this.playerBones[27].Find("mixamorig:LeftHandThumb3");
		this.playerBones[29] = this.playerBones[28].Find("mixamorig:LeftHandThumb4");
		this.playerBones[30] = this.playerBones[29].Find("mixamorig:LeftHandThumb4End");
		this.playerBones[35] = this.playerBones[16].Find("mixamorig:RightShoulder");
		this.playerBones[36] = this.playerBones[35].Find("mixamorig:RightArm");
		this.playerBones[37] = this.playerBones[36].Find("mixamorig:RightForeArm");
		this.playerBones[38] = this.playerBones[37].Find("mixamorig:RightHand");
		this.playerBones[39] = this.playerBones[38].Find("mixamorig:RightHandIndex1");
		this.playerBones[40] = this.playerBones[39].Find("mixamorig:RightHandIndex2");
		this.playerBones[41] = this.playerBones[40].Find("mixamorig:RightHandIndex3");
		this.playerBones[42] = this.playerBones[41].Find("mixamorig:RightHandIndex4");
		this.playerBones[43] = this.playerBones[42].Find("mixamorig:RightHandIndex4End");
		this.playerBones[44] = this.playerBones[38].Find("mixamorig:RightHandThumb1");
		this.playerBones[45] = this.playerBones[44].Find("mixamorig:RightHandThumb2");
		this.playerBones[46] = this.playerBones[45].Find("mixamorig:RightHandThumb3");
		this.playerBones[47] = this.playerBones[46].Find("mixamorig:RightHandThumb4");
		this.playerBones[48] = this.playerBones[47].Find("mixamorig:RightHandThumb4End");
		this.playerTransform = base.transform;
		this.spine01 = this.playerBones[15];
		this.root = this.playerBones[0];
		this.ballL = this.playerBones[5];
		this.ballR = this.playerBones[11];
		this.Reset();
		if (base.GetComponentInParent<BoardPlayer>() != null && BoardModifier.IsBoardModifierActive(BoardModifierID.BigHead))
		{
			this.BigHead = true;
		}
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Reset()
	{
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x00014DD3 File Offset: 0x00012FD3
	public void Awake()
	{
		this.Setup();
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x000BC280 File Offset: 0x000BA480
	public void Start()
	{
		if (this.m_isPropCharacter)
		{
			if (this.m_possibleHats != null && this.m_possibleHats.Length >= 1)
			{
				int num = UnityEngine.Random.Range(0, this.m_possibleHats.Length - 1);
				this.SetPlayerHat(this.m_possibleHats[num]);
			}
			if (this.m_possibleColors == null || this.m_possibleColors.Length == 0)
			{
				int index = UnityEngine.Random.Range(0, GameManager.GetPlayerColorCount() - 1);
				this.SetPlayerColor(GameManager.GetColorAtIndex(index));
				this.SetPlayerColor(new PlayerColor
				{
					skinColor1 = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 0f, 0.4f, 0.6f, 1f, 1f)
				});
			}
			else
			{
				int num2 = UnityEngine.Random.Range(0, this.m_possibleColors.Length - 1);
				this.SetPlayerColor(this.m_possibleColors[num2]);
			}
			int index2 = UnityEngine.Random.Range(0, GameManager.GetPlayerSkinCount() - 1);
			this.SetSkin(GameManager.GetSkinAtIndex(index2));
			this.Animator.SetInteger("RandomIndex", UnityEngine.Random.Range(1, 4));
			this.Animator.SetFloat("Time", UnityEngine.Random.value);
		}
		if (this.testCape)
		{
			this.SetupCape((byte)(PlayerAnimation.m_cape % 5));
			PlayerAnimation.m_cape++;
		}
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x000BC3C8 File Offset: 0x000BA5C8
	public void LateUpdate()
	{
		if (this.spine01 != null)
		{
			Quaternion rhs = Quaternion.Euler(this.spineRotation - this.spineZOffset, this.spineYOffset, 0f);
			this.spine01.rotation = this.spine01.rotation * rhs;
		}
		if (this.playerTransform != null && this.usePlayerRotation)
		{
			this.playerTransform.rotation = Quaternion.Euler(0f, this.playerRotation, 0f);
			this.curMovementAxis = Vector2.MoveTowards(this.curMovementAxis, this.movementAxis, this.movementLerpSpeed * Time.deltaTime);
		}
		if (this.BigHead)
		{
			this.SetHeadScale(2f);
		}
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x000BC48C File Offset: 0x000BA68C
	public void Update()
	{
		if (this.playerTransform != null && this.usePlayerRotation)
		{
			this.playerRotation = Mathf.MoveTowardsAngle(this.playerRotation, this.playerRotationTarget, this.playerRotationSpeed * Time.deltaTime);
		}
		if (Time.time - this.lastIdleAnimCheck > this.idleAnimCheckInterval && this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle0"))
		{
			this.lastIdleAnimCheck = Time.time;
			if (UnityEngine.Random.value > 0.98f)
			{
				this.anim.SetTrigger((UnityEngine.Random.value > 0.5f) ? "Idle1" : "Idle2");
			}
		}
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x00014DDB File Offset: 0x00012FDB
	public GameObject SpawnRagdoll(Vector3 force)
	{
		PlayerRagdoll ragdoll = PlayerRagdoll.GetRagdoll();
		ragdoll.Spawn(base.transform.root);
		ragdoll.ApplyForce(force, 100f);
		ragdoll.anim.SetPlayer(this.player);
		return ragdoll.gameObject;
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x000BC53C File Offset: 0x000BA73C
	public void UpdateAnimationState()
	{
		if (!this.anim.isInitialized || !base.isActiveAndEnabled)
		{
			return;
		}
		this.anim.SetFloat("MovementAxisX", this.curMovementAxis.x);
		this.anim.SetFloat("MovementAxisY", this.curMovementAxis.y);
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x00014E15 File Offset: 0x00013015
	public void SetPlayerRotation(float angle)
	{
		this.playerRotationTarget = angle;
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x00014E1E File Offset: 0x0001301E
	public void FireJumpTrigger()
	{
		this.anim.SetTrigger("Jump");
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x00014E30 File Offset: 0x00013030
	public void FireThrowObjectTrigger()
	{
		this.anim.SetTrigger("ThrowObject");
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x00014E42 File Offset: 0x00013042
	public void FirePunchTrigger(bool right)
	{
		this.anim.SetTrigger(right ? "RightPunch" : "LeftPunch");
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x00014E5E File Offset: 0x0001305E
	public void FireShootTrigger()
	{
		this.anim.SetTrigger("ShootShotgun");
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x00014E70 File Offset: 0x00013070
	public void FireDeathTrigger()
	{
		this.anim.SetTrigger("Death");
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x00014E82 File Offset: 0x00013082
	public void FireReviveTrigger()
	{
		this.anim.SetTrigger("Revive");
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x00014E94 File Offset: 0x00013094
	public void FireFallingTrigger()
	{
		this.anim.SetTrigger("Falling");
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x00014EA6 File Offset: 0x000130A6
	public void FireFallTrigger()
	{
		this.anim.SetTrigger("Fall");
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x000BC598 File Offset: 0x000BA798
	public void FireHitTrigger()
	{
		if (!this.wasHit)
		{
			this.wasHit = true;
			this.anim.SetTrigger("Hit" + UnityEngine.Random.Range(0, 3).ToString());
		}
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x00014EB8 File Offset: 0x000130B8
	public void EndHit()
	{
		this.wasHit = false;
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x00014EC1 File Offset: 0x000130C1
	public void SetPlayer(int playerID)
	{
		this.SetPlayer(GameManager.GetPlayerAt(playerID));
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x000BC5D8 File Offset: 0x000BA7D8
	public void SetPlayer(GamePlayer player)
	{
		this.Setup();
		this.player = player;
		this.SetSkin(this.player.Skin);
		this.SetupCape(this.player.CapeIndex);
		this.SetPlayerColor(this.player.Color);
		this.SetPlayerHat(this.player.Hat);
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x000BC638 File Offset: 0x000BA838
	public void SetSkin(PlayerSkin skin)
	{
		if (this.curSkin.skinName == skin.skinName)
		{
			return;
		}
		this.curSkin = skin;
		for (int i = 0; i < this.accessories.Count; i++)
		{
			UnityEngine.Object.Destroy(this.accessories[i]);
		}
		this.accessories.Clear();
		this.skinnedRenderer.sharedMesh = skin.editedMesh;
		this.skinnedRenderer.material = new Material(skin.mat);
		for (int j = 0; j < skin.accessories.Length; j++)
		{
			SkinAccessory skinAccessory = skin.accessories[j];
			GameObject gameObject = new GameObject(skinAccessory.mesh.name, new Type[]
			{
				typeof(MeshFilter),
				typeof(MeshRenderer)
			});
			Transform transform = this.playerBones[(int)skinAccessory.bone];
			gameObject.GetComponent<MeshFilter>().mesh = skinAccessory.mesh;
			gameObject.GetComponent<MeshRenderer>().material = skinAccessory.mat;
			gameObject.transform.parent = transform;
			gameObject.layer = transform.gameObject.layer;
			gameObject.transform.localPosition = skinAccessory.position;
			gameObject.transform.localRotation = Quaternion.Euler(skinAccessory.rotation);
			gameObject.transform.localScale = skinAccessory.scale;
			this.accessories.Add(gameObject);
		}
		this.SetPlayerColor(this.curColor);
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x000BC7B8 File Offset: 0x000BA9B8
	public void SetupCape(byte capeIndex)
	{
		if (!this.spawnCape)
		{
			return;
		}
		if (this.capeController == null && this.spawnCape)
		{
			this.capeController = UnityEngine.Object.Instantiate<GameObject>(this.capePrefab, this.GetBone(PlayerBone.Root)).GetComponent<CapeController>();
			this.capeController.playerAnim = this;
		}
		if (this.curCape != capeIndex)
		{
			this.capeController.Setup((CapeType)capeIndex);
			this.curCape = capeIndex;
		}
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x000BC82C File Offset: 0x000BAA2C
	public void SetPlayerHat(CharacterHat hat)
	{
		if (hat == this.curHat || this.ignoreHat)
		{
			return;
		}
		if (this.hatObject != null)
		{
			UnityEngine.Object.Destroy(this.hatObject);
		}
		this.curHat = hat;
		if (this.curRequest != null)
		{
			this.curRequest.completed -= this.Req_completed;
			this.curRequest = null;
		}
		this.curRequest = Resources.LoadAsync<GameObject>(hat.prefabName);
		this.curRequest.completed += this.Req_completed;
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x000BC8C0 File Offset: 0x000BAAC0
	private void Req_completed(AsyncOperation obj)
	{
		Transform transform = this.playerBones[(int)this.curHat.bone];
		this.hatObject = UnityEngine.Object.Instantiate<GameObject>((GameObject)this.curRequest.asset);
		this.hatObject.SetLayer(transform.gameObject.layer, true);
		this.hatObject.transform.parent = transform;
		this.hatObject.layer = transform.gameObject.layer;
		this.hatObject.transform.localPosition = this.curHat.position;
		this.hatObject.transform.localRotation = Quaternion.Euler(this.curHat.rotation);
		this.hatObject.transform.localScale = this.curHat.scale;
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x00014ECF File Offset: 0x000130CF
	public void SetPlayerColor(PlayerColor color)
	{
		this.Setup();
		this.SetPlayerColor(color, this.skinnedRenderer);
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x000BC990 File Offset: 0x000BAB90
	public void SetPlayerColor(PlayerColor color, SkinnedMeshRenderer mr)
	{
		this.Setup();
		mr.material = new Material(mr.sharedMaterial)
		{
			color = color.skinColor1
		};
		this.curColor = color;
		foreach (OutlineSource outlineSource in base.transform.root.gameObject.GetComponentsInChildren<OutlineSource>(true))
		{
			if (outlineSource != null)
			{
				outlineSource.outlineColor = color.uiColor;
			}
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x00014EE4 File Offset: 0x000130E4
	public Transform GetBone(PlayerBone bone)
	{
		return this.playerBones[(int)bone];
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000BCA08 File Offset: 0x000BAC08
	public byte GetMovementAxis()
	{
		if (this.movementAxis.sqrMagnitude < 0.25f)
		{
			return 8;
		}
		if (this.movementAxis.y > 0.5f && Mathf.Abs(this.movementAxis.x) < 0.5f)
		{
			return 0;
		}
		if (this.movementAxis.y < -0.5f && Mathf.Abs(this.movementAxis.x) < 0.5f)
		{
			return 1;
		}
		if (this.movementAxis.x < -0.5f && Mathf.Abs(this.movementAxis.y) < 0.5f)
		{
			return 2;
		}
		if (this.movementAxis.x > 0.5f && Mathf.Abs(this.movementAxis.y) < 0.5f)
		{
			return 3;
		}
		if (this.movementAxis.x > 0.5f && this.movementAxis.y > 0.5f)
		{
			return 4;
		}
		if (this.movementAxis.x < -0.5f && this.movementAxis.y > 0.5f)
		{
			return 5;
		}
		if (this.movementAxis.x > 0.5f && this.movementAxis.y < -0.5f)
		{
			return 6;
		}
		if (this.movementAxis.x < -0.5f && this.movementAxis.y < -0.5f)
		{
			return 7;
		}
		return 0;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x000BCB70 File Offset: 0x000BAD70
	public void FireEvent(PlayerAnimationEvent anim_event)
	{
		List<AnimationEventListener> list = this.listeners[anim_event.event_type];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != null)
			{
				list[i](anim_event);
			}
		}
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x00014EEE File Offset: 0x000130EE
	public void RegisterListener(AnimationEventListener listener, AnimationEventType type)
	{
		this.listeners[type].Add(listener);
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x000BCBB8 File Offset: 0x000BADB8
	public void UnregisterListener(AnimationEventListener listener, AnimationEventType type)
	{
		for (int i = this.listeners[type].Count - 1; i >= 0; i--)
		{
			if (this.listeners[type][i] == listener)
			{
				this.listeners[type].RemoveAt(i);
			}
		}
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x000BCC10 File Offset: 0x000BAE10
	public void PlayerStep(int foot_index)
	{
		if (this.ballL != null && this.ballR != null && this.grounded)
		{
			if (this.footStepSounds.Length != 0)
			{
				if (this.pointSound)
				{
					AudioSystem.PlayOneShot(this.footStepSounds[UnityEngine.Random.Range(0, this.footStepSounds.Length)], this.root.position, this.footStepVolume, AudioRolloffMode.Logarithmic, 20f, 50f, 0f);
				}
				else
				{
					AudioSystem.PlayOneShot(this.footStepSounds[UnityEngine.Random.Range(0, this.footStepSounds.Length)], this.footStepVolume, 0f, 1f);
				}
			}
			if (this.footStepEffects.Length != 0 && this.doFootStepEffects)
			{
				int num = UnityEngine.Random.Range(0, this.footStepEffects.Length);
				Vector3 position = (foot_index == 0) ? this.ballL.position : this.ballR.position;
				UnityEngine.Object.Instantiate<GameObject>(this.footStepEffects[num], position, Quaternion.LookRotation(Vector3.up));
			}
		}
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.FootStep));
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000BCD24 File Offset: 0x000BAF24
	public void PlayerLand()
	{
		if (this.root != null)
		{
			if (this.landSounds.Length != 0)
			{
				AudioSystem.PlayOneShot(this.landSounds[UnityEngine.Random.Range(0, this.landSounds.Length)], this.root.position, 1f, AudioRolloffMode.Logarithmic, 20f, 50f, 0.1f);
			}
			if (this.landEffects.Length != 0)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.landEffects[UnityEngine.Random.Range(0, this.landEffects.Length)], this.root.position, Quaternion.LookRotation(Vector3.up));
			}
		}
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.Land));
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x00014F02 File Offset: 0x00013102
	public void PlayerJumpPeek()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.JumpPeek));
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x00014F10 File Offset: 0x00013110
	public void PlayerThrowRelease()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.ThrowRelease));
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x00014F1E File Offset: 0x0001311E
	public void PlayerPunchImpact()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.PunchImpact));
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x00014F2C File Offset: 0x0001312C
	public void OnDeathAnimationFinished()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.DeathFinish));
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00014F3A File Offset: 0x0001313A
	public void OnWarlockKnockbackFinish()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.WarlockKnockbackFinish));
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x00014F48 File Offset: 0x00013148
	public void OnWarlockAttackFinish()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.WarlockAttackFinish));
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00014F56 File Offset: 0x00013156
	public void OnBarnBrawlShotgunShot()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.BarnBrawlShotgunShot));
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x00014F65 File Offset: 0x00013165
	public void OnPickupHalf()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.BarnBrawlPickup));
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x00014F73 File Offset: 0x00013173
	public void OnSwordHit()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.SwordHit));
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00014F82 File Offset: 0x00013182
	public void OnSlashEnd()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.SlashEnd));
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00014F91 File Offset: 0x00013191
	public void OnStaffRaise()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.StaffRaise));
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x00014FA0 File Offset: 0x000131A0
	public void OnStaffHit()
	{
		this.FireEvent(new PlayerAnimationEvent(AnimationEventType.StaffHit));
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x00014FAF File Offset: 0x000131AF
	public void SetPlayerRotationImmediate(float y_rotation)
	{
		this.playerRotation = y_rotation;
		this.playerRotationTarget = y_rotation;
		this.playerTransform.rotation = Quaternion.Euler(0f, y_rotation, 0f);
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06001C89 RID: 7305 RVA: 0x00014FDA File Offset: 0x000131DA
	public Animator Animator
	{
		get
		{
			return this.anim;
		}
	}

	// Token: 0x04001E5F RID: 7775
	public bool testCape;

	// Token: 0x04001E60 RID: 7776
	public SkinnedMeshRenderer skinnedRenderer;

	// Token: 0x04001E61 RID: 7777
	public GameObject capePrefab;

	// Token: 0x04001E62 RID: 7778
	public bool spawnCape = true;

	// Token: 0x04001E63 RID: 7779
	public bool ignoreHat;

	// Token: 0x04001E64 RID: 7780
	public bool usePlayerRotation = true;

	// Token: 0x04001E65 RID: 7781
	public float playerRotationSpeed = 720f;

	// Token: 0x04001E66 RID: 7782
	public float movementLerpSpeed = 2f;

	// Token: 0x04001E67 RID: 7783
	public float impactVelocity = -12f;

	// Token: 0x04001E68 RID: 7784
	public float spineYOffset;

	// Token: 0x04001E69 RID: 7785
	public float spineZOffset;

	// Token: 0x04001E6A RID: 7786
	public bool doFootStepEffects = true;

	// Token: 0x04001E6B RID: 7787
	public GameObject[] footStepEffects;

	// Token: 0x04001E6C RID: 7788
	public GameObject[] landEffects;

	// Token: 0x04001E6D RID: 7789
	public float footStepVolume = 0.2f;

	// Token: 0x04001E6E RID: 7790
	public bool pointSound = true;

	// Token: 0x04001E6F RID: 7791
	public AudioClip[] landSounds;

	// Token: 0x04001E70 RID: 7792
	public AudioClip[] footStepSounds;

	// Token: 0x04001E71 RID: 7793
	public GameObject playerRagdoll;

	// Token: 0x04001E72 RID: 7794
	[Header("Prop Character")]
	[SerializeField]
	private bool m_isPropCharacter;

	// Token: 0x04001E73 RID: 7795
	[SerializeField]
	private CharacterHat[] m_possibleHats;

	// Token: 0x04001E74 RID: 7796
	[SerializeField]
	private PlayerColor[] m_possibleColors;

	// Token: 0x04001E75 RID: 7797
	private Vector2[] movementAxisList = new Vector2[]
	{
		new Vector2(0f, 1f),
		new Vector2(0f, -1f),
		new Vector2(-1f, 0f),
		new Vector2(1f, 0f),
		new Vector2(1f, 1f),
		new Vector2(-1f, 1f),
		new Vector2(1f, -1f),
		new Vector2(-1f, -1f),
		new Vector2(0f, 0f)
	};

	// Token: 0x04001E76 RID: 7798
	private Dictionary<AnimationEventType, List<AnimationEventListener>> listeners;

	// Token: 0x04001E77 RID: 7799
	private float velocity;

	// Token: 0x04001E78 RID: 7800
	private float velocityY;

	// Token: 0x04001E79 RID: 7801
	private float shotgunStrength;

	// Token: 0x04001E7A RID: 7802
	private bool grounded;

	// Token: 0x04001E7B RID: 7803
	private bool movementAxisZero;

	// Token: 0x04001E7C RID: 7804
	private bool carrying;

	// Token: 0x04001E7D RID: 7805
	private bool pistolGripRight;

	// Token: 0x04001E7E RID: 7806
	private int carryingSide;

	// Token: 0x04001E7F RID: 7807
	private bool surfing;

	// Token: 0x04001E80 RID: 7808
	private bool driving;

	// Token: 0x04001E81 RID: 7809
	private bool stunned;

	// Token: 0x04001E82 RID: 7810
	private bool holdingRifle;

	// Token: 0x04001E83 RID: 7811
	private bool crouching;

	// Token: 0x04001E84 RID: 7812
	private Vector2 movementAxis;

	// Token: 0x04001E85 RID: 7813
	private Vector2 curMovementAxis = Vector2.zero;

	// Token: 0x04001E86 RID: 7814
	private float spineRotation;

	// Token: 0x04001E87 RID: 7815
	private Animator anim;

	// Token: 0x04001E88 RID: 7816
	private Transform[] playerBones = new Transform[49];

	// Token: 0x04001E89 RID: 7817
	private Transform playerTransform;

	// Token: 0x04001E8A RID: 7818
	private Transform root;

	// Token: 0x04001E8B RID: 7819
	private Transform spine01;

	// Token: 0x04001E8C RID: 7820
	private Transform ballL;

	// Token: 0x04001E8D RID: 7821
	private Transform ballR;

	// Token: 0x04001E8E RID: 7822
	private float playerRotation;

	// Token: 0x04001E8F RID: 7823
	private float playerRotationTarget;

	// Token: 0x04001E90 RID: 7824
	private float playerRotationOffset;

	// Token: 0x04001E91 RID: 7825
	private float playerRotationOffsetTarget;

	// Token: 0x04001E92 RID: 7826
	private AnimatorStateInfo last_state;

	// Token: 0x04001E93 RID: 7827
	private bool setup;

	// Token: 0x04001E94 RID: 7828
	private PlayerColor curColor;

	// Token: 0x04001E95 RID: 7829
	private PlayerSkin curSkin;

	// Token: 0x04001E96 RID: 7830
	private byte curCape = byte.MaxValue;

	// Token: 0x04001E97 RID: 7831
	private CharacterHat curHat;

	// Token: 0x04001E98 RID: 7832
	private GamePlayer player;

	// Token: 0x04001E99 RID: 7833
	private Material goldMat;

	// Token: 0x04001E9A RID: 7834
	private bool BigHead;

	// Token: 0x04001E9B RID: 7835
	private static int m_cape = 1;

	// Token: 0x04001E9C RID: 7836
	private float lastIdleAnimCheck;

	// Token: 0x04001E9D RID: 7837
	private float idleAnimCheckInterval = 0.2f;

	// Token: 0x04001E9E RID: 7838
	private bool wasHit;

	// Token: 0x04001E9F RID: 7839
	private List<GameObject> accessories = new List<GameObject>();

	// Token: 0x04001EA0 RID: 7840
	private GameObject hatObject;

	// Token: 0x04001EA1 RID: 7841
	private CapeController capeController;

	// Token: 0x04001EA2 RID: 7842
	private ResourceRequest curRequest;
}
