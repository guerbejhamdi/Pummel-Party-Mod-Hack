using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x020001ED RID: 493
public class NetworkTestPlayerNew : Movement1
{
	// Token: 0x06000E5F RID: 3679 RVA: 0x00072F98 File Offset: 0x00071198
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		if (!base.IsOwner)
		{
			Debug.Log("Adding Recieve Handlers");
			NetVec3 position = this.position;
			position.Recieve = (RecieveProxy)Delegate.Combine(position.Recieve, new RecieveProxy(this.TestRecievePosition));
			NetVec3 velocity = this.velocity;
			velocity.Recieve = (RecieveProxy)Delegate.Combine(velocity.Recieve, new RecieveProxy(this.TestRecieveVelocity));
		}
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x00073018 File Offset: 0x00071218
	protected override void Start()
	{
		base.Start();
		this.minigameController = (NetworkTestController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		if (!base.IsOwner)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.debugWorldText, GameManager.UIController.transform);
			this.debugText = gameObject.GetComponent<DebugWorldText>();
			this.debugText.Initialize(base.transform, "Test: ", Color.white, GameManager.Minigame.MinigameCamera, 25, Vector3.up * 0.75f);
		}
		this.mover.SetForwardVector(Vector3.left);
		this.player_root.GetComponent<MeshRenderer>().material.color = GameManager.GetColorAtIndex((int)base.OwnerSlot).skinColor1;
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x000730DC File Offset: 0x000712DC
	private void Update()
	{
		base.UpdateController();
		int i = 0;
		while (i < this.recievedPositions.Count)
		{
			if (Time.time - this.recievedPositions[i] > 1f)
			{
				this.recievedPositions.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		int j = 0;
		while (j < this.fakeRecievedPositions.Count)
		{
			if (Time.time - this.fakeRecievedPositions[j] > 1f)
			{
				this.fakeRecievedPositions.RemoveAt(j);
			}
			else
			{
				j++;
			}
		}
		int k = 0;
		while (k < this.recievedVelocities.Count)
		{
			if (Time.time - this.recievedVelocities[k] > 1f)
			{
				this.recievedVelocities.RemoveAt(k);
			}
			else
			{
				k++;
			}
		}
		if (!base.IsOwner)
		{
			this.debugText.SetText(string.Concat(new string[]
			{
				this.recievedPositions.Count.ToString(),
				",",
				this.fakeRecievedPositions.Count.ToString(),
				",",
				this.samePositionCount.ToString()
			}));
		}
		Debug.DrawLine(this.lastPosition, base.transform.position, this.colorAlternate ? Color.red : Color.green, 1f);
		this.colorAlternate = !this.colorAlternate;
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0007324C File Offset: 0x0007144C
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead;
		if (this.right && base.transform.position.z >= 6f)
		{
			this.right = false;
		}
		else if (!this.right && base.transform.position.z <= -6f)
		{
			this.right = true;
		}
		input = new CharacterMoverInput(false, false, !this.right, this.right, false, false);
		input.NullInput(val);
		this.mover.CalculateVelocity(input, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		this.velocity.Value = this.mover.Velocity;
		base.DoMovement();
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00073340 File Offset: 0x00071540
	private void TestRecievePosition(object pos)
	{
		if (((Vector3)pos).Equals(this.lastnetPosition))
		{
			this.samePositionCount++;
		}
		else
		{
			this.lastnetPosition = (Vector3)pos;
		}
		if (!this.recievedPositions.Contains(Time.time))
		{
			this.recievedPositions.Add(Time.time);
		}
		this.fakeRecievedPositions.Add(Time.time);
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0000CB9F File Offset: 0x0000AD9F
	private void TestRecieveVelocity(object pos)
	{
		this.recievedVelocities.Add(Time.time);
	}

	// Token: 0x04000DD9 RID: 3545
	public GameObject debugWorldText;

	// Token: 0x04000DDA RID: 3546
	private NetworkTestController minigameController;

	// Token: 0x04000DDB RID: 3547
	private CharacterMover mover;

	// Token: 0x04000DDC RID: 3548
	private List<float> recievedPositions = new List<float>();

	// Token: 0x04000DDD RID: 3549
	private List<float> fakeRecievedPositions = new List<float>();

	// Token: 0x04000DDE RID: 3550
	private List<float> recievedVelocities = new List<float>();

	// Token: 0x04000DDF RID: 3551
	private DebugWorldText debugText;

	// Token: 0x04000DE0 RID: 3552
	private Vector3 lastnetPosition = Vector3.zero;

	// Token: 0x04000DE1 RID: 3553
	private int samePositionCount;

	// Token: 0x04000DE2 RID: 3554
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x04000DE3 RID: 3555
	private bool colorAlternate;

	// Token: 0x04000DE4 RID: 3556
	private bool right = true;
}
