using System;

namespace Rewired
{
	// Token: 0x02000641 RID: 1601
	public sealed class GamepadTemplate : ControllerTemplate, IGamepadTemplate, IControllerTemplate
	{
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x0001D6CE File Offset: 0x0001B8CE
		IControllerTemplateButton IGamepadTemplate.actionBottomRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x0001D6CE File Offset: 0x0001B8CE
		IControllerTemplateButton IGamepadTemplate.a
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x0001D6D7 File Offset: 0x0001B8D7
		IControllerTemplateButton IGamepadTemplate.actionBottomRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x0001D6D7 File Offset: 0x0001B8D7
		IControllerTemplateButton IGamepadTemplate.b
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
		IControllerTemplateButton IGamepadTemplate.actionBottomRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
		IControllerTemplateButton IGamepadTemplate.c
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x0001D6E9 File Offset: 0x0001B8E9
		IControllerTemplateButton IGamepadTemplate.actionTopRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x0001D6E9 File Offset: 0x0001B8E9
		IControllerTemplateButton IGamepadTemplate.x
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x0001D6F2 File Offset: 0x0001B8F2
		IControllerTemplateButton IGamepadTemplate.actionTopRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002AFC RID: 11004 RVA: 0x0001D6F2 File Offset: 0x0001B8F2
		IControllerTemplateButton IGamepadTemplate.y
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002AFD RID: 11005 RVA: 0x0001D6FB File Offset: 0x0001B8FB
		IControllerTemplateButton IGamepadTemplate.actionTopRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002AFE RID: 11006 RVA: 0x0001D6FB File Offset: 0x0001B8FB
		IControllerTemplateButton IGamepadTemplate.z
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x0001D705 File Offset: 0x0001B905
		IControllerTemplateButton IGamepadTemplate.leftShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002B00 RID: 11008 RVA: 0x0001D705 File Offset: 0x0001B905
		IControllerTemplateButton IGamepadTemplate.leftBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x0001D70F File Offset: 0x0001B90F
		IControllerTemplateAxis IGamepadTemplate.leftShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06002B02 RID: 11010 RVA: 0x0001D70F File Offset: 0x0001B90F
		IControllerTemplateAxis IGamepadTemplate.leftTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x0001D719 File Offset: 0x0001B919
		IControllerTemplateButton IGamepadTemplate.rightShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002B04 RID: 11012 RVA: 0x0001D719 File Offset: 0x0001B919
		IControllerTemplateButton IGamepadTemplate.rightBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x0001D723 File Offset: 0x0001B923
		IControllerTemplateAxis IGamepadTemplate.rightShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x0001D723 File Offset: 0x0001B923
		IControllerTemplateAxis IGamepadTemplate.rightTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002B07 RID: 11015 RVA: 0x0001D72D File Offset: 0x0001B92D
		IControllerTemplateButton IGamepadTemplate.center1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002B08 RID: 11016 RVA: 0x0001D72D File Offset: 0x0001B92D
		IControllerTemplateButton IGamepadTemplate.back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x0001D737 File Offset: 0x0001B937
		IControllerTemplateButton IGamepadTemplate.center2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06002B0A RID: 11018 RVA: 0x0001D737 File Offset: 0x0001B937
		IControllerTemplateButton IGamepadTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06002B0B RID: 11019 RVA: 0x0001D741 File Offset: 0x0001B941
		IControllerTemplateButton IGamepadTemplate.center3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002B0C RID: 11020 RVA: 0x0001D741 File Offset: 0x0001B941
		IControllerTemplateButton IGamepadTemplate.guide
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002B0D RID: 11021 RVA: 0x0001D74B File Offset: 0x0001B94B
		IControllerTemplateThumbStick IGamepadTemplate.leftStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(23);
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x0001D755 File Offset: 0x0001B955
		IControllerTemplateThumbStick IGamepadTemplate.rightStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(24);
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x0001D75F File Offset: 0x0001B95F
		IControllerTemplateDPad IGamepadTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(25);
			}
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x0001D769 File Offset: 0x0001B969
		public GamepadTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04002C4B RID: 11339
		public static readonly Guid typeGuid = new Guid("83b427e4-086f-47f3-bb06-be266abd1ca5");

		// Token: 0x04002C4C RID: 11340
		public const int elementId_leftStickX = 0;

		// Token: 0x04002C4D RID: 11341
		public const int elementId_leftStickY = 1;

		// Token: 0x04002C4E RID: 11342
		public const int elementId_rightStickX = 2;

		// Token: 0x04002C4F RID: 11343
		public const int elementId_rightStickY = 3;

		// Token: 0x04002C50 RID: 11344
		public const int elementId_actionBottomRow1 = 4;

		// Token: 0x04002C51 RID: 11345
		public const int elementId_a = 4;

		// Token: 0x04002C52 RID: 11346
		public const int elementId_actionBottomRow2 = 5;

		// Token: 0x04002C53 RID: 11347
		public const int elementId_b = 5;

		// Token: 0x04002C54 RID: 11348
		public const int elementId_actionBottomRow3 = 6;

		// Token: 0x04002C55 RID: 11349
		public const int elementId_c = 6;

		// Token: 0x04002C56 RID: 11350
		public const int elementId_actionTopRow1 = 7;

		// Token: 0x04002C57 RID: 11351
		public const int elementId_x = 7;

		// Token: 0x04002C58 RID: 11352
		public const int elementId_actionTopRow2 = 8;

		// Token: 0x04002C59 RID: 11353
		public const int elementId_y = 8;

		// Token: 0x04002C5A RID: 11354
		public const int elementId_actionTopRow3 = 9;

		// Token: 0x04002C5B RID: 11355
		public const int elementId_z = 9;

		// Token: 0x04002C5C RID: 11356
		public const int elementId_leftShoulder1 = 10;

		// Token: 0x04002C5D RID: 11357
		public const int elementId_leftBumper = 10;

		// Token: 0x04002C5E RID: 11358
		public const int elementId_leftShoulder2 = 11;

		// Token: 0x04002C5F RID: 11359
		public const int elementId_leftTrigger = 11;

		// Token: 0x04002C60 RID: 11360
		public const int elementId_rightShoulder1 = 12;

		// Token: 0x04002C61 RID: 11361
		public const int elementId_rightBumper = 12;

		// Token: 0x04002C62 RID: 11362
		public const int elementId_rightShoulder2 = 13;

		// Token: 0x04002C63 RID: 11363
		public const int elementId_rightTrigger = 13;

		// Token: 0x04002C64 RID: 11364
		public const int elementId_center1 = 14;

		// Token: 0x04002C65 RID: 11365
		public const int elementId_back = 14;

		// Token: 0x04002C66 RID: 11366
		public const int elementId_center2 = 15;

		// Token: 0x04002C67 RID: 11367
		public const int elementId_start = 15;

		// Token: 0x04002C68 RID: 11368
		public const int elementId_center3 = 16;

		// Token: 0x04002C69 RID: 11369
		public const int elementId_guide = 16;

		// Token: 0x04002C6A RID: 11370
		public const int elementId_leftStickButton = 17;

		// Token: 0x04002C6B RID: 11371
		public const int elementId_rightStickButton = 18;

		// Token: 0x04002C6C RID: 11372
		public const int elementId_dPadUp = 19;

		// Token: 0x04002C6D RID: 11373
		public const int elementId_dPadRight = 20;

		// Token: 0x04002C6E RID: 11374
		public const int elementId_dPadDown = 21;

		// Token: 0x04002C6F RID: 11375
		public const int elementId_dPadLeft = 22;

		// Token: 0x04002C70 RID: 11376
		public const int elementId_leftStick = 23;

		// Token: 0x04002C71 RID: 11377
		public const int elementId_rightStick = 24;

		// Token: 0x04002C72 RID: 11378
		public const int elementId_dPad = 25;
	}
}
