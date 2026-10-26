using UnityEngine;

namespace Examples.Flappy
{
    public class ObstacleFlappy : MonoBehaviour
    {
        public System.Action<ObstacleFlappy> OnDestroy;

        public void CheckToDestroy()
        {
            if (transform.position.x - Camera.main.transform.position.x < -7.5f)
            {
                if (OnDestroy != null)
                    OnDestroy.Invoke(this);

                Destroy(gameObject);
            }

        }
    }
}