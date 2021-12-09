using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class TempleMapEvent : MainBoardEvent
{
	// Token: 0x06001E33 RID: 7731 RVA: 0x000C3980 File Offset: 0x000C1B80
	public override void Start()
	{
		base.Start();
		for (int i = 0; i < this.connections.Length; i++)
		{
			this.connections[i].startPosition = this.connections[i].visual.transform.position;
			base.StartCoroutine(this.DoAnimation(this.connections[i]));
		}
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x00016447 File Offset: 0x00014647
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		base.DoGenericBoardEventActions();
		yield return new WaitForSeconds(0.25f);
		int num;
		for (int i = 0; i < this.connections.Length; i = num)
		{
			Vector3 position = this.connections[i].visual.transform.position;
			position.y = 3.8f;
			GameManager.Board.boardCamera.MoveTo(position);
			yield return new WaitUntil(() => GameManager.Board.boardCamera.WithinDistance(0.3f));
			TempAudioSource t = AudioSystem.PlayLooping(this.rumbleClip, this.rumbleVolume, 1f);
			this.connections[i].active = !this.connections[i].active;
			base.StartCoroutine(this.DoAnimation(this.connections[i]));
			yield return new WaitForSeconds(this.animationTime);
			t.FadeAudio(0.5f, FadeType.Out);
			yield return new WaitForSeconds(0.25f);
			t = null;
			num = i + 1;
		}
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x00016456 File Offset: 0x00014656
	public IEnumerator DoAnimation(TempleMapEvent.TempleMapConnection con)
	{
		if (!con.active)
		{
			this.AddConnection(con.startNode, con.endNode, BoardNodeConnectionDirection.Forward, BoardNodeTransition.Walking);
			this.AddConnection(con.endNode, con.startNode, BoardNodeConnectionDirection.Back, BoardNodeTransition.Walking);
		}
		else
		{
			this.RemoveConnection(con.startNode, con.endNode);
			this.RemoveConnection(con.endNode, con.startNode);
		}
		LeanTween.moveY(con.visual, con.startPosition.y + (con.active ? this.heightOffset : 0f), this.animationTime).setEaseInSine();
		yield return null;
		yield break;
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x00039BD4 File Offset: 0x00037DD4
	private void AddConnection(BoardNode node, BoardNode node2, BoardNodeConnectionDirection type, BoardNodeTransition transitionType = BoardNodeTransition.Walking)
	{
		int num = node.nodeConnections.Length;
		BoardNodeConnection[] array = new BoardNodeConnection[num + 1];
		for (int i = 0; i < num; i++)
		{
			array[i] = node.nodeConnections[i];
		}
		array[num] = new BoardNodeConnection
		{
			connection_type = type,
			node = node2,
			transition = transitionType
		};
		node.nodeConnections = array;
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x000C39EC File Offset: 0x000C1BEC
	private void RemoveConnection(BoardNode node, BoardNode node2)
	{
		BoardNodeConnection[] array = new BoardNodeConnection[node.nodeConnections.Length - 1];
		int num = 0;
		for (int i = 0; i < node.nodeConnections.Length; i++)
		{
			if (node.nodeConnections[i].node != node2)
			{
				array[num] = node.nodeConnections[i];
				num++;
			}
		}
		node.nodeConnections = array;
	}

	// Token: 0x0400211D RID: 8477
	public float animationTime = 0.5f;

	// Token: 0x0400211E RID: 8478
	public TempleMapEvent.TempleMapConnection[] connections;

	// Token: 0x0400211F RID: 8479
	public float heightOffset = 3.38f;

	// Token: 0x04002120 RID: 8480
	public AudioClip rumbleClip;

	// Token: 0x04002121 RID: 8481
	public float rumbleVolume = 1f;

	// Token: 0x0200044A RID: 1098
	[Serializable]
	public struct TempleMapConnection
	{
		// Token: 0x04002122 RID: 8482
		public bool active;

		// Token: 0x04002123 RID: 8483
		public BoardNode startNode;

		// Token: 0x04002124 RID: 8484
		public BoardNode endNode;

		// Token: 0x04002125 RID: 8485
		public GameObject visual;

		// Token: 0x04002126 RID: 8486
		[HideInInspector]
		public Vector3 startPosition;
	}
}
