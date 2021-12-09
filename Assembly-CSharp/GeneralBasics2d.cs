using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class GeneralBasics2d : MonoBehaviour
{
	// Token: 0x06000426 RID: 1062 RVA: 0x0003DDD0 File Offset: 0x0003BFD0
	private void Start()
	{
		GameObject gameObject = this.createSpriteDude("avatarRotate", new Vector3(-2.51208f, 10.7119f, -14.37754f), true);
		GameObject gameObject2 = this.createSpriteDude("avatarScale", new Vector3(2.51208f, 10.2119f, -14.37754f), true);
		GameObject gameObject3 = this.createSpriteDude("avatarMove", new Vector3(-3.1208f, 7.100643f, -14.37754f), true);
		LeanTween.rotateAround(gameObject, Vector3.forward, -360f, 5f);
		LeanTween.scale(gameObject2, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
		LeanTween.moveX(gameObject2, gameObject2.transform.position.x + 1f, 5f).setEase(LeanTweenType.easeOutBounce);
		LeanTween.move(gameObject3, gameObject3.transform.position + new Vector3(1.7f, 0f, 0f), 2f).setEase(LeanTweenType.easeInQuad);
		LeanTween.move(gameObject3, gameObject3.transform.position + new Vector3(2f, -1f, 0f), 2f).setDelay(3f);
		LeanTween.scale(gameObject2, new Vector3(0.2f, 0.2f, 0.2f), 1f).setDelay(7f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong(3);
		LeanTween.delayedCall(base.gameObject, 0.2f, new Action(this.advancedExamples));
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0003DF68 File Offset: 0x0003C168
	private GameObject createSpriteDude(string name, Vector3 pos, bool hasParticles = true)
	{
		GameObject gameObject = new GameObject(name);
		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0.70980394f, 1f);
		spriteRenderer.sprite = Sprite.Create(this.dudeTexture, new Rect(0f, 0f, 256f, 256f), new Vector2(0.5f, 0f), 256f);
		gameObject.transform.position = pos;
		if (hasParticles)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.prefabParticles, Vector3.zero, this.prefabParticles.transform.rotation);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = this.prefabParticles.transform.position;
		}
		return gameObject;
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00006485 File Offset: 0x00004685
	private void advancedExamples()
	{
		LeanTween.delayedCall(base.gameObject, 14f, delegate()
		{
			for (int i = 0; i < 10; i++)
			{
				GameObject rotator = new GameObject("rotator" + i.ToString());
				rotator.transform.position = new Vector3(2.71208f, 7.100643f, -12.37754f);
				GameObject gameObject = this.createSpriteDude("dude" + i.ToString(), new Vector3(-2.51208f, 7.100643f, -14.37754f), false);
				gameObject.transform.parent = rotator.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0.5f, 0.5f * (float)i);
				gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
				LeanTween.scale(gameObject, new Vector3(0.65f, 0.65f, 0.65f), 1f).setDelay((float)i * 0.2f).setEase(LeanTweenType.easeOutBack);
				float num = LeanTween.tau / 10f * (float)i;
				float r = Mathf.Sin(num + LeanTween.tau * 0f / 3f) * 0.5f + 0.5f;
				float g = Mathf.Sin(num + LeanTween.tau * 1f / 3f) * 0.5f + 0.5f;
				float b = Mathf.Sin(num + LeanTween.tau * 2f / 3f) * 0.5f + 0.5f;
				Color to = new Color(r, g, b);
				LeanTween.color(gameObject, to, 0.3f).setDelay(1.2f + (float)i * 0.4f);
				LeanTween.moveLocalZ(gameObject, -2f, 0.3f).setDelay(1.2f + (float)i * 0.4f).setEase(LeanTweenType.easeSpring).setOnComplete(delegate()
				{
					LeanTween.rotateAround(rotator, Vector3.forward, -1080f, 12f);
				});
				LeanTween.moveLocalY(gameObject, 1.17f, 1.2f).setDelay(5f + (float)i * 0.2f).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad);
				LeanTween.alpha(gameObject, 0f, 0.6f).setDelay(9.2f + (float)i * 0.4f).setDestroyOnComplete(true).setOnComplete(delegate()
				{
					UnityEngine.Object.Destroy(rotator);
				});
			}
		}).setOnCompleteOnStart(true).setRepeat(-1);
	}

	// Token: 0x04000486 RID: 1158
	public Texture2D dudeTexture;

	// Token: 0x04000487 RID: 1159
	public GameObject prefabParticles;
}
