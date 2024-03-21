using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   [ExecuteInEditMode]
   public class NeverEndingUpdateCallsInEditor : MonoBehaviour
   {
      
      public bool forceLoopCall = true;
      [HideInInspector] public bool loopCall = true;

      #if UNITY_EDITOR
      // void OnDrawGizmos() {
      //     // Your gizmo drawing thing goes here if required...
     
      //    #if UNITY_EDITOR
      //     // Ensure continuous Update calls.
      //    if (!Application.isPlaying && true) {
      //       Debug.Log("loop");
      //       UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
      //       UnityEditor.SceneView.RepaintAll();
      //    }
      //    #endif
      // }
      private void OnEnable() {
         CallLoop();
      }
      private void LateUpdate() {
         CallLoop();
      }

      public async void CallLoop() {
         await System.Threading.Tasks.Task.Delay(1);
          // Ensure continuous Update calls.
         if (!Application.isPlaying && (forceLoopCall || loopCall)) {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
         }
      }
      #endif
   }
