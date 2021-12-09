using System;

namespace Rewired
{
	// Token: 0x02000642 RID: 1602
	public sealed class RacingWheelTemplate : ControllerTemplate, IRacingWheelTemplate, IControllerTemplate
	{
		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002B12 RID: 11026 RVA: 0x0001D783 File Offset: 0x0001B983
		IControllerTemplateAxis IRacingWheelTemplate.wheel
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002B13 RID: 11027 RVA: 0x0001D78C File Offset: 0x0001B98C
		IControllerTemplateAxis IRacingWheelTemplate.accelerator
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x0001D795 File Offset: 0x0001B995
		IControllerTemplateAxis IRacingWheelTemplate.brake
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x0001D79E File Offset: 0x0001B99E
		IControllerTemplateAxis IRacingWheelTemplate.clutch
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(3);
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x0001D6CE File Offset: 0x0001B8CE
		IControllerTemplateButton IRacingWheelTemplate.shiftDown
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002B17 RID: 11031 RVA: 0x0001D6D7 File Offset: 0x0001B8D7
		IControllerTemplateButton IRacingWheelTemplate.shiftUp
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
		IControllerTemplateButton IRacingWheelTemplate.wheelButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002B19 RID: 11033 RVA: 0x0001D6E9 File Offset: 0x0001B8E9
		IControllerTemplateButton IRacingWheelTemplate.wheelButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x0001D6F2 File Offset: 0x0001B8F2
		IControllerTemplateButton IRacingWheelTemplate.wheelButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x0001D6FB File Offset: 0x0001B8FB
		IControllerTemplateButton IRacingWheelTemplate.wheelButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002B1C RID: 11036 RVA: 0x0001D705 File Offset: 0x0001B905
		IControllerTemplateButton IRacingWheelTemplate.wheelButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002B1D RID: 11037 RVA: 0x0001D7A7 File Offset: 0x0001B9A7
		IControllerTemplateButton IRacingWheelTemplate.wheelButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002B1E RID: 11038 RVA: 0x0001D719 File Offset: 0x0001B919
		IControllerTemplateButton IRacingWheelTemplate.wheelButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002B1F RID: 11039 RVA: 0x0001D7B1 File Offset: 0x0001B9B1
		IControllerTemplateButton IRacingWheelTemplate.wheelButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002B20 RID: 11040 RVA: 0x0001D72D File Offset: 0x0001B92D
		IControllerTemplateButton IRacingWheelTemplate.wheelButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002B21 RID: 11041 RVA: 0x0001D737 File Offset: 0x0001B937
		IControllerTemplateButton IRacingWheelTemplate.wheelButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002B22 RID: 11042 RVA: 0x0001D741 File Offset: 0x0001B941
		IControllerTemplateButton IRacingWheelTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002B23 RID: 11043 RVA: 0x0001D7BB File Offset: 0x0001B9BB
		IControllerTemplateButton IRacingWheelTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002B24 RID: 11044 RVA: 0x0001D7C5 File Offset: 0x0001B9C5
		IControllerTemplateButton IRacingWheelTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002B25 RID: 11045 RVA: 0x0001D7CF File Offset: 0x0001B9CF
		IControllerTemplateButton IRacingWheelTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x0001D7D9 File Offset: 0x0001B9D9
		IControllerTemplateButton IRacingWheelTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002B27 RID: 11047 RVA: 0x0001D7E3 File Offset: 0x0001B9E3
		IControllerTemplateButton IRacingWheelTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x0001D7ED File Offset: 0x0001B9ED
		IControllerTemplateButton IRacingWheelTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002B29 RID: 11049 RVA: 0x0001D7F7 File Offset: 0x0001B9F7
		IControllerTemplateButton IRacingWheelTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x0001D801 File Offset: 0x0001BA01
		IControllerTemplateButton IRacingWheelTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002B2B RID: 11051 RVA: 0x0001D80B File Offset: 0x0001BA0B
		IControllerTemplateButton IRacingWheelTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x0001D815 File Offset: 0x0001BA15
		IControllerTemplateButton IRacingWheelTemplate.shifter1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002B2D RID: 11053 RVA: 0x0001D81F File Offset: 0x0001BA1F
		IControllerTemplateButton IRacingWheelTemplate.shifter2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002B2E RID: 11054 RVA: 0x0001D829 File Offset: 0x0001BA29
		IControllerTemplateButton IRacingWheelTemplate.shifter3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002B2F RID: 11055 RVA: 0x0001D833 File Offset: 0x0001BA33
		IControllerTemplateButton IRacingWheelTemplate.shifter4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002B30 RID: 11056 RVA: 0x0001D83D File Offset: 0x0001BA3D
		IControllerTemplateButton IRacingWheelTemplate.shifter5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x0001D847 File Offset: 0x0001BA47
		IControllerTemplateButton IRacingWheelTemplate.shifter6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002B32 RID: 11058 RVA: 0x0001D851 File Offset: 0x0001BA51
		IControllerTemplateButton IRacingWheelTemplate.shifter7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(32);
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002B33 RID: 11059 RVA: 0x0001D85B File Offset: 0x0001BA5B
		IControllerTemplateButton IRacingWheelTemplate.shifter8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(33);
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002B34 RID: 11060 RVA: 0x0001D865 File Offset: 0x0001BA65
		IControllerTemplateButton IRacingWheelTemplate.shifter9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(34);
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002B35 RID: 11061 RVA: 0x0001D86F File Offset: 0x0001BA6F
		IControllerTemplateButton IRacingWheelTemplate.shifter10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(35);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002B36 RID: 11062 RVA: 0x0001D879 File Offset: 0x0001BA79
		IControllerTemplateButton IRacingWheelTemplate.reverseGear
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06002B37 RID: 11063 RVA: 0x0001D883 File Offset: 0x0001BA83
		IControllerTemplateButton IRacingWheelTemplate.select
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(36);
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x0001D88D File Offset: 0x0001BA8D
		IControllerTemplateButton IRacingWheelTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(37);
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002B39 RID: 11065 RVA: 0x0001D897 File Offset: 0x0001BA97
		IControllerTemplateButton IRacingWheelTemplate.systemButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(38);
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002B3A RID: 11066 RVA: 0x0001D8A1 File Offset: 0x0001BAA1
		IControllerTemplateButton IRacingWheelTemplate.horn
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x0001D8AB File Offset: 0x0001BAAB
		IControllerTemplateDPad IRacingWheelTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(45);
			}
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x0001D769 File Offset: 0x0001B969
		public RacingWheelTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04002C73 RID: 11379
		public static readonly Guid typeGuid = new Guid("104e31d8-9115-4dd5-a398-2e54d35e6c83");

		// Token: 0x04002C74 RID: 11380
		public const int elementId_wheel = 0;

		// Token: 0x04002C75 RID: 11381
		public const int elementId_accelerator = 1;

		// Token: 0x04002C76 RID: 11382
		public const int elementId_brake = 2;

		// Token: 0x04002C77 RID: 11383
		public const int elementId_clutch = 3;

		// Token: 0x04002C78 RID: 11384
		public const int elementId_shiftDown = 4;

		// Token: 0x04002C79 RID: 11385
		public const int elementId_shiftUp = 5;

		// Token: 0x04002C7A RID: 11386
		public const int elementId_wheelButton1 = 6;

		// Token: 0x04002C7B RID: 11387
		public const int elementId_wheelButton2 = 7;

		// Token: 0x04002C7C RID: 11388
		public const int elementId_wheelButton3 = 8;

		// Token: 0x04002C7D RID: 11389
		public const int elementId_wheelButton4 = 9;

		// Token: 0x04002C7E RID: 11390
		public const int elementId_wheelButton5 = 10;

		// Token: 0x04002C7F RID: 11391
		public const int elementId_wheelButton6 = 11;

		// Token: 0x04002C80 RID: 11392
		public const int elementId_wheelButton7 = 12;

		// Token: 0x04002C81 RID: 11393
		public const int elementId_wheelButton8 = 13;

		// Token: 0x04002C82 RID: 11394
		public const int elementId_wheelButton9 = 14;

		// Token: 0x04002C83 RID: 11395
		public const int elementId_wheelButton10 = 15;

		// Token: 0x04002C84 RID: 11396
		public const int elementId_consoleButton1 = 16;

		// Token: 0x04002C85 RID: 11397
		public const int elementId_consoleButton2 = 17;

		// Token: 0x04002C86 RID: 11398
		public const int elementId_consoleButton3 = 18;

		// Token: 0x04002C87 RID: 11399
		public const int elementId_consoleButton4 = 19;

		// Token: 0x04002C88 RID: 11400
		public const int elementId_consoleButton5 = 20;

		// Token: 0x04002C89 RID: 11401
		public const int elementId_consoleButton6 = 21;

		// Token: 0x04002C8A RID: 11402
		public const int elementId_consoleButton7 = 22;

		// Token: 0x04002C8B RID: 11403
		public const int elementId_consoleButton8 = 23;

		// Token: 0x04002C8C RID: 11404
		public const int elementId_consoleButton9 = 24;

		// Token: 0x04002C8D RID: 11405
		public const int elementId_consoleButton10 = 25;

		// Token: 0x04002C8E RID: 11406
		public const int elementId_shifter1 = 26;

		// Token: 0x04002C8F RID: 11407
		public const int elementId_shifter2 = 27;

		// Token: 0x04002C90 RID: 11408
		public const int elementId_shifter3 = 28;

		// Token: 0x04002C91 RID: 11409
		public const int elementId_shifter4 = 29;

		// Token: 0x04002C92 RID: 11410
		public const int elementId_shifter5 = 30;

		// Token: 0x04002C93 RID: 11411
		public const int elementId_shifter6 = 31;

		// Token: 0x04002C94 RID: 11412
		public const int elementId_shifter7 = 32;

		// Token: 0x04002C95 RID: 11413
		public const int elementId_shifter8 = 33;

		// Token: 0x04002C96 RID: 11414
		public const int elementId_shifter9 = 34;

		// Token: 0x04002C97 RID: 11415
		public const int elementId_shifter10 = 35;

		// Token: 0x04002C98 RID: 11416
		public const int elementId_reverseGear = 44;

		// Token: 0x04002C99 RID: 11417
		public const int elementId_select = 36;

		// Token: 0x04002C9A RID: 11418
		public const int elementId_start = 37;

		// Token: 0x04002C9B RID: 11419
		public const int elementId_systemButton = 38;

		// Token: 0x04002C9C RID: 11420
		public const int elementId_horn = 43;

		// Token: 0x04002C9D RID: 11421
		public const int elementId_dPadUp = 39;

		// Token: 0x04002C9E RID: 11422
		public const int elementId_dPadRight = 40;

		// Token: 0x04002C9F RID: 11423
		public const int elementId_dPadDown = 41;

		// Token: 0x04002CA0 RID: 11424
		public const int elementId_dPadLeft = 42;

		// Token: 0x04002CA1 RID: 11425
		public const int elementId_dPad = 45;
	}
}
