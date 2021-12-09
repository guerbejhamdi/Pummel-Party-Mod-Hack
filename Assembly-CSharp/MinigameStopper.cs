using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020003FD RID: 1021
public class MinigameStopper : MonoBehaviour
{
	// Token: 0x06001C43 RID: 7235 RVA: 0x000BBA60 File Offset: 0x000B9C60
	public void Update()
	{
		if (GameManager.DEBUGGING && NetSystem.IsServer && Input.GetKeyDown(KeyCode.E) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
		{
			GameManager.Minigame.EndRound(0f, 0f, true);
		}
	}
}
