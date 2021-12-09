using System;

// Token: 0x020002A8 RID: 680
public class Modifier_OneHitKill : BoardModifier
{
	// Token: 0x060013EA RID: 5098 RVA: 0x0000FB68 File Offset: 0x0000DD68
	protected override int GetModifierID()
	{
		return 5;
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x0000FB6B File Offset: 0x0000DD6B
	public override void OnApplyDamage(BoardPlayer target, ref DamageInstance d)
	{
		d.damage = (int)target.LocalHealth;
	}
}
