using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x020007AB RID: 1963
	public class Messages : MonoBehaviour
	{
		// Token: 0x060037D8 RID: 14296 RVA: 0x0011A5C0 File Offset: 0x001187C0
		public virtual void SendMessage(Component component)
		{
			foreach (MesssageItem messsageItem in this.messages)
			{
				if (messsageItem.message == string.Empty || !messsageItem.Active)
				{
					break;
				}
				if (this.UseSendMessage)
				{
					this.DeliverMessage(messsageItem, component.transform.root);
				}
				else
				{
					IAnimatorListener componentInParent = component.GetComponentInParent<IAnimatorListener>();
					if (componentInParent != null)
					{
						this.DeliverListener(messsageItem, componentInParent);
					}
				}
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x00112A94 File Offset: 0x00110C94
		private void DeliverMessage(MesssageItem m, Component component)
		{
			switch (m.typeM)
			{
			case TypeMessage.Bool:
				component.SendMessage(m.message, m.boolValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.Int:
				component.SendMessage(m.message, m.intValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.Float:
				component.SendMessage(m.message, m.floatValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.String:
				component.SendMessage(m.message, m.stringValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.Void:
				component.SendMessage(m.message, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.IntVar:
				component.SendMessage(m.message, m.intVarValue, SendMessageOptions.DontRequireReceiver);
				return;
			default:
				return;
			}
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x00112B54 File Offset: 0x00110D54
		private void DeliverListener(MesssageItem m, IAnimatorListener listener)
		{
			switch (m.typeM)
			{
			case TypeMessage.Bool:
				listener.OnAnimatorBehaviourMessage(m.message, m.boolValue);
				return;
			case TypeMessage.Int:
				listener.OnAnimatorBehaviourMessage(m.message, m.intValue);
				return;
			case TypeMessage.Float:
				listener.OnAnimatorBehaviourMessage(m.message, m.floatValue);
				return;
			case TypeMessage.String:
				listener.OnAnimatorBehaviourMessage(m.message, m.stringValue);
				return;
			case TypeMessage.Void:
				listener.OnAnimatorBehaviourMessage(m.message, null);
				return;
			case TypeMessage.IntVar:
				listener.OnAnimatorBehaviourMessage(m.message, m.intVarValue);
				return;
			default:
				return;
			}
		}

		// Token: 0x040036BE RID: 14014
		public MesssageItem[] messages;

		// Token: 0x040036BF RID: 14015
		public bool UseSendMessage;

		// Token: 0x040036C0 RID: 14016
		public Component component;
	}
}
