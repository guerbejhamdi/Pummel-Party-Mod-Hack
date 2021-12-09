using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class GeneralEventsListeners : MonoBehaviour
{
	// Token: 0x0600043C RID: 1084 RVA: 0x0000650E File Offset: 0x0000470E
	private void Awake()
	{
		LeanTween.LISTENERS_MAX = 100;
		LeanTween.EVENTS_MAX = 2;
		this.fromColor = base.GetComponent<Renderer>().material.color;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00006533 File Offset: 0x00004733
	private void Start()
	{
		LeanTween.addListener(base.gameObject, 0, new Action<LTEvent>(this.changeColor));
		LeanTween.addListener(base.gameObject, 1, new Action<LTEvent>(this.jumpUp));
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00006565 File Offset: 0x00004765
	private void jumpUp(LTEvent e)
	{
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300f);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0003E9E8 File Offset: 0x0003CBE8
	private void changeColor(LTEvent e)
	{
		float num = Vector3.Distance(((Transform)e.data).position, base.transform.position);
		Color to = new Color(UnityEngine.Random.Range(0f, 1f), 0f, UnityEngine.Random.Range(0f, 1f));
		LeanTween.value(base.gameObject, this.fromColor, to, 0.8f).setLoopPingPong(1).setDelay(num * 0.05f).setOnUpdate(delegate(Color col)
		{
			base.GetComponent<Renderer>().material.color = col;
		});
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00006581 File Offset: 0x00004781
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.towardsRotation = new Vector3(0f, (float)UnityEngine.Random.Range(-180, 180), 0f);
		}
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x000065B6 File Offset: 0x000047B6
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.turnForIter = 0f;
			this.turnForLength = UnityEngine.Random.Range(0.5f, 1.5f);
		}
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0003EA7C File Offset: 0x0003CC7C
	private void FixedUpdate()
	{
		if (this.turnForIter < this.turnForLength)
		{
			base.GetComponent<Rigidbody>().MoveRotation(base.GetComponent<Rigidbody>().rotation * Quaternion.Euler(this.towardsRotation * Time.deltaTime));
			this.turnForIter += Time.deltaTime;
		}
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4.5f);
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x000065E6 File Offset: 0x000047E6
	private void OnMouseDown()
	{
		if (Input.GetKey(KeyCode.J))
		{
			LeanTween.dispatchEvent(1);
			return;
		}
		LeanTween.dispatchEvent(0, base.transform);
	}

	// Token: 0x04000496 RID: 1174
	private Vector3 towardsRotation;

	// Token: 0x04000497 RID: 1175
	private float turnForLength = 0.5f;

	// Token: 0x04000498 RID: 1176
	private float turnForIter;

	// Token: 0x04000499 RID: 1177
	private Color fromColor;

	// Token: 0x020000D6 RID: 214
	public enum MyEvents
	{
		// Token: 0x0400049B RID: 1179
		CHANGE_COLOR,
		// Token: 0x0400049C RID: 1180
		JUMP,
		// Token: 0x0400049D RID: 1181
		LENGTH
	}
}
