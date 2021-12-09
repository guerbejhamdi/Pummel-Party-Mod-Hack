using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class Following : MonoBehaviour
{
	// Token: 0x06000418 RID: 1048 RVA: 0x0003CBAC File Offset: 0x0003ADAC
	private void Start()
	{
		this.followArrow.gameObject.LeanDelayedCall(3f, new Action(this.moveArrow)).setOnStart(new Action(this.moveArrow)).setRepeat(-1);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.localY, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.localY, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.localY, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.localY, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.localY, 50f);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.color, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.color, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.color, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.color, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.color, 0.5f);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.scale, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.scale, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.scale, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.scale, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.scale, 5f);
		Vector3 offset = new Vector3(0f, -20f, -18f);
		LeanTween.followDamp(this.dude1Title, this.dude1, LeanProp.localPosition, 0.6f, -1f).setOffset(offset);
		LeanTween.followSpring(this.dude2Title, this.dude2, LeanProp.localPosition, 0.6f, -1f, 2f, 0.5f).setOffset(offset);
		LeanTween.followBounceOut(this.dude3Title, this.dude3, LeanProp.localPosition, 0.6f, -1f, 2f, 0.5f, 0.9f).setOffset(offset);
		LeanTween.followSpring(this.dude4Title, this.dude4, LeanProp.localPosition, 0.6f, -1f, 1.5f, 0.8f).setOffset(offset);
		LeanTween.followLinear(this.dude5Title, this.dude5, LeanProp.localPosition, 30f).setOffset(offset);
		Vector3 point = Camera.main.transform.InverseTransformPoint(this.planet.transform.position);
		LeanTween.rotateAround(Camera.main.gameObject, Vector3.left, 360f, 300f).setPoint(point).setRepeat(-1);
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0003CF34 File Offset: 0x0003B134
	private void Update()
	{
		this.fromY = LeanSmooth.spring(this.fromY, this.followArrow.localPosition.y, ref this.velocityY, 1.1f, -1f, -1f, 2f, 0.5f);
		this.fromVec3 = LeanSmooth.spring(this.fromVec3, this.dude5Title.localPosition, ref this.velocityVec3, 1.1f, -1f, -1f, 2f, 0.5f);
		this.fromColor = LeanSmooth.spring(this.fromColor, this.dude1.GetComponent<Renderer>().material.color, ref this.velocityColor, 1.1f, -1f, -1f, 2f, 0.5f);
		string[] array = new string[6];
		array[0] = "Smoothed y:";
		array[1] = this.fromY.ToString();
		array[2] = " vec3:";
		int num = 3;
		Vector3 vector = this.fromVec3;
		array[num] = vector.ToString();
		array[4] = " color:";
		int num2 = 5;
		Color color = this.fromColor;
		array[num2] = color.ToString();
		Debug.Log(string.Concat(array));
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0003D068 File Offset: 0x0003B268
	private void moveArrow()
	{
		LeanTween.moveLocalY(this.followArrow.gameObject, UnityEngine.Random.Range(-100f, 100f), 0f);
		Color to = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		LeanTween.color(this.followArrow.gameObject, to, 0f);
		float d = UnityEngine.Random.Range(5f, 10f);
		this.followArrow.localScale = Vector3.one * d;
	}

	// Token: 0x04000466 RID: 1126
	public Transform planet;

	// Token: 0x04000467 RID: 1127
	public Transform followArrow;

	// Token: 0x04000468 RID: 1128
	public Transform dude1;

	// Token: 0x04000469 RID: 1129
	public Transform dude2;

	// Token: 0x0400046A RID: 1130
	public Transform dude3;

	// Token: 0x0400046B RID: 1131
	public Transform dude4;

	// Token: 0x0400046C RID: 1132
	public Transform dude5;

	// Token: 0x0400046D RID: 1133
	public Transform dude1Title;

	// Token: 0x0400046E RID: 1134
	public Transform dude2Title;

	// Token: 0x0400046F RID: 1135
	public Transform dude3Title;

	// Token: 0x04000470 RID: 1136
	public Transform dude4Title;

	// Token: 0x04000471 RID: 1137
	public Transform dude5Title;

	// Token: 0x04000472 RID: 1138
	private Color dude1ColorVelocity;

	// Token: 0x04000473 RID: 1139
	private Vector3 velocityPos;

	// Token: 0x04000474 RID: 1140
	private float fromY;

	// Token: 0x04000475 RID: 1141
	private float velocityY;

	// Token: 0x04000476 RID: 1142
	private Vector3 fromVec3;

	// Token: 0x04000477 RID: 1143
	private Vector3 velocityVec3;

	// Token: 0x04000478 RID: 1144
	private Color fromColor;

	// Token: 0x04000479 RID: 1145
	private Color velocityColor;
}
