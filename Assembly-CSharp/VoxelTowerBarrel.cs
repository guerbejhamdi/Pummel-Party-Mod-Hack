using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000286 RID: 646
public class VoxelTowerBarrel : MonoBehaviour
{
	// Token: 0x060012E1 RID: 4833 RVA: 0x000920A4 File Offset: 0x000902A4
	public void Setup(short _id, short targetPlayer, float _explode_time)
	{
		this.targetPlayer = targetPlayer;
		this.id = _id;
		this.explode_time = _explode_time;
		this.fuse_length = this.explode_time - NetSystem.NetTime.GameTime;
		this.minigame_controller = (VoxelTowerController)GameManager.Minigame;
		this.rigid_body = base.GetComponent<Rigidbody>();
		this.root = base.transform.Find("Root").gameObject;
		this.graphic = this.root.transform.Find("Graphic");
		this.base_mat = this.graphic.GetComponent<MeshRenderer>().materials[0];
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x00092148 File Offset: 0x00090348
	private void Update()
	{
		float num = this.explode_time - NetSystem.NetTime.GameTime;
		float num2 = Mathf.Clamp01((this.fuse_length - num) / this.fuse_length);
		float num3 = this.pulseCurve.Evaluate(Time.time * 5f) * Mathf.Clamp01(num2 * 2f - 1f);
		this.base_mat.SetColor("_EmissionColor", this.flash_color * num3 * 1.5f);
		this.graphic.localScale = new Vector3(0.01f, 0.01f, 0.01f) * (1f + num3 * 0.1f);
		this.val += -Mathf.Log(num / 18f) * this.speed;
		this.meterTransform.localScale = new Vector3(0.01f, 0.01f * num2, 0.01f);
		if (this.holding_player != -1)
		{
			base.transform.position = this.hold_transform.position;
			base.transform.rotation = this.hold_transform.rotation;
			return;
		}
		if (NetSystem.NetTime.GameTime > this.explode_time)
		{
			this.Explode();
		}
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0000F22B File Offset: 0x0000D42B
	public void SetActive(bool val)
	{
		this.root.SetActive(val);
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x00092290 File Offset: 0x00090490
	public void Throw(Vector3 start_position, Vector3 dir, Vector3 angular_velocity, float velocity)
	{
		if (this.holding_player != -1)
		{
			((VoxelTowerPlayer)this.minigame_controller.GetPlayerInSlot(this.holding_player)).held = null;
			this.holding_player = -1;
		}
		this.rigid_body.constraints = RigidbodyConstraints.None;
		base.transform.position = start_position;
		this.rigid_body.velocity = Vector3.zero;
		this.rigid_body.AddForce(dir * velocity, ForceMode.VelocityChange);
		this.rigid_body.angularVelocity = angular_velocity;
		this.thrown = true;
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x00092318 File Offset: 0x00090518
	public void Pickup(short player_slot)
	{
		this.holding_player = player_slot;
		this.lastHoldingPlayer = player_slot;
		((VoxelTowerPlayer)this.minigame_controller.GetPlayerInSlot(player_slot)).held = this;
		this.hold_transform = ((VoxelTowerPlayer)this.minigame_controller.GetPlayerInSlot(this.holding_player)).GetHoldTransform().Find("TargetTransform");
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x0000F239 File Offset: 0x0000D439
	public void OnTriggerEnter(Collider c)
	{
		if (c.name == "BarrelDeathZone")
		{
			if (NetSystem.IsServer)
			{
				this.minigame_controller.CreateBomb(this.lastHoldingPlayer);
			}
			this.Explode();
		}
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x00092378 File Offset: 0x00090578
	public void Explode()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.explode_effect, base.transform.position, Quaternion.identity);
		AudioSystem.PlayOneShot(this.explode_sound, 0.5f, 0.25f, 1f);
		if (this.minigame_controller != null)
		{
			if (NetSystem.IsServer)
			{
				for (int i = 0; i < this.minigame_controller.voxel_towers.Length; i++)
				{
					if (this.minigame_controller.voxel_towers[i] != null)
					{
						this.minigame_controller.voxel_towers[i].Edit(base.transform.position);
					}
				}
			}
			this.minigame_controller.barrels.Remove(this);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x00092438 File Offset: 0x00090638
	public void OnCollisionEnter(Collision c)
	{
		if (this.thrown)
		{
			this.thrown = false;
			this.rigid_body.constraints = RigidbodyConstraints.FreezeAll;
			VoxelTowerGrid component = c.transform.root.GetComponent<VoxelTowerGrid>();
			if (component != null)
			{
				this.lastHitID = (short)component.OwnerSlot;
			}
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060012EA RID: 4842 RVA: 0x0000F26B File Offset: 0x0000D46B
	// (set) Token: 0x060012EB RID: 4843 RVA: 0x0000F273 File Offset: 0x0000D473
	public bool Outline
	{
		get
		{
			return this.outline;
		}
		set
		{
			this.outline = value;
			this.outlineSource.enabled = value;
		}
	}

	// Token: 0x04001423 RID: 5155
	public bool thrown;

	// Token: 0x04001424 RID: 5156
	public short id;

	// Token: 0x04001425 RID: 5157
	public short holding_player = -1;

	// Token: 0x04001426 RID: 5158
	public GameObject explode_effect;

	// Token: 0x04001427 RID: 5159
	public AudioClip explode_sound;

	// Token: 0x04001428 RID: 5160
	public OutlineSource outlineSource;

	// Token: 0x04001429 RID: 5161
	public Transform meterTransform;

	// Token: 0x0400142A RID: 5162
	public AnimationCurve pulseCurve;

	// Token: 0x0400142B RID: 5163
	private VoxelTowerController minigame_controller;

	// Token: 0x0400142C RID: 5164
	private Rigidbody rigid_body;

	// Token: 0x0400142D RID: 5165
	private GameObject root;

	// Token: 0x0400142E RID: 5166
	private Transform hold_transform;

	// Token: 0x0400142F RID: 5167
	public short lastHitID;

	// Token: 0x04001430 RID: 5168
	private bool outline;

	// Token: 0x04001431 RID: 5169
	private float fuse_length;

	// Token: 0x04001432 RID: 5170
	private float explode_time;

	// Token: 0x04001433 RID: 5171
	public Color flash_color = Color.red;

	// Token: 0x04001434 RID: 5172
	public Color base_color = Color.white;

	// Token: 0x04001435 RID: 5173
	private Material base_mat;

	// Token: 0x04001436 RID: 5174
	private float val;

	// Token: 0x04001437 RID: 5175
	private float speed = 0.11f;

	// Token: 0x04001438 RID: 5176
	private short targetPlayer;

	// Token: 0x04001439 RID: 5177
	private short lastHoldingPlayer = -1;

	// Token: 0x0400143A RID: 5178
	private Transform graphic;
}
