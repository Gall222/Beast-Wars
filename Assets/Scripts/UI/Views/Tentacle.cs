using UnityEngine;

namespace Game.UI.Views
{
    public class Tentacle : MonoBehaviour
    {
        public int length;
        public LineRenderer lineRenderer;
        public Vector3[] segmentPoses;
        private Vector3[] segmentV;

        public Transform targetDir;
        public float targetDistance;
        public float smoothSpeed;
        public float trailSpeed;

        public float wiggleSpeed;
        public float wiggleMagnitude;
        public Transform wiggleDir;

        void Start()
        {
            lineRenderer.positionCount = length;
            segmentPoses = new Vector3[length];
            segmentV = new Vector3[length];
        }

        private void Update()
        {
            wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

            segmentPoses[0] = targetDir.position;

            for (int i = 1; i < segmentPoses.Length; i++)
            {
                segmentPoses[i] = Vector3.SmoothDamp(
                    segmentPoses[i],
                    segmentPoses[i - 1] + targetDir.right * targetDistance,
                    ref segmentV[i],
                    smoothSpeed + i / trailSpeed
                    );
            }

            lineRenderer.SetPositions(segmentPoses);
        }
    }
}