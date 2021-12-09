using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006CF RID: 1743
	[AddComponentMenu("")]
	public class PlayerMouseSpriteExample : MonoBehaviour
	{
		// Token: 0x060032F0 RID: 13040 RVA: 0x00108D2C File Offset: 0x00106F2C
		private void Awake()
		{
			this.pointer = UnityEngine.Object.Instantiate<GameObject>(this.pointerPrefab);
			this.pointer.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
			if (this.hideHardwarePointer)
			{
				Cursor.visible = false;
			}
			this.mouse = PlayerMouse.Factory.Create();
			this.mouse.playerId = this.playerId;
			this.mouse.xAxis.actionName = this.horizontalAction;
			this.mouse.yAxis.actionName = this.verticalAction;
			this.mouse.wheel.yAxis.actionName = this.wheelAction;
			this.mouse.leftButton.actionName = this.leftButtonAction;
			this.mouse.rightButton.actionName = this.rightButtonAction;
			this.mouse.middleButton.actionName = this.middleButtonAction;
			this.mouse.pointerSpeed = 1f;
			this.mouse.wheel.yAxis.repeatRate = 5f;
			this.mouse.screenPosition = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
			this.mouse.ScreenPositionChangedEvent += this.OnScreenPositionChanged;
			this.OnScreenPositionChanged(this.mouse.screenPosition);
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x00108EA0 File Offset: 0x001070A0
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.pointer.transform.Rotate(Vector3.forward, this.mouse.wheel.yAxis.value * 20f);
			if (this.mouse.leftButton.justPressed)
			{
				this.CreateClickEffect(new Color(0f, 1f, 0f, 1f));
			}
			if (this.mouse.rightButton.justPressed)
			{
				this.CreateClickEffect(new Color(1f, 0f, 0f, 1f));
			}
			if (this.mouse.middleButton.justPressed)
			{
				this.CreateClickEffect(new Color(1f, 1f, 0f, 1f));
			}
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x00022A65 File Offset: 0x00020C65
		private void OnDestroy()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.mouse.ScreenPositionChangedEvent -= this.OnScreenPositionChanged;
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x00108F78 File Offset: 0x00107178
		private void CreateClickEffect(Color color)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.clickEffectPrefab);
			gameObject.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
			gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(this.mouse.screenPosition.x, this.mouse.screenPosition.y, this.distanceFromCamera));
			gameObject.GetComponentInChildren<SpriteRenderer>().color = color;
			UnityEngine.Object.Destroy(gameObject, 0.5f);
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x00109008 File Offset: 0x00107208
		private void OnScreenPositionChanged(Vector2 position)
		{
			Vector3 position2 = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, this.distanceFromCamera));
			this.pointer.transform.position = position2;
		}

		// Token: 0x0400313A RID: 12602
		[Tooltip("The Player that will control the mouse")]
		public int playerId;

		// Token: 0x0400313B RID: 12603
		[Tooltip("The Rewired Action used for the mouse horizontal axis.")]
		public string horizontalAction = "MouseX";

		// Token: 0x0400313C RID: 12604
		[Tooltip("The Rewired Action used for the mouse vertical axis.")]
		public string verticalAction = "MouseY";

		// Token: 0x0400313D RID: 12605
		[Tooltip("The Rewired Action used for the mouse wheel axis.")]
		public string wheelAction = "MouseWheel";

		// Token: 0x0400313E RID: 12606
		[Tooltip("The Rewired Action used for the mouse left button.")]
		public string leftButtonAction = "MouseLeftButton";

		// Token: 0x0400313F RID: 12607
		[Tooltip("The Rewired Action used for the mouse right button.")]
		public string rightButtonAction = "MouseRightButton";

		// Token: 0x04003140 RID: 12608
		[Tooltip("The Rewired Action used for the mouse middle button.")]
		public string middleButtonAction = "MouseMiddleButton";

		// Token: 0x04003141 RID: 12609
		[Tooltip("The distance from the camera that the pointer will be drawn.")]
		public float distanceFromCamera = 1f;

		// Token: 0x04003142 RID: 12610
		[Tooltip("The scale of the sprite pointer.")]
		public float spriteScale = 0.05f;

		// Token: 0x04003143 RID: 12611
		[Tooltip("The pointer prefab.")]
		public GameObject pointerPrefab;

		// Token: 0x04003144 RID: 12612
		[Tooltip("The click effect prefab.")]
		public GameObject clickEffectPrefab;

		// Token: 0x04003145 RID: 12613
		[Tooltip("Should the hardware pointer be hidden?")]
		public bool hideHardwarePointer = true;

		// Token: 0x04003146 RID: 12614
		[NonSerialized]
		private GameObject pointer;

		// Token: 0x04003147 RID: 12615
		[NonSerialized]
		private PlayerMouse mouse;
	}
}
