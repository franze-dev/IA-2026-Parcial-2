using UnityEngine;

namespace Examples.Flappy
{
    public class BackgroundManagerFlappy : MonoBehaviour
    {
        public GameObject[] frames;
        float lastCameraPos;
        float accumPos = 0;

        private static BackgroundManagerFlappy instance = null;

        public static BackgroundManagerFlappy Instance
        {
            get
            {
                if (instance == null)
                    instance = FindFirstObjectByType<BackgroundManagerFlappy>();

                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        public void Reset()
        {
            transform.position = new Vector3(0, 0, 10);
            lastCameraPos = 0;
            accumPos = 0;

            float posx = -4;

            foreach (GameObject go in frames)
            {
                Vector3 pos = go.transform.position;
                pos.x = posx;
                go.transform.position = pos;
                posx += 7.2f;
            }
        }

        void Update()
        {
            float delta = Camera.main.transform.position.x - lastCameraPos;

            Vector3 parallax = transform.position;
            parallax.x += delta * 0.2f;
            transform.position = parallax;

            delta -= delta * 0.2f;

            lastCameraPos = Camera.main.transform.position.x;
            accumPos += delta;

            if (accumPos >= 7.2f)
            {
                foreach (GameObject go in frames)
                {
                    Vector3 pos = go.transform.position;
                    pos.x += 7.2f;
                    go.transform.position = pos;
                }
                accumPos -= 7.2f;
            }
        }
    }
}