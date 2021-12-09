using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x02000853 RID: 2131
	public class JSONNode
	{
		// Token: 0x06003C2D RID: 15405 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000A3A RID: 2618
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000A3B RID: 2619
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003C32 RID: 15410 RVA: 0x0001B956 File Offset: 0x00019B56
		// (set) Token: 0x06003C33 RID: 15411 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06003C34 RID: 15412 RVA: 0x0000539F File Offset: 0x0000359F
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x00028455 File Offset: 0x00026655
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0000FA9A File Offset: 0x0000DC9A
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06003C39 RID: 15417 RVA: 0x00028463 File Offset: 0x00026663
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003C3A RID: 15418 RVA: 0x0002846C File Offset: 0x0002666C
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode jsonnode in this.Childs)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChilds)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x0002847C File Offset: 0x0002667C
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x0002847C File Offset: 0x0002667C
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x0012EA4C File Offset: 0x0012CC4C
		// (set) Token: 0x06003C3E RID: 15422 RVA: 0x00028483 File Offset: 0x00026683
		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x0012EA70 File Offset: 0x0012CC70
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x00028492 File Offset: 0x00026692
		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x0012EA9C File Offset: 0x0012CC9C
		// (set) Token: 0x06003C42 RID: 15426 RVA: 0x000284A1 File Offset: 0x000266A1
		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x0012EAD0 File Offset: 0x0012CCD0
		// (set) Token: 0x06003C44 RID: 15428 RVA: 0x000284B0 File Offset: 0x000266B0
		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x000284C7 File Offset: 0x000266C7
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06003C46 RID: 15430 RVA: 0x000284CF File Offset: 0x000266CF
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x000284D7 File Offset: 0x000266D7
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x000284DF File Offset: 0x000266DF
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x000284F2 File Offset: 0x000266F2
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x00028505 File Offset: 0x00026705
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x00028511 File Offset: 0x00026711
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x00028517 File Offset: 0x00026717
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x0012EB00 File Offset: 0x0012CD00
		internal static string Escape(string aText)
		{
			string text = "";
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				case '\v':
					goto IL_A3;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_A3;
						}
						text += "\\\\";
					}
					else
					{
						text += "\\\"";
					}
					break;
				}
				IL_B1:
				i++;
				continue;
				IL_A3:
				text += c.ToString();
				goto IL_B1;
			}
			return text;
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x0012EBD0 File Offset: 0x0012CDD0
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = "";
			string text2 = "";
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_429;
						case '\v':
						case '\f':
							goto IL_412;
						default:
							if (c != ' ')
							{
								goto IL_412;
							}
							break;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_412;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
						else
						{
							if (text != "")
							{
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(text);
								}
								else if (text2 != "")
								{
									jsonnode.Add(text2, text);
								}
							}
							text2 = "";
							text = "";
						}
					}
					else
					{
						flag = !flag;
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									text += aJSON[i].ToString();
									goto IL_429;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != "")
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = "";
								text = "";
								jsonnode = stack.Peek();
								goto IL_429;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											text += "\b";
											goto IL_429;
										}
										if (c2 == 'f')
										{
											text += "\f";
											goto IL_429;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											text += "\n";
											goto IL_429;
										}
										switch (c2)
										{
										case 'r':
											text += "\r";
											goto IL_429;
										case 't':
											text += "\t";
											goto IL_429;
										case 'u':
										{
											string s = aJSON.Substring(i + 1, 4);
											text += ((char)int.Parse(s, NumberStyles.AllowHexSpecifier)).ToString();
											i += 4;
											goto IL_429;
										}
										}
									}
									text += c2.ToString();
									goto IL_429;
								}
								goto IL_429;
							case ']':
								break;
							default:
								goto IL_412;
							}
						}
						else
						{
							if (flag)
							{
								text += aJSON[i].ToString();
								goto IL_429;
							}
							text2 = text;
							text = "";
							goto IL_429;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_412;
						}
					}
					else
					{
						if (flag)
						{
							text += aJSON[i].ToString();
							goto IL_429;
						}
						stack.Push(new JSONClass());
						if (jsonnode != null)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stack.Peek());
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, stack.Peek());
							}
						}
						text2 = "";
						text = "";
						jsonnode = stack.Peek();
						goto IL_429;
					}
					if (flag)
					{
						text += aJSON[i].ToString();
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != "")
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(text);
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, text);
							}
						}
						text2 = "";
						text = "";
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_429:
				i++;
				continue;
				IL_412:
				text += aJSON[i].ToString();
				goto IL_429;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0012F028 File Offset: 0x0012D228
		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x0002851F File Offset: 0x0002671F
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x0002851F File Offset: 0x0002671F
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x0002851F File Offset: 0x0002671F
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x0012F044 File Offset: 0x0012D244
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0012F094 File Offset: 0x0012D294
		public string SaveToBase64()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x0012F0E0 File Offset: 0x0012D2E0
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string aKey = aReader.ReadString();
					JSONNode aItem = JSONNode.Deserialize(aReader);
					jsonclass.Add(aKey, aItem);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag.ToString());
			}
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x0002851F File Offset: 0x0002671F
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x0002851F File Offset: 0x0002671F
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x0002851F File Offset: 0x0002671F
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x0012F1DC File Offset: 0x0012D3DC
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0012F214 File Offset: 0x0012D414
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode result;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				result = JSONNode.LoadFromStream(fileStream);
			}
			return result;
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0002852B File Offset: 0x0002672B
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
