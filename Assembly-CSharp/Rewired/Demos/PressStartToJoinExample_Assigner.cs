using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006D4 RID: 1748
	[AddComponentMenu("")]
	public class PressStartToJoinExample_Assigner : MonoBehaviour
	{
		// Token: 0x06003317 RID: 13079 RVA: 0x001098E8 File Offset: 0x00107AE8
		public static Player GetRewiredPlayer(int gamePlayerId)
		{
			if (!ReInput.isReady)
			{
				return null;
			}
			if (PressStartToJoinExample_Assigner.instance == null)
			{
				Debug.LogError("Not initialized. Do you have a PressStartToJoinPlayerSelector in your scehe?");
				return null;
			}
			for (int i = 0; i < PressStartToJoinExample_Assigner.instance.playerMap.Count; i++)
			{
				if (PressStartToJoinExample_Assigner.instance.playerMap[i].gamePlayerId == gamePlayerId)
				{
					return ReInput.players.GetPlayer(PressStartToJoinExample_Assigner.instance.playerMap[i].rewiredPlayerId);
				}
			}
			return null;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x00022BEA File Offset: 0x00020DEA
		private void Awake()
		{
			this.playerMap = new List<PressStartToJoinExample_Assigner.PlayerMap>();
			PressStartToJoinExample_Assigner.instance = this;
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x0010996C File Offset: 0x00107B6C
		private void Update()
		{
			for (int i = 0; i < ReInput.players.playerCount; i++)
			{
				if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
				{
					this.AssignNextPlayer(i);
				}
			}
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x001099AC File Offset: 0x00107BAC
		private void AssignNextPlayer(int rewiredPlayerId)
		{
			if (this.playerMap.Count >= this.maxPlayers)
			{
				Debug.LogError("Max player limit already reached!");
				return;
			}
			int nextGamePlayerId = this.GetNextGamePlayerId();
			this.playerMap.Add(new PressStartToJoinExample_Assigner.PlayerMap(rewiredPlayerId, nextGamePlayerId));
			Player player = ReInput.players.GetPlayer(rewiredPlayerId);
			player.controllers.maps.SetMapsEnabled(false, "Assignment");
			player.controllers.maps.SetMapsEnabled(true, "Default");
			Debug.Log("Added Rewired Player id " + rewiredPlayerId.ToString() + " to game player " + nextGamePlayerId.ToString());
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00109A4C File Offset: 0x00107C4C
		private int GetNextGamePlayerId()
		{
			int num = this.gamePlayerIdCounter;
			this.gamePlayerIdCounter = num + 1;
			return num;
		}

		// Token: 0x04003155 RID: 12629
		private static PressStartToJoinExample_Assigner instance;

		// Token: 0x04003156 RID: 12630
		public int maxPlayers = 4;

		// Token: 0x04003157 RID: 12631
		private List<PressStartToJoinExample_Assigner.PlayerMap> playerMap;

		// Token: 0x04003158 RID: 12632
		private int gamePlayerIdCounter;

		// Token: 0x020006D5 RID: 1749
		private class PlayerMap
		{
			// Token: 0x0600331D RID: 13085 RVA: 0x00022C0C File Offset: 0x00020E0C
			public PlayerMap(int rewiredPlayerId, int gamePlayerId)
			{
				this.rewiredPlayerId = rewiredPlayerId;
				this.gamePlayerId = gamePlayerId;
			}

			// Token: 0x04003159 RID: 12633
			public int rewiredPlayerId;

			// Token: 0x0400315A RID: 12634
			public int gamePlayerId;
		}
	}
}
