using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class GeneralSequencer : MonoBehaviour
{
	// Token: 0x06000446 RID: 1094 RVA: 0x0003EAF4 File Offset: 0x0003CCF4
	public void Start()
	{
		LTSeq ltseq = LeanTween.sequence(true);
		ltseq.append(LeanTween.moveY(this.avatar1, this.avatar1.transform.localPosition.y + 6f, 1f).setEaseOutQuad());
		ltseq.insert(LeanTween.alpha(this.star, 0f, 1f));
		ltseq.insert(LeanTween.scale(this.star, Vector3.one * 3f, 1f));
		ltseq.append(LeanTween.rotateAround(this.avatar1, Vector3.forward, 360f, 0.6f).setEaseInBack());
		ltseq.append(LeanTween.moveY(this.avatar1, this.avatar1.transform.localPosition.y, 1f).setEaseInQuad());
		ltseq.append(delegate()
		{
			int num = 0;
			while ((float)num < 50f)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dustCloudPrefab);
				gameObject.transform.parent = this.avatar1.transform;
				gameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0f, 0f);
				gameObject.transform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0f, 360f));
				Vector3 to = new Vector3(gameObject.transform.localPosition.x, UnityEngine.Random.Range(2f, 4f), UnityEngine.Random.Range(-10f, 10f));
				LeanTween.moveLocal(gameObject, to, 3f * this.speedScale).setEaseOutCirc();
				LeanTween.rotateAround(gameObject, Vector3.forward, 720f, 3f * this.speedScale).setEaseOutCirc();
				LeanTween.alpha(gameObject, 0f, 3f * this.speedScale).setEaseOutCirc().setDestroyOnComplete(true);
				num++;
			}
		});
		ltseq.setScale(this.speedScale);
	}

	// Token: 0x0400049E RID: 1182
	public GameObject avatar1;

	// Token: 0x0400049F RID: 1183
	public GameObject star;

	// Token: 0x040004A0 RID: 1184
	public GameObject dustCloudPrefab;

	// Token: 0x040004A1 RID: 1185
	public float speedScale = 1f;
}
