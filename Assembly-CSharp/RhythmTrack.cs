using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200021F RID: 543
public class RhythmTrack
{
	// Token: 0x04000FFF RID: 4095
	public RectTransform spawn;

	// Token: 0x04001000 RID: 4096
	public RectTransform hit;

	// Token: 0x04001001 RID: 4097
	public RectTransform buttonContainer;

	// Token: 0x04001002 RID: 4098
	public List<RhythmButtonUI> activeButtons = new List<RhythmButtonUI>();

	// Token: 0x04001003 RID: 4099
	public RhythmInputType inputType;

	// Token: 0x04001004 RID: 4100
	public Text comboTxt;

	// Token: 0x04001005 RID: 4101
	public Transform comboBonus;
}
