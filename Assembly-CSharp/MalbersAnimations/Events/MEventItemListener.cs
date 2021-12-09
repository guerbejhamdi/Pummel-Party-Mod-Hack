using System;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations.Events
{
	// Token: 0x02000777 RID: 1911
	[Serializable]
	public class MEventItemListener
	{
		// Token: 0x060036BD RID: 14013 RVA: 0x000254ED File Offset: 0x000236ED
		public virtual void OnEventInvoked()
		{
			this.Response.Invoke();
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x000254FA File Offset: 0x000236FA
		public virtual void OnEventInvoked(string value)
		{
			this.ResponseString.Invoke(value);
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x00025508 File Offset: 0x00023708
		public virtual void OnEventInvoked(float value)
		{
			this.ResponseFloat.Invoke(value);
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x00025516 File Offset: 0x00023716
		public virtual void OnEventInvoked(int value)
		{
			this.ResponseInt.Invoke(value);
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x00025524 File Offset: 0x00023724
		public virtual void OnEventInvoked(bool value)
		{
			this.ResponseBool.Invoke(value);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00025532 File Offset: 0x00023732
		public virtual void OnEventInvoked(GameObject value)
		{
			this.ResponseGO.Invoke(value);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x00025540 File Offset: 0x00023740
		public virtual void OnEventInvoked(Transform value)
		{
			this.ResponseTransform.Invoke(value);
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x0002554E File Offset: 0x0002374E
		public virtual void OnEventInvoked(Vector3 value)
		{
			this.ResponseVector.Invoke(value);
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x001171B8 File Offset: 0x001153B8
		public MEventItemListener()
		{
			this.useVoid = true;
			this.useInt = (this.useFloat = (this.useString = (this.useBool = (this.useGO = (this.useTransform = (this.useVector = false))))));
		}

		// Token: 0x040035EE RID: 13806
		public MEvent Event;

		// Token: 0x040035EF RID: 13807
		[HideInInspector]
		public bool useInt;

		// Token: 0x040035F0 RID: 13808
		[HideInInspector]
		public bool useFloat;

		// Token: 0x040035F1 RID: 13809
		[HideInInspector]
		public bool useVoid = true;

		// Token: 0x040035F2 RID: 13810
		[HideInInspector]
		public bool useString;

		// Token: 0x040035F3 RID: 13811
		[HideInInspector]
		public bool useBool;

		// Token: 0x040035F4 RID: 13812
		[HideInInspector]
		public bool useGO;

		// Token: 0x040035F5 RID: 13813
		[HideInInspector]
		public bool useTransform;

		// Token: 0x040035F6 RID: 13814
		[HideInInspector]
		public bool useVector;

		// Token: 0x040035F7 RID: 13815
		public UnityEvent Response = new UnityEvent();

		// Token: 0x040035F8 RID: 13816
		public FloatEvent ResponseFloat = new FloatEvent();

		// Token: 0x040035F9 RID: 13817
		public IntEvent ResponseInt = new IntEvent();

		// Token: 0x040035FA RID: 13818
		public BoolEvent ResponseBool = new BoolEvent();

		// Token: 0x040035FB RID: 13819
		public StringEvent ResponseString = new StringEvent();

		// Token: 0x040035FC RID: 13820
		public GameObjectEvent ResponseGO = new GameObjectEvent();

		// Token: 0x040035FD RID: 13821
		public TransformEvent ResponseTransform = new TransformEvent();

		// Token: 0x040035FE RID: 13822
		public Vector3Event ResponseVector = new Vector3Event();
	}
}
