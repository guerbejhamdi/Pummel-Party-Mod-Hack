using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class PlayerCosmetics : MonoBehaviour
{
	// Token: 0x1700034E RID: 846
	// (set) Token: 0x06001C91 RID: 7313 RVA: 0x0001500F File Offset: 0x0001320F
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

	// Token: 0x1700034F RID: 847
	// (set) Token: 0x06001C92 RID: 7314 RVA: 0x0001503B File Offset: 0x0001323B
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

	// Token: 0x17000350 RID: 848
	// (set) Token: 0x06001C93 RID: 7315 RVA: 0x00015067 File Offset: 0x00013267
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

	// Token: 0x17000351 RID: 849
	// (set) Token: 0x06001C94 RID: 7316 RVA: 0x00015093 File Offset: 0x00013293
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

	// Token: 0x17000352 RID: 850
	// (set) Token: 0x06001C95 RID: 7317 RVA: 0x000150BF File Offset: 0x000132BF
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

	// Token: 0x17000353 RID: 851
	// (set) Token: 0x06001C96 RID: 7318 RVA: 0x000150EB File Offset: 0x000132EB
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

	// Token: 0x17000354 RID: 852
	// (set) Token: 0x06001C97 RID: 7319 RVA: 0x00015117 File Offset: 0x00013317
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

	// Token: 0x17000355 RID: 853
	// (set) Token: 0x06001C98 RID: 7320 RVA: 0x00015143 File Offset: 0x00013343
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

	// Token: 0x17000356 RID: 854
	// (set) Token: 0x06001C99 RID: 7321 RVA: 0x0001516F File Offset: 0x0001336F
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

	// Token: 0x17000357 RID: 855
	// (set) Token: 0x06001C9A RID: 7322 RVA: 0x0001519B File Offset: 0x0001339B
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

	// Token: 0x17000358 RID: 856
	// (set) Token: 0x06001C9B RID: 7323 RVA: 0x000151C7 File Offset: 0x000133C7
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

	// Token: 0x17000359 RID: 857
	// (set) Token: 0x06001C9C RID: 7324 RVA: 0x000151F3 File Offset: 0x000133F3
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

	// Token: 0x1700035A RID: 858
	// (set) Token: 0x06001C9D RID: 7325 RVA: 0x000BCDC8 File Offset: 0x000BAFC8
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

	// Token: 0x1700035B RID: 859
	// (set) Token: 0x06001C9E RID: 7326 RVA: 0x0001521F File Offset: 0x0001341F
	public byte MovementAxisByte
	{
		set
		{
			this.MovementAxis = this.movementAxisList[(int)value];
		}
	}

	// Token: 0x1700035C RID: 860
	// (set) Token: 0x06001C9F RID: 7327 RVA: 0x00015233 File Offset: 0x00013433
	public float SpineRotation
	{
		set
		{
			this.spineRotation = value;
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x0001523C File Offset: 0x0001343C
	public float PlayerRotation
	{
		get
		{
			return this.playerRotation;
		}
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x000BCE18 File Offset: 0x000BB018
	public void Setup()
	{
		if (this.setup)
		{
			return;
		}
		this.goldMat = (Material)Resources.Load("GoalAssets/GoldMat");
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
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x00015244 File Offset: 0x00013444
	public void Awake()
	{
		this.Setup();
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x000BD3E4 File Offset: 0x000BB5E4
	public GameObject SpawnRagdoll(Vector3 force)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerRagdoll, base.transform.position, Quaternion.identity);
		gameObject.transform.localScale = base.transform.localScale;
		PlayerRagdoll component = gameObject.GetComponent<PlayerRagdoll>();
		component.Spawn(base.transform.root);
		component.ApplyForce(force, 100f);
		gameObject.GetComponentInChildren<PlayerAnimation>(true).SetPlayer(this.player);
		return gameObject;
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x0001524C File Offset: 0x0001344C
	public void SetPlayer(int playerID)
	{
		this.SetPlayer(GameManager.GetPlayerAt(playerID));
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x000BD458 File Offset: 0x000BB658
	public void SetPlayer(GamePlayer player)
	{
		this.Setup();
		this.player = player;
		this.SetSkin(this.player.Skin);
		this.SetupCape(this.player.CapeIndex);
		this.SetPlayerColor(this.player.Color);
		this.SetPlayerHat(this.player.Hat);
		if (player.BoardObject != null)
		{
			for (int i = 0; i < 6; i++)
			{
				this.SetPlayerGoalObject(i, player.BoardObject.GetGoalStatus(i), false);
			}
		}
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x0001525A File Offset: 0x0001345A
	public void SetPlayerGoalObject(int i, bool state, bool doEffects)
	{
		if (this.player.BoardObject != null)
		{
			if (i <= 2)
			{
				return;
			}
			if (i - 3 > 2)
			{
				return;
			}
		}
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x000BD4E4 File Offset: 0x000BB6E4
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

	// Token: 0x06001CA8 RID: 7336 RVA: 0x000BD664 File Offset: 0x000BB864
	public void SetupCape(byte capeIndex)
	{
		if (!this.spawnCape)
		{
			return;
		}
		if (this.capeController == null && this.spawnCape)
		{
			this.capeController = UnityEngine.Object.Instantiate<GameObject>(this.capePrefab, this.GetBone(PlayerBone.Root)).GetComponent<CapeController>();
			this.capeController.playerCosmetics = this;
		}
		if (this.curCape != capeIndex)
		{
			this.capeController.Setup((CapeType)capeIndex);
			this.curCape = capeIndex;
		}
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x000BD6D8 File Offset: 0x000BB8D8
	public void SetPlayerHat(CharacterHat hat)
	{
		if (hat == this.curHat)
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

	// Token: 0x06001CAA RID: 7338 RVA: 0x000BD764 File Offset: 0x000BB964
	private void Req_completed(AsyncOperation obj)
	{
		Transform transform = this.playerBones[(int)this.curHat.bone];
		this.hatObject = UnityEngine.Object.Instantiate<GameObject>((GameObject)this.curRequest.asset);
		this.hatObject.transform.parent = transform;
		this.hatObject.layer = transform.gameObject.layer;
		this.hatObject.transform.localPosition = this.curHat.position;
		this.hatObject.transform.localRotation = Quaternion.Euler(this.curHat.rotation);
		this.hatObject.transform.localScale = this.curHat.scale;
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x0001527F File Offset: 0x0001347F
	public void SetPlayerColor(PlayerColor color)
	{
		this.Setup();
		this.SetPlayerColor(color, this.skinnedRenderer);
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x000BD81C File Offset: 0x000BBA1C
	public void SetPlayerColor(PlayerColor color, SkinnedMeshRenderer mr)
	{
		this.Setup();
		mr.sharedMaterial.color = color.skinColor1;
		this.curColor = color;
		foreach (OutlineSource outlineSource in base.transform.root.gameObject.GetComponentsInChildren<OutlineSource>(true))
		{
			if (outlineSource != null)
			{
				outlineSource.outlineColor = color.uiColor;
			}
		}
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x00015294 File Offset: 0x00013494
	public Transform GetBone(PlayerBone bone)
	{
		return this.playerBones[(int)bone];
	}

	// Token: 0x04001EE7 RID: 7911
	public SkinnedMeshRenderer skinnedRenderer;

	// Token: 0x04001EE8 RID: 7912
	public GameObject capePrefab;

	// Token: 0x04001EE9 RID: 7913
	public bool spawnCape = true;

	// Token: 0x04001EEA RID: 7914
	public bool usePlayerRotation = true;

	// Token: 0x04001EEB RID: 7915
	public float playerRotationSpeed = 720f;

	// Token: 0x04001EEC RID: 7916
	public float movementLerpSpeed = 2f;

	// Token: 0x04001EED RID: 7917
	public float impactVelocity = -12f;

	// Token: 0x04001EEE RID: 7918
	public float spineYOffset;

	// Token: 0x04001EEF RID: 7919
	public float spineZOffset;

	// Token: 0x04001EF0 RID: 7920
	public GameObject[] footStepEffects;

	// Token: 0x04001EF1 RID: 7921
	public GameObject[] landEffects;

	// Token: 0x04001EF2 RID: 7922
	public float footStepVolume = 0.2f;

	// Token: 0x04001EF3 RID: 7923
	public bool pointSound = true;

	// Token: 0x04001EF4 RID: 7924
	public AudioClip[] landSounds;

	// Token: 0x04001EF5 RID: 7925
	public AudioClip[] footStepSounds;

	// Token: 0x04001EF6 RID: 7926
	public GameObject playerRagdoll;

	// Token: 0x04001EF7 RID: 7927
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

	// Token: 0x04001EF8 RID: 7928
	private Dictionary<AnimationEventType, List<AnimationEventListener>> listeners;

	// Token: 0x04001EF9 RID: 7929
	private float velocity;

	// Token: 0x04001EFA RID: 7930
	private float velocityY;

	// Token: 0x04001EFB RID: 7931
	private float shotgunStrength;

	// Token: 0x04001EFC RID: 7932
	private bool grounded;

	// Token: 0x04001EFD RID: 7933
	private bool movementAxisZero;

	// Token: 0x04001EFE RID: 7934
	private bool carrying;

	// Token: 0x04001EFF RID: 7935
	private bool pistolGripRight;

	// Token: 0x04001F00 RID: 7936
	private int carryingSide;

	// Token: 0x04001F01 RID: 7937
	private bool surfing;

	// Token: 0x04001F02 RID: 7938
	private bool driving;

	// Token: 0x04001F03 RID: 7939
	private bool stunned;

	// Token: 0x04001F04 RID: 7940
	private bool holdingRifle;

	// Token: 0x04001F05 RID: 7941
	private bool crouching;

	// Token: 0x04001F06 RID: 7942
	private Vector2 movementAxis;

	// Token: 0x04001F07 RID: 7943
	private Vector2 curMovementAxis = Vector2.zero;

	// Token: 0x04001F08 RID: 7944
	private float spineRotation;

	// Token: 0x04001F09 RID: 7945
	private Animator anim;

	// Token: 0x04001F0A RID: 7946
	private Transform[] playerBones = new Transform[49];

	// Token: 0x04001F0B RID: 7947
	private Transform playerTransform;

	// Token: 0x04001F0C RID: 7948
	private Transform root;

	// Token: 0x04001F0D RID: 7949
	private Transform spine01;

	// Token: 0x04001F0E RID: 7950
	private Transform ballL;

	// Token: 0x04001F0F RID: 7951
	private Transform ballR;

	// Token: 0x04001F10 RID: 7952
	private float playerRotation;

	// Token: 0x04001F11 RID: 7953
	private float playerRotationTarget;

	// Token: 0x04001F12 RID: 7954
	private float playerRotationOffset;

	// Token: 0x04001F13 RID: 7955
	private float playerRotationOffsetTarget;

	// Token: 0x04001F14 RID: 7956
	private AnimatorStateInfo last_state;

	// Token: 0x04001F15 RID: 7957
	private bool setup;

	// Token: 0x04001F16 RID: 7958
	private PlayerColor curColor;

	// Token: 0x04001F17 RID: 7959
	private PlayerSkin curSkin;

	// Token: 0x04001F18 RID: 7960
	private byte curCape = byte.MaxValue;

	// Token: 0x04001F19 RID: 7961
	private CharacterHat curHat;

	// Token: 0x04001F1A RID: 7962
	private GamePlayer player;

	// Token: 0x04001F1B RID: 7963
	private Material goldMat;

	// Token: 0x04001F1C RID: 7964
	private List<GameObject> accessories = new List<GameObject>();

	// Token: 0x04001F1D RID: 7965
	private GameObject hatObject;

	// Token: 0x04001F1E RID: 7966
	private CapeController capeController;

	// Token: 0x04001F1F RID: 7967
	private ResourceRequest curRequest;
}
