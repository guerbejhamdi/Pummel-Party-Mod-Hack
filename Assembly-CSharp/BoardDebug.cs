using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class BoardDebug : MonoBehaviour
{
	// Token: 0x06000071 RID: 113 RVA: 0x0002C750 File Offset: 0x0002A950
	private void Update()
	{
		if (!GameManager.DEBUGGING)
		{
			return;
		}
		this.MoveablePlayers();
		this.KeySpawn();
		this.textFitTimer.Elapsed(true);
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F10))
		{
			Cursor.visible = !Cursor.visible;
		}
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F9))
		{
			Canvas[] array = UnityEngine.Object.FindObjectsOfType<Canvas>();
			Debug.Log(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = !array[i].enabled;
				array[i].enabled = false;
			}
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			Input.GetKeyDown(KeyCode.F8);
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			Input.GetKeyDown(KeyCode.F6);
		}
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F2))
		{
			for (int j = 0; j < GameManager.PlayerList.Count; j++)
			{
				if (GameManager.PlayerList[j].BoardObject.diceEffect != null)
				{
					GameManager.PlayerList[j].BoardObject.diceEffect.gameObject.SetActive(false);
				}
			}
			GameManager.Board.boardCamera.enabled = false;
			Vector3 position = GameObject.Find("Aimer").transform.position;
			GameManager.Board.boardCamera.transform.position = position + Vector3.up * 6f + Vector3.left * 5f;
			this.doingThing2 = !this.doingThing2;
		}
		if (this.doingThing2)
		{
			Vector3 position2 = GameObject.Find("Aimer").transform.position;
			GameManager.Board.boardCamera.transform.RotateAround(position2, Vector3.up, 15f * Time.deltaTime);
			GameManager.Board.boardCamera.transform.rotation = Quaternion.LookRotation(position2 - GameManager.Board.boardCamera.transform.position);
		}
		if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.F1))
		{
			for (int k = 0; k < GameManager.PlayerList.Count; k++)
			{
				if (GameManager.PlayerList[k].BoardObject.diceEffect != null)
				{
					GameManager.PlayerList[k].BoardObject.diceEffect.gameObject.SetActive(false);
				}
			}
			GameManager.Board.boardCamera.enabled = false;
			GameManager.Board.boardCamera.transform.position = GameManager.PlayerList[0].BoardObject.CurrentNode.transform.position + Vector3.up * 1.4f + Vector3.left * 5f;
			this.doingThing = !this.doingThing;
			this.timer.Start();
		}
		if (this.doingThing)
		{
			if (this.timer.Elapsed(true))
			{
				List<int> list = new List<int>();
				List<int> list2 = new List<int>();
				for (int l = 0; l < GameManager.ColorCount(); l++)
				{
					list.Add(l);
				}
				for (int m = 0; m < GameManager.GetPlayerHatCount(); m++)
				{
					list2.Add(m);
				}
				for (int n = 0; n < GameManager.PlayerList.Count; n++)
				{
					int index = UnityEngine.Random.Range(0, list.Count);
					int index2 = UnityEngine.Random.Range(0, list2.Count);
					GameManager.PlayerList[n].BoardObject.PlayerAnimation.SetPlayerHat(GameManager.GetHatAtIndex(list2[index2]));
					GameManager.PlayerList[n].BoardObject.PlayerAnimation.SetPlayerColor(GameManager.GetColorAtIndex(list[index]));
					list.RemoveAt(index);
					list2.RemoveAt(index2);
				}
			}
			Vector3 vector = GameManager.PlayerList[0].BoardObject.CurrentNode.transform.position + Vector3.up;
			GameManager.Board.boardCamera.transform.RotateAround(vector, Vector3.up, 15f * Time.deltaTime);
			GameManager.Board.boardCamera.transform.rotation = Quaternion.LookRotation(vector - GameManager.Board.boardCamera.transform.position);
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0002CC1C File Offset: 0x0002AE1C
	private void MoveablePlayers()
	{
		RaycastHit raycastHit;
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 2048f, 1024))
		{
			float num = float.MaxValue;
			if (this.targetPlayer == null)
			{
				for (int i = 0; i < GameManager.GetPlayerCount(); i++)
				{
					BoardPlayer boardObject = GameManager.GetPlayerAt(i).BoardObject;
					float sqrMagnitude = (boardObject.transform.position - raycastHit.point).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						this.targetPlayer = boardObject;
						num = sqrMagnitude;
					}
				}
				return;
			}
			BoardNode[] boardNodes = GameManager.Board.BoardNodes;
			BoardNode boardNode = null;
			foreach (BoardNode boardNode2 in boardNodes)
			{
				float sqrMagnitude2 = (boardNode2.NodePosition - raycastHit.point).sqrMagnitude;
				if (sqrMagnitude2 < num)
				{
					boardNode = boardNode2;
					num = sqrMagnitude2;
				}
			}
			GameManager.Board.MovePlayer(this.targetPlayer.GamePlayer.GlobalID, boardNode.NodeID, true);
			this.targetPlayer = null;
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0002CD48 File Offset: 0x0002AF48
	private void KeySpawn()
	{
		RaycastHit raycastHit;
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 2048f, 1024))
		{
			float num = float.MaxValue;
			BoardNode[] boardNodes = GameManager.Board.BoardNodes;
			BoardNode node = null;
			foreach (BoardNode boardNode in boardNodes)
			{
				float sqrMagnitude = (boardNode.NodePosition - raycastHit.point).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					node = boardNode;
					num = sqrMagnitude;
				}
			}
			GameManager.KeyController.SpawnKeys(1, node, null);
		}
	}

	// Token: 0x04000071 RID: 113
	private bool doingThing;

	// Token: 0x04000072 RID: 114
	private bool doingThing2;

	// Token: 0x04000073 RID: 115
	private ActionTimer timer = new ActionTimer(0.115f);

	// Token: 0x04000074 RID: 116
	private ActionTimer textFitTimer = new ActionTimer(1f);

	// Token: 0x04000075 RID: 117
	private BoardPlayer targetPlayer;
}
