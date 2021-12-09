using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class RulesetUIStyles : MonoBehaviour
{
	// Token: 0x0600032C RID: 812 RVA: 0x00005A72 File Offset: 0x00003C72
	public void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (RulesetUIStyles.Instance != null)
		{
			UnityEngine.Object.Destroy(RulesetUIStyles.Instance);
		}
		RulesetUIStyles.Instance = this;
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600032D RID: 813 RVA: 0x00005A9C File Offset: 0x00003C9C
	public static RulesetElementStyle DefaultStyle
	{
		get
		{
			return RulesetUIStyles.Instance.defaultStyle;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600032E RID: 814 RVA: 0x00005AA8 File Offset: 0x00003CA8
	public static RulesetElementStyle DefaultStyleActive
	{
		get
		{
			return RulesetUIStyles.Instance.defaultStyleActive;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600032F RID: 815 RVA: 0x00005AB4 File Offset: 0x00003CB4
	public static RulesetElementStyle DefaultRulesetStyle
	{
		get
		{
			return RulesetUIStyles.Instance.defaultRulesetStyle;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000330 RID: 816 RVA: 0x00005AC0 File Offset: 0x00003CC0
	public static RulesetElementStyle DefaultRulesetStyleActive
	{
		get
		{
			return RulesetUIStyles.Instance.defaultRulesetStyleActive;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000331 RID: 817 RVA: 0x00005ACC File Offset: 0x00003CCC
	public static RulesetElementStyle NewRulesetStyle
	{
		get
		{
			return RulesetUIStyles.Instance.newRulesetStyle;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000332 RID: 818 RVA: 0x00005AD8 File Offset: 0x00003CD8
	public static RulesetElementStyle DeleteRulesetStyle
	{
		get
		{
			return RulesetUIStyles.Instance.deleteRulesetStyle;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000333 RID: 819 RVA: 0x00005AE4 File Offset: 0x00003CE4
	public static RulesetElementStyle BackStyle
	{
		get
		{
			return RulesetUIStyles.Instance.backStyle;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000334 RID: 820 RVA: 0x00005AF0 File Offset: 0x00003CF0
	public static RulesetElementStyle NextPageStyle
	{
		get
		{
			return RulesetUIStyles.Instance.nextPageStyle;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000335 RID: 821 RVA: 0x00005AFC File Offset: 0x00003CFC
	public static RulesetElementStyle PreviousPageStyle
	{
		get
		{
			return RulesetUIStyles.Instance.previousPageStyle;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000336 RID: 822 RVA: 0x00005B08 File Offset: 0x00003D08
	public static RulesetElementStyle DefaultChallenge
	{
		get
		{
			return RulesetUIStyles.Instance.defaultChallenge;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000337 RID: 823 RVA: 0x00005B14 File Offset: 0x00003D14
	public static RulesetElementStyle NewChallenge
	{
		get
		{
			return RulesetUIStyles.Instance.newChallenge;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000338 RID: 824 RVA: 0x00005B20 File Offset: 0x00003D20
	public static RulesetElementStyle SelectActiveRulesetStyle
	{
		get
		{
			return RulesetUIStyles.Instance.selectActiveRulesetStyle;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000339 RID: 825 RVA: 0x00005B2C File Offset: 0x00003D2C
	public static RulesetElementStyle Minigame
	{
		get
		{
			return RulesetUIStyles.Instance.minigame;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600033A RID: 826 RVA: 0x00005B38 File Offset: 0x00003D38
	public static RulesetElementStyle Item
	{
		get
		{
			return RulesetUIStyles.Instance.item;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600033B RID: 827 RVA: 0x00005B44 File Offset: 0x00003D44
	public static RulesetElementStyle Rename
	{
		get
		{
			return RulesetUIStyles.Instance.rename;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600033C RID: 828 RVA: 0x00005B50 File Offset: 0x00003D50
	public static RulesetElementStyle Modifier
	{
		get
		{
			return RulesetUIStyles.Instance.modifier;
		}
	}

	// Token: 0x0400034C RID: 844
	public RulesetElementStyle defaultStyle;

	// Token: 0x0400034D RID: 845
	public RulesetElementStyle defaultStyleActive;

	// Token: 0x0400034E RID: 846
	public RulesetElementStyle defaultRulesetStyle;

	// Token: 0x0400034F RID: 847
	public RulesetElementStyle defaultRulesetStyleActive;

	// Token: 0x04000350 RID: 848
	public RulesetElementStyle newRulesetStyle;

	// Token: 0x04000351 RID: 849
	public RulesetElementStyle deleteRulesetStyle;

	// Token: 0x04000352 RID: 850
	public RulesetElementStyle backStyle;

	// Token: 0x04000353 RID: 851
	public RulesetElementStyle nextPageStyle;

	// Token: 0x04000354 RID: 852
	public RulesetElementStyle previousPageStyle;

	// Token: 0x04000355 RID: 853
	public RulesetElementStyle selectActiveRulesetStyle;

	// Token: 0x04000356 RID: 854
	public RulesetElementStyle defaultChallenge;

	// Token: 0x04000357 RID: 855
	public RulesetElementStyle newChallenge;

	// Token: 0x04000358 RID: 856
	public RulesetElementStyle minigame;

	// Token: 0x04000359 RID: 857
	public RulesetElementStyle item;

	// Token: 0x0400035A RID: 858
	public RulesetElementStyle rename;

	// Token: 0x0400035B RID: 859
	public RulesetElementStyle modifier;

	// Token: 0x0400035C RID: 860
	public static RulesetUIStyles Instance;
}
