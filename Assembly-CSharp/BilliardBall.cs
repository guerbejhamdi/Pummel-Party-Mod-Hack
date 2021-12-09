using System;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200014F RID: 335
public class BilliardBall : NetBehaviour
{
	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000994 RID: 2452 RVA: 0x0000A5B9 File Offset: 0x000087B9
	// (set) Token: 0x06000995 RID: 2453 RVA: 0x0000A5C1 File Offset: 0x000087C1
	public bool Pocketed { get; set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000996 RID: 2454 RVA: 0x0000A5CA File Offset: 0x000087CA
	// (set) Token: 0x06000997 RID: 2455 RVA: 0x0000A5D7 File Offset: 0x000087D7
	public byte LastPlayer
	{
		get
		{
			return this.lastPlayer.Value;
		}
		set
		{
			if (NetSystem.IsServer)
			{
				this.lastPlayer.Value = value;
			}
			this.mr.sharedMaterial = this.minigameController.ballMaterials[(int)value];
		}
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00056000 File Offset: 0x00054200
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (BilliardBattleController)GameManager.Minigame;
		if (NetSystem.IsServer)
		{
			this.posX.Value = ZPMath.CompressFloatToShort(base.transform.position.x, BilliardBattleController.minX, BilliardBattleController.maxX);
			this.posY.Value = ZPMath.CompressFloatToByte(base.transform.position.y, BilliardBattleController.minY, BilliardBattleController.maxNetY);
			this.posZ.Value = ZPMath.CompressFloatToShort(base.transform.position.z, BilliardBattleController.minZ, BilliardBattleController.maxZ);
			return;
		}
		UnityEngine.Object.Destroy(this.rb);
		UnityEngine.Object.Destroy(this.sphereCollider);
		NetVar<byte> netVar = this.lastPlayer;
		netVar.Recieve = (RecieveProxy)Delegate.Combine(netVar.Recieve, new RecieveProxy(this.RecieveLastPlayer));
		this.minigameController.balls.Add(this);
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x000560F8 File Offset: 0x000542F8
	private void Update()
	{
		if (NetSystem.IsServer)
		{
			this.posX.Value = ZPMath.CompressFloatToShort(base.transform.position.x, BilliardBattleController.minX, BilliardBattleController.maxX);
			this.posY.Value = ZPMath.CompressFloatToByte(base.transform.position.y, BilliardBattleController.minY, BilliardBattleController.maxNetY);
			this.posZ.Value = ZPMath.CompressFloatToShort(base.transform.position.z, BilliardBattleController.minZ, BilliardBattleController.maxZ);
			return;
		}
		Vector3 position = base.transform.position;
		base.transform.position = new Vector3(ZPMath.DecompressShortToFloat(this.posX.Value, BilliardBattleController.minX, BilliardBattleController.maxX), ZPMath.DecompressByteToFloat(this.posY.Value, BilliardBattleController.minY, BilliardBattleController.maxNetY), ZPMath.DecompressShortToFloat(this.posZ.Value, BilliardBattleController.minZ, BilliardBattleController.maxZ));
		if (position != base.transform.position)
		{
			this.DoRotation(position);
		}
		if (base.transform.position.y > 1f)
		{
			base.transform.rotation = Quaternion.identity;
		}
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00056238 File Offset: 0x00054438
	private void DoRotation(Vector3 prePosition)
	{
		Vector3 vector = base.transform.position - prePosition;
		vector.y = 0f;
		Vector3 axis = Vector3.Cross(vector.normalized, Vector3.up);
		float magnitude = vector.magnitude;
		float num = 7.0371675f;
		base.transform.RotateAround(base.transform.position, axis, -(magnitude / num * 360f));
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x000562A4 File Offset: 0x000544A4
	private void FixedUpdate()
	{
		if (NetSystem.IsServer && !this.Pocketed)
		{
			base.transform.position = new Vector3(Mathf.Clamp(base.transform.position.x, BilliardBattleController.minX, BilliardBattleController.maxX), Mathf.Clamp(base.transform.position.y, BilliardBattleController.minY, BilliardBattleController.maxY), Mathf.Clamp(base.transform.position.z, BilliardBattleController.minZ, BilliardBattleController.maxZ));
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00056330 File Offset: 0x00054530
	private void OnCollisionEnter(Collision collision)
	{
		if (NetSystem.IsServer && collision.gameObject.CompareTag("BilliardBall") && !this.Pocketed)
		{
			float magnitude = collision.relativeVelocity.magnitude;
			if (magnitude > 1f)
			{
				AudioSystem.PlayOneShot(this.hitSound, Mathf.Lerp(0f, 1f, Mathf.Clamp(magnitude, 0f, 5f) / 5f), 0.05f, 1f);
				BilliardBattlePlayer component = collision.gameObject.GetComponent<BilliardBattlePlayer>();
				if (component != null)
				{
					this.LastPlayer = (byte)component.OwnerSlot;
				}
			}
		}
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x000563D4 File Offset: 0x000545D4
	private void OnTriggerEnter(Collider other)
	{
		if (NetSystem.IsServer)
		{
			if (this.LastPlayer != 255)
			{
				this.rb.isKinematic = true;
				this.rb.velocity = Vector3.zero;
				this.rb.angularVelocity = Vector3.zero;
				Transform child = this.minigameController.pocketPositionsRoot.GetChild((int)this.LastPlayer);
				base.transform.position = child.position + child.right * (float)this.minigameController.GetPlayer((int)this.LastPlayer).Score * 1.2f;
				base.transform.rotation = Quaternion.identity;
				this.Pocketed = true;
				this.minigameController.Pocketed(this.LastPlayer);
				return;
			}
			base.transform.position = this.minigameController.GetFreePosition();
			this.rb.velocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x0000A604 File Offset: 0x00008804
	private void RecieveLastPlayer(object val)
	{
		this.LastPlayer = (byte)val;
	}

	// Token: 0x0400084B RID: 2123
	public Rigidbody rb;

	// Token: 0x0400084C RID: 2124
	public SphereCollider sphereCollider;

	// Token: 0x0400084D RID: 2125
	public AudioClip hitSound;

	// Token: 0x0400084E RID: 2126
	public MeshRenderer mr;

	// Token: 0x0400084F RID: 2127
	private BilliardBattleController minigameController;

	// Token: 0x04000851 RID: 2129
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> posX = new NetVar<short>(0);

	// Token: 0x04000852 RID: 2130
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> posY = new NetVar<byte>(0);

	// Token: 0x04000853 RID: 2131
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> posZ = new NetVar<short>(0);

	// Token: 0x04000854 RID: 2132
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> lastPlayer = new NetVar<byte>(byte.MaxValue);
}
