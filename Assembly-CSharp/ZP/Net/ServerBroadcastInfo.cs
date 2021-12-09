using System;

namespace ZP.Net
{
	// Token: 0x02000607 RID: 1543
	public class ServerBroadcastInfo
	{
		// Token: 0x06002849 RID: 10313 RVA: 0x0001C537 File Offset: 0x0001A737
		public ServerBroadcastInfo(string _server_name, byte _players, byte _observers, byte _game_type)
		{
			this.server_name = _server_name;
			this.players = _players;
			this.observers = _observers;
			this.game_type = _game_type;
		}

		// Token: 0x04002B03 RID: 11011
		public string server_name;

		// Token: 0x04002B04 RID: 11012
		public byte players;

		// Token: 0x04002B05 RID: 11013
		public byte observers;

		// Token: 0x04002B06 RID: 11014
		public byte game_type;
	}
}
