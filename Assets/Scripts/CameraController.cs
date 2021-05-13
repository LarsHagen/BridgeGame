using System;
using System.Collections;
using UnityEngine;

namespace BridgeGame
{
    public class CameraController : MonoBehaviour
    {
        private Matrix4x4 orthographic;
        private Matrix4x4 perspective;

        public AnimationCurve curveToPerspective;
        public AnimationCurve curveToOrtho;

        private void Start()
        {
            Camera.main.orthographic = false;
            perspective = Camera.main.projectionMatrix;
            Camera.main.orthographic = true;
            orthographic = Camera.main.projectionMatrix;
            Camera.main.orthographic = false;
            SwitchToBuildCam();
        }

        public void SwitchToBuildCam()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateProjectionMatix(orthographic, curveToOrtho));
        }

        public void SwitchToPlayCam()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateProjectionMatix(perspective, curveToPerspective));
        }

        private IEnumerator AnimateProjectionMatix(Matrix4x4 target, AnimationCurve curve)
        {
            float animationTime = 0.5f;

            Matrix4x4 start = Camera.main.projectionMatrix;
            for (float f = 0; f < 1f; f += Time.deltaTime / animationTime)
            {
                Camera.main.projectionMatrix = MatrixLerp(start, target, curve.Evaluate(f));
                yield return null;
            }
            Camera.main.projectionMatrix = target;
        }

        private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
        {
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            var newMatrix = new Matrix4x4();
            newMatrix.SetRow(0, Vector4.Lerp(from.GetRow(0), to.GetRow(0), t));
            newMatrix.SetRow(1, Vector4.Lerp(from.GetRow(1), to.GetRow(1), t));
            newMatrix.SetRow(2, Vector4.Lerp(from.GetRow(2), to.GetRow(2), t));
            newMatrix.SetRow(3, Vector4.Lerp(from.GetRow(3), to.GetRow(3), t));
            return newMatrix;
        }
    }
}
