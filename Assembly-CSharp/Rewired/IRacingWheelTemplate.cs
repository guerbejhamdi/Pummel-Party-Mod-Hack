using System;

namespace Rewired
{
	// Token: 0x0200063C RID: 1596
	public interface IRacingWheelTemplate : IControllerTemplate
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06002A14 RID: 10772
		IControllerTemplateAxis wheel { get; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06002A15 RID: 10773
		IControllerTemplateAxis accelerator { get; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06002A16 RID: 10774
		IControllerTemplateAxis brake { get; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06002A17 RID: 10775
		IControllerTemplateAxis clutch { get; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06002A18 RID: 10776
		IControllerTemplateButton shiftDown { get; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06002A19 RID: 10777
		IControllerTemplateButton shiftUp { get; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06002A1A RID: 10778
		IControllerTemplateButton wheelButton1 { get; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06002A1B RID: 10779
		IControllerTemplateButton wheelButton2 { get; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002A1C RID: 10780
		IControllerTemplateButton wheelButton3 { get; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002A1D RID: 10781
		IControllerTemplateButton wheelButton4 { get; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002A1E RID: 10782
		IControllerTemplateButton wheelButton5 { get; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002A1F RID: 10783
		IControllerTemplateButton wheelButton6 { get; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06002A20 RID: 10784
		IControllerTemplateButton wheelButton7 { get; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002A21 RID: 10785
		IControllerTemplateButton wheelButton8 { get; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06002A22 RID: 10786
		IControllerTemplateButton wheelButton9 { get; }

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06002A23 RID: 10787
		IControllerTemplateButton wheelButton10 { get; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06002A24 RID: 10788
		IControllerTemplateButton consoleButton1 { get; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002A25 RID: 10789
		IControllerTemplateButton consoleButton2 { get; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06002A26 RID: 10790
		IControllerTemplateButton consoleButton3 { get; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06002A27 RID: 10791
		IControllerTemplateButton consoleButton4 { get; }

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06002A28 RID: 10792
		IControllerTemplateButton consoleButton5 { get; }

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002A29 RID: 10793
		IControllerTemplateButton consoleButton6 { get; }

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002A2A RID: 10794
		IControllerTemplateButton consoleButton7 { get; }

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002A2B RID: 10795
		IControllerTemplateButton consoleButton8 { get; }

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002A2C RID: 10796
		IControllerTemplateButton consoleButton9 { get; }

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06002A2D RID: 10797
		IControllerTemplateButton consoleButton10 { get; }

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06002A2E RID: 10798
		IControllerTemplateButton shifter1 { get; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002A2F RID: 10799
		IControllerTemplateButton shifter2 { get; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002A30 RID: 10800
		IControllerTemplateButton shifter3 { get; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06002A31 RID: 10801
		IControllerTemplateButton shifter4 { get; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002A32 RID: 10802
		IControllerTemplateButton shifter5 { get; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002A33 RID: 10803
		IControllerTemplateButton shifter6 { get; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06002A34 RID: 10804
		IControllerTemplateButton shifter7 { get; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002A35 RID: 10805
		IControllerTemplateButton shifter8 { get; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06002A36 RID: 10806
		IControllerTemplateButton shifter9 { get; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002A37 RID: 10807
		IControllerTemplateButton shifter10 { get; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002A38 RID: 10808
		IControllerTemplateButton reverseGear { get; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002A39 RID: 10809
		IControllerTemplateButton select { get; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002A3A RID: 10810
		IControllerTemplateButton start { get; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002A3B RID: 10811
		IControllerTemplateButton systemButton { get; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002A3C RID: 10812
		IControllerTemplateButton horn { get; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002A3D RID: 10813
		IControllerTemplateDPad dPad { get; }
	}
}
