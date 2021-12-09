using System;

namespace ZP.Net
{
	// Token: 0x02000608 RID: 1544
	public class ServerData
	{
		// Token: 0x0600284A RID: 10314 RVA: 0x0001C55C File Offset: 0x0001A75C
		public ServerData(string _server_name, int _players, int _observers, int _game_type, string _ip)
		{
			this.server_name = _server_name;
			this.players = _players;
			this.observers = _observers;
			this.game_type = _game_type;
			this.ip = _ip;
		}

		// Token: 0x04002B07 RID: 11015
		public string server_name;

		// Token: 0x04002B08 RID: 11016
		public int players;

		// Token: 0x04002B09 RID: 11017
		public int observers;

		// Token: 0x04002B0A RID: 11018
		public int game_type;

		// Token: 0x04002B0B RID: 11019
		public string ip;
	}
}
