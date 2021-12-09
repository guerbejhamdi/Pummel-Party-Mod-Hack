using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005D4 RID: 1492
	public class TextMeshProFloatingText : MonoBehaviour
	{
		// Token: 0x06002648 RID: 9800 RVA: 0x0001B527 File Offset: 0x00019727
		private void Awake()
		{
			this.m_transform = base.transform;
			this.m_floatingText = new GameObject(base.name + " floating text");
			this.m_cameraTransform = Camera.main.transform;
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000E770C File Offset: 0x000E590C
		private void Start()
		{
			if (this.SpawnType == 0)
			{
				this.m_textMeshPro = this.m_floatingText.AddComponent<TextMeshPro>();
				this.m_textMeshPro.rectTransform.sizeDelta = new Vector2(3f, 3f);
				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0f, 15f, 0f);
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
				this.m_textMeshPro.fontSize = 24f;
				this.m_textMeshPro.enableKerning = false;
				this.m_textMeshPro.text = string.Empty;
				this.m_textMeshPro.isTextObjectScaleStatic = this.IsTextObjectScaleStatic;
				base.StartCoroutine(this.DisplayTextMeshProFloatingText());
				return;
			}
			if (this.SpawnType == 1)
			{
				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0f, 15f, 0f);
				this.m_textMesh = this.m_floatingText.AddComponent<TextMesh>();
				this.m_textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
				this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				this.m_textMesh.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
				this.m_textMesh.anchor = TextAnchor.LowerCenter;
				this.m_textMesh.fontSize = 24;
				base.StartCoroutine(this.DisplayTextMeshFloatingText());
				return;
			}
			int spawnType = this.SpawnType;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x0001B560 File Offset: 0x00019760
		public IEnumerator DisplayTextMeshProFloatingText()
		{
			float CountDuration = 2f;
			float starting_Count = UnityEngine.Random.Range(5f, 20f);
			float current_Count = starting_Count;
			Vector3 start_pos = this.m_floatingText_Transform.position;
			Color32 start_color = this.m_textMeshPro.color;
			float alpha = 255f;
			float fadeDuration = 3f / starting_Count * CountDuration;
			while (current_Count > 0f)
			{
				current_Count -= Time.deltaTime / CountDuration * starting_Count;
				if (current_Count <= 3f)
				{
					alpha = Mathf.Clamp(alpha - Time.deltaTime / fadeDuration * 255f, 0f, 255f);
				}
				int num = (int)current_Count;
				this.m_textMeshPro.text = num.ToString();
				this.m_textMeshPro.color = new Color32(start_color.r, start_color.g, start_color.b, (byte)alpha);
				this.m_floatingText_Transform.position += new Vector3(0f, starting_Count * Time.deltaTime, 0f);
				if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
				{
					this.lastPOS = this.m_cameraTransform.position;
					this.lastRotation = this.m_cameraTransform.rotation;
					this.m_floatingText_Transform.rotation = this.lastRotation;
					Vector3 vector = this.m_transform.position - this.lastPOS;
					this.m_transform.forward = new Vector3(vector.x, 0f, vector.z);
				}
				yield return TextMeshProFloatingText.k_WaitForEndOfFrame;
			}
			yield return TextMeshProFloatingText.k_WaitForSecondsRandom[UnityEngine.Random.Range(0, 19)];
			this.m_floatingText_Transform.position = start_pos;
			base.StartCoroutine(this.DisplayTextMeshProFloatingText());
			yield break;
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x0001B56F File Offset: 0x0001976F
		public IEnumerator DisplayTextMeshFloatingText()
		{
			float CountDuration = 2f;
			float starting_Count = UnityEngine.Random.Range(5f, 20f);
			float current_Count = starting_Count;
			Vector3 start_pos = this.m_floatingText_Transform.position;
			Color32 start_color = this.m_textMesh.color;
			float alpha = 255f;
			float fadeDuration = 3f / starting_Count * CountDuration;
			while (current_Count > 0f)
			{
				current_Count -= Time.deltaTime / CountDuration * starting_Count;
				if (current_Count <= 3f)
				{
					alpha = Mathf.Clamp(alpha - Time.deltaTime / fadeDuration * 255f, 0f, 255f);
				}
				int num = (int)current_Count;
				this.m_textMesh.text = num.ToString();
				this.m_textMesh.color = new Color32(start_color.r, start_color.g, start_color.b, (byte)alpha);
				this.m_floatingText_Transform.position += new Vector3(0f, starting_Count * Time.deltaTime, 0f);
				if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
				{
					this.lastPOS = this.m_cameraTransform.position;
					this.lastRotation = this.m_cameraTransform.rotation;
					this.m_floatingText_Transform.rotation = this.lastRotation;
					Vector3 vector = this.m_transform.position - this.lastPOS;
					this.m_transform.forward = new Vector3(vector.x, 0f, vector.z);
				}
				yield return TextMeshProFloatingText.k_WaitForEndOfFrame;
			}
			yield return TextMeshProFloatingText.k_WaitForSecondsRandom[UnityEngine.Random.Range(0, 20)];
			this.m_floatingText_Transform.position = start_pos;
			base.StartCoroutine(this.DisplayTextMeshFloatingText());
			yield break;
		}

		// Token: 0x04002A01 RID: 10753
		public Font TheFont;

		// Token: 0x04002A02 RID: 10754
		private GameObject m_floatingText;

		// Token: 0x04002A03 RID: 10755
		private TextMeshPro m_textMeshPro;

		// Token: 0x04002A04 RID: 10756
		private TextMesh m_textMesh;

		// Token: 0x04002A05 RID: 10757
		private Transform m_transform;

		// Token: 0x04002A06 RID: 10758
		private Transform m_floatingText_Transform;

		// Token: 0x04002A07 RID: 10759
		private Transform m_cameraTransform;

		// Token: 0x04002A08 RID: 10760
		private Vector3 lastPOS = Vector3.zero;

		// Token: 0x04002A09 RID: 10761
		private Quaternion lastRotation = Quaternion.identity;

		// Token: 0x04002A0A RID: 10762
		public int SpawnType;

		// Token: 0x04002A0B RID: 10763
		public bool IsTextObjectScaleStatic;

		// Token: 0x04002A0C RID: 10764
		private static WaitForEndOfFrame k_WaitForEndOfFrame = new WaitForEndOfFrame();

		// Token: 0x04002A0D RID: 10765
		private static WaitForSeconds[] k_WaitForSecondsRandom = new WaitForSeconds[]
		{
			new WaitForSeconds(0.05f),
			new WaitForSeconds(0.1f),
			new WaitForSeconds(0.15f),
			new WaitForSeconds(0.2f),
			new WaitForSeconds(0.25f),
			new WaitForSeconds(0.3f),
			new WaitForSeconds(0.35f),
			new WaitForSeconds(0.4f),
			new WaitForSeconds(0.45f),
			new WaitForSeconds(0.5f),
			new WaitForSeconds(0.55f),
			new WaitForSeconds(0.6f),
			new WaitForSeconds(0.65f),
			new WaitForSeconds(0.7f),
			new WaitForSeconds(0.75f),
			new WaitForSeconds(0.8f),
			new WaitForSeconds(0.85f),
			new WaitForSeconds(0.9f),
			new WaitForSeconds(0.95f),
			new WaitForSeconds(1f)
		};
	}
}
