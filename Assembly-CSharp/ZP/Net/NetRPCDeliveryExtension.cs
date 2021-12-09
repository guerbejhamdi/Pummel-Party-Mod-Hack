using System;
using Lidgren.Network;

namespace ZP.Net
{
	// Token: 0x0200061A RID: 1562
	internal static class NetRPCDeliveryExtension
	{
		// Token: 0x060028E6 RID: 10470 RVA: 0x0001CAC1 File Offset: 0x0001ACC1
		public static NetDeliveryMethod GetLidgrenDelivery(this NetRPCDelivery type)
		{
			switch (type)
			{
			case NetRPCDelivery.UNRELIABLE:
				return NetDeliveryMethod.Unreliable;
			case NetRPCDelivery.UNRELIABLE_SEQUENCED:
				return NetDeliveryMethod.UnreliableSequenced;
			case NetRPCDelivery.RELIABLE_UNORDERED:
				return NetDeliveryMethod.ReliableUnordered;
			case NetRPCDelivery.RELIABLE_SEQUENCED:
				return NetDeliveryMethod.ReliableSequenced;
			case NetRPCDelivery.RELIABLE_ORDERED:
				return NetDeliveryMethod.ReliableOrdered;
			default:
				return NetDeliveryMethod.Unreliable;
			}
		}
	}
}
