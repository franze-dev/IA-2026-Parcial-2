using UnityEngine;

namespace Examples.Flappy
{
    public class CameraFollowFlappy : MonoBehaviour
    {
        public GameObject agent;

        private static CameraFollowFlappy instance = null;

        public static CameraFollowFlappy Instance
        {
            get
            {
                if (instance == null)
                    instance = FindFirstObjectByType<CameraFollowFlappy>();

                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        public void Reset()
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }

        public void UpdateCamera()
        {
            Vector3 follow = Vector3.zero;

            if (PopulationManagerFlappy.Instance != null)
            {
                if (PopulationManagerFlappy.Instance.GetBestAgent() != null)
                    follow = PopulationManagerFlappy.Instance.GetBestAgent().transform.position;
                else
                    return;
            }
            else
                follow = agent.transform.position;

            Vector3 pos = transform.position;
            pos.x = follow.x;
            transform.position = pos;
        }
    }
}