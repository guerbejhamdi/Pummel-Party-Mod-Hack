using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
[Serializable]
public class RulesetElementStyle
{
	// Token: 0x06000310 RID: 784 RVA: 0x00037C58 File Offset: 0x00035E58
	public RulesetElementStyle Clone()
	{
		return new RulesetElementStyle
		{
			elementIcon = this.elementIcon,
			iconBackgroundColor = this.iconBackgroundColor,
			iconColor = this.iconColor,
			iconPadding = this.iconPadding,
			iconAspectRatio = this.iconAspectRatio,
			border = this.border,
			borderColor = this.borderColor,
			fontColor = this.fontColor,
			backgroundStripes = this.backgroundStripes,
			backgroundStripesColor = this.backgroundStripesColor
		};
	}

	// Token: 0x04000313 RID: 787
	public Sprite elementIcon;

	// Token: 0x04000314 RID: 788
	public Color iconBackgroundColor = new Color(0.13f, 0.13f, 0.13f, 1f);

	// Token: 0x04000315 RID: 789
	public Color iconColor = new Color(0.76f, 0.76f, 0.76f, 0.71f);

	// Token: 0x04000316 RID: 790
	public int iconPadding = 10;

	// Token: 0x04000317 RID: 791
	public float iconAspectRatio = 1f;

	// Token: 0x04000318 RID: 792
	public bool border;

	// Token: 0x04000319 RID: 793
	public Color borderColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x0400031A RID: 794
	public Color fontColor = Color.white;

	// Token: 0x0400031B RID: 795
	public bool backgroundStripes;

	// Token: 0x0400031C RID: 796
	public Color backgroundStripesColor = Color.white;
}
