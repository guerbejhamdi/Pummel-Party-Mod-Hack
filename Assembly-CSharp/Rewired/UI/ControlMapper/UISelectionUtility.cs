using System;
using System.Collections.Generic;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020006A6 RID: 1702
	public static class UISelectionUtility
	{
		// Token: 0x06003128 RID: 12584 RVA: 0x0002153A File Offset: 0x0001F73A
		public static Selectable FindNextSelectable(Selectable selectable, Transform transform, List<Selectable> allSelectables, Vector3 direction)
		{
			return UISelectionUtility.FindNextSelectables(selectable, transform, allSelectables, direction);
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x0010289C File Offset: 0x00100A9C
		public static Selectable FindNextSelectable(Selectable selectable, Transform transform, Vector3 direction)
		{
			if (Selectable.allSelectableCount > UISelectionUtility.s_reusableAllSelectables.Length)
			{
				UISelectionUtility.s_reusableAllSelectables = new Selectable[Selectable.allSelectableCount];
			}
			Selectable.AllSelectablesNoAlloc(UISelectionUtility.s_reusableAllSelectables);
			IList<Selectable> allSelectables = UISelectionUtility.s_reusableAllSelectables;
			return UISelectionUtility.FindNextSelectables(selectable, transform, allSelectables, direction);
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x001028E0 File Offset: 0x00100AE0
		private static Selectable FindNextSelectables(Selectable selectable, Transform transform, IList<Selectable> allSelectables, Vector3 direction)
		{
			RectTransform rectTransform = transform as RectTransform;
			if (rectTransform == null)
			{
				return null;
			}
			direction.Normalize();
			Vector2 vector = direction;
			Vector2 vector2 = UITools.GetPointOnRectEdge(rectTransform, vector);
			bool flag = vector == Vector2.right * -1f || vector == Vector2.right;
			float num = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			Selectable selectable2 = null;
			Selectable selectable3 = null;
			Vector2 point = vector2 + vector * 999999f;
			for (int i = 0; i < allSelectables.Count; i++)
			{
				Selectable selectable4 = allSelectables[i];
				if (!(selectable4 == selectable) && !(selectable4 == null) && selectable4.navigation.mode != Navigation.Mode.None && (selectable4.IsInteractable() || ReflectionTools.GetPrivateField<Selectable, bool>(selectable4, "m_GroupsAllowInteraction")))
				{
					RectTransform rectTransform2 = selectable4.transform as RectTransform;
					if (!(rectTransform2 == null))
					{
						Rect rect = UITools.InvertY(UITools.TransformRectTo(rectTransform2, transform, rectTransform2.rect));
						float num3;
						if (MathTools.LineIntersectsRect(vector2, point, rect, out num3))
						{
							if (flag)
							{
								num3 *= 0.25f;
							}
							if (num3 < num2)
							{
								num2 = num3;
								selectable3 = selectable4;
							}
						}
						Vector2 to = UnityTools.TransformPoint(rectTransform2, transform, rectTransform2.rect.center) - vector2;
						if (Mathf.Abs(Vector2.Angle(vector, to)) <= 75f)
						{
							float sqrMagnitude = to.sqrMagnitude;
							if (sqrMagnitude < num)
							{
								num = sqrMagnitude;
								selectable2 = selectable4;
							}
						}
					}
				}
			}
			if (!(selectable3 != null) || !(selectable2 != null))
			{
				Array.Clear(UISelectionUtility.s_reusableAllSelectables, 0, UISelectionUtility.s_reusableAllSelectables.Length);
				return selectable3 ?? selectable2;
			}
			if (num2 > num)
			{
				return selectable2;
			}
			return selectable3;
		}

		// Token: 0x04003049 RID: 12361
		private static Selectable[] s_reusableAllSelectables = new Selectable[0];
	}
}
