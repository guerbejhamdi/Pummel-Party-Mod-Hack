using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000717 RID: 1815
	public class MessagesBehavior : StateMachineBehaviour
	{
		// Token: 0x0600351F RID: 13599 RVA: 0x001128D0 File Offset: 0x00110AD0
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.listeners = animator.GetComponents<IAnimatorListener>();
			MesssageItem[] array = this.onTimeMessage;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sent = false;
			}
			foreach (MesssageItem messsageItem in this.onEnterMessage)
			{
				if (messsageItem.Active && messsageItem.message != string.Empty)
				{
					if (this.UseSendMessage)
					{
						this.DeliverMessage(messsageItem, animator);
					}
					else
					{
						foreach (IAnimatorListener listener in this.listeners)
						{
							this.DeliverListener(messsageItem, listener);
						}
					}
				}
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x00112974 File Offset: 0x00110B74
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			foreach (MesssageItem messsageItem in this.onExitMessage)
			{
				if (messsageItem.Active && messsageItem.message != string.Empty)
				{
					if (this.UseSendMessage)
					{
						this.DeliverMessage(messsageItem, animator);
					}
					else
					{
						foreach (IAnimatorListener listener in this.listeners)
						{
							this.DeliverListener(messsageItem, listener);
						}
					}
				}
			}
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x001129F0 File Offset: 0x00110BF0
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			foreach (MesssageItem messsageItem in this.onTimeMessage)
			{
				if (messsageItem.Active && messsageItem.message != string.Empty && !messsageItem.sent && stateInfo.normalizedTime % 1f >= messsageItem.time)
				{
					messsageItem.sent = true;
					if (this.UseSendMessage)
					{
						this.DeliverMessage(messsageItem, animator);
					}
					else
					{
						foreach (IAnimatorListener listener in this.listeners)
						{
							this.DeliverListener(messsageItem, listener);
						}
					}
				}
			}
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x00112A94 File Offset: 0x00110C94
		private void DeliverMessage(MesssageItem m, Animator anim)
		{
			switch (m.typeM)
			{
			case TypeMessage.Bool:
				anim.SendMessage(m.message, m.boolValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.Int:
				anim.SendMessage(m.message, m.intValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.Float:
				anim.SendMessage(m.message, m.floatValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.String:
				anim.SendMessage(m.message, m.stringValue, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.Void:
				anim.SendMessage(m.message, SendMessageOptions.DontRequireReceiver);
				return;
			case TypeMessage.IntVar:
				anim.SendMessage(m.message, m.intVarValue, SendMessageOptions.DontRequireReceiver);
				return;
			default:
				return;
			}
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x00112B54 File Offset: 0x00110D54
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

		// Token: 0x04003422 RID: 13346
		public bool UseSendMessage;

		// Token: 0x04003423 RID: 13347
		public MesssageItem[] onEnterMessage;

		// Token: 0x04003424 RID: 13348
		public MesssageItem[] onExitMessage;

		// Token: 0x04003425 RID: 13349
		public MesssageItem[] onTimeMessage;

		// Token: 0x04003426 RID: 13350
		private IAnimatorListener[] listeners;
	}
}
