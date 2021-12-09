using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;

// Token: 0x020001BC RID: 444
public class IcebergMover : MonoBehaviour
{
	// Token: 0x06000CCE RID: 3278 RVA: 0x0000BDDE File Offset: 0x00009FDE
	public void Awake()
	{
		this.m_startPos = base.transform.position;
		this.m_moveTime = this.m_timeOffset;
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x0000BDFD File Offset: 0x00009FFD
	private void Update()
	{
		if (!IcebergMover.c_useFixedUpdate)
		{
			this.UpdateMover();
			this.UpdateTime(Time.deltaTime);
		}
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0000BE17 File Offset: 0x0000A017
	private void FixedUpdate()
	{
		if (IcebergMover.c_useFixedUpdate)
		{
			this.UpdateMover();
			this.UpdateTime(Time.fixedDeltaTime);
		}
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x0006A724 File Offset: 0x00068924
	private void UpdateTime(float delta)
	{
		IcebergUpdateType updateType = this.m_updateType;
		if (updateType != IcebergUpdateType.Time)
		{
			return;
		}
		this.m_moveTime += delta;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0006A750 File Offset: 0x00068950
	private void UpdateMover()
	{
		Vector3 position = base.transform.position;
		float d = this.m_movementCurve.Evaluate(this.GetTime() * this.m_movementSpeed);
		base.transform.position = this.m_startPos + this.m_movementAxis * d;
		Vector3 vector = base.transform.position - position;
		foreach (KeyValuePair<CharacterController, int> keyValuePair in this.m_players)
		{
			if (keyValuePair.Key != null)
			{
				keyValuePair.Key.Move(vector);
			}
		}
		foreach (KeyValuePair<NavMeshAgent, int> keyValuePair2 in this.m_ai)
		{
			if (keyValuePair2.Key != null)
			{
				keyValuePair2.Key.transform.position += vector;
			}
		}
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0006A87C File Offset: 0x00068A7C
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			IcebergPlayer componentInChildren = collision.gameObject.GetComponentInChildren<IcebergPlayer>();
			if (componentInChildren != null && !componentInChildren.GamePlayer.IsAI)
			{
				CharacterController componentInChildren2 = collision.gameObject.GetComponentInChildren<CharacterController>();
				if (componentInChildren2 != null && !this.m_players.ContainsKey(componentInChildren2))
				{
					this.m_players.Add(componentInChildren2, 1);
					return;
				}
				Dictionary<CharacterController, int> players = this.m_players;
				CharacterController key = componentInChildren2;
				int num = players[key];
				players[key] = num + 1;
				return;
			}
			else if (componentInChildren != null)
			{
				NavMeshAgent componentInChildren3 = collision.gameObject.GetComponentInChildren<NavMeshAgent>();
				if (componentInChildren3 != null && !this.m_ai.ContainsKey(componentInChildren3))
				{
					this.m_ai.Add(componentInChildren3, 1);
					return;
				}
				Dictionary<NavMeshAgent, int> ai = this.m_ai;
				NavMeshAgent key2 = componentInChildren3;
				int num = ai[key2];
				ai[key2] = num + 1;
			}
		}
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0006A96C File Offset: 0x00068B6C
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			IcebergPlayer componentInChildren = collision.gameObject.GetComponentInChildren<IcebergPlayer>();
			if (componentInChildren != null && !componentInChildren.GamePlayer.IsAI)
			{
				CharacterController componentInChildren2 = collision.gameObject.GetComponentInChildren<CharacterController>();
				if (componentInChildren2 != null && this.m_players.ContainsKey(componentInChildren2))
				{
					if (this.m_players[componentInChildren2] <= 1)
					{
						this.m_players.Remove(componentInChildren2);
						return;
					}
					Dictionary<CharacterController, int> players = this.m_players;
					CharacterController key = componentInChildren2;
					int num = players[key];
					players[key] = num - 1;
					return;
				}
			}
			else if (componentInChildren != null)
			{
				NavMeshAgent componentInChildren3 = collision.gameObject.GetComponentInChildren<NavMeshAgent>();
				if (componentInChildren3 != null && this.m_ai.ContainsKey(componentInChildren3))
				{
					if (this.m_ai[componentInChildren3] <= 1)
					{
						this.m_ai.Remove(componentInChildren3);
						return;
					}
					Dictionary<NavMeshAgent, int> ai = this.m_ai;
					NavMeshAgent key2 = componentInChildren3;
					int num = ai[key2];
					ai[key2] = num - 1;
				}
			}
		}
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x0000BE31 File Offset: 0x0000A031
	public void Reset()
	{
		if (NetSystem.IsServer)
		{
			this.m_moveTime = 0f;
		}
		else
		{
			this.m_moveTime = 0.3f;
		}
		base.transform.position = this.m_startPos;
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x0000BE63 File Offset: 0x0000A063
	private float GetTime()
	{
		if (NetSystem.IsConnected)
		{
			return NetSystem.NetTime.GameTime;
		}
		return Time.time;
	}

	// Token: 0x04000C22 RID: 3106
	[SerializeField]
	protected IcebergUpdateType m_updateType;

	// Token: 0x04000C23 RID: 3107
	[SerializeField]
	protected AnimationCurve m_movementCurve;

	// Token: 0x04000C24 RID: 3108
	[SerializeField]
	protected float m_movementSpeed;

	// Token: 0x04000C25 RID: 3109
	[SerializeField]
	protected Vector3 m_movementAxis;

	// Token: 0x04000C26 RID: 3110
	[SerializeField]
	protected float m_timeOffset;

	// Token: 0x04000C27 RID: 3111
	private float m_moveTime;

	// Token: 0x04000C28 RID: 3112
	private Vector3 m_startPos;

	// Token: 0x04000C29 RID: 3113
	private static bool c_useFixedUpdate = true;

	// Token: 0x04000C2A RID: 3114
	private Dictionary<CharacterController, int> m_players = new Dictionary<CharacterController, int>();

	// Token: 0x04000C2B RID: 3115
	private Dictionary<NavMeshAgent, int> m_ai = new Dictionary<NavMeshAgent, int>();

	// Token: 0x04000C2C RID: 3116
	private float m_collisionStayVelocity = 1f;
}
