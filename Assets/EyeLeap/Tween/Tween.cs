using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace NelowGames {
	public class ITween {
		public float startTime, endTime;
		public Action<float> tween;
		public Action callback = null;
		public bool unscaledTime = false;
		public bool isDead { get { return endTime <= Time.time; } }
		public float superSample = 1;
		public ITween (Action<float> tween, float time, float endTime, bool unscaledTime, Action callback, int superSample = 1) {
			this.tween = tween;
			this.callback = callback;
			this.startTime = time;
			this.endTime = endTime;
			this.unscaledTime = unscaledTime;
			this.superSample = superSample;
		}
		public void DiePeacefully(bool removeCallbacks = true, bool callTween = false, bool callEndCallback = false) {
			startTime = -1;
			endTime = 0;
			if(callTween && tween != null) tween(1);
			if(callEndCallback && callback != null) callback();
			if(removeCallbacks) {
				tween = null;
				callback = null;
			}
		}
		
    	public static implicit operator bool (ITween d) => d != null && !d.isDead;
	}
	[ExecuteAlways]
	public class Tween : Singleton<Tween> {
		List<ITween> tweens = new List<ITween>();
		Queue<ITween> tweensQueue = new Queue<ITween>();
		List<ITween> tweensLate = new List<ITween>();
		Queue<ITween> tweensLateQueue = new Queue<ITween>();
		List<ITween> tweensFixed = new List<ITween>();
		Queue<ITween> tweensQueueFixed = new Queue<ITween>();
		List<KeyValuePair<System.Action, float>> delayedEvents = new List<KeyValuePair<System.Action, float>>();
		protected override void Awake() {
			base.Awake();
		}
		public static ITween DoTween(Action<float> tween, float time, Action callback = null, float delay = 0, bool unscaledTime = false) {
			ITween i = CreateTween(tween, time, callback, delay, unscaledTime);
			if(Time.inFixedTimeStep)
				ForceGetInstance().tweensQueueFixed.Enqueue( i );
			else
				ForceGetInstance().tweensQueue.Enqueue( i );
				
			if(tween != null) tween(0);
			// } else Debug.Log("can't use TweenInEditor")
			
			#if UNITY_EDITOR
				NeverEndingUpdateCallsInEditor caller = ForceGetInstance().GetComponent<NeverEndingUpdateCallsInEditor>();
				if(!caller) caller = ForceGetInstance().gameObject.AddComponent<NeverEndingUpdateCallsInEditor>();
				caller.loopCall = true;
			#endif
			return i;
		}
		public static ITween DoLateTween(Action<float> tween, float time, Action callback = null, float delay = 0, bool unscaledTime = false) {
			ITween i = CreateTween(tween, time, callback, delay, unscaledTime);
			
			ForceGetInstance().tweensLateQueue.Enqueue( i );
				
			if(tween != null) tween(0);
			// } else Debug.Log("can't use TweenInEditor")
			
			#if UNITY_EDITOR
				NeverEndingUpdateCallsInEditor caller = ForceGetInstance().GetComponent<NeverEndingUpdateCallsInEditor>();
				if(!caller) caller = ForceGetInstance().gameObject.AddComponent<NeverEndingUpdateCallsInEditor>();
				caller.loopCall = true;
			#endif
			return i;
		}
		public static ITween DoTween(Action<float> tween, bool fixedTimeStep, float time, Action callback = null, float delay = 0, bool unscaledTime = false) {
			ITween i = CreateTween(tween, time, callback, delay, unscaledTime);
			if(fixedTimeStep)
				ForceGetInstance().tweensQueueFixed.Enqueue( i );
			else
				ForceGetInstance().tweensQueue.Enqueue( i );
				
			if(tween != null) tween(0);
			// } else Debug.Log("can't use TweenInEditor")
			
			#if UNITY_EDITOR
				NeverEndingUpdateCallsInEditor caller = ForceGetInstance().GetComponent<NeverEndingUpdateCallsInEditor>();
				if(!caller) caller = ForceGetInstance().gameObject.AddComponent<NeverEndingUpdateCallsInEditor>();
				caller.loopCall = true;
			#endif
			return i;
		}
		public static ITween CreateTween(Action<float> tween, float time, Action callback = null, float delay = 0, bool unscaledTime = false) {
			if(unscaledTime) return new ITween(tween, Time.unscaledTime + delay, 	Time.unscaledTime + delay + time, unscaledTime, callback);
			else return 			new ITween(tween, Time.time + delay, 					Time.time + delay + time, unscaledTime, callback);
		}
		public static void Delay(Action action, float delay) {
			ForceGetInstance().delayedEvents.Add( new KeyValuePair<System.Action, float>(action, Time.time + delay) );
		}
		
		// float lastUpdate = Time.time;
		public void Update() {
			#if UNITY_EDITOR
				NeverEndingUpdateCallsInEditor caller = ForceGetInstance().GetComponent<NeverEndingUpdateCallsInEditor>();
				if(!caller) caller = ForceGetInstance().gameObject.AddComponent<NeverEndingUpdateCallsInEditor>();
				caller.loopCall = tweensQueue.Count > 0;
			#endif

			// deferred queue to be able to Add a tween from a tween's end
			while(tweensQueue.Count > 0) {
				tweens.Add(tweensQueue.Dequeue());
			}


			foreach(var tween in tweens) {
				// tween.delta = Mathf.Clamp01(tween.delta + (tween.unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / tween.time);
				if(tween.unscaledTime) {
					if(tween.tween != null) tween.tween(Map01Clamped(Time.unscaledTime, tween.startTime, tween.endTime));
				} else {
					if(tween.tween != null) tween.tween(Map01Clamped(Time.time, tween.startTime, tween.endTime));
				}
				if(tween.isDead && tween.callback != null) tween.callback();
			}
			tweens.RemoveAll(t => t.isDead);

			int c2 = delayedEvents.Count;
			for (int i = 0; i < c2; i++) {
				if(delayedEvents[i].Value <= Time.time) {
					delayedEvents[i].Key();
					delayedEvents.RemoveAt(i);
					i--;
					c2--;
				}
			}

		}

		public void LateUpdate() {
			#if UNITY_EDITOR
				NeverEndingUpdateCallsInEditor caller = ForceGetInstance().GetComponent<NeverEndingUpdateCallsInEditor>();
				if(!caller) caller = ForceGetInstance().gameObject.AddComponent<NeverEndingUpdateCallsInEditor>();
				caller.loopCall = tweensQueue.Count > 0 || tweensLateQueue.Count > 0;
			#endif

			// deferred queue to be able to Add a tween from a tween's end
			while(tweensLateQueue.Count > 0) {
				tweensLate.Add(tweensLateQueue.Dequeue());
			}


			foreach(var tween in tweensLate) {
				// tween.delta = Mathf.Clamp01(tween.delta + (tween.unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / tween.time);
				if(tween.unscaledTime) {
					if(tween.tween != null) tween.tween(Map01Clamped(Time.unscaledTime, tween.startTime, tween.endTime));
				} else {
					if(tween.tween != null) tween.tween(Map01Clamped(Time.time, tween.startTime, tween.endTime));
				}
				if(tween.isDead && tween.callback != null) tween.callback();
			}
			tweensLate.RemoveAll(t => t.isDead);
		}
		public void FixedUpdate() {
			// deferred queue to be able to Add a tween from a tween's end
			while(tweensQueueFixed.Count > 0) {
				tweensFixed.Add(tweensQueueFixed.Dequeue());
			}


			foreach(var tween in tweensFixed) {
				// tween.delta = Mathf.Clamp01(tween.delta + (tween.unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / tween.time);
				if(tween.unscaledTime) {
					if(tween.tween != null) tween.tween(Map01Clamped(Time.unscaledTime, tween.startTime, tween.endTime));
				} else {
					if(tween.tween != null) tween.tween(Map01Clamped(Time.time, tween.startTime, tween.endTime));
				}
				if(tween.isDead && tween.callback != null) tween.callback();
			}
			tweensFixed.RemoveAll(t => t.isDead);
		}

		
		public static float Map01Clamped (float _value, float amin, float amax) {
			if(amin == amax) return 1;
			return Mathf.Clamp01((_value-amin)/(amax-amin));
		}
	}
}
