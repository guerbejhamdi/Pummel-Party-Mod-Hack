using System;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
public class TestScaler : MonoBehaviour
{
	// Token: 0x060020B4 RID: 8372 RVA: 0x00017CA9 File Offset: 0x00015EA9
	private void Start()
	{
		this.startPos = base.gameObject.GetComponent<RectTransform>().anchoredPosition;
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x000CCB5C File Offset: 0x000CAD5C
	private void Update()
	{
		base.gameObject.GetComponent<RectTransform>().localScale = Vector3.one * this.curve.Evaluate(Mathf.PingPong(Time.time, this.speed));
		base.gameObject.GetComponent<RectTransform>().anchoredPosition = this.startPos + this.offset * Mathf.PingPong(Time.time, this.speed);
	}

	// Token: 0x0400237C RID: 9084
	public AnimationCurve curve;

	// Token: 0x0400237D RID: 9085
	public float speed = 1f;

	// Token: 0x0400237E RID: 9086
	private Vector2 startPos;

	// Token: 0x0400237F RID: 9087
	private Vector2 offset = new Vector2(0f, 500f);
}
