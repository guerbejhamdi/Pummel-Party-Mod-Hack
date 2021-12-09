using System;

namespace Rewired
{
	// Token: 0x02000646 RID: 1606
	public sealed class SixDofControllerTemplate : ControllerTemplate, ISixDofControllerTemplate, IControllerTemplate
	{
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x0001DC5F File Offset: 0x0001BE5F
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(8);
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x0001DC68 File Offset: 0x0001BE68
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(9);
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x0001DC72 File Offset: 0x0001BE72
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(10);
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x0001D70F File Offset: 0x0001B90F
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x0001D719 File Offset: 0x0001B919
		IControllerTemplateButton ISixDofControllerTemplate.button1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x0001D7B1 File Offset: 0x0001B9B1
		IControllerTemplateButton ISixDofControllerTemplate.button2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x0001D72D File Offset: 0x0001B92D
		IControllerTemplateButton ISixDofControllerTemplate.button3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x0001D737 File Offset: 0x0001B937
		IControllerTemplateButton ISixDofControllerTemplate.button4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x0001D741 File Offset: 0x0001B941
		IControllerTemplateButton ISixDofControllerTemplate.button5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x0001D7BB File Offset: 0x0001B9BB
		IControllerTemplateButton ISixDofControllerTemplate.button6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002BDA RID: 11226 RVA: 0x0001D7C5 File Offset: 0x0001B9C5
		IControllerTemplateButton ISixDofControllerTemplate.button7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002BDB RID: 11227 RVA: 0x0001D7CF File Offset: 0x0001B9CF
		IControllerTemplateButton ISixDofControllerTemplate.button8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002BDC RID: 11228 RVA: 0x0001D7D9 File Offset: 0x0001B9D9
		IControllerTemplateButton ISixDofControllerTemplate.button9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002BDD RID: 11229 RVA: 0x0001D7E3 File Offset: 0x0001B9E3
		IControllerTemplateButton ISixDofControllerTemplate.button10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002BDE RID: 11230 RVA: 0x0001D7ED File Offset: 0x0001B9ED
		IControllerTemplateButton ISixDofControllerTemplate.button11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x0001D7F7 File Offset: 0x0001B9F7
		IControllerTemplateButton ISixDofControllerTemplate.button12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002BE0 RID: 11232 RVA: 0x0001D801 File Offset: 0x0001BA01
		IControllerTemplateButton ISixDofControllerTemplate.button13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x0001D80B File Offset: 0x0001BA0B
		IControllerTemplateButton ISixDofControllerTemplate.button14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002BE2 RID: 11234 RVA: 0x0001D815 File Offset: 0x0001BA15
		IControllerTemplateButton ISixDofControllerTemplate.button15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002BE3 RID: 11235 RVA: 0x0001D81F File Offset: 0x0001BA1F
		IControllerTemplateButton ISixDofControllerTemplate.button16
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002BE4 RID: 11236 RVA: 0x0001D829 File Offset: 0x0001BA29
		IControllerTemplateButton ISixDofControllerTemplate.button17
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002BE5 RID: 11237 RVA: 0x0001D833 File Offset: 0x0001BA33
		IControllerTemplateButton ISixDofControllerTemplate.button18
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002BE6 RID: 11238 RVA: 0x0001D83D File Offset: 0x0001BA3D
		IControllerTemplateButton ISixDofControllerTemplate.button19
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002BE7 RID: 11239 RVA: 0x0001D847 File Offset: 0x0001BA47
		IControllerTemplateButton ISixDofControllerTemplate.button20
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x0001D93C File Offset: 0x0001BB3C
		IControllerTemplateButton ISixDofControllerTemplate.button21
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x0001D946 File Offset: 0x0001BB46
		IControllerTemplateButton ISixDofControllerTemplate.button22
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002BEA RID: 11242 RVA: 0x0001D950 File Offset: 0x0001BB50
		IControllerTemplateButton ISixDofControllerTemplate.button23
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002BEB RID: 11243 RVA: 0x0001D95A File Offset: 0x0001BB5A
		IControllerTemplateButton ISixDofControllerTemplate.button24
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002BEC RID: 11244 RVA: 0x0001D964 File Offset: 0x0001BB64
		IControllerTemplateButton ISixDofControllerTemplate.button25
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002BED RID: 11245 RVA: 0x0001D96E File Offset: 0x0001BB6E
		IControllerTemplateButton ISixDofControllerTemplate.button26
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002BEE RID: 11246 RVA: 0x0001D978 File Offset: 0x0001BB78
		IControllerTemplateButton ISixDofControllerTemplate.button27
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002BEF RID: 11247 RVA: 0x0001D982 File Offset: 0x0001BB82
		IControllerTemplateButton ISixDofControllerTemplate.button28
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x0001D98C File Offset: 0x0001BB8C
		IControllerTemplateButton ISixDofControllerTemplate.button29
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x0001D996 File Offset: 0x0001BB96
		IControllerTemplateButton ISixDofControllerTemplate.button30
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x0001D9A0 File Offset: 0x0001BBA0
		IControllerTemplateButton ISixDofControllerTemplate.button31
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x0001D9AA File Offset: 0x0001BBAA
		IControllerTemplateButton ISixDofControllerTemplate.button32
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x0001DC7C File Offset: 0x0001BE7C
		IControllerTemplateHat ISixDofControllerTemplate.hat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(48);
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x0001DC86 File Offset: 0x0001BE86
		IControllerTemplateHat ISixDofControllerTemplate.hat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(49);
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002BF6 RID: 11254 RVA: 0x0001DC90 File Offset: 0x0001BE90
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(52);
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x0001DC9A File Offset: 0x0001BE9A
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(53);
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		IControllerTemplateStick6D ISixDofControllerTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick6D>(54);
			}
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x0001D769 File Offset: 0x0001B969
		public SixDofControllerTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04002D9D RID: 11677
		public static readonly Guid typeGuid = new Guid("2599beb3-522b-43dd-a4ef-93fd60e5eafa");

		// Token: 0x04002D9E RID: 11678
		public const int elementId_positionX = 1;

		// Token: 0x04002D9F RID: 11679
		public const int elementId_positionY = 2;

		// Token: 0x04002DA0 RID: 11680
		public const int elementId_positionZ = 0;

		// Token: 0x04002DA1 RID: 11681
		public const int elementId_rotationX = 3;

		// Token: 0x04002DA2 RID: 11682
		public const int elementId_rotationY = 5;

		// Token: 0x04002DA3 RID: 11683
		public const int elementId_rotationZ = 4;

		// Token: 0x04002DA4 RID: 11684
		public const int elementId_throttle1Axis = 6;

		// Token: 0x04002DA5 RID: 11685
		public const int elementId_throttle1MinDetent = 50;

		// Token: 0x04002DA6 RID: 11686
		public const int elementId_throttle2Axis = 7;

		// Token: 0x04002DA7 RID: 11687
		public const int elementId_throttle2MinDetent = 51;

		// Token: 0x04002DA8 RID: 11688
		public const int elementId_extraAxis1 = 8;

		// Token: 0x04002DA9 RID: 11689
		public const int elementId_extraAxis2 = 9;

		// Token: 0x04002DAA RID: 11690
		public const int elementId_extraAxis3 = 10;

		// Token: 0x04002DAB RID: 11691
		public const int elementId_extraAxis4 = 11;

		// Token: 0x04002DAC RID: 11692
		public const int elementId_button1 = 12;

		// Token: 0x04002DAD RID: 11693
		public const int elementId_button2 = 13;

		// Token: 0x04002DAE RID: 11694
		public const int elementId_button3 = 14;

		// Token: 0x04002DAF RID: 11695
		public const int elementId_button4 = 15;

		// Token: 0x04002DB0 RID: 11696
		public const int elementId_button5 = 16;

		// Token: 0x04002DB1 RID: 11697
		public const int elementId_button6 = 17;

		// Token: 0x04002DB2 RID: 11698
		public const int elementId_button7 = 18;

		// Token: 0x04002DB3 RID: 11699
		public const int elementId_button8 = 19;

		// Token: 0x04002DB4 RID: 11700
		public const int elementId_button9 = 20;

		// Token: 0x04002DB5 RID: 11701
		public const int elementId_button10 = 21;

		// Token: 0x04002DB6 RID: 11702
		public const int elementId_button11 = 22;

		// Token: 0x04002DB7 RID: 11703
		public const int elementId_button12 = 23;

		// Token: 0x04002DB8 RID: 11704
		public const int elementId_button13 = 24;

		// Token: 0x04002DB9 RID: 11705
		public const int elementId_button14 = 25;

		// Token: 0x04002DBA RID: 11706
		public const int elementId_button15 = 26;

		// Token: 0x04002DBB RID: 11707
		public const int elementId_button16 = 27;

		// Token: 0x04002DBC RID: 11708
		public const int elementId_button17 = 28;

		// Token: 0x04002DBD RID: 11709
		public const int elementId_button18 = 29;

		// Token: 0x04002DBE RID: 11710
		public const int elementId_button19 = 30;

		// Token: 0x04002DBF RID: 11711
		public const int elementId_button20 = 31;

		// Token: 0x04002DC0 RID: 11712
		public const int elementId_button21 = 55;

		// Token: 0x04002DC1 RID: 11713
		public const int elementId_button22 = 56;

		// Token: 0x04002DC2 RID: 11714
		public const int elementId_button23 = 57;

		// Token: 0x04002DC3 RID: 11715
		public const int elementId_button24 = 58;

		// Token: 0x04002DC4 RID: 11716
		public const int elementId_button25 = 59;

		// Token: 0x04002DC5 RID: 11717
		public const int elementId_button26 = 60;

		// Token: 0x04002DC6 RID: 11718
		public const int elementId_button27 = 61;

		// Token: 0x04002DC7 RID: 11719
		public const int elementId_button28 = 62;

		// Token: 0x04002DC8 RID: 11720
		public const int elementId_button29 = 63;

		// Token: 0x04002DC9 RID: 11721
		public const int elementId_button30 = 64;

		// Token: 0x04002DCA RID: 11722
		public const int elementId_button31 = 65;

		// Token: 0x04002DCB RID: 11723
		public const int elementId_button32 = 66;

		// Token: 0x04002DCC RID: 11724
		public const int elementId_hat1Up = 32;

		// Token: 0x04002DCD RID: 11725
		public const int elementId_hat1UpRight = 33;

		// Token: 0x04002DCE RID: 11726
		public const int elementId_hat1Right = 34;

		// Token: 0x04002DCF RID: 11727
		public const int elementId_hat1DownRight = 35;

		// Token: 0x04002DD0 RID: 11728
		public const int elementId_hat1Down = 36;

		// Token: 0x04002DD1 RID: 11729
		public const int elementId_hat1DownLeft = 37;

		// Token: 0x04002DD2 RID: 11730
		public const int elementId_hat1Left = 38;

		// Token: 0x04002DD3 RID: 11731
		public const int elementId_hat1UpLeft = 39;

		// Token: 0x04002DD4 RID: 11732
		public const int elementId_hat2Up = 40;

		// Token: 0x04002DD5 RID: 11733
		public const int elementId_hat2UpRight = 41;

		// Token: 0x04002DD6 RID: 11734
		public const int elementId_hat2Right = 42;

		// Token: 0x04002DD7 RID: 11735
		public const int elementId_hat2DownRight = 43;

		// Token: 0x04002DD8 RID: 11736
		public const int elementId_hat2Down = 44;

		// Token: 0x04002DD9 RID: 11737
		public const int elementId_hat2DownLeft = 45;

		// Token: 0x04002DDA RID: 11738
		public const int elementId_hat2Left = 46;

		// Token: 0x04002DDB RID: 11739
		public const int elementId_hat2UpLeft = 47;

		// Token: 0x04002DDC RID: 11740
		public const int elementId_hat1 = 48;

		// Token: 0x04002DDD RID: 11741
		public const int elementId_hat2 = 49;

		// Token: 0x04002DDE RID: 11742
		public const int elementId_throttle1 = 52;

		// Token: 0x04002DDF RID: 11743
		public const int elementId_throttle2 = 53;

		// Token: 0x04002DE0 RID: 11744
		public const int elementId_stick = 54;
	}
}
