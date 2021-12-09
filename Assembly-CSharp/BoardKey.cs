using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x0200001C RID: 28
public class BoardKey : MonoBehaviour
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000077 RID: 119 RVA: 0x00003DBC File Offset: 0x00001FBC
	// (set) Token: 0x06000078 RID: 120 RVA: 0x00003DC4 File Offset: 0x00001FC4
	public BoardPlayer Owner { get; private set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000079 RID: 121 RVA: 0x00003DCD File Offset: 0x00001FCD
	// (set) Token: 0x0600007A RID: 122 RVA: 0x00003DD5 File Offset: 0x00001FD5
	public int ID { get; private set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600007B RID: 123 RVA: 0x00003DDE File Offset: 0x00001FDE
	public BoardKey.BoardKeyState CurState
	{
		get
		{
			return this.curState;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600007C RID: 124 RVA: 0x00003DE6 File Offset: 0x00001FE6
	// (set) Token: 0x0600007D RID: 125 RVA: 0x00003DEE File Offset: 0x00001FEE
	public int Seed { get; private set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600007E RID: 126 RVA: 0x00003DF7 File Offset: 0x00001FF7
	// (set) Token: 0x0600007F RID: 127 RVA: 0x00003DFF File Offset: 0x00001FFF
	public BoardNode Node { get; private set; }

	// Token: 0x06000080 RID: 128 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
	public void Setup(BoardPlayer owner, int id, BoardNode node, int seed)
	{
		this.Owner = owner;
		this.ID = id;
		this.Node = node;
		this.Seed = seed;
		this.rand = new System.Random(seed);
		if (BoardKey.materials == null)
		{
			BoardKey.materials = new Material[GameManager.ColorCount()];
			for (int i = 0; i < BoardKey.materials.Length; i++)
			{
				Color skinColor = GameManager.GetColorAtIndex(i).skinColor1;
				skinColor.a = 0.39215687f;
				BoardKey.materials[i] = new Material(this.lootSignalMat.sharedMaterial);
				BoardKey.materials[i].SetColor("_TintColor", skinColor);
			}
		}
		this.spawnTime = Time.time;
		float magnitude;
		if (this.Owner != null)
		{
			Color skinColor2 = owner.GamePlayer.Color.skinColor1;
			this.keyLight.color = skinColor2;
			this.lootSignalMat.sharedMaterial = BoardKey.materials[(int)owner.GamePlayer.ColorIndex];
			this.particles.startColor = skinColor2;
			Vector3 a = node.nodeConnections[this.rand.Next(0, node.nodeConnections.Length)].node.NodePosition - node.transform.position;
			magnitude = a.magnitude;
			float d = ZPMath.RandomFloat(this.rand, 0f, magnitude);
			a.Normalize();
			this.ejectTargetPos = node.transform.position + a * d;
			float num = 1f;
			float d2 = ZPMath.RandomFloat(this.rand, -num, num);
			a.y = 0f;
			Vector3 a2 = Quaternion.LookRotation(a.normalized) * Quaternion.Euler(0f, 90f, 0f) * Vector3.forward;
			this.ejectTargetPos += a2 * d2;
			this.ejectTargetPos += Vector3.up * 0.5f;
			this.ejectStartPos = owner.MidPoint;
		}
		else
		{
			Vector3 vector = ZPMath.RandomPointInUnitSphere(this.rand);
			vector.y = 0f;
			vector.Normalize();
			float d3 = ZPMath.RandomFloat(this.rand, -3.5f, 3.5f);
			vector *= d3;
			magnitude = vector.magnitude;
			this.ejectTargetPos = node.transform.position + vector + Vector3.up * 0.75f;
			this.ejectStartPos = node.transform.position + Vector3.up * 0.75f;
		}
		this.ejectLength = magnitude / this.ejectSpeed;
		this.ejectHeight = magnitude * 0.2f;
		base.transform.position = this.ejectStartPos;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0002D1C8 File Offset: 0x0002B3C8
	public void Update()
	{
		switch (this.curState)
		{
		case BoardKey.BoardKeyState.Ejecting:
		{
			float num = (Time.time - this.spawnTime) / this.ejectLength;
			if (num >= 1f)
			{
				base.transform.position = this.ejectTargetPos;
				this.timer.Start();
				this.curState = BoardKey.BoardKeyState.Delay;
				return;
			}
			base.transform.position = Vector3.Lerp(this.ejectStartPos, this.ejectTargetPos, this.moveCurve.Evaluate(num));
			base.transform.position += Vector3.up * (this.ejectHeight * this.heightCurve.Evaluate(num));
			return;
		}
		case BoardKey.BoardKeyState.Delay:
			if (this.timer.Elapsed(true))
			{
				if (this.pickupPlayer != null)
				{
					this.pickupTime = Time.time;
					this.curState = BoardKey.BoardKeyState.Pickup;
					return;
				}
				this.curState = BoardKey.BoardKeyState.Idle;
				return;
			}
			break;
		case BoardKey.BoardKeyState.Idle:
			break;
		case BoardKey.BoardKeyState.Pickup:
		{
			Vector3 vector = this.pickupPlayer.MidPoint - base.transform.position;
			float magnitude = vector.magnitude;
			float num2 = Time.deltaTime * this.curSpeed;
			if (magnitude < num2)
			{
				this.pickupPlayer.GiveGold(1, true);
				base.transform.position = this.pickupPlayer.MidPoint;
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			base.transform.position += vector.normalized * num2;
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00003E08 File Offset: 0x00002008
	private void FixedUpdate()
	{
		if (this.curState == BoardKey.BoardKeyState.Pickup)
		{
			this.curSpeed *= 1.125f;
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00003E25 File Offset: 0x00002025
	public void Pickup(BoardPlayer player)
	{
		this.pickupPlayer = player;
		this.pickupTime = Time.time;
		if (this.curState == BoardKey.BoardKeyState.Idle)
		{
			this.curState = BoardKey.BoardKeyState.Pickup;
		}
	}

	// Token: 0x0400007B RID: 123
	public AudioClip pickup;

	// Token: 0x0400007C RID: 124
	public AnimationCurve heightCurve;

	// Token: 0x0400007D RID: 125
	public AnimationCurve moveCurve;

	// Token: 0x0400007E RID: 126
	public MeshRenderer lootSignalMat;

	// Token: 0x0400007F RID: 127
	public Light keyLight;

	// Token: 0x04000080 RID: 128
	public ParticleSystem particles;

	// Token: 0x04000085 RID: 133
	private BoardPlayer pickupPlayer;

	// Token: 0x04000086 RID: 134
	private System.Random rand;

	// Token: 0x04000087 RID: 135
	private float pickupTime;

	// Token: 0x04000088 RID: 136
	private BoardKey.BoardKeyState curState;

	// Token: 0x04000089 RID: 137
	private float ejectSpeed = 7f;

	// Token: 0x0400008A RID: 138
	private float spawnTime;

	// Token: 0x0400008B RID: 139
	private Vector3 ejectStartPos;

	// Token: 0x0400008C RID: 140
	private Vector3 ejectTargetPos;

	// Token: 0x0400008D RID: 141
	private float ejectHeight;

	// Token: 0x0400008E RID: 142
	private float ejectLength;

	// Token: 0x0400008F RID: 143
	private static Material[] materials;

	// Token: 0x04000090 RID: 144
	private ActionTimer timer = new ActionTimer(0.4f, 0.8f);

	// Token: 0x04000091 RID: 145
	private float curSpeed = 1f;

	// Token: 0x0200001D RID: 29
	public enum BoardKeyState
	{
		// Token: 0x04000093 RID: 147
		Ejecting,
		// Token: 0x04000094 RID: 148
		Delay,
		// Token: 0x04000095 RID: 149
		Idle,
		// Token: 0x04000096 RID: 150
		Pickup
	}
}
