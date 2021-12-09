using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000566 RID: 1382
public class UIWorldText : MonoBehaviour
{
	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x0600245F RID: 9311 RVA: 0x0001A238 File Offset: 0x00018438
	public float TimeAlive
	{
		get
		{
			return this.counter;
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06002460 RID: 9312 RVA: 0x0001A240 File Offset: 0x00018440
	public bool IsAlive
	{
		get
		{
			return this.is_alive;
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06002461 RID: 9313 RVA: 0x0001A248 File Offset: 0x00018448
	// (set) Token: 0x06002462 RID: 9314 RVA: 0x0001A250 File Offset: 0x00018450
	public bool Active
	{
		get
		{
			return this.active;
		}
		set
		{
			this.text_obj.enabled = value;
			this.active = value;
		}
	}

	// Token: 0x06002463 RID: 9315 RVA: 0x0001A265 File Offset: 0x00018465
	public void Awake()
	{
		this.text_obj = base.GetComponent<Text>();
		this.outline_obj = base.GetComponent<NicerOutline>();
		this.shadow_obj = base.GetComponent<Shadow>();
	}

	// Token: 0x06002464 RID: 9316 RVA: 0x000DA908 File Offset: 0x000D8B08
	public void Initialize(string _text, Vector3 _position, float _life, WorldTextData _data, RectTransform _canvas_rect)
	{
		this.Active = true;
		this.text = _text;
		this.position = _position;
		this.life = _life;
		this.text_data = _data;
		this.text_obj = base.GetComponent<Text>();
		this.cam = GameManager.CurrentCamera();
		this.canvas_rect = _canvas_rect;
		this.text_obj.text = this.text;
		this.text_obj.color = this.text_data.text_color;
		this.text_obj.fontSize = this.text_data.font_size;
		this.shadow_obj.enabled = this.text_data.shadow;
		this.shadow_obj.effectColor = this.text_data.shadow_color;
		this.outline_obj.enabled = this.text_data.outline;
		this.outline_obj.effectColor = this.text_data.outline_color;
		this.outline_obj.effectDistance = this.text_data.outlineEffectDistance;
		this.counter = 0f;
		this.is_alive = true;
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x0001A28B File Offset: 0x0001848B
	public void UpdateText(float delta)
	{
		this.MyPreRender(this.cam);
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x0001A299 File Offset: 0x00018499
	public void SetCamera(Camera camera)
	{
		this.cam = camera;
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x000DAA18 File Offset: 0x000D8C18
	public void MyPreRender(Camera cam)
	{
		if (this.is_alive && this.text_obj != null)
		{
			this.counter += Time.deltaTime * 2f;
			if (this.counter > this.life)
			{
				this.is_alive = false;
			}
			float time = this.counter / this.life;
			Color color = this.text_obj.color;
			color.a = this.text_data.alpha.Evaluate(time);
			this.text_obj.color = color;
			this.cur_height = this.text_data.height.Evaluate(time);
			float num = this.text_data.scale.Evaluate(time);
			this.text_obj.rectTransform.localScale = new Vector3(num, num, 1f);
			Vector3 a = this.position + new Vector3(0f, this.cur_height * this.text_data.text_height, 0f);
			float num2 = -1f;
			if (cam != null)
			{
				num2 = Vector3.Dot(cam.transform.forward, (a - cam.transform.position).normalized);
			}
			this.text_obj.enabled = (num2 > 0f);
			Vector2 vector = -Vector2.one;
			if (cam != null)
			{
				vector = cam.WorldToViewportPoint(this.position + new Vector3(0f, this.cur_height * this.text_data.text_height, 0f));
			}
			Vector2 anchoredPosition = new Vector2(vector.x * this.canvas_rect.sizeDelta.x - this.canvas_rect.sizeDelta.x * 0.5f, vector.y * this.canvas_rect.sizeDelta.y - this.canvas_rect.sizeDelta.y * 0.5f);
			this.text_obj.rectTransform.anchoredPosition = anchoredPosition;
		}
	}

	// Token: 0x04002795 RID: 10133
	private float life;

	// Token: 0x04002796 RID: 10134
	private Vector3 position;

	// Token: 0x04002797 RID: 10135
	private string text;

	// Token: 0x04002798 RID: 10136
	private WorldTextData text_data;

	// Token: 0x04002799 RID: 10137
	private Camera cam;

	// Token: 0x0400279A RID: 10138
	private float cur_height;

	// Token: 0x0400279B RID: 10139
	private float counter;

	// Token: 0x0400279C RID: 10140
	private bool is_alive;

	// Token: 0x0400279D RID: 10141
	private Text text_obj;

	// Token: 0x0400279E RID: 10142
	private NicerOutline outline_obj;

	// Token: 0x0400279F RID: 10143
	private Shadow shadow_obj;

	// Token: 0x040027A0 RID: 10144
	private RectTransform canvas_rect;

	// Token: 0x040027A1 RID: 10145
	private bool active;
}
