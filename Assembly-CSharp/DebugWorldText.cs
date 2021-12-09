using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004F4 RID: 1268
public class DebugWorldText : MonoBehaviour
{
	// Token: 0x0600216C RID: 8556 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Awake()
	{
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000CF07C File Offset: 0x000CD27C
	public void Initialize(Transform track, string text, Color text_color, Camera new_cam, int fontSize, Vector3 offset)
	{
		this.cam = new_cam;
		this.canvas_rect = GameManager.UIController.Canvas.GetComponent<RectTransform>();
		this.text_obj = base.GetComponent<Text>();
		this.offset = offset;
		this.tracked_tr = track;
		this.text_obj.text = text;
		this.text_obj.color = text_color;
		this.text_obj.fontSize = fontSize;
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x0001248D File Offset: 0x0001068D
	public void Destroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000CF0E8 File Offset: 0x000CD2E8
	private void Update()
	{
		if (this.tracked_tr != null)
		{
			this.world_pos = this.tracked_tr.position + this.offset;
			Vector2 vector = this.cam.WorldToViewportPoint(this.world_pos);
			Vector2 anchoredPosition = new Vector2(vector.x * this.canvas_rect.sizeDelta.x - this.canvas_rect.sizeDelta.x * 0.5f, vector.y * this.canvas_rect.sizeDelta.y - this.canvas_rect.sizeDelta.y * 0.5f);
			this.text_obj.rectTransform.anchoredPosition = anchoredPosition;
		}
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x0001835B File Offset: 0x0001655B
	public void SetVisible(bool val)
	{
		this.text_obj.enabled = val;
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x00018369 File Offset: 0x00016569
	public void SetWorldPos(Vector3 pos)
	{
		this.world_pos = pos;
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000CF1B0 File Offset: 0x000CD3B0
	public void UpdatePosition(Vector3 pos)
	{
		this.world_pos = pos;
		Vector2 vector = this.cam.WorldToViewportPoint(this.world_pos);
		Vector2 anchoredPosition = new Vector2(vector.x * this.canvas_rect.sizeDelta.x - this.canvas_rect.sizeDelta.x * 0.5f, vector.y * this.canvas_rect.sizeDelta.y - this.canvas_rect.sizeDelta.y * 0.5f);
		this.text_obj.rectTransform.anchoredPosition = anchoredPosition;
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x00018372 File Offset: 0x00016572
	public void SetText(string text)
	{
		if (text != this.last_text)
		{
			this.text_obj.text = text;
			this.last_text = text;
		}
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x00018395 File Offset: 0x00016595
	public void SetTextColor(Color color)
	{
		this.text_obj.color = color;
	}

	// Token: 0x0400241A RID: 9242
	private Vector3 world_pos = new Vector3(0f, 0f, 0f);

	// Token: 0x0400241B RID: 9243
	private Camera cam;

	// Token: 0x0400241C RID: 9244
	private RectTransform canvas_rect;

	// Token: 0x0400241D RID: 9245
	private Text text_obj;

	// Token: 0x0400241E RID: 9246
	private Transform tracked_tr;

	// Token: 0x0400241F RID: 9247
	private Vector3 offset = Vector3.zero;

	// Token: 0x04002420 RID: 9248
	private string last_text = "";
}
