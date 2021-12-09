using System;
using System.Collections;
using LlockhamIndustries.Decals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007B8 RID: 1976
	public class TransitionKit : MonoBehaviour
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06003867 RID: 14439 RVA: 0x0011B674 File Offset: 0x00119874
		// (remove) Token: 0x06003868 RID: 14440 RVA: 0x0011B6A8 File Offset: 0x001198A8
		public static event Action onScreenObscured;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06003869 RID: 14441 RVA: 0x0011B6DC File Offset: 0x001198DC
		// (remove) Token: 0x0600386A RID: 14442 RVA: 0x0011B710 File Offset: 0x00119910
		public static event Action onTransitionComplete;

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600386B RID: 14443 RVA: 0x000267A6 File Offset: 0x000249A6
		public float deltaTime
		{
			get
			{
				if (!this.useUnscaledDeltaTime)
				{
					return Time.deltaTime;
				}
				return Time.unscaledDeltaTime;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600386C RID: 14444 RVA: 0x0011B744 File Offset: 0x00119944
		public static TransitionKit instance
		{
			get
			{
				if (!TransitionKit._instance)
				{
					TransitionKit._instance = (UnityEngine.Object.FindObjectOfType(typeof(TransitionKit)) as TransitionKit);
					if (!TransitionKit._instance)
					{
						GameObject gameObject = new GameObject("TransitionKit");
						gameObject.layer = 31;
						gameObject.transform.position = new Vector3(99999f, 99999f, 99999f);
						TransitionKit._instance = gameObject.AddComponent<TransitionKit>();
						TransitionKit._audio_source = gameObject.AddComponent<AudioSource>();
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return TransitionKit._instance;
			}
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x0011B7D4 File Offset: 0x001199D4
		private T getOrAddComponent<T>() where T : Component
		{
			T t = base.gameObject.GetComponent<T>();
			if (t == null)
			{
				t = base.gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x0011B808 File Offset: 0x00119A08
		private T getOrAddComponentChild<T>() where T : Component
		{
			Transform transform = base.transform.Find("Obj");
			if (transform == null)
			{
				GameObject gameObject = new GameObject("Obj");
				gameObject.transform.SetParent(base.transform, false);
				gameObject.layer = LayerMask.NameToLayer("ParticleUI");
				transform = gameObject.transform;
			}
			T t = transform.gameObject.GetComponent<T>();
			if (t == null)
			{
				t = transform.gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x0011B888 File Offset: 0x00119A88
		private void initialize()
		{
			this.getOrAddComponent<MeshFilter>().mesh = (this._transitionKitDelegate.meshForDisplay() ?? this.generateQuadMesh());
			this.material = this.getOrAddComponent<MeshRenderer>().material;
			this.material.shader = (this._transitionKitDelegate.shaderForTransition() ?? Shader.Find("prime[31]/Transitions/Texture With Alpha"));
			this.material.color = Color.white;
			TransitionKit._instance.StartCoroutine(TransitionKit._instance.setupCameraAndTexture());
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0011B910 File Offset: 0x00119B10
		private Mesh generateQuadMesh()
		{
			float num = 5f;
			float num2 = num * ((float)Screen.width / (float)Screen.height);
			return new Mesh
			{
				vertices = new Vector3[]
				{
					new Vector3(-num2, -num, 0f),
					new Vector3(-num2, num, 0f),
					new Vector3(num2, -num, 0f),
					new Vector3(num2, num, 0f)
				},
				uv = new Vector2[]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 1f),
					new Vector2(1f, 0f),
					new Vector2(1f, 1f)
				},
				triangles = new int[]
				{
					0,
					1,
					2,
					3,
					2,
					1
				}
			};
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000267BB File Offset: 0x000249BB
		private IEnumerator setupCameraAndTexture()
		{
			yield return new WaitForEndOfFrame();
			this.material.mainTexture = (this._transitionKitDelegate.textureForDisplay() ?? this.getScreenshotTexture());
			this.transitionKitCamera = this.getOrAddComponent<Camera>();
			this.getOrAddComponent<ProjectionBlocker>();
			this.transitionKitCamera.orthographic = true;
			this.transitionKitCamera.nearClipPlane = -1f;
			this.transitionKitCamera.farClipPlane = 1f;
			this.transitionKitCamera.depth = float.MaxValue;
			this.transitionKitCamera.cullingMask = int.MinValue;
			this.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			this.transitionKitCamera.enabled = true;
			if (TransitionKit.onScreenObscured != null)
			{
				TransitionKit.onScreenObscured();
			}
			yield return base.StartCoroutine(this._transitionKitDelegate.onScreenObscured(this));
			this.cleanup();
			yield break;
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x000267CA File Offset: 0x000249CA
		private Texture2D getScreenshotTexture()
		{
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false, false);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x0011BA14 File Offset: 0x00119C14
		private void cleanup()
		{
			if (TransitionKit._instance == null)
			{
				return;
			}
			if (TransitionKit.onTransitionComplete != null)
			{
				TransitionKit.onTransitionComplete();
			}
			this._transitionKitDelegate = null;
			this.context = null;
			if (TransitionKit.keepTransitionKitInstance)
			{
				base.GetComponent<MeshRenderer>().material.mainTexture = null;
				base.GetComponent<MeshFilter>().mesh = null;
				base.gameObject.SetActive(false);
				this.transitionKitCamera.enabled = false;
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			TransitionKit._instance = null;
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x00026808 File Offset: 0x00024A08
		public void transitionWithDelegate(TransitionKitDelegate transitionKitDelegate)
		{
			base.gameObject.SetActive(true);
			this._transitionKitDelegate = transitionKitDelegate;
			this.initialize();
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0011BA9C File Offset: 0x00119C9C
		public void makeTextureTransparent()
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, Color.clear);
			texture2D.Apply();
			this.material.mainTexture = texture2D;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00026823 File Offset: 0x00024A23
		public void PlaySound(AudioClip clip)
		{
			TransitionKit._audio_source.volume = AudioSystem.GetVolume(SoundType.Effect, 1f);
			TransitionKit._audio_source.PlayOneShot(clip);
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x00026845 File Offset: 0x00024A45
		public IEnumerator waitForLevelToLoad(int level)
		{
			while (SceneManager.GetActiveScene().buildIndex != level)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x00026854 File Offset: 0x00024A54
		public IEnumerator waitForLevelToLoad(string level)
		{
			while (SceneManager.GetActiveScene().name != level)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x00026863 File Offset: 0x00024A63
		public IEnumerator waitForAsyncLoad(AsyncOperation op)
		{
			while (!op.isDone)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x00026872 File Offset: 0x00024A72
		public IEnumerator tickProgressPropertyInMaterial(float duration, bool reverseDirection = false)
		{
			float start = reverseDirection ? 1f : 0f;
			float end = reverseDirection ? 0f : 1f;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += this.deltaTime;
				this.Set(start, end, elapsed, duration);
				yield return null;
			}
			this.Set(start, end, duration, duration);
			yield break;
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x0011BAD0 File Offset: 0x00119CD0
		private void Set(float start, float end, float elapsed, float duration)
		{
			float num = Mathf.Lerp(start, end, Mathf.Pow(elapsed / duration, 2f));
			if (num >= 1f)
			{
				num = 1000f;
			}
			this.material.SetFloat("_Progress", num);
		}

		// Token: 0x04003708 RID: 14088
		public static bool keepTransitionKitInstance;

		// Token: 0x04003709 RID: 14089
		private const int _transitionKitLayer = 31;

		// Token: 0x0400370A RID: 14090
		private TransitionKitDelegate _transitionKitDelegate;

		// Token: 0x0400370B RID: 14091
		public Camera transitionKitCamera;

		// Token: 0x0400370C RID: 14092
		public Canvas transitionKitCanvas;

		// Token: 0x0400370D RID: 14093
		public Material material;

		// Token: 0x0400370E RID: 14094
		public bool useUnscaledDeltaTime;

		// Token: 0x0400370F RID: 14095
		public object context;

		// Token: 0x04003710 RID: 14096
		private static TransitionKit _instance;

		// Token: 0x04003711 RID: 14097
		private static AudioSource _audio_source;
	}
}
