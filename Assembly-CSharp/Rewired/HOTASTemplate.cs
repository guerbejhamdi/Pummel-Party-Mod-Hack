using System;

namespace Rewired
{
	// Token: 0x02000643 RID: 1603
	public sealed class HOTASTemplate : ControllerTemplate, IHOTASTemplate, IControllerTemplate
	{
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002B3E RID: 11070 RVA: 0x0001D8C6 File Offset: 0x0001BAC6
		IControllerTemplateButton IHOTASTemplate.stickTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(3);
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002B3F RID: 11071 RVA: 0x0001D6CE File Offset: 0x0001B8CE
		IControllerTemplateButton IHOTASTemplate.stickTriggerStage2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002B40 RID: 11072 RVA: 0x0001D6D7 File Offset: 0x0001B8D7
		IControllerTemplateButton IHOTASTemplate.stickPinkyButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002B41 RID: 11073 RVA: 0x0001D8CF File Offset: 0x0001BACF
		IControllerTemplateButton IHOTASTemplate.stickPinkyTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(154);
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002B42 RID: 11074 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
		IControllerTemplateButton IHOTASTemplate.stickButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002B43 RID: 11075 RVA: 0x0001D6E9 File Offset: 0x0001B8E9
		IControllerTemplateButton IHOTASTemplate.stickButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002B44 RID: 11076 RVA: 0x0001D6F2 File Offset: 0x0001B8F2
		IControllerTemplateButton IHOTASTemplate.stickButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002B45 RID: 11077 RVA: 0x0001D6FB File Offset: 0x0001B8FB
		IControllerTemplateButton IHOTASTemplate.stickButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002B46 RID: 11078 RVA: 0x0001D705 File Offset: 0x0001B905
		IControllerTemplateButton IHOTASTemplate.stickButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06002B47 RID: 11079 RVA: 0x0001D7A7 File Offset: 0x0001B9A7
		IControllerTemplateButton IHOTASTemplate.stickButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002B48 RID: 11080 RVA: 0x0001D719 File Offset: 0x0001B919
		IControllerTemplateButton IHOTASTemplate.stickButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002B49 RID: 11081 RVA: 0x0001D7B1 File Offset: 0x0001B9B1
		IControllerTemplateButton IHOTASTemplate.stickButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002B4A RID: 11082 RVA: 0x0001D72D File Offset: 0x0001B92D
		IControllerTemplateButton IHOTASTemplate.stickButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002B4B RID: 11083 RVA: 0x0001D737 File Offset: 0x0001B937
		IControllerTemplateButton IHOTASTemplate.stickButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002B4C RID: 11084 RVA: 0x0001D7C5 File Offset: 0x0001B9C5
		IControllerTemplateButton IHOTASTemplate.stickBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002B4D RID: 11085 RVA: 0x0001D7CF File Offset: 0x0001B9CF
		IControllerTemplateButton IHOTASTemplate.stickBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002B4E RID: 11086 RVA: 0x0001D7D9 File Offset: 0x0001B9D9
		IControllerTemplateButton IHOTASTemplate.stickBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002B4F RID: 11087 RVA: 0x0001D7E3 File Offset: 0x0001B9E3
		IControllerTemplateButton IHOTASTemplate.stickBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002B50 RID: 11088 RVA: 0x0001D7ED File Offset: 0x0001B9ED
		IControllerTemplateButton IHOTASTemplate.stickBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x0001D7F7 File Offset: 0x0001B9F7
		IControllerTemplateButton IHOTASTemplate.stickBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x0001D801 File Offset: 0x0001BA01
		IControllerTemplateButton IHOTASTemplate.stickBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x0001D80B File Offset: 0x0001BA0B
		IControllerTemplateButton IHOTASTemplate.stickBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002B54 RID: 11092 RVA: 0x0001D815 File Offset: 0x0001BA15
		IControllerTemplateButton IHOTASTemplate.stickBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002B55 RID: 11093 RVA: 0x0001D81F File Offset: 0x0001BA1F
		IControllerTemplateButton IHOTASTemplate.stickBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002B56 RID: 11094 RVA: 0x0001D8DC File Offset: 0x0001BADC
		IControllerTemplateButton IHOTASTemplate.stickBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(161);
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002B57 RID: 11095 RVA: 0x0001D8E9 File Offset: 0x0001BAE9
		IControllerTemplateButton IHOTASTemplate.stickBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(162);
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002B58 RID: 11096 RVA: 0x0001D879 File Offset: 0x0001BA79
		IControllerTemplateButton IHOTASTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x0001D8F6 File Offset: 0x0001BAF6
		IControllerTemplateButton IHOTASTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002B5A RID: 11098 RVA: 0x0001D900 File Offset: 0x0001BB00
		IControllerTemplateButton IHOTASTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x0001D90A File Offset: 0x0001BB0A
		IControllerTemplateButton IHOTASTemplate.throttleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x0001D914 File Offset: 0x0001BB14
		IControllerTemplateButton IHOTASTemplate.throttleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x0001D91E File Offset: 0x0001BB1E
		IControllerTemplateButton IHOTASTemplate.throttleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002B5E RID: 11102 RVA: 0x0001D928 File Offset: 0x0001BB28
		IControllerTemplateButton IHOTASTemplate.throttleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x0001D932 File Offset: 0x0001BB32
		IControllerTemplateButton IHOTASTemplate.throttleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002B60 RID: 11104 RVA: 0x0001D93C File Offset: 0x0001BB3C
		IControllerTemplateButton IHOTASTemplate.throttleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x0001D946 File Offset: 0x0001BB46
		IControllerTemplateButton IHOTASTemplate.throttleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002B62 RID: 11106 RVA: 0x0001D950 File Offset: 0x0001BB50
		IControllerTemplateButton IHOTASTemplate.throttleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x0001D95A File Offset: 0x0001BB5A
		IControllerTemplateButton IHOTASTemplate.throttleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002B64 RID: 11108 RVA: 0x0001D964 File Offset: 0x0001BB64
		IControllerTemplateButton IHOTASTemplate.throttleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x0001D96E File Offset: 0x0001BB6E
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002B66 RID: 11110 RVA: 0x0001D978 File Offset: 0x0001BB78
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x0001D982 File Offset: 0x0001BB82
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x0001D98C File Offset: 0x0001BB8C
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x0001D996 File Offset: 0x0001BB96
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002B6A RID: 11114 RVA: 0x0001D9A0 File Offset: 0x0001BBA0
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x0001D9AA File Offset: 0x0001BBAA
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002B6C RID: 11116 RVA: 0x0001D9B4 File Offset: 0x0001BBB4
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(67);
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x0001D9BE File Offset: 0x0001BBBE
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(68);
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002B6E RID: 11118 RVA: 0x0001D9C8 File Offset: 0x0001BBC8
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(69);
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002B6F RID: 11119 RVA: 0x0001D9D2 File Offset: 0x0001BBD2
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(132);
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002B70 RID: 11120 RVA: 0x0001D9DF File Offset: 0x0001BBDF
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(133);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002B71 RID: 11121 RVA: 0x0001D9EC File Offset: 0x0001BBEC
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(134);
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002B72 RID: 11122 RVA: 0x0001D9F9 File Offset: 0x0001BBF9
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(135);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002B73 RID: 11123 RVA: 0x0001DA06 File Offset: 0x0001BC06
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(136);
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002B74 RID: 11124 RVA: 0x0001DA13 File Offset: 0x0001BC13
		IControllerTemplateAxis IHOTASTemplate.throttleSlider1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(70);
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x0001DA1D File Offset: 0x0001BC1D
		IControllerTemplateAxis IHOTASTemplate.throttleSlider2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(71);
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002B76 RID: 11126 RVA: 0x0001DA27 File Offset: 0x0001BC27
		IControllerTemplateAxis IHOTASTemplate.throttleSlider3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(72);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002B77 RID: 11127 RVA: 0x0001DA31 File Offset: 0x0001BC31
		IControllerTemplateAxis IHOTASTemplate.throttleSlider4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(73);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002B78 RID: 11128 RVA: 0x0001DA3B File Offset: 0x0001BC3B
		IControllerTemplateAxis IHOTASTemplate.throttleDial1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(74);
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002B79 RID: 11129 RVA: 0x0001DA45 File Offset: 0x0001BC45
		IControllerTemplateAxis IHOTASTemplate.throttleDial2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(142);
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002B7A RID: 11130 RVA: 0x0001DA52 File Offset: 0x0001BC52
		IControllerTemplateAxis IHOTASTemplate.throttleDial3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(143);
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002B7B RID: 11131 RVA: 0x0001DA5F File Offset: 0x0001BC5F
		IControllerTemplateAxis IHOTASTemplate.throttleDial4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(144);
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002B7C RID: 11132 RVA: 0x0001DA6C File Offset: 0x0001BC6C
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(145);
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002B7D RID: 11133 RVA: 0x0001DA79 File Offset: 0x0001BC79
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(146);
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x0001DA86 File Offset: 0x0001BC86
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(147);
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x0001DA93 File Offset: 0x0001BC93
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(148);
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(149);
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002B81 RID: 11137 RVA: 0x0001DAAD File Offset: 0x0001BCAD
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(150);
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x0001DABA File Offset: 0x0001BCBA
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(151);
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002B83 RID: 11139 RVA: 0x0001DAC7 File Offset: 0x0001BCC7
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(152);
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(153);
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002B85 RID: 11141 RVA: 0x0001DAE1 File Offset: 0x0001BCE1
		IControllerTemplateAxis IHOTASTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(168);
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x0001DAEE File Offset: 0x0001BCEE
		IControllerTemplateAxis IHOTASTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(169);
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002B87 RID: 11143 RVA: 0x0001DAFB File Offset: 0x0001BCFB
		IControllerTemplateAxis IHOTASTemplate.slidePedals
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(170);
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002B88 RID: 11144 RVA: 0x0001DB08 File Offset: 0x0001BD08
		IControllerTemplateStick IHOTASTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick>(171);
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002B89 RID: 11145 RVA: 0x0001DB15 File Offset: 0x0001BD15
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick1
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(172);
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x0001DB22 File Offset: 0x0001BD22
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick2
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(173);
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x0001DB2F File Offset: 0x0001BD2F
		IControllerTemplateHat IHOTASTemplate.stickHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(174);
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002B8C RID: 11148 RVA: 0x0001DB3C File Offset: 0x0001BD3C
		IControllerTemplateHat IHOTASTemplate.stickHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(175);
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x0001DB49 File Offset: 0x0001BD49
		IControllerTemplateHat IHOTASTemplate.stickHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(176);
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002B8E RID: 11150 RVA: 0x0001DB56 File Offset: 0x0001BD56
		IControllerTemplateHat IHOTASTemplate.stickHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(177);
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002B8F RID: 11151 RVA: 0x0001DB63 File Offset: 0x0001BD63
		IControllerTemplateThrottle IHOTASTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(178);
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002B90 RID: 11152 RVA: 0x0001DB70 File Offset: 0x0001BD70
		IControllerTemplateThrottle IHOTASTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(179);
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002B91 RID: 11153 RVA: 0x0001DB7D File Offset: 0x0001BD7D
		IControllerTemplateThumbStick IHOTASTemplate.throttleMiniStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(180);
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002B92 RID: 11154 RVA: 0x0001DB8A File Offset: 0x0001BD8A
		IControllerTemplateHat IHOTASTemplate.throttleHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(181);
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002B93 RID: 11155 RVA: 0x0001DB97 File Offset: 0x0001BD97
		IControllerTemplateHat IHOTASTemplate.throttleHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(182);
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002B94 RID: 11156 RVA: 0x0001DBA4 File Offset: 0x0001BDA4
		IControllerTemplateHat IHOTASTemplate.throttleHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(183);
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002B95 RID: 11157 RVA: 0x0001DBB1 File Offset: 0x0001BDB1
		IControllerTemplateHat IHOTASTemplate.throttleHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(184);
			}
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x0001D769 File Offset: 0x0001B969
		public HOTASTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04002CA2 RID: 11426
		public static readonly Guid typeGuid = new Guid("061a00cf-d8c2-4f8d-8cb5-a15a010bc53e");

		// Token: 0x04002CA3 RID: 11427
		public const int elementId_stickX = 0;

		// Token: 0x04002CA4 RID: 11428
		public const int elementId_stickY = 1;

		// Token: 0x04002CA5 RID: 11429
		public const int elementId_stickRotate = 2;

		// Token: 0x04002CA6 RID: 11430
		public const int elementId_stickMiniStick1X = 78;

		// Token: 0x04002CA7 RID: 11431
		public const int elementId_stickMiniStick1Y = 79;

		// Token: 0x04002CA8 RID: 11432
		public const int elementId_stickMiniStick1Press = 80;

		// Token: 0x04002CA9 RID: 11433
		public const int elementId_stickMiniStick2X = 81;

		// Token: 0x04002CAA RID: 11434
		public const int elementId_stickMiniStick2Y = 82;

		// Token: 0x04002CAB RID: 11435
		public const int elementId_stickMiniStick2Press = 83;

		// Token: 0x04002CAC RID: 11436
		public const int elementId_stickTrigger = 3;

		// Token: 0x04002CAD RID: 11437
		public const int elementId_stickTriggerStage2 = 4;

		// Token: 0x04002CAE RID: 11438
		public const int elementId_stickPinkyButton = 5;

		// Token: 0x04002CAF RID: 11439
		public const int elementId_stickPinkyTrigger = 154;

		// Token: 0x04002CB0 RID: 11440
		public const int elementId_stickButton1 = 6;

		// Token: 0x04002CB1 RID: 11441
		public const int elementId_stickButton2 = 7;

		// Token: 0x04002CB2 RID: 11442
		public const int elementId_stickButton3 = 8;

		// Token: 0x04002CB3 RID: 11443
		public const int elementId_stickButton4 = 9;

		// Token: 0x04002CB4 RID: 11444
		public const int elementId_stickButton5 = 10;

		// Token: 0x04002CB5 RID: 11445
		public const int elementId_stickButton6 = 11;

		// Token: 0x04002CB6 RID: 11446
		public const int elementId_stickButton7 = 12;

		// Token: 0x04002CB7 RID: 11447
		public const int elementId_stickButton8 = 13;

		// Token: 0x04002CB8 RID: 11448
		public const int elementId_stickButton9 = 14;

		// Token: 0x04002CB9 RID: 11449
		public const int elementId_stickButton10 = 15;

		// Token: 0x04002CBA RID: 11450
		public const int elementId_stickBaseButton1 = 18;

		// Token: 0x04002CBB RID: 11451
		public const int elementId_stickBaseButton2 = 19;

		// Token: 0x04002CBC RID: 11452
		public const int elementId_stickBaseButton3 = 20;

		// Token: 0x04002CBD RID: 11453
		public const int elementId_stickBaseButton4 = 21;

		// Token: 0x04002CBE RID: 11454
		public const int elementId_stickBaseButton5 = 22;

		// Token: 0x04002CBF RID: 11455
		public const int elementId_stickBaseButton6 = 23;

		// Token: 0x04002CC0 RID: 11456
		public const int elementId_stickBaseButton7 = 24;

		// Token: 0x04002CC1 RID: 11457
		public const int elementId_stickBaseButton8 = 25;

		// Token: 0x04002CC2 RID: 11458
		public const int elementId_stickBaseButton9 = 26;

		// Token: 0x04002CC3 RID: 11459
		public const int elementId_stickBaseButton10 = 27;

		// Token: 0x04002CC4 RID: 11460
		public const int elementId_stickBaseButton11 = 161;

		// Token: 0x04002CC5 RID: 11461
		public const int elementId_stickBaseButton12 = 162;

		// Token: 0x04002CC6 RID: 11462
		public const int elementId_stickHat1Up = 28;

		// Token: 0x04002CC7 RID: 11463
		public const int elementId_stickHat1UpRight = 29;

		// Token: 0x04002CC8 RID: 11464
		public const int elementId_stickHat1Right = 30;

		// Token: 0x04002CC9 RID: 11465
		public const int elementId_stickHat1DownRight = 31;

		// Token: 0x04002CCA RID: 11466
		public const int elementId_stickHat1Down = 32;

		// Token: 0x04002CCB RID: 11467
		public const int elementId_stickHat1DownLeft = 33;

		// Token: 0x04002CCC RID: 11468
		public const int elementId_stickHat1Left = 34;

		// Token: 0x04002CCD RID: 11469
		public const int elementId_stickHat1Up_Left = 35;

		// Token: 0x04002CCE RID: 11470
		public const int elementId_stickHat2Up = 36;

		// Token: 0x04002CCF RID: 11471
		public const int elementId_stickHat2Up_right = 37;

		// Token: 0x04002CD0 RID: 11472
		public const int elementId_stickHat2Right = 38;

		// Token: 0x04002CD1 RID: 11473
		public const int elementId_stickHat2Down_Right = 39;

		// Token: 0x04002CD2 RID: 11474
		public const int elementId_stickHat2Down = 40;

		// Token: 0x04002CD3 RID: 11475
		public const int elementId_stickHat2Down_Left = 41;

		// Token: 0x04002CD4 RID: 11476
		public const int elementId_stickHat2Left = 42;

		// Token: 0x04002CD5 RID: 11477
		public const int elementId_stickHat2Up_Left = 43;

		// Token: 0x04002CD6 RID: 11478
		public const int elementId_stickHat3Up = 84;

		// Token: 0x04002CD7 RID: 11479
		public const int elementId_stickHat3Up_Right = 85;

		// Token: 0x04002CD8 RID: 11480
		public const int elementId_stickHat3Right = 86;

		// Token: 0x04002CD9 RID: 11481
		public const int elementId_stickHat3Down_Right = 87;

		// Token: 0x04002CDA RID: 11482
		public const int elementId_stickHat3Down = 88;

		// Token: 0x04002CDB RID: 11483
		public const int elementId_stickHat3Down_Left = 89;

		// Token: 0x04002CDC RID: 11484
		public const int elementId_stickHat3Left = 90;

		// Token: 0x04002CDD RID: 11485
		public const int elementId_stickHat3Up_Left = 91;

		// Token: 0x04002CDE RID: 11486
		public const int elementId_stickHat4Up = 92;

		// Token: 0x04002CDF RID: 11487
		public const int elementId_stickHat4Up_Right = 93;

		// Token: 0x04002CE0 RID: 11488
		public const int elementId_stickHat4Right = 94;

		// Token: 0x04002CE1 RID: 11489
		public const int elementId_stickHat4Down_Right = 95;

		// Token: 0x04002CE2 RID: 11490
		public const int elementId_stickHat4Down = 96;

		// Token: 0x04002CE3 RID: 11491
		public const int elementId_stickHat4Down_Left = 97;

		// Token: 0x04002CE4 RID: 11492
		public const int elementId_stickHat4Left = 98;

		// Token: 0x04002CE5 RID: 11493
		public const int elementId_stickHat4Up_Left = 99;

		// Token: 0x04002CE6 RID: 11494
		public const int elementId_mode1 = 44;

		// Token: 0x04002CE7 RID: 11495
		public const int elementId_mode2 = 45;

		// Token: 0x04002CE8 RID: 11496
		public const int elementId_mode3 = 46;

		// Token: 0x04002CE9 RID: 11497
		public const int elementId_throttle1Axis = 49;

		// Token: 0x04002CEA RID: 11498
		public const int elementId_throttle2Axis = 155;

		// Token: 0x04002CEB RID: 11499
		public const int elementId_throttle1MinDetent = 166;

		// Token: 0x04002CEC RID: 11500
		public const int elementId_throttle2MinDetent = 167;

		// Token: 0x04002CED RID: 11501
		public const int elementId_throttleButton1 = 50;

		// Token: 0x04002CEE RID: 11502
		public const int elementId_throttleButton2 = 51;

		// Token: 0x04002CEF RID: 11503
		public const int elementId_throttleButton3 = 52;

		// Token: 0x04002CF0 RID: 11504
		public const int elementId_throttleButton4 = 53;

		// Token: 0x04002CF1 RID: 11505
		public const int elementId_throttleButton5 = 54;

		// Token: 0x04002CF2 RID: 11506
		public const int elementId_throttleButton6 = 55;

		// Token: 0x04002CF3 RID: 11507
		public const int elementId_throttleButton7 = 56;

		// Token: 0x04002CF4 RID: 11508
		public const int elementId_throttleButton8 = 57;

		// Token: 0x04002CF5 RID: 11509
		public const int elementId_throttleButton9 = 58;

		// Token: 0x04002CF6 RID: 11510
		public const int elementId_throttleButton10 = 59;

		// Token: 0x04002CF7 RID: 11511
		public const int elementId_throttleBaseButton1 = 60;

		// Token: 0x04002CF8 RID: 11512
		public const int elementId_throttleBaseButton2 = 61;

		// Token: 0x04002CF9 RID: 11513
		public const int elementId_throttleBaseButton3 = 62;

		// Token: 0x04002CFA RID: 11514
		public const int elementId_throttleBaseButton4 = 63;

		// Token: 0x04002CFB RID: 11515
		public const int elementId_throttleBaseButton5 = 64;

		// Token: 0x04002CFC RID: 11516
		public const int elementId_throttleBaseButton6 = 65;

		// Token: 0x04002CFD RID: 11517
		public const int elementId_throttleBaseButton7 = 66;

		// Token: 0x04002CFE RID: 11518
		public const int elementId_throttleBaseButton8 = 67;

		// Token: 0x04002CFF RID: 11519
		public const int elementId_throttleBaseButton9 = 68;

		// Token: 0x04002D00 RID: 11520
		public const int elementId_throttleBaseButton10 = 69;

		// Token: 0x04002D01 RID: 11521
		public const int elementId_throttleBaseButton11 = 132;

		// Token: 0x04002D02 RID: 11522
		public const int elementId_throttleBaseButton12 = 133;

		// Token: 0x04002D03 RID: 11523
		public const int elementId_throttleBaseButton13 = 134;

		// Token: 0x04002D04 RID: 11524
		public const int elementId_throttleBaseButton14 = 135;

		// Token: 0x04002D05 RID: 11525
		public const int elementId_throttleBaseButton15 = 136;

		// Token: 0x04002D06 RID: 11526
		public const int elementId_throttleSlider1 = 70;

		// Token: 0x04002D07 RID: 11527
		public const int elementId_throttleSlider2 = 71;

		// Token: 0x04002D08 RID: 11528
		public const int elementId_throttleSlider3 = 72;

		// Token: 0x04002D09 RID: 11529
		public const int elementId_throttleSlider4 = 73;

		// Token: 0x04002D0A RID: 11530
		public const int elementId_throttleDial1 = 74;

		// Token: 0x04002D0B RID: 11531
		public const int elementId_throttleDial2 = 142;

		// Token: 0x04002D0C RID: 11532
		public const int elementId_throttleDial3 = 143;

		// Token: 0x04002D0D RID: 11533
		public const int elementId_throttleDial4 = 144;

		// Token: 0x04002D0E RID: 11534
		public const int elementId_throttleMiniStickX = 75;

		// Token: 0x04002D0F RID: 11535
		public const int elementId_throttleMiniStickY = 76;

		// Token: 0x04002D10 RID: 11536
		public const int elementId_throttleMiniStickPress = 77;

		// Token: 0x04002D11 RID: 11537
		public const int elementId_throttleWheel1Forward = 145;

		// Token: 0x04002D12 RID: 11538
		public const int elementId_throttleWheel1Back = 146;

		// Token: 0x04002D13 RID: 11539
		public const int elementId_throttleWheel1Press = 147;

		// Token: 0x04002D14 RID: 11540
		public const int elementId_throttleWheel2Forward = 148;

		// Token: 0x04002D15 RID: 11541
		public const int elementId_throttleWheel2Back = 149;

		// Token: 0x04002D16 RID: 11542
		public const int elementId_throttleWheel2Press = 150;

		// Token: 0x04002D17 RID: 11543
		public const int elementId_throttleWheel3Forward = 151;

		// Token: 0x04002D18 RID: 11544
		public const int elementId_throttleWheel3Back = 152;

		// Token: 0x04002D19 RID: 11545
		public const int elementId_throttleWheel3Press = 153;

		// Token: 0x04002D1A RID: 11546
		public const int elementId_throttleHat1Up = 100;

		// Token: 0x04002D1B RID: 11547
		public const int elementId_throttleHat1Up_Right = 101;

		// Token: 0x04002D1C RID: 11548
		public const int elementId_throttleHat1Right = 102;

		// Token: 0x04002D1D RID: 11549
		public const int elementId_throttleHat1Down_Right = 103;

		// Token: 0x04002D1E RID: 11550
		public const int elementId_throttleHat1Down = 104;

		// Token: 0x04002D1F RID: 11551
		public const int elementId_throttleHat1Down_Left = 105;

		// Token: 0x04002D20 RID: 11552
		public const int elementId_throttleHat1Left = 106;

		// Token: 0x04002D21 RID: 11553
		public const int elementId_throttleHat1Up_Left = 107;

		// Token: 0x04002D22 RID: 11554
		public const int elementId_throttleHat2Up = 108;

		// Token: 0x04002D23 RID: 11555
		public const int elementId_throttleHat2Up_Right = 109;

		// Token: 0x04002D24 RID: 11556
		public const int elementId_throttleHat2Right = 110;

		// Token: 0x04002D25 RID: 11557
		public const int elementId_throttleHat2Down_Right = 111;

		// Token: 0x04002D26 RID: 11558
		public const int elementId_throttleHat2Down = 112;

		// Token: 0x04002D27 RID: 11559
		public const int elementId_throttleHat2Down_Left = 113;

		// Token: 0x04002D28 RID: 11560
		public const int elementId_throttleHat2Left = 114;

		// Token: 0x04002D29 RID: 11561
		public const int elementId_throttleHat2Up_Left = 115;

		// Token: 0x04002D2A RID: 11562
		public const int elementId_throttleHat3Up = 116;

		// Token: 0x04002D2B RID: 11563
		public const int elementId_throttleHat3Up_Right = 117;

		// Token: 0x04002D2C RID: 11564
		public const int elementId_throttleHat3Right = 118;

		// Token: 0x04002D2D RID: 11565
		public const int elementId_throttleHat3Down_Right = 119;

		// Token: 0x04002D2E RID: 11566
		public const int elementId_throttleHat3Down = 120;

		// Token: 0x04002D2F RID: 11567
		public const int elementId_throttleHat3Down_Left = 121;

		// Token: 0x04002D30 RID: 11568
		public const int elementId_throttleHat3Left = 122;

		// Token: 0x04002D31 RID: 11569
		public const int elementId_throttleHat3Up_Left = 123;

		// Token: 0x04002D32 RID: 11570
		public const int elementId_throttleHat4Up = 124;

		// Token: 0x04002D33 RID: 11571
		public const int elementId_throttleHat4Up_Right = 125;

		// Token: 0x04002D34 RID: 11572
		public const int elementId_throttleHat4Right = 126;

		// Token: 0x04002D35 RID: 11573
		public const int elementId_throttleHat4Down_Right = 127;

		// Token: 0x04002D36 RID: 11574
		public const int elementId_throttleHat4Down = 128;

		// Token: 0x04002D37 RID: 11575
		public const int elementId_throttleHat4Down_Left = 129;

		// Token: 0x04002D38 RID: 11576
		public const int elementId_throttleHat4Left = 130;

		// Token: 0x04002D39 RID: 11577
		public const int elementId_throttleHat4Up_Left = 131;

		// Token: 0x04002D3A RID: 11578
		public const int elementId_leftPedal = 168;

		// Token: 0x04002D3B RID: 11579
		public const int elementId_rightPedal = 169;

		// Token: 0x04002D3C RID: 11580
		public const int elementId_slidePedals = 170;

		// Token: 0x04002D3D RID: 11581
		public const int elementId_stick = 171;

		// Token: 0x04002D3E RID: 11582
		public const int elementId_stickMiniStick1 = 172;

		// Token: 0x04002D3F RID: 11583
		public const int elementId_stickMiniStick2 = 173;

		// Token: 0x04002D40 RID: 11584
		public const int elementId_stickHat1 = 174;

		// Token: 0x04002D41 RID: 11585
		public const int elementId_stickHat2 = 175;

		// Token: 0x04002D42 RID: 11586
		public const int elementId_stickHat3 = 176;

		// Token: 0x04002D43 RID: 11587
		public const int elementId_stickHat4 = 177;

		// Token: 0x04002D44 RID: 11588
		public const int elementId_throttle1 = 178;

		// Token: 0x04002D45 RID: 11589
		public const int elementId_throttle2 = 179;

		// Token: 0x04002D46 RID: 11590
		public const int elementId_throttleMiniStick = 180;

		// Token: 0x04002D47 RID: 11591
		public const int elementId_throttleHat1 = 181;

		// Token: 0x04002D48 RID: 11592
		public const int elementId_throttleHat2 = 182;

		// Token: 0x04002D49 RID: 11593
		public const int elementId_throttleHat3 = 183;

		// Token: 0x04002D4A RID: 11594
		public const int elementId_throttleHat4 = 184;
	}
}
