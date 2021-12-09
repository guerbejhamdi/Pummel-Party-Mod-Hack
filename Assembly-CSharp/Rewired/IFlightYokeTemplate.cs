using System;

namespace Rewired
{
	// Token: 0x0200063E RID: 1598
	public interface IFlightYokeTemplate : IControllerTemplate
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002A96 RID: 10902
		IControllerTemplateButton leftPaddle { get; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002A97 RID: 10903
		IControllerTemplateButton rightPaddle { get; }

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002A98 RID: 10904
		IControllerTemplateButton leftGripButton1 { get; }

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002A99 RID: 10905
		IControllerTemplateButton leftGripButton2 { get; }

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06002A9A RID: 10906
		IControllerTemplateButton leftGripButton3 { get; }

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06002A9B RID: 10907
		IControllerTemplateButton leftGripButton4 { get; }

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002A9C RID: 10908
		IControllerTemplateButton leftGripButton5 { get; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002A9D RID: 10909
		IControllerTemplateButton leftGripButton6 { get; }

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002A9E RID: 10910
		IControllerTemplateButton rightGripButton1 { get; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002A9F RID: 10911
		IControllerTemplateButton rightGripButton2 { get; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002AA0 RID: 10912
		IControllerTemplateButton rightGripButton3 { get; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06002AA1 RID: 10913
		IControllerTemplateButton rightGripButton4 { get; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002AA2 RID: 10914
		IControllerTemplateButton rightGripButton5 { get; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06002AA3 RID: 10915
		IControllerTemplateButton rightGripButton6 { get; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002AA4 RID: 10916
		IControllerTemplateButton centerButton1 { get; }

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002AA5 RID: 10917
		IControllerTemplateButton centerButton2 { get; }

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002AA6 RID: 10918
		IControllerTemplateButton centerButton3 { get; }

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002AA7 RID: 10919
		IControllerTemplateButton centerButton4 { get; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002AA8 RID: 10920
		IControllerTemplateButton centerButton5 { get; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06002AA9 RID: 10921
		IControllerTemplateButton centerButton6 { get; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002AAA RID: 10922
		IControllerTemplateButton centerButton7 { get; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002AAB RID: 10923
		IControllerTemplateButton centerButton8 { get; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002AAC RID: 10924
		IControllerTemplateButton wheel1Up { get; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002AAD RID: 10925
		IControllerTemplateButton wheel1Down { get; }

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002AAE RID: 10926
		IControllerTemplateButton wheel1Press { get; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002AAF RID: 10927
		IControllerTemplateButton wheel2Up { get; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002AB0 RID: 10928
		IControllerTemplateButton wheel2Down { get; }

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002AB1 RID: 10929
		IControllerTemplateButton wheel2Press { get; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002AB2 RID: 10930
		IControllerTemplateButton consoleButton1 { get; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002AB3 RID: 10931
		IControllerTemplateButton consoleButton2 { get; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002AB4 RID: 10932
		IControllerTemplateButton consoleButton3 { get; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002AB5 RID: 10933
		IControllerTemplateButton consoleButton4 { get; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002AB6 RID: 10934
		IControllerTemplateButton consoleButton5 { get; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002AB7 RID: 10935
		IControllerTemplateButton consoleButton6 { get; }

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002AB8 RID: 10936
		IControllerTemplateButton consoleButton7 { get; }

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002AB9 RID: 10937
		IControllerTemplateButton consoleButton8 { get; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002ABA RID: 10938
		IControllerTemplateButton consoleButton9 { get; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002ABB RID: 10939
		IControllerTemplateButton consoleButton10 { get; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002ABC RID: 10940
		IControllerTemplateButton mode1 { get; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002ABD RID: 10941
		IControllerTemplateButton mode2 { get; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002ABE RID: 10942
		IControllerTemplateButton mode3 { get; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002ABF RID: 10943
		IControllerTemplateYoke yoke { get; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002AC0 RID: 10944
		IControllerTemplateThrottle lever1 { get; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002AC1 RID: 10945
		IControllerTemplateThrottle lever2 { get; }

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002AC2 RID: 10946
		IControllerTemplateThrottle lever3 { get; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002AC3 RID: 10947
		IControllerTemplateThrottle lever4 { get; }

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002AC4 RID: 10948
		IControllerTemplateThrottle lever5 { get; }

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002AC5 RID: 10949
		IControllerTemplateHat leftGripHat { get; }

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002AC6 RID: 10950
		IControllerTemplateHat rightGripHat { get; }
	}
}
