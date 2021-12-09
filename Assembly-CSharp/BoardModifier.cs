using System;
using System.Collections.Generic;

// Token: 0x020002A0 RID: 672
public abstract class BoardModifier : GameModifier
{
	// Token: 0x060013C2 RID: 5058 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void OnDestroy()
	{
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void BoardPreInitialize(GameBoardController controller)
	{
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x0000FA9A File Offset: 0x0000DC9A
	public virtual string ModifyMapScene(string scene)
	{
		return scene;
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void OnBoardEnterMinigame()
	{
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void OnBoardReturnFromMinigame()
	{
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x00005651 File Offset: 0x00003851
	public virtual bool OnShouldConsumeItems()
	{
		return true;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void OnApplyDamage(BoardPlayer target, ref DamageInstance d)
	{
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x00096FCC File Offset: 0x000951CC
	public static bool IsBoardModifierActive(BoardModifierID id)
	{
		using (List<BoardModifier>.Enumerator enumerator = BoardModifier.ActiveModifiers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetModifierID() == (int)id)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x00097028 File Offset: 0x00095228
	public static void InitializeModifiers()
	{
		for (int i = 0; i < BoardModifier.ActiveModifiers.Count; i++)
		{
			BoardModifier.ActiveModifiers[i].Initialize();
		}
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x0009705C File Offset: 0x0009525C
	public static string OnModifyMapScene(string scene)
	{
		for (int i = 0; i < BoardModifier.ActiveModifiers.Count; i++)
		{
			scene = BoardModifier.ActiveModifiers[i].ModifyMapScene(scene);
		}
		return scene;
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00097094 File Offset: 0x00095294
	public static void BoardEnterMinigame()
	{
		for (int i = 0; i < BoardModifier.ActiveModifiers.Count; i++)
		{
			BoardModifier.ActiveModifiers[i].OnBoardEnterMinigame();
		}
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x000970C8 File Offset: 0x000952C8
	public static void BoardReturnFromMinigame()
	{
		for (int i = 0; i < BoardModifier.ActiveModifiers.Count; i++)
		{
			BoardModifier.ActiveModifiers[i].OnBoardReturnFromMinigame();
		}
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x000970FC File Offset: 0x000952FC
	public static void Destroy()
	{
		for (int i = 0; i < BoardModifier.ActiveModifiers.Count; i++)
		{
			BoardModifier.ActiveModifiers[i].OnDestroy();
		}
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00097130 File Offset: 0x00095330
	public static bool ShouldConsumeItems()
	{
		bool flag = true;
		for (int i = 0; i < BoardModifier.ActiveModifiers.Count; i++)
		{
			flag &= BoardModifier.ActiveModifiers[i].OnShouldConsumeItems();
		}
		return flag;
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00097168 File Offset: 0x00095368
	public static void ModifyAppliedDamage(BoardPlayer player, ref DamageInstance d)
	{
		for (int i = 0; i < BoardModifier.ActiveModifiers.Count; i++)
		{
			BoardModifier.ActiveModifiers[i].OnApplyDamage(player, ref d);
		}
	}

	// Token: 0x0400150F RID: 5391
	public static List<BoardModifier> ActiveModifiers = new List<BoardModifier>();
}
