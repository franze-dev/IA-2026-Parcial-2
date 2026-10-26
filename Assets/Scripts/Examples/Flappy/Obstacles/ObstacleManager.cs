using UnityEngine;
using System.Collections.Generic;

namespace Examples.Flappy
{
    public class ObstacleManager : MonoBehaviour
    {
        const float DISTANCE_BETWEEN_OBSTACLES = 6f;
        const float HEIGHT_RANDOM = 3f;
        const int MIN_COUNT = 3;
        public GameObject prefab;
        Vector3 pos = new Vector3(DISTANCE_BETWEEN_OBSTACLES, 0, 0);

        List<ObstacleFlappy> obstacles = new List<ObstacleFlappy>();

        private static ObstacleManager instance = null;

        public static ObstacleManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindFirstObjectByType<ObstacleManager>();

                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        public void Reset()
        {
            for (int i = 0; i < obstacles.Count; i++)
                Destroy(obstacles[i].gameObject);

            obstacles.Clear();

            pos.x = 0;

            InstantiateObstacle();
            InstantiateObstacle();
        }

        public ObstacleFlappy GetNextObstacle(Vector3 pos)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (pos.x < obstacles[i].transform.position.x + 2f)
                    return obstacles[i];
            }

            return null;
        }

        public bool IsColliding(Vector3 pos)
        {
            Collider2D collider = Physics2D.OverlapBox(pos, new Vector2(0.3f, 0.3f), 0);

            if (collider != null)
                return true;

            return false;
        }

        public void CheckAndInstatiate()
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].CheckToDestroy();
            }

            while (obstacles.Count < MIN_COUNT)
                InstantiateObstacle();
        }

        void InstantiateObstacle()
        {
            pos.x += DISTANCE_BETWEEN_OBSTACLES;
            pos.y = Random.Range(-HEIGHT_RANDOM, HEIGHT_RANDOM);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            go.transform.SetParent(transform, false);
            ObstacleFlappy obstacle = go.GetComponent<ObstacleFlappy>();
            obstacle.OnDestroy += OnObstacleDestroy;
            obstacles.Add(obstacle);
        }

        void OnObstacleDestroy(ObstacleFlappy obstacle)
        {
            obstacle.OnDestroy -= OnObstacleDestroy;
            obstacles.Remove(obstacle);
        }
    }
}