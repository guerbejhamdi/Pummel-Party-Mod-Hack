using System;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class Modifier_MirrorMapHorizontal : BoardModifier
{
	// Token: 0x060013E6 RID: 5094 RVA: 0x00005651 File Offset: 0x00003851
	protected override int GetModifierID()
	{
		return 1;
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x0000FA9A File Offset: 0x0000DC9A
	public override string ModifyMapScene(string scene)
	{
		return scene;
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x0000FB13 File Offset: 0x0000DD13
	public override void BoardPreInitialize(GameBoardController controller)
	{
		GameObject.Find("BoardCamera").GetComponentInChildren<Camera>().gameObject.AddComponent<MirrorCameraHelper>().SetMirror(MirrorCameraType.Horizontal);
	}

	// Token: 0x04001516 RID: 5398
	private string[] m_mirroredScenes = new string[]
	{
		"WinterMap_Scene",
		"HalloweenMap_Scene",
		"PirateMap_Scene",
		"PostApocMap_Scene"
	};
}
