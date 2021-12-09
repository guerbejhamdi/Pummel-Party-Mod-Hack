using System;

namespace Rewired
{
	// Token: 0x0200063B RID: 1595
	public interface IGamepadTemplate : IControllerTemplate
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060029F7 RID: 10743
		IControllerTemplateButton actionBottomRow1 { get; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x060029F8 RID: 10744
		IControllerTemplateButton a { get; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x060029F9 RID: 10745
		IControllerTemplateButton actionBottomRow2 { get; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x060029FA RID: 10746
		IControllerTemplateButton b { get; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x060029FB RID: 10747
		IControllerTemplateButton actionBottomRow3 { get; }

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x060029FC RID: 10748
		IControllerTemplateButton c { get; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060029FD RID: 10749
		IControllerTemplateButton actionTopRow1 { get; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060029FE RID: 10750
		IControllerTemplateButton x { get; }

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060029FF RID: 10751
		IControllerTemplateButton actionTopRow2 { get; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06002A00 RID: 10752
		IControllerTemplateButton y { get; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06002A01 RID: 10753
		IControllerTemplateButton actionTopRow3 { get; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002A02 RID: 10754
		IControllerTemplateButton z { get; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002A03 RID: 10755
		IControllerTemplateButton leftShoulder1 { get; }

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06002A04 RID: 10756
		IControllerTemplateButton leftBumper { get; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06002A05 RID: 10757
		IControllerTemplateAxis leftShoulder2 { get; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06002A06 RID: 10758
		IControllerTemplateAxis leftTrigger { get; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06002A07 RID: 10759
		IControllerTemplateButton rightShoulder1 { get; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06002A08 RID: 10760
		IControllerTemplateButton rightBumper { get; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06002A09 RID: 10761
		IControllerTemplateAxis rightShoulder2 { get; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06002A0A RID: 10762
		IControllerTemplateAxis rightTrigger { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06002A0B RID: 10763
		IControllerTemplateButton center1 { get; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06002A0C RID: 10764
		IControllerTemplateButton back { get; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002A0D RID: 10765
		IControllerTemplateButton center2 { get; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06002A0E RID: 10766
		IControllerTemplateButton start { get; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002A0F RID: 10767
		IControllerTemplateButton center3 { get; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06002A10 RID: 10768
		IControllerTemplateButton guide { get; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06002A11 RID: 10769
		IControllerTemplateThumbStick leftStick { get; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06002A12 RID: 10770
		IControllerTemplateThumbStick rightStick { get; }

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002A13 RID: 10771
		IControllerTemplateDPad dPad { get; }
	}
}
