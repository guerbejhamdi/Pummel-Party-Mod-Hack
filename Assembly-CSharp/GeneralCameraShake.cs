using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class GeneralCameraShake : MonoBehaviour
{
	// Token: 0x0600042E RID: 1070 RVA: 0x0003E2A8 File Offset: 0x0003C4A8
	private void Start()
	{
		this.avatarBig = GameObject.Find("AvatarBig");
		AnimationCurve volume = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(8.130963E-06f, 0.06526042f, 0f, -1f),
			new Keyframe(0.0007692695f, 2.449077f, 9.078861f, 9.078861f),
			new Keyframe(0.01541314f, 0.9343268f, -40f, -40f),
			new Keyframe(0.05169491f, 0.03835937f, -0.08621139f, -0.08621139f)
		});
		AnimationCurve frequency = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.003005181f, 0f, 0f),
			new Keyframe(0.01507768f, 0.002227979f, 0f, 0f)
		});
		this.boomAudioClip = LeanAudio.createAudio(volume, frequency, LeanAudio.options().setVibrato(new Vector3[]
		{
			new Vector3(0.1f, 0f, 0f)
		}));
		this.bigGuyJump();
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0003E3DC File Offset: 0x0003C5DC
	private void bigGuyJump()
	{
		float height = Mathf.PerlinNoise(this.jumpIter, 0f) * 10f;
		height = height * height * 0.3f;
		Action <>9__1;
		LeanTween.moveY(this.avatarBig, height, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(delegate()
		{
			LTDescr ltdescr = LeanTween.moveY(this.avatarBig, 0f, 0.27f).setEase(LeanTweenType.easeInQuad);
			Action onComplete;
			if ((onComplete = <>9__1) == null)
			{
				onComplete = (<>9__1 = delegate()
				{
					LeanTween.cancel(this.gameObject);
					float num = height * 0.2f;
					float time = 0.42f;
					float time2 = 1.6f;
					LTDescr shakeTween = LeanTween.rotateAroundLocal(this.gameObject, Vector3.right, num, time).setEase(LeanTweenType.easeShake).setLoopClamp().setRepeat(-1);
					LeanTween.value(this.gameObject, num, 0f, time2).setOnUpdate(delegate(float val)
					{
						shakeTween.setTo(Vector3.right * val);
					}).setEase(LeanTweenType.easeOutQuad);
					GameObject[] array = GameObject.FindGameObjectsWithTag("Respawn");
					for (int i = 0; i < array.Length; i++)
					{
						array[i].GetComponent<Rigidbody>().AddForce(Vector3.up * 100f * height);
					}
					foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("GameController"))
					{
						float num2 = gameObject.transform.eulerAngles.z;
						num2 = (float)((num2 > 0f && num2 < 180f) ? 1 : -1);
						gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(num2, 0f, 0f) * 15f * height);
					}
					LeanAudio.play(this.boomAudioClip, this.transform.position, height * 0.2f);
					LeanTween.delayedCall(2f, new Action(this.bigGuyJump));
				});
			}
			ltdescr.setOnComplete(onComplete);
		});
		this.jumpIter += 5.2f;
	}

	// Token: 0x04000489 RID: 1161
	private GameObject avatarBig;

	// Token: 0x0400048A RID: 1162
	private float jumpIter = 9.5f;

	// Token: 0x0400048B RID: 1163
	private AudioClip boomAudioClip;
}
