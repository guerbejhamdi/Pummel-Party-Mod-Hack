using System;

namespace Rewired
{
	// Token: 0x02000644 RID: 1604
	public sealed class FlightYokeTemplate : ControllerTemplate, IFlightYokeTemplate, IControllerTemplate
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002B98 RID: 11160 RVA: 0x0001D964 File Offset: 0x0001BB64
		IControllerTemplateButton IFlightYokeTemplate.leftPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002B99 RID: 11161 RVA: 0x0001D96E File Offset: 0x0001BB6E
		IControllerTemplateButton IFlightYokeTemplate.rightPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x0001D6E9 File Offset: 0x0001B8E9
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002B9B RID: 11163 RVA: 0x0001D6F2 File Offset: 0x0001B8F2
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002B9C RID: 11164 RVA: 0x0001D6FB File Offset: 0x0001B8FB
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002B9D RID: 11165 RVA: 0x0001D705 File Offset: 0x0001B905
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002B9E RID: 11166 RVA: 0x0001D7A7 File Offset: 0x0001B9A7
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002B9F RID: 11167 RVA: 0x0001D719 File Offset: 0x0001B919
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x0001D7B1 File Offset: 0x0001B9B1
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x0001D72D File Offset: 0x0001B92D
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002BA2 RID: 11170 RVA: 0x0001D737 File Offset: 0x0001B937
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x0001D741 File Offset: 0x0001B941
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002BA4 RID: 11172 RVA: 0x0001D7BB File Offset: 0x0001B9BB
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x0001D7C5 File Offset: 0x0001B9C5
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x0001D7CF File Offset: 0x0001B9CF
		IControllerTemplateButton IFlightYokeTemplate.centerButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x0001D7D9 File Offset: 0x0001B9D9
		IControllerTemplateButton IFlightYokeTemplate.centerButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x0001D7E3 File Offset: 0x0001B9E3
		IControllerTemplateButton IFlightYokeTemplate.centerButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x0001D7ED File Offset: 0x0001B9ED
		IControllerTemplateButton IFlightYokeTemplate.centerButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002BAA RID: 11178 RVA: 0x0001D7F7 File Offset: 0x0001B9F7
		IControllerTemplateButton IFlightYokeTemplate.centerButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002BAB RID: 11179 RVA: 0x0001D801 File Offset: 0x0001BA01
		IControllerTemplateButton IFlightYokeTemplate.centerButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x0001D80B File Offset: 0x0001BA0B
		IControllerTemplateButton IFlightYokeTemplate.centerButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002BAD RID: 11181 RVA: 0x0001D815 File Offset: 0x0001BA15
		IControllerTemplateButton IFlightYokeTemplate.centerButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x0001D928 File Offset: 0x0001BB28
		IControllerTemplateButton IFlightYokeTemplate.wheel1Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002BAF RID: 11183 RVA: 0x0001D932 File Offset: 0x0001BB32
		IControllerTemplateButton IFlightYokeTemplate.wheel1Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x0001D93C File Offset: 0x0001BB3C
		IControllerTemplateButton IFlightYokeTemplate.wheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002BB1 RID: 11185 RVA: 0x0001D946 File Offset: 0x0001BB46
		IControllerTemplateButton IFlightYokeTemplate.wheel2Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x0001D950 File Offset: 0x0001BB50
		IControllerTemplateButton IFlightYokeTemplate.wheel2Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002BB3 RID: 11187 RVA: 0x0001D95A File Offset: 0x0001BB5A
		IControllerTemplateButton IFlightYokeTemplate.wheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x0001D8A1 File Offset: 0x0001BAA1
		IControllerTemplateButton IFlightYokeTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x0001D879 File Offset: 0x0001BA79
		IControllerTemplateButton IFlightYokeTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x0001D8F6 File Offset: 0x0001BAF6
		IControllerTemplateButton IFlightYokeTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002BB7 RID: 11191 RVA: 0x0001D900 File Offset: 0x0001BB00
		IControllerTemplateButton IFlightYokeTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x0001DBCF File Offset: 0x0001BDCF
		IControllerTemplateButton IFlightYokeTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(47);
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002BB9 RID: 11193 RVA: 0x0001DBD9 File Offset: 0x0001BDD9
		IControllerTemplateButton IFlightYokeTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(48);
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002BBA RID: 11194 RVA: 0x0001DBE3 File Offset: 0x0001BDE3
		IControllerTemplateButton IFlightYokeTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(49);
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002BBB RID: 11195 RVA: 0x0001D90A File Offset: 0x0001BB0A
		IControllerTemplateButton IFlightYokeTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x0001D914 File Offset: 0x0001BB14
		IControllerTemplateButton IFlightYokeTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002BBD RID: 11197 RVA: 0x0001D91E File Offset: 0x0001BB1E
		IControllerTemplateButton IFlightYokeTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002BBE RID: 11198 RVA: 0x0001D978 File Offset: 0x0001BB78
		IControllerTemplateButton IFlightYokeTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002BBF RID: 11199 RVA: 0x0001D982 File Offset: 0x0001BB82
		IControllerTemplateButton IFlightYokeTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x0001D98C File Offset: 0x0001BB8C
		IControllerTemplateButton IFlightYokeTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x0001DBED File Offset: 0x0001BDED
		IControllerTemplateYoke IFlightYokeTemplate.yoke
		{
			get
			{
				return base.GetElement<IControllerTemplateYoke>(69);
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x0001DBF7 File Offset: 0x0001BDF7
		IControllerTemplateThrottle IFlightYokeTemplate.lever1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(70);
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x0001DC01 File Offset: 0x0001BE01
		IControllerTemplateThrottle IFlightYokeTemplate.lever2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(71);
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x0001DC0B File Offset: 0x0001BE0B
		IControllerTemplateThrottle IFlightYokeTemplate.lever3
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(72);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x0001DC15 File Offset: 0x0001BE15
		IControllerTemplateThrottle IFlightYokeTemplate.lever4
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(73);
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002BC6 RID: 11206 RVA: 0x0001DC1F File Offset: 0x0001BE1F
		IControllerTemplateThrottle IFlightYokeTemplate.lever5
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(74);
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x0001DC29 File Offset: 0x0001BE29
		IControllerTemplateHat IFlightYokeTemplate.leftGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(75);
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x0001DC33 File Offset: 0x0001BE33
		IControllerTemplateHat IFlightYokeTemplate.rightGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(76);
			}
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x0001D769 File Offset: 0x0001B969
		public FlightYokeTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04002D4B RID: 11595
		public static readonly Guid typeGuid = new Guid("f311fa16-0ccc-41c0-ac4b-50f7100bb8ff");

		// Token: 0x04002D4C RID: 11596
		public const int elementId_rotateYoke = 0;

		// Token: 0x04002D4D RID: 11597
		public const int elementId_yokeZ = 1;

		// Token: 0x04002D4E RID: 11598
		public const int elementId_leftPaddle = 59;

		// Token: 0x04002D4F RID: 11599
		public const int elementId_rightPaddle = 60;

		// Token: 0x04002D50 RID: 11600
		public const int elementId_lever1Axis = 2;

		// Token: 0x04002D51 RID: 11601
		public const int elementId_lever1MinDetent = 64;

		// Token: 0x04002D52 RID: 11602
		public const int elementId_lever2Axis = 3;

		// Token: 0x04002D53 RID: 11603
		public const int elementId_lever2MinDetent = 65;

		// Token: 0x04002D54 RID: 11604
		public const int elementId_lever3Axis = 4;

		// Token: 0x04002D55 RID: 11605
		public const int elementId_lever3MinDetent = 66;

		// Token: 0x04002D56 RID: 11606
		public const int elementId_lever4Axis = 5;

		// Token: 0x04002D57 RID: 11607
		public const int elementId_lever4MinDetent = 67;

		// Token: 0x04002D58 RID: 11608
		public const int elementId_lever5Axis = 6;

		// Token: 0x04002D59 RID: 11609
		public const int elementId_lever5MinDetent = 68;

		// Token: 0x04002D5A RID: 11610
		public const int elementId_leftGripButton1 = 7;

		// Token: 0x04002D5B RID: 11611
		public const int elementId_leftGripButton2 = 8;

		// Token: 0x04002D5C RID: 11612
		public const int elementId_leftGripButton3 = 9;

		// Token: 0x04002D5D RID: 11613
		public const int elementId_leftGripButton4 = 10;

		// Token: 0x04002D5E RID: 11614
		public const int elementId_leftGripButton5 = 11;

		// Token: 0x04002D5F RID: 11615
		public const int elementId_leftGripButton6 = 12;

		// Token: 0x04002D60 RID: 11616
		public const int elementId_rightGripButton1 = 13;

		// Token: 0x04002D61 RID: 11617
		public const int elementId_rightGripButton2 = 14;

		// Token: 0x04002D62 RID: 11618
		public const int elementId_rightGripButton3 = 15;

		// Token: 0x04002D63 RID: 11619
		public const int elementId_rightGripButton4 = 16;

		// Token: 0x04002D64 RID: 11620
		public const int elementId_rightGripButton5 = 17;

		// Token: 0x04002D65 RID: 11621
		public const int elementId_rightGripButton6 = 18;

		// Token: 0x04002D66 RID: 11622
		public const int elementId_centerButton1 = 19;

		// Token: 0x04002D67 RID: 11623
		public const int elementId_centerButton2 = 20;

		// Token: 0x04002D68 RID: 11624
		public const int elementId_centerButton3 = 21;

		// Token: 0x04002D69 RID: 11625
		public const int elementId_centerButton4 = 22;

		// Token: 0x04002D6A RID: 11626
		public const int elementId_centerButton5 = 23;

		// Token: 0x04002D6B RID: 11627
		public const int elementId_centerButton6 = 24;

		// Token: 0x04002D6C RID: 11628
		public const int elementId_centerButton7 = 25;

		// Token: 0x04002D6D RID: 11629
		public const int elementId_centerButton8 = 26;

		// Token: 0x04002D6E RID: 11630
		public const int elementId_wheel1Up = 53;

		// Token: 0x04002D6F RID: 11631
		public const int elementId_wheel1Down = 54;

		// Token: 0x04002D70 RID: 11632
		public const int elementId_wheel1Press = 55;

		// Token: 0x04002D71 RID: 11633
		public const int elementId_wheel2Up = 56;

		// Token: 0x04002D72 RID: 11634
		public const int elementId_wheel2Down = 57;

		// Token: 0x04002D73 RID: 11635
		public const int elementId_wheel2Press = 58;

		// Token: 0x04002D74 RID: 11636
		public const int elementId_leftGripHatUp = 27;

		// Token: 0x04002D75 RID: 11637
		public const int elementId_leftGripHatUpRight = 28;

		// Token: 0x04002D76 RID: 11638
		public const int elementId_leftGripHatRight = 29;

		// Token: 0x04002D77 RID: 11639
		public const int elementId_leftGripHatDownRight = 30;

		// Token: 0x04002D78 RID: 11640
		public const int elementId_leftGripHatDown = 31;

		// Token: 0x04002D79 RID: 11641
		public const int elementId_leftGripHatDownLeft = 32;

		// Token: 0x04002D7A RID: 11642
		public const int elementId_leftGripHatLeft = 33;

		// Token: 0x04002D7B RID: 11643
		public const int elementId_leftGripHatUpLeft = 34;

		// Token: 0x04002D7C RID: 11644
		public const int elementId_rightGripHatUp = 35;

		// Token: 0x04002D7D RID: 11645
		public const int elementId_rightGripHatUpRight = 36;

		// Token: 0x04002D7E RID: 11646
		public const int elementId_rightGripHatRight = 37;

		// Token: 0x04002D7F RID: 11647
		public const int elementId_rightGripHatDownRight = 38;

		// Token: 0x04002D80 RID: 11648
		public const int elementId_rightGripHatDown = 39;

		// Token: 0x04002D81 RID: 11649
		public const int elementId_rightGripHatDownLeft = 40;

		// Token: 0x04002D82 RID: 11650
		public const int elementId_rightGripHatLeft = 41;

		// Token: 0x04002D83 RID: 11651
		public const int elementId_rightGripHatUpLeft = 42;

		// Token: 0x04002D84 RID: 11652
		public const int elementId_consoleButton1 = 43;

		// Token: 0x04002D85 RID: 11653
		public const int elementId_consoleButton2 = 44;

		// Token: 0x04002D86 RID: 11654
		public const int elementId_consoleButton3 = 45;

		// Token: 0x04002D87 RID: 11655
		public const int elementId_consoleButton4 = 46;

		// Token: 0x04002D88 RID: 11656
		public const int elementId_consoleButton5 = 47;

		// Token: 0x04002D89 RID: 11657
		public const int elementId_consoleButton6 = 48;

		// Token: 0x04002D8A RID: 11658
		public const int elementId_consoleButton7 = 49;

		// Token: 0x04002D8B RID: 11659
		public const int elementId_consoleButton8 = 50;

		// Token: 0x04002D8C RID: 11660
		public const int elementId_consoleButton9 = 51;

		// Token: 0x04002D8D RID: 11661
		public const int elementId_consoleButton10 = 52;

		// Token: 0x04002D8E RID: 11662
		public const int elementId_mode1 = 61;

		// Token: 0x04002D8F RID: 11663
		public const int elementId_mode2 = 62;

		// Token: 0x04002D90 RID: 11664
		public const int elementId_mode3 = 63;

		// Token: 0x04002D91 RID: 11665
		public const int elementId_yoke = 69;

		// Token: 0x04002D92 RID: 11666
		public const int elementId_lever1 = 70;

		// Token: 0x04002D93 RID: 11667
		public const int elementId_lever2 = 71;

		// Token: 0x04002D94 RID: 11668
		public const int elementId_lever3 = 72;

		// Token: 0x04002D95 RID: 11669
		public const int elementId_lever4 = 73;

		// Token: 0x04002D96 RID: 11670
		public const int elementId_lever5 = 74;

		// Token: 0x04002D97 RID: 11671
		public const int elementId_leftGripHat = 75;

		// Token: 0x04002D98 RID: 11672
		public const int elementId_rightGripHat = 76;
	}
}
