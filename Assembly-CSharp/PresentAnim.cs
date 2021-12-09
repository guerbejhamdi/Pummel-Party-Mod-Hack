using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class PresentAnim : MonoBehaviour
{
	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x0600152D RID: 5421 RVA: 0x00010273 File Offset: 0x0000E473
	// (set) Token: 0x0600152E RID: 5422 RVA: 0x0001027B File Offset: 0x0000E47B
	public bool IsGoodPresent { get; set; }

	// Token: 0x0600152F RID: 5423 RVA: 0x00010284 File Offset: 0x0000E484
	public void Awake()
	{
		this.m_controller = base.GetComponent<PresentAnimController>();
		this.IsGoodPresent = true;
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x0009A3D8 File Offset: 0x000985D8
	public void OnDestroy()
	{
		foreach (KeyValuePair<Material, WarpTarget> keyValuePair in this.m_targets)
		{
			foreach (Renderer renderer in keyValuePair.Value.renderers)
			{
				if (!(renderer == null))
				{
					if (keyValuePair.Value.disabled)
					{
						renderer.enabled = true;
					}
					else
					{
						renderer.sharedMaterial = keyValuePair.Value.origMaterial;
					}
				}
			}
		}
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x0009A498 File Offset: 0x00098698
	public void Update()
	{
		if (this.m_warpAmount != this.m_lastWarpAmount)
		{
			foreach (KeyValuePair<Material, WarpTarget> keyValuePair in this.m_targets)
			{
				if (!(keyValuePair.Value.warpMaterial == null))
				{
					keyValuePair.Value.warpMaterial.SetFloat("_WarpAmount", this.m_warpAmount);
				}
			}
			this.m_lastWarpAmount = this.m_warpAmount;
		}
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x00010299 File Offset: 0x0000E499
	public void SetGoodPresent(bool val)
	{
		this.m_goodPresentEffects.SetActive(val);
		this.m_badPresentEffects.SetActive(!val);
		this.IsGoodPresent = val;
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x0009A530 File Offset: 0x00098730
	public void SetTarget(GameObject targetObject)
	{
		Dictionary<string, Shader> dictionary = new Dictionary<string, Shader>();
		dictionary.Add("Standard", Shader.Find("RBGames/Warp/Standard"));
		dictionary.Add("RBGames/Character", Shader.Find("RBGames/Warp/Character"));
		dictionary.Add("RBGames/CharacterCape", Shader.Find("RBGames/Warp/CharacterCape"));
		dictionary.Add("RBGames/CharacterEffectCape", Shader.Find("RBGames/Warp/CharacterEffectCape"));
		dictionary.Add("RBGames/CharacterEffectCapeUber", Shader.Find("RBGames/CharacterEffectCapeUber"));
		dictionary.Add("RBGames/FacetCharacterCape", Shader.Find("RBGames/Warp/FacetCharacterCape"));
		foreach (Renderer renderer in targetObject.GetComponentsInChildren<Renderer>())
		{
			Shader shader = null;
			if (!(renderer == null) && !(renderer.sharedMaterial == null) && !(renderer.sharedMaterial.shader == null))
			{
				if (dictionary.TryGetValue(renderer.sharedMaterial.shader.name, out shader))
				{
					WarpTarget warpTarget = null;
					if (!this.m_targets.TryGetValue(renderer.sharedMaterial, out warpTarget))
					{
						Material material = new Material(shader);
						material.CopyPropertiesFromMaterial(renderer.sharedMaterial);
						material.EnableKeyword("_WARP_ON");
						material.DisableKeyword("_WARP_OFF");
						material.SetVector("_WarpPosition", base.transform.position + new Vector3(0f, 0.5f, 0f));
						warpTarget = new WarpTarget(renderer.sharedMaterial, material, false);
						this.m_targets.Add(renderer.sharedMaterial, warpTarget);
						renderer.sharedMaterial = material;
					}
					else
					{
						renderer.sharedMaterial = warpTarget.warpMaterial;
					}
					warpTarget.renderers.Add(renderer);
				}
				else
				{
					WarpTarget warpTarget2 = null;
					if (!this.m_targets.TryGetValue(renderer.sharedMaterial, out warpTarget2))
					{
						warpTarget2 = new WarpTarget(renderer.sharedMaterial, null, true);
						this.m_targets.Add(renderer.sharedMaterial, warpTarget2);
						renderer.enabled = false;
					}
					else
					{
						renderer.enabled = false;
					}
					warpTarget2.renderers.Add(renderer);
				}
			}
		}
		this.UpdateMaterial();
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x0009A758 File Offset: 0x00098958
	private void UpdateMaterial()
	{
		foreach (KeyValuePair<Material, WarpTarget> keyValuePair in this.m_targets)
		{
			if (!(keyValuePair.Value.warpMaterial == null))
			{
				keyValuePair.Value.warpMaterial.SetVector("_WarpPosition", base.transform.position + new Vector3(0f, 0.5f, 0f));
			}
		}
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x000102BD File Offset: 0x0000E4BD
	public void SetWarpAmount(float val)
	{
		this.m_warpAmount = val;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x000102C6 File Offset: 0x0000E4C6
	public IEnumerator DoOpenAnimation(Vector3 startPosition)
	{
		float num = 0.35f;
		float num2 = 0.15f;
		float totalTime = num + num2;
		Vector3 origin = startPosition + new Vector3(0f, 10f, -1f);
		Vector3 endPosition = startPosition - new Vector3(0f, -1f, 0f);
		RaycastHit raycastHit;
		if (Physics.Raycast(origin, Vector3.down, out raycastHit, 100f, LayerMask.GetMask(new string[]
		{
			"WorldGround"
		})))
		{
			endPosition = raycastHit.point + new Vector3(0f, 0.2f, 0f);
			Debug.DrawLine(base.transform.position, raycastHit.point, Color.green, 5f);
		}
		bool hitGround = false;
		float curTime = 0f;
		while (curTime < totalTime)
		{
			float num3 = Mathf.Clamp01(curTime / totalTime);
			float t = this.m_verticalCurve.Evaluate(num3);
			float t2 = this.m_horizontalCurve.Evaluate(num3);
			float y = this.m_verticalScaleCurve.Evaluate(num3);
			float num4 = this.m_horizontalScaleCurve.Evaluate(num3);
			Vector3 position = new Vector3(Mathf.LerpUnclamped(startPosition.x, endPosition.x, t2), Mathf.LerpUnclamped(startPosition.y, endPosition.y, t), Mathf.LerpUnclamped(startPosition.z, endPosition.z, t2));
			this.m_present.position = position;
			Vector3 localScale = new Vector3(num4, y, num4);
			this.m_present.localScale = localScale;
			if (num3 >= this.m_hitGroundTime && !hitGround)
			{
				AudioSystem.PlayOneShot(this.m_hitGroundClip, 1f, 0f, 1f);
				UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_hitgroundEffect, endPosition, Quaternion.LookRotation(Vector3.up)), 3f);
				hitGround = true;
			}
			curTime += Time.deltaTime;
			yield return null;
		}
		this.m_present.position = endPosition;
		this.m_present.localScale = Vector3.one;
		yield return new WaitForSeconds(0.25f);
		yield break;
	}

	// Token: 0x04001636 RID: 5686
	[SerializeField]
	private AnimationCurve m_verticalCurve;

	// Token: 0x04001637 RID: 5687
	[SerializeField]
	private AnimationCurve m_horizontalCurve;

	// Token: 0x04001638 RID: 5688
	[SerializeField]
	private AnimationCurve m_verticalScaleCurve;

	// Token: 0x04001639 RID: 5689
	[SerializeField]
	private AnimationCurve m_horizontalScaleCurve;

	// Token: 0x0400163A RID: 5690
	[SerializeField]
	private float m_hitGroundTime = 0.8f;

	// Token: 0x0400163B RID: 5691
	[SerializeField]
	private AudioClip m_hitGroundClip;

	// Token: 0x0400163C RID: 5692
	[SerializeField]
	private GameObject m_hitgroundEffect;

	// Token: 0x0400163D RID: 5693
	[SerializeField]
	private GameObject m_goodPresentEffects;

	// Token: 0x0400163E RID: 5694
	[SerializeField]
	private GameObject m_badPresentEffects;

	// Token: 0x0400163F RID: 5695
	[SerializeField]
	private Transform m_present;

	// Token: 0x04001640 RID: 5696
	[SerializeField]
	private float m_warpAmount;

	// Token: 0x04001641 RID: 5697
	private PresentAnimController m_controller;

	// Token: 0x04001642 RID: 5698
	private Dictionary<Material, WarpTarget> m_targets = new Dictionary<Material, WarpTarget>();

	// Token: 0x04001644 RID: 5700
	private float m_lastWarpAmount;
}
