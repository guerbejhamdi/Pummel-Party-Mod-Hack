using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007E9 RID: 2025
	public class RealTimeTranslation : MonoBehaviour
	{
		// Token: 0x06003987 RID: 14727 RVA: 0x00120504 File Offset: 0x0011E704
		public void OnGUI()
		{
			GUILayout.Label("Translate:", Array.Empty<GUILayoutOption>());
			this.OriginalText = GUILayout.TextArea(this.OriginalText, new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("English -> Español", new GUILayoutOption[]
			{
				GUILayout.Height(100f)
			}))
			{
				this.StartTranslating("en", "es");
			}
			if (GUILayout.Button("Español -> English", new GUILayoutOption[]
			{
				GUILayout.Height(100f)
			}))
			{
				this.StartTranslating("es", "en");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.TextArea("Multiple Translation with 1 call:\n'This is an example' -> en,zh\n'Hola' -> en", Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Multi Translate", new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true)
			}))
			{
				this.ExampleMultiTranslations_Async();
			}
			GUILayout.EndHorizontal();
			GUILayout.TextArea(this.TranslatedText, new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.Space(10f);
			if (this.IsTranslating)
			{
				GUILayout.Label("Contacting Google....", Array.Empty<GUILayoutOption>());
			}
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x000270D6 File Offset: 0x000252D6
		public void StartTranslating(string fromCode, string toCode)
		{
			this.IsTranslating = true;
			GoogleTranslation.Translate(this.OriginalText, fromCode, toCode, new GoogleTranslation.fnOnTranslated(this.OnTranslationReady));
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x000270F8 File Offset: 0x000252F8
		private void OnTranslationReady(string Translation, string errorMsg)
		{
			this.IsTranslating = false;
			if (errorMsg != null)
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.TranslatedText = Translation;
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x00120648 File Offset: 0x0011E848
		public void ExampleMultiTranslations_Blocking()
		{
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			if (!GoogleTranslation.ForceTranslate(dictionary, true))
			{
				return;
			}
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "en", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "zh", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "", dictionary));
			Debug.Log(dictionary["Hola"].Results[0]);
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x001206FC File Offset: 0x0011E8FC
		public void ExampleMultiTranslations_Async()
		{
			this.IsTranslating = true;
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			GoogleTranslation.Translate(dictionary, new GoogleTranslation.fnOnTranslationReady(this.OnMultitranslationReady), true);
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x00120768 File Offset: 0x0011E968
		private void OnMultitranslationReady(Dictionary<string, TranslationQuery> dict, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.IsTranslating = false;
			this.TranslatedText = "";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "es", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "zh", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "", dict) + "\n";
			this.TranslatedText += dict["Hola"].Results[0];
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x00027112 File Offset: 0x00025312
		public bool IsWaitingForTranslation()
		{
			return this.IsTranslating;
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x0002711A File Offset: 0x0002531A
		public string GetTranslatedText()
		{
			return this.TranslatedText;
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x00027122 File Offset: 0x00025322
		public void SetOriginalText(string text)
		{
			this.OriginalText = text;
		}

		// Token: 0x0400380A RID: 14346
		private string OriginalText = "This is an example showing how to use the google translator to translate chat messages within the game.\nIt also supports multiline translations.";

		// Token: 0x0400380B RID: 14347
		private string TranslatedText = string.Empty;

		// Token: 0x0400380C RID: 14348
		private bool IsTranslating;
	}
}
