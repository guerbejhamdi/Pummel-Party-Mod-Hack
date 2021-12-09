using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class CameraFixer : MonoBehaviour
{
	// Token: 0x060000A1 RID: 161 RVA: 0x00003F63 File Offset: 0x00002163
	private void Start()
	{
		this.controller = GameManager.Minigame;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00003F70 File Offset: 0x00002170
	private void Update()
	{
		if (this.controller.State == MinigameControllerState.EnablePlayers)
		{
			base.transform.position = this.lastPosition;
			return;
		}
		this.lastPosition = base.transform.position;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00003F70 File Offset: 0x00002170
	private void LateUpdate()
	{
		if (this.controller.State == MinigameControllerState.EnablePlayers)
		{
			base.transform.position = this.lastPosition;
			return;
		}
		this.lastPosition = base.transform.position;
	}

	// Token: 0x040000B9 RID: 185
	private MinigameController controller;

	// Token: 0x040000BA RID: 186
	private Vector3 lastPosition;
}
