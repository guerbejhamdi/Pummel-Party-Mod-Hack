using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class LTDescrOptional
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000608 RID: 1544 RVA: 0x000081C0 File Offset: 0x000063C0
	// (set) Token: 0x06000609 RID: 1545 RVA: 0x000081C8 File Offset: 0x000063C8
	public Transform toTrans { get; set; }

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600060A RID: 1546 RVA: 0x000081D1 File Offset: 0x000063D1
	// (set) Token: 0x0600060B RID: 1547 RVA: 0x000081D9 File Offset: 0x000063D9
	public Vector3 point { get; set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600060C RID: 1548 RVA: 0x000081E2 File Offset: 0x000063E2
	// (set) Token: 0x0600060D RID: 1549 RVA: 0x000081EA File Offset: 0x000063EA
	public Vector3 axis { get; set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600060E RID: 1550 RVA: 0x000081F3 File Offset: 0x000063F3
	// (set) Token: 0x0600060F RID: 1551 RVA: 0x000081FB File Offset: 0x000063FB
	public float lastVal { get; set; }

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000610 RID: 1552 RVA: 0x00008204 File Offset: 0x00006404
	// (set) Token: 0x06000611 RID: 1553 RVA: 0x0000820C File Offset: 0x0000640C
	public Quaternion origRotation { get; set; }

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000612 RID: 1554 RVA: 0x00008215 File Offset: 0x00006415
	// (set) Token: 0x06000613 RID: 1555 RVA: 0x0000821D File Offset: 0x0000641D
	public LTBezierPath path { get; set; }

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000614 RID: 1556 RVA: 0x00008226 File Offset: 0x00006426
	// (set) Token: 0x06000615 RID: 1557 RVA: 0x0000822E File Offset: 0x0000642E
	public LTSpline spline { get; set; }

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000616 RID: 1558 RVA: 0x00008237 File Offset: 0x00006437
	// (set) Token: 0x06000617 RID: 1559 RVA: 0x0000823F File Offset: 0x0000643F
	public LTRect ltRect { get; set; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000618 RID: 1560 RVA: 0x00008248 File Offset: 0x00006448
	// (set) Token: 0x06000619 RID: 1561 RVA: 0x00008250 File Offset: 0x00006450
	public Action<float> onUpdateFloat { get; set; }

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x00008259 File Offset: 0x00006459
	// (set) Token: 0x0600061B RID: 1563 RVA: 0x00008261 File Offset: 0x00006461
	public Action<float, float> onUpdateFloatRatio { get; set; }

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600061C RID: 1564 RVA: 0x0000826A File Offset: 0x0000646A
	// (set) Token: 0x0600061D RID: 1565 RVA: 0x00008272 File Offset: 0x00006472
	public Action<float, object> onUpdateFloatObject { get; set; }

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600061E RID: 1566 RVA: 0x0000827B File Offset: 0x0000647B
	// (set) Token: 0x0600061F RID: 1567 RVA: 0x00008283 File Offset: 0x00006483
	public Action<Vector2> onUpdateVector2 { get; set; }

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x0000828C File Offset: 0x0000648C
	// (set) Token: 0x06000621 RID: 1569 RVA: 0x00008294 File Offset: 0x00006494
	public Action<Vector3> onUpdateVector3 { get; set; }

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000622 RID: 1570 RVA: 0x0000829D File Offset: 0x0000649D
	// (set) Token: 0x06000623 RID: 1571 RVA: 0x000082A5 File Offset: 0x000064A5
	public Action<Vector3, object> onUpdateVector3Object { get; set; }

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000624 RID: 1572 RVA: 0x000082AE File Offset: 0x000064AE
	// (set) Token: 0x06000625 RID: 1573 RVA: 0x000082B6 File Offset: 0x000064B6
	public Action<Color> onUpdateColor { get; set; }

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000626 RID: 1574 RVA: 0x000082BF File Offset: 0x000064BF
	// (set) Token: 0x06000627 RID: 1575 RVA: 0x000082C7 File Offset: 0x000064C7
	public Action<Color, object> onUpdateColorObject { get; set; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000628 RID: 1576 RVA: 0x000082D0 File Offset: 0x000064D0
	// (set) Token: 0x06000629 RID: 1577 RVA: 0x000082D8 File Offset: 0x000064D8
	public Action onComplete { get; set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x0600062A RID: 1578 RVA: 0x000082E1 File Offset: 0x000064E1
	// (set) Token: 0x0600062B RID: 1579 RVA: 0x000082E9 File Offset: 0x000064E9
	public Action<object> onCompleteObject { get; set; }

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x000082F2 File Offset: 0x000064F2
	// (set) Token: 0x0600062D RID: 1581 RVA: 0x000082FA File Offset: 0x000064FA
	public object onCompleteParam { get; set; }

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x0600062E RID: 1582 RVA: 0x00008303 File Offset: 0x00006503
	// (set) Token: 0x0600062F RID: 1583 RVA: 0x0000830B File Offset: 0x0000650B
	public object onUpdateParam { get; set; }

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x00008314 File Offset: 0x00006514
	// (set) Token: 0x06000631 RID: 1585 RVA: 0x0000831C File Offset: 0x0000651C
	public Action onStart { get; set; }

	// Token: 0x06000632 RID: 1586 RVA: 0x00045994 File Offset: 0x00043B94
	public void reset()
	{
		this.animationCurve = null;
		this.onUpdateFloat = null;
		this.onUpdateFloatRatio = null;
		this.onUpdateVector2 = null;
		this.onUpdateVector3 = null;
		this.onUpdateFloatObject = null;
		this.onUpdateVector3Object = null;
		this.onUpdateColor = null;
		this.onComplete = null;
		this.onCompleteObject = null;
		this.onCompleteParam = null;
		this.onStart = null;
		this.point = Vector3.zero;
		this.initFrameCount = 0;
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00045A08 File Offset: 0x00043C08
	public void callOnUpdate(float val, float ratioPassed)
	{
		if (this.onUpdateFloat != null)
		{
			this.onUpdateFloat(val);
		}
		if (this.onUpdateFloatRatio != null)
		{
			this.onUpdateFloatRatio(val, ratioPassed);
			return;
		}
		if (this.onUpdateFloatObject != null)
		{
			this.onUpdateFloatObject(val, this.onUpdateParam);
			return;
		}
		if (this.onUpdateVector3Object != null)
		{
			this.onUpdateVector3Object(LTDescr.newVect, this.onUpdateParam);
			return;
		}
		if (this.onUpdateVector3 != null)
		{
			this.onUpdateVector3(LTDescr.newVect);
			return;
		}
		if (this.onUpdateVector2 != null)
		{
			this.onUpdateVector2(new Vector2(LTDescr.newVect.x, LTDescr.newVect.y));
		}
	}

	// Token: 0x04000551 RID: 1361
	public AnimationCurve animationCurve;

	// Token: 0x04000552 RID: 1362
	public int initFrameCount;

	// Token: 0x04000553 RID: 1363
	public Color color;
}
