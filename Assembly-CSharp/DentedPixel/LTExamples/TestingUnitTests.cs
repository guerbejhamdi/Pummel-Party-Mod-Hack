using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DentedPixel.LTExamples
{
	// Token: 0x020007DC RID: 2012
	public class TestingUnitTests : MonoBehaviour
	{
		// Token: 0x0600392C RID: 14636 RVA: 0x00026DC9 File Offset: 0x00024FC9
		private void Awake()
		{
			this.boxNoCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
			UnityEngine.Object.Destroy(this.boxNoCollider.GetComponent(typeof(BoxCollider)));
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x0011D6D8 File Offset: 0x0011B8D8
		private void Start()
		{
			LeanTest.timeout = 46f;
			LeanTest.expected = 62;
			LeanTween.init(1300);
			LeanTween.addListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled));
			LeanTest.expect(!LeanTween.isTweening(null), "NOTHING TWEEENING AT BEGINNING", null);
			LeanTest.expect(!LeanTween.isTweening(this.cube1), "OBJECT NOT TWEEENING AT BEGINNING", null);
			LeanTween.scaleX(this.cube4, 2f, 0f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cube4.transform.localScale.x == 2f, "TWEENED WITH ZERO TIME", null);
			});
			LeanTween.dispatchEvent(0);
			LeanTest.expect(this.eventGameObjectWasCalled, "EVENT GAMEOBJECT RECEIVED", null);
			LeanTest.expect(!LeanTween.removeListener(this.cube2, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT NOT REMOVED", null);
			LeanTest.expect(LeanTween.removeListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT REMOVED", null);
			LeanTween.addListener(1, new Action<LTEvent>(this.eventGeneralCalled));
			LeanTween.dispatchEvent(1);
			LeanTest.expect(this.eventGeneralWasCalled, "EVENT ALL RECEIVED", null);
			LeanTest.expect(LeanTween.removeListener(1, new Action<LTEvent>(this.eventGeneralCalled)), "EVENT ALL REMOVED", null);
			this.lt1Id = LeanTween.move(this.cube1, new Vector3(3f, 2f, 0.5f), 1.1f).id;
			LeanTween.move(this.cube2, new Vector3(-3f, -2f, -0.5f), 1.1f);
			LeanTween.reset();
			GameObject[] cubes = new GameObject[99];
			int[] tweenIds = new int[cubes.Length];
			for (int i = 0; i < cubes.Length; i++)
			{
				GameObject gameObject = this.cubeNamed("Cancel" + i.ToString());
				tweenIds[i] = LeanTween.moveX(gameObject, 100f, 1f).id;
				cubes[i] = gameObject;
			}
			int onCompleteCount = 0;
			Action <>9__21;
			LeanTween.delayedCall(cubes[0], 0.2f, delegate()
			{
				for (int l = 0; l < cubes.Length; l++)
				{
					if (l % 3 == 0)
					{
						LeanTween.cancel(cubes[l]);
					}
					else if (l % 3 == 1)
					{
						LeanTween.cancel(tweenIds[l]);
					}
					else if (l % 3 == 2)
					{
						LTDescr ltdescr3 = LeanTween.descr(tweenIds[l]);
						Action onComplete2;
						if ((onComplete2 = <>9__21) == null)
						{
							onComplete2 = (<>9__21 = delegate()
							{
								int onCompleteCount = onCompleteCount;
								onCompleteCount++;
								if (onCompleteCount >= 33)
								{
									LeanTest.expect(true, "CANCELS DO NOT EFFECT FINISHING", null);
								}
							});
						}
						ltdescr3.setOnComplete(onComplete2);
					}
				}
			});
			new LTSpline(new Vector3[]
			{
				new Vector3(-1f, 0f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(4f, 0f, 0f),
				new Vector3(20f, 0f, 0f),
				new Vector3(30f, 0f, 0f)
			}).place(this.cube4.transform, 0.5f);
			LeanTest.expect(Vector3.Distance(this.cube4.transform.position, new Vector3(10f, 0f, 0f)) <= 0.7f, "SPLINE POSITIONING AT HALFWAY", "position is:" + this.cube4.transform.position.ToString() + " but should be:(10f,0f,0f)");
			LeanTween.color(this.cube4, Color.green, 0.01f);
			GameObject gameObject2 = this.cubeNamed("cubeDest");
			Vector3 cubeDestEnd = new Vector3(100f, 20f, 0f);
			LeanTween.move(gameObject2, cubeDestEnd, 0.7f);
			GameObject cubeToTrans = this.cubeNamed("cubeToTrans");
			LeanTween.move(cubeToTrans, gameObject2.transform, 1.2f).setEase(LeanTweenType.easeOutQuad).setOnComplete(delegate()
			{
				LeanTest.expect(cubeToTrans.transform.position == cubeDestEnd, "MOVE TO TRANSFORM WORKS", null);
			});
			GameObject gameObject3 = this.cubeNamed("cubeDestroy");
			LeanTween.moveX(gameObject3, 200f, 0.05f).setDelay(0.02f).setDestroyOnComplete(true);
			LeanTween.moveX(gameObject3, 200f, 0.1f).setDestroyOnComplete(true).setOnComplete(delegate()
			{
				LeanTest.expect(true, "TWO DESTROY ON COMPLETE'S SUCCEED", null);
			});
			GameObject cubeSpline = this.cubeNamed("cubeSpline");
			LeanTween.moveSpline(cubeSpline, new Vector3[]
			{
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(0.75f, 0f, 0.75f),
				new Vector3(1f, 0f, 1f),
				new Vector3(1f, 0f, 1f)
			}, 0.1f).setOnComplete(delegate()
			{
				LeanTest.expect(Vector3.Distance(new Vector3(1f, 0f, 1f), cubeSpline.transform.position) < 0.01f, "SPLINE WITH TWO POINTS SUCCEEDS", null);
			});
			GameObject jumpCube = this.cubeNamed("jumpTime");
			jumpCube.transform.position = new Vector3(100f, 0f, 0f);
			jumpCube.transform.localScale *= 100f;
			int jumpTimeId = LeanTween.moveX(jumpCube, 200f, 1f).id;
			LeanTween.delayedCall(base.gameObject, 0.2f, delegate()
			{
				LTDescr ltdescr3 = LeanTween.descr(jumpTimeId);
				float beforeX = jumpCube.transform.position.x;
				ltdescr3.setTime(0.5f);
				LeanTween.delayedCall(0f, delegate()
				{
				}).setOnStart(delegate
				{
					float num = 1f;
					beforeX += Time.deltaTime * 100f * 2f;
					LeanTest.expect(Mathf.Abs(jumpCube.transform.position.x - beforeX) < num, "CHANGING TIME DOESN'T JUMP AHEAD", string.Concat(new string[]
					{
						"Difference:",
						Mathf.Abs(jumpCube.transform.position.x - beforeX).ToString(),
						" beforeX:",
						beforeX.ToString(),
						" now:",
						jumpCube.transform.position.x.ToString(),
						" dt:",
						Time.deltaTime.ToString()
					}));
				});
			});
			GameObject zeroCube = this.cubeNamed("zeroCube");
			LeanTween.moveX(zeroCube, 10f, 0f).setOnComplete(delegate()
			{
				LeanTest.expect(zeroCube.transform.position.x == 10f, "ZERO TIME FINSHES CORRECTLY", "final x:" + zeroCube.transform.position.x.ToString());
			});
			GameObject cubeScale = this.cubeNamed("cubeScale");
			LeanTween.scale(cubeScale, new Vector3(5f, 5f, 5f), 0.01f).setOnStart(delegate
			{
				LeanTest.expect(true, "ON START WAS CALLED", null);
			}).setOnComplete(delegate()
			{
				LeanTest.expect(cubeScale.transform.localScale.z == 5f, "SCALE", "expected scale z:" + 5f.ToString() + " returned:" + cubeScale.transform.localScale.z.ToString());
			});
			GameObject cubeRotate = this.cubeNamed("cubeRotate");
			LeanTween.rotate(cubeRotate, new Vector3(0f, 180f, 0f), 0.02f).setOnComplete(delegate()
			{
				LeanTest.expect(cubeRotate.transform.eulerAngles.y == 180f, "ROTATE", "expected rotate y:" + 180f.ToString() + " returned:" + cubeRotate.transform.eulerAngles.y.ToString());
			});
			GameObject cubeRotateA = this.cubeNamed("cubeRotateA");
			LeanTween.rotateAround(cubeRotateA, Vector3.forward, 90f, 0.3f).setOnComplete(delegate()
			{
				LeanTest.expect(cubeRotateA.transform.eulerAngles.z == 90f, "ROTATE AROUND", "expected rotate z:" + 90f.ToString() + " returned:" + cubeRotateA.transform.eulerAngles.z.ToString());
			});
			GameObject cubeRotateB = this.cubeNamed("cubeRotateB");
			cubeRotateB.transform.position = new Vector3(200f, 10f, 8f);
			LeanTween.rotateAround(cubeRotateB, Vector3.forward, 360f, 0.3f).setPoint(new Vector3(5f, 3f, 2f)).setOnComplete(delegate()
			{
				LeanTest.expect(cubeRotateB.transform.position.ToString() == new Vector3(200f, 10f, 8f).ToString(), "ROTATE AROUND 360", "expected rotate pos:" + new Vector3(200f, 10f, 8f).ToString() + " returned:" + cubeRotateB.transform.position.ToString());
			});
			LeanTween.alpha(this.cubeAlpha1, 0.5f, 0.1f).setOnUpdate(delegate(float val)
			{
				LeanTest.expect(val != 0f, "ON UPDATE VAL", null);
			}).setOnCompleteParam("Hi!").setOnComplete(delegate(object completeObj)
			{
				LeanTest.expect((string)completeObj == "Hi!", "ONCOMPLETE OBJECT", null);
				LeanTest.expect(this.cubeAlpha1.GetComponent<Renderer>().material.color.a == 0.5f, "ALPHA", null);
			});
			float onStartTime = -1f;
			LeanTween.color(this.cubeAlpha2, Color.cyan, 0.3f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cubeAlpha2.GetComponent<Renderer>().material.color == Color.cyan, "COLOR", null);
				LeanTest.expect(onStartTime >= 0f && onStartTime < Time.time, "ON START", "onStartTime:" + onStartTime.ToString() + " time:" + Time.time.ToString());
			}).setOnStart(delegate
			{
				onStartTime = Time.time;
			});
			Vector3 beforePos = this.cubeAlpha1.transform.position;
			LeanTween.moveY(this.cubeAlpha1, 3f, 0.2f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cubeAlpha1.transform.position.x == beforePos.x && this.cubeAlpha1.transform.position.z == beforePos.z, "MOVE Y", null);
			});
			Vector3 beforePos2 = this.cubeAlpha2.transform.localPosition;
			LeanTween.moveLocalZ(this.cubeAlpha2, 12f, 0.2f).setOnComplete(delegate()
			{
				bool didPass = this.cubeAlpha2.transform.localPosition.x == beforePos2.x && this.cubeAlpha2.transform.localPosition.y == beforePos2.y;
				string definition = "MOVE LOCAL Z";
				string[] array = new string[8];
				array[0] = "ax:";
				int num = 1;
				Vector3 localPosition = this.cubeAlpha2.transform.localPosition;
				array[num] = localPosition.x.ToString();
				array[2] = " bx:";
				array[3] = beforePos.x.ToString();
				array[4] = " ay:";
				int num2 = 5;
				localPosition = this.cubeAlpha2.transform.localPosition;
				array[num2] = localPosition.y.ToString();
				array[6] = " by:";
				array[7] = beforePos2.y.ToString();
				LeanTest.expect(didPass, definition, string.Concat(array));
			});
			AudioClip audio = LeanAudio.createAudio(new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 1f, 0f, -1f),
				new Keyframe(1f, 0f, -1f, 0f)
			}), new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.001f, 0f, 0f),
				new Keyframe(1f, 0.001f, 0f, 0f)
			}), LeanAudio.options());
			LeanTween.delayedSound(base.gameObject, audio, new Vector3(0f, 0f, 0f), 0.1f).setDelay(0.2f).setOnComplete(delegate()
			{
				LeanTest.expect(Time.time > 0f, "DELAYED SOUND", null);
			});
			int totalEasingCheck = 0;
			int totalEasingCheckSuccess = 0;
			for (int j = 0; j < 2; j++)
			{
				bool flag = j == 1;
				int totalTweenTypeLength = 33;
				Action<object> <>9__24;
				for (int k = 0; k < totalTweenTypeLength; k++)
				{
					LeanTweenType leanTweenType = (LeanTweenType)k;
					GameObject gameObject4 = this.cubeNamed("cube" + leanTweenType.ToString());
					LTDescr ltdescr = LeanTween.moveLocalX(gameObject4, 5f, 0.1f);
					Action<object> onComplete;
					if ((onComplete = <>9__24) == null)
					{
						onComplete = (<>9__24 = delegate(object obj)
						{
							GameObject gameObject5 = obj as GameObject;
							int num = totalEasingCheck;
							totalEasingCheck = num + 1;
							if (gameObject5.transform.position.x == 5f)
							{
								num = totalEasingCheckSuccess;
								totalEasingCheckSuccess = num + 1;
							}
							if (totalEasingCheck == 2 * totalTweenTypeLength)
							{
								LeanTest.expect(totalEasingCheck == totalEasingCheckSuccess, "EASING TYPES", null);
							}
						});
					}
					LTDescr ltdescr2 = ltdescr.setOnComplete(onComplete).setOnCompleteParam(gameObject4);
					if (flag)
					{
						ltdescr2.setFrom(-5f);
					}
				}
			}
			bool value2UpdateCalled = false;
			LeanTween.value(base.gameObject, new Vector2(0f, 0f), new Vector2(256f, 96f), 0.1f).setOnUpdate(delegate(Vector2 value)
			{
				value2UpdateCalled = true;
			}, null);
			LeanTween.delayedCall(0.2f, delegate()
			{
				LeanTest.expect(value2UpdateCalled, "VALUE2 UPDATE", null);
			});
			base.StartCoroutine(this.timeBasedTesting());
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x00026DF1 File Offset: 0x00024FF1
		private GameObject cubeNamed(string name)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
			gameObject.name = name;
			return gameObject;
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x00026E05 File Offset: 0x00025005
		private IEnumerator timeBasedTesting()
		{
			yield return new WaitForEndOfFrame();
			GameObject gameObject = this.cubeNamed("normalTimeScale");
			LeanTween.moveX(gameObject, 12f, 1.5f).setIgnoreTimeScale(false).setOnComplete(delegate()
			{
				this.timeElapsedNormalTimeScale = Time.time;
			});
			LTDescr[] array = LeanTween.descriptions(gameObject);
			LeanTest.expect(array.Length >= 0 && array[0].to.x == 12f, "WE CAN RETRIEVE A DESCRIPTION", null);
			LeanTween.moveX(this.cubeNamed("ignoreTimeScale"), 5f, 1.5f).setIgnoreTimeScale(true).setOnComplete(delegate()
			{
				this.timeElapsedIgnoreTimeScale = Time.time;
			});
			yield return new WaitForSeconds(1.5f);
			LeanTest.expect(Mathf.Abs(this.timeElapsedNormalTimeScale - this.timeElapsedIgnoreTimeScale) < 0.7f, "START IGNORE TIMING", "timeElapsedIgnoreTimeScale:" + this.timeElapsedIgnoreTimeScale.ToString() + " timeElapsedNormalTimeScale:" + this.timeElapsedNormalTimeScale.ToString());
			Time.timeScale = 4f;
			int pauseCount = 0;
			LeanTween.value(base.gameObject, 0f, 1f, 1f).setOnUpdate(delegate(float val)
			{
				int pauseCount = pauseCount;
				pauseCount++;
			}).pause();
			Vector3[] array2 = new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(-9.1f, 25.1f, 0f),
				new Vector3(-1.2f, 15.9f, 0f),
				new Vector3(-25f, 25f, 0f),
				new Vector3(-25f, 25f, 0f),
				new Vector3(-50.1f, 15.9f, 0f),
				new Vector3(-40.9f, 25.1f, 0f),
				new Vector3(-50f, 0f, 0f),
				new Vector3(-50f, 0f, 0f),
				new Vector3(-40.9f, -25.1f, 0f),
				new Vector3(-50.1f, -15.9f, 0f),
				new Vector3(-25f, -25f, 0f),
				new Vector3(-25f, -25f, 0f),
				new Vector3(0f, -15.9f, 0f),
				new Vector3(-9.1f, -25.1f, 0f),
				new Vector3(0f, 0f, 0f)
			};
			GameObject cubeRound = this.cubeNamed("bRound");
			Vector3 onStartPos = cubeRound.transform.position;
			LeanTween.moveLocal(cubeRound, array2, 0.5f).setOnComplete(delegate()
			{
				Vector3 onStartPos;
				bool didPass = cubeRound.transform.position == onStartPos;
				string definition = "BEZIER CLOSED LOOP SHOULD END AT START";
				string str = "onStartPos:";
				onStartPos = onStartPos;
				LeanTest.expect(didPass, definition, str + onStartPos.ToString() + " onEnd:" + cubeRound.transform.position.ToString());
			});
			LeanTest.expect(object.Equals(new LTBezierPath(array2).ratioAtPoint(new Vector3(-25f, 25f, 0f), 0.01f), 0.25f), "BEZIER RATIO POINT", null);
			Vector3[] to = new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(2f, 0f, 0f),
				new Vector3(0.9f, 2f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, 0f, 0f)
			};
			GameObject cubeSpline = this.cubeNamed("bSpline");
			Vector3 onStartPosSpline = cubeSpline.transform.position;
			LeanTween.moveSplineLocal(cubeSpline, to, 0.5f).setOnComplete(delegate()
			{
				Vector3 onStartPosSpline;
				bool didPass = Vector3.Distance(onStartPosSpline, cubeSpline.transform.position) <= 0.01f;
				string definition = "SPLINE CLOSED LOOP SHOULD END AT START";
				string[] array5 = new string[6];
				array5[0] = "onStartPos:";
				int num6 = 1;
				onStartPosSpline = onStartPosSpline;
				array5[num6] = onStartPosSpline.ToString();
				array5[2] = " onEnd:";
				array5[3] = cubeSpline.transform.position.ToString();
				array5[4] = " dist:";
				array5[5] = Vector3.Distance(onStartPosSpline, cubeSpline.transform.position).ToString();
				LeanTest.expect(didPass, definition, string.Concat(array5));
			});
			GameObject cubeSeq = this.cubeNamed("cSeq");
			LTSeq ltseq = LeanTween.sequence(true).append(LeanTween.moveX(cubeSeq, 100f, 0.2f));
			ltseq.append(0.1f).append(LeanTween.scaleX(cubeSeq, 2f, 0.1f));
			ltseq.append(delegate()
			{
				bool didPass = cubeSeq.transform.position.x == 100f;
				string definition = "SEQ MOVE X FINISHED";
				string str = "move x:";
				Vector3 vector2 = cubeSeq.transform.position;
				LeanTest.expect(didPass, definition, str + vector2.x.ToString());
				bool didPass2 = cubeSeq.transform.localScale.x == 2f;
				string definition2 = "SEQ SCALE X FINISHED";
				string str2 = "scale x:";
				vector2 = cubeSeq.transform.localScale;
				LeanTest.expect(didPass2, definition2, str2 + vector2.x.ToString());
			}).setScale(0.2f);
			GameObject cubeBounds = this.cubeNamed("cBounds");
			bool didPassBounds = true;
			Vector3 failPoint = Vector3.zero;
			LeanTween.move(cubeBounds, new Vector3(10f, 10f, 10f), 0.1f).setOnUpdate(delegate(float val)
			{
				if (cubeBounds.transform.position.x < 0f || cubeBounds.transform.position.x > 10f || cubeBounds.transform.position.y < 0f || cubeBounds.transform.position.y > 10f || cubeBounds.transform.position.z < 0f || cubeBounds.transform.position.z > 10f)
				{
					didPassBounds = false;
					failPoint = cubeBounds.transform.position;
				}
			}).setLoopPingPong().setRepeat(8).setOnComplete(delegate()
			{
				LeanTest.expect(didPassBounds, "OUT OF BOUNDS", string.Concat(new string[]
				{
					"pos x:",
					failPoint.x.ToString(),
					" y:",
					failPoint.y.ToString(),
					" z:",
					failPoint.z.ToString()
				}));
			});
			this.groupTweens = new LTDescr[1200];
			this.groupGOs = new GameObject[this.groupTweens.Length];
			this.groupTweensCnt = 0;
			int descriptionMatchCount = 0;
			for (int i = 0; i < this.groupTweens.Length; i++)
			{
				GameObject gameObject2 = this.cubeNamed("c" + i.ToString());
				gameObject2.transform.position = new Vector3(0f, 0f, (float)(i * 3));
				this.groupGOs[i] = gameObject2;
			}
			yield return new WaitForEndOfFrame();
			bool hasGroupTweensCheckStarted = false;
			int setOnStartNum = 0;
			int setPosNum = 0;
			bool setPosOnUpdate = true;
			Action <>9__13;
			Action<Vector3> <>9__14;
			Action <>9__16;
			Action<object> <>9__15;
			for (int j = 0; j < this.groupTweens.Length; j++)
			{
				Vector3 vector = base.transform.position + Vector3.one * 3f;
				Dictionary<string, object> onCompleteParam = new Dictionary<string, object>
				{
					{
						"final",
						vector
					},
					{
						"go",
						this.groupGOs[j]
					}
				};
				LTDescr[] array3 = this.groupTweens;
				int num = j;
				LTDescr ltdescr = LeanTween.move(this.groupGOs[j], vector, 3f);
				Action onStart;
				if ((onStart = <>9__13) == null)
				{
					onStart = (<>9__13 = delegate()
					{
						int setOnStartNum = setOnStartNum;
						setOnStartNum++;
					});
				}
				LTDescr ltdescr2 = ltdescr.setOnStart(onStart);
				Action<Vector3> onUpdate;
				if ((onUpdate = <>9__14) == null)
				{
					onUpdate = (<>9__14 = delegate(Vector3 newPosition)
					{
						if (this.transform.position.z > newPosition.z)
						{
							setPosOnUpdate = false;
						}
					});
				}
				LTDescr ltdescr3 = ltdescr2.setOnUpdate(onUpdate, null).setOnCompleteParam(onCompleteParam);
				Action<object> onComplete;
				if ((onComplete = <>9__15) == null)
				{
					onComplete = (<>9__15 = delegate(object param)
					{
						Dictionary<string, object> dictionary = param as Dictionary<string, object>;
						Vector3 vector2 = (Vector3)dictionary["final"];
						GameObject gameObject3 = dictionary["go"] as GameObject;
						int setPosNum;
						if (vector2.ToString() == gameObject3.transform.position.ToString())
						{
							setPosNum = setPosNum;
							setPosNum++;
						}
						if (!hasGroupTweensCheckStarted)
						{
							hasGroupTweensCheckStarted = true;
							GameObject gameObject4 = this.gameObject;
							float delayTime = 0.1f;
							Action callback;
							if ((callback = <>9__16) == null)
							{
								callback = (<>9__16 = delegate()
								{
									LeanTest.expect(setOnStartNum == this.groupTweens.Length, "SETONSTART CALLS", "expected:" + this.groupTweens.Length.ToString() + " was:" + setOnStartNum.ToString());
									LeanTest.expect(this.groupTweensCnt == this.groupTweens.Length, "GROUP FINISH", "expected " + this.groupTweens.Length.ToString() + " tweens but got " + this.groupTweensCnt.ToString());
									LeanTest.expect(setPosNum == this.groupTweens.Length, "GROUP POSITION FINISH", "expected " + this.groupTweens.Length.ToString() + " tweens but got " + setPosNum.ToString());
									LeanTest.expect(setPosOnUpdate, "GROUP POSITION ON UPDATE", null);
								});
							}
							LeanTween.delayedCall(gameObject4, delayTime, callback);
						}
						this.groupTweensCnt++;
					});
				}
				array3[num] = ltdescr3.setOnComplete(onComplete);
				if (LeanTween.description(this.groupTweens[j].id).trans == this.groupTweens[j].trans)
				{
					int k = descriptionMatchCount;
					descriptionMatchCount = k + 1;
				}
			}
			while (LeanTween.tweensRunning < this.groupTweens.Length)
			{
				yield return null;
			}
			LeanTest.expect(descriptionMatchCount == this.groupTweens.Length, "GROUP IDS MATCH", null);
			int num2 = this.groupTweens.Length + 7;
			LeanTest.expect(LeanTween.maxSearch <= num2, "MAX SEARCH OPTIMIZED", "maxSearch:" + LeanTween.maxSearch.ToString() + " should be:" + num2.ToString());
			LeanTest.expect(LeanTween.isTweening(null), "SOMETHING IS TWEENING", null);
			float previousXlt4 = this.cube4.transform.position.x;
			this.lt4 = LeanTween.moveX(this.cube4, 5f, 1.1f).setOnComplete(delegate()
			{
				bool didPass = this.cube4 != null && previousXlt4 != this.cube4.transform.position.x;
				string definition = "RESUME OUT OF ORDER";
				string[] array5 = new string[6];
				array5[0] = "cube4:";
				int num6 = 1;
				GameObject gameObject3 = this.cube4;
				array5[num6] = ((gameObject3 != null) ? gameObject3.ToString() : null);
				array5[2] = " previousXlt4:";
				array5[3] = previousXlt4.ToString();
				array5[4] = " cube4.transform.position.x:";
				array5[5] = ((this.cube4 != null) ? this.cube4.transform.position.x : 0f).ToString();
				LeanTest.expect(didPass, definition, string.Concat(array5));
			}).setDestroyOnComplete(true);
			this.lt4.resume();
			this.rotateRepeat = (this.rotateRepeatAngle = 0);
			LeanTween.rotateAround(this.cube3, Vector3.forward, 360f, 0.1f).setRepeat(3).setOnComplete(new Action(this.rotateRepeatFinished)).setOnCompleteOnRepeat(true).setDestroyOnComplete(true);
			yield return new WaitForEndOfFrame();
			LeanTween.delayedCall(1.8f, new Action(this.rotateRepeatAllFinished));
			int tweensRunning = LeanTween.tweensRunning;
			LeanTween.cancel(this.lt1Id);
			LeanTest.expect(tweensRunning == LeanTween.tweensRunning, "CANCEL AFTER RESET SHOULD FAIL", "expected " + tweensRunning.ToString() + " but got " + LeanTween.tweensRunning.ToString());
			LeanTween.cancel(this.cube2);
			int num3 = 0;
			for (int l = 0; l < this.groupTweens.Length; l++)
			{
				if (LeanTween.isTweening(this.groupGOs[l]))
				{
					num3++;
				}
				if (l % 3 == 0)
				{
					LeanTween.pause(this.groupGOs[l]);
				}
				else if (l % 3 == 1)
				{
					this.groupTweens[l].pause();
				}
				else
				{
					LeanTween.pause(this.groupTweens[l].id);
				}
			}
			LeanTest.expect(num3 == this.groupTweens.Length, "GROUP ISTWEENING", "expected " + this.groupTweens.Length.ToString() + " tweens but got " + num3.ToString());
			yield return new WaitForEndOfFrame();
			num3 = 0;
			for (int m = 0; m < this.groupTweens.Length; m++)
			{
				if (m % 3 == 0)
				{
					LeanTween.resume(this.groupGOs[m]);
				}
				else if (m % 3 == 1)
				{
					this.groupTweens[m].resume();
				}
				else
				{
					LeanTween.resume(this.groupTweens[m].id);
				}
				if ((m % 2 == 0) ? LeanTween.isTweening(this.groupTweens[m].id) : LeanTween.isTweening(this.groupGOs[m]))
				{
					num3++;
				}
			}
			LeanTest.expect(num3 == this.groupTweens.Length, "GROUP RESUME", null);
			LeanTest.expect(!LeanTween.isTweening(this.cube1), "CANCEL TWEEN LTDESCR", null);
			LeanTest.expect(!LeanTween.isTweening(this.cube2), "CANCEL TWEEN LEANTWEEN", null);
			LeanTest.expect(pauseCount == 0, "ON UPDATE NOT CALLED DURING PAUSE", "expect pause count of 0, but got " + pauseCount.ToString());
			yield return new WaitForEndOfFrame();
			Time.timeScale = 0.25f;
			float num4 = 0.2f;
			float expectedTime = num4 * (1f / Time.timeScale);
			float start = Time.realtimeSinceStartup;
			bool onUpdateWasCalled = false;
			LeanTween.moveX(this.cube1, -5f, num4).setOnUpdate(delegate(float val)
			{
				onUpdateWasCalled = true;
			}).setOnComplete(delegate()
			{
				float num6 = Time.realtimeSinceStartup - start;
				LeanTest.expect(Mathf.Abs(expectedTime - num6) < 0.06f, "SCALED TIMING DIFFERENCE", "expected to complete in roughly " + expectedTime.ToString() + " but completed in " + num6.ToString());
				LeanTest.expect(Mathf.Approximately(this.cube1.transform.position.x, -5f), "SCALED ENDING POSITION", "expected to end at -5f, but it ended at " + this.cube1.transform.position.x.ToString());
				LeanTest.expect(onUpdateWasCalled, "ON UPDATE FIRED", null);
			});
			bool didGetCorrectOnUpdate = false;
			LeanTween.value(base.gameObject, new Vector3(1f, 1f, 1f), new Vector3(10f, 10f, 10f), 1f).setOnUpdate(delegate(Vector3 val)
			{
				didGetCorrectOnUpdate = (val.x >= 1f && val.y >= 1f && val.z >= 1f);
			}, null).setOnComplete(delegate()
			{
				LeanTest.expect(didGetCorrectOnUpdate, "VECTOR3 CALLBACK CALLED", null);
			});
			yield return new WaitForSeconds(expectedTime);
			Time.timeScale = 1f;
			int num5 = 0;
			GameObject[] array4 = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
			for (int k = 0; k < array4.Length; k++)
			{
				if (array4[k].name == "~LeanTween")
				{
					num5++;
				}
			}
			LeanTest.expect(num5 == 1, "RESET CORRECTLY CLEANS UP", null);
			base.StartCoroutine(this.lotsOfCancels());
			yield break;
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x00026E14 File Offset: 0x00025014
		private IEnumerator lotsOfCancels()
		{
			yield return new WaitForEndOfFrame();
			Time.timeScale = 4f;
			int cubeCount = 10;
			int[] tweensA = new int[cubeCount];
			GameObject[] aGOs = new GameObject[cubeCount];
			for (int i = 0; i < aGOs.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
				gameObject.transform.position = new Vector3(0f, 0f, (float)i * 2f);
				gameObject.name = "a" + i.ToString();
				aGOs[i] = gameObject;
				tweensA[i] = LeanTween.move(gameObject, gameObject.transform.position + new Vector3(10f, 0f, 0f), 0.5f + 1f * (1f / (float)aGOs.Length)).id;
				LeanTween.color(gameObject, Color.red, 0.01f);
			}
			yield return new WaitForSeconds(1f);
			int[] tweensB = new int[cubeCount];
			GameObject[] bGOs = new GameObject[cubeCount];
			for (int j = 0; j < bGOs.Length; j++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
				gameObject2.transform.position = new Vector3(0f, 0f, (float)j * 2f);
				gameObject2.name = "b" + j.ToString();
				bGOs[j] = gameObject2;
				tweensB[j] = LeanTween.move(gameObject2, gameObject2.transform.position + new Vector3(10f, 0f, 0f), 2f).id;
			}
			for (int k = 0; k < aGOs.Length; k++)
			{
				LeanTween.cancel(aGOs[k]);
				GameObject gameObject3 = aGOs[k];
				tweensA[k] = LeanTween.move(gameObject3, new Vector3(0f, 0f, (float)k * 2f), 2f).id;
			}
			yield return new WaitForSeconds(0.5f);
			for (int l = 0; l < aGOs.Length; l++)
			{
				LeanTween.cancel(aGOs[l]);
				GameObject gameObject4 = aGOs[l];
				tweensA[l] = LeanTween.move(gameObject4, new Vector3(0f, 0f, (float)l * 2f) + new Vector3(10f, 0f, 0f), 2f).id;
			}
			for (int m = 0; m < bGOs.Length; m++)
			{
				LeanTween.cancel(bGOs[m]);
				GameObject gameObject5 = bGOs[m];
				tweensB[m] = LeanTween.move(gameObject5, new Vector3(0f, 0f, (float)m * 2f), 2f).id;
			}
			yield return new WaitForSeconds(2.1f);
			bool didPass = true;
			for (int n = 0; n < aGOs.Length; n++)
			{
				if (Vector3.Distance(aGOs[n].transform.position, new Vector3(0f, 0f, (float)n * 2f) + new Vector3(10f, 0f, 0f)) > 0.1f)
				{
					didPass = false;
				}
			}
			for (int num = 0; num < bGOs.Length; num++)
			{
				if (Vector3.Distance(bGOs[num].transform.position, new Vector3(0f, 0f, (float)num * 2f)) > 0.1f)
				{
					didPass = false;
				}
			}
			LeanTest.expect(didPass, "AFTER LOTS OF CANCELS", null);
			this.cubeNamed("cPaused").LeanMoveX(10f, 1f).setOnComplete(delegate()
			{
				this.pauseTweenDidFinish = true;
			});
			base.StartCoroutine(this.pauseTimeNow());
			yield break;
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x00026E23 File Offset: 0x00025023
		private IEnumerator pauseTimeNow()
		{
			yield return new WaitForSeconds(0.5f);
			Time.timeScale = 0f;
			LeanTween.delayedCall(0.5f, delegate()
			{
				Time.timeScale = 1f;
			}).setUseEstimatedTime(true);
			LeanTween.delayedCall(1.5f, delegate()
			{
				LeanTest.expect(this.pauseTweenDidFinish, "PAUSE BY TIMESCALE FINISHES", null);
			}).setUseEstimatedTime(true);
			yield break;
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x00026E32 File Offset: 0x00025032
		private void rotateRepeatFinished()
		{
			if (Mathf.Abs(this.cube3.transform.eulerAngles.z) < 0.0001f)
			{
				this.rotateRepeatAngle++;
			}
			this.rotateRepeat++;
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x0011E138 File Offset: 0x0011C338
		private void rotateRepeatAllFinished()
		{
			LeanTest.expect(this.rotateRepeatAngle == 3, "ROTATE AROUND MULTIPLE", "expected 3 times received " + this.rotateRepeatAngle.ToString() + " times");
			LeanTest.expect(this.rotateRepeat == 3, "ROTATE REPEAT", "expected 3 times received " + this.rotateRepeat.ToString() + " times");
			bool didPass = this.cube3 == null;
			string definition = "DESTROY ON COMPLETE";
			string str = "cube3:";
			GameObject gameObject = this.cube3;
			LeanTest.expect(didPass, definition, str + ((gameObject != null) ? gameObject.ToString() : null));
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x00026E71 File Offset: 0x00025071
		private void eventGameObjectCalled(LTEvent e)
		{
			this.eventGameObjectWasCalled = true;
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x00026E7A File Offset: 0x0002507A
		private void eventGeneralCalled(LTEvent e)
		{
			this.eventGeneralWasCalled = true;
		}

		// Token: 0x040037AA RID: 14250
		public GameObject cube1;

		// Token: 0x040037AB RID: 14251
		public GameObject cube2;

		// Token: 0x040037AC RID: 14252
		public GameObject cube3;

		// Token: 0x040037AD RID: 14253
		public GameObject cube4;

		// Token: 0x040037AE RID: 14254
		public GameObject cubeAlpha1;

		// Token: 0x040037AF RID: 14255
		public GameObject cubeAlpha2;

		// Token: 0x040037B0 RID: 14256
		private bool eventGameObjectWasCalled;

		// Token: 0x040037B1 RID: 14257
		private bool eventGeneralWasCalled;

		// Token: 0x040037B2 RID: 14258
		private int lt1Id;

		// Token: 0x040037B3 RID: 14259
		private LTDescr lt2;

		// Token: 0x040037B4 RID: 14260
		private LTDescr lt3;

		// Token: 0x040037B5 RID: 14261
		private LTDescr lt4;

		// Token: 0x040037B6 RID: 14262
		private LTDescr[] groupTweens;

		// Token: 0x040037B7 RID: 14263
		private GameObject[] groupGOs;

		// Token: 0x040037B8 RID: 14264
		private int groupTweensCnt;

		// Token: 0x040037B9 RID: 14265
		private int rotateRepeat;

		// Token: 0x040037BA RID: 14266
		private int rotateRepeatAngle;

		// Token: 0x040037BB RID: 14267
		private GameObject boxNoCollider;

		// Token: 0x040037BC RID: 14268
		private float timeElapsedNormalTimeScale;

		// Token: 0x040037BD RID: 14269
		private float timeElapsedIgnoreTimeScale;

		// Token: 0x040037BE RID: 14270
		private bool pauseTweenDidFinish;
	}
}
