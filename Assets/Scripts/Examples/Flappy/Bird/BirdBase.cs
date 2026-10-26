using UnityEngine;

namespace Examples.Flappy
{
    public class BirdBase : MonoBehaviour
    {
        public enum State
        {
            Alive,
            Dead
        }

        public State state
        {
            get; private set;
        }

        protected GenomeFlappy genome;
        protected NeuralNetworkFlappy brain;
        protected BirdBehaviour birdBehaviour;

        private void Awake()
        {
            birdBehaviour = GetComponent<BirdBehaviour>();
        }

        public void SetBrain(GenomeFlappy genome, NeuralNetworkFlappy brain)
        {
            this.genome = genome;
            this.brain = brain;
            state = State.Alive;
            birdBehaviour.Reset();
            OnReset();
        }

        public void Flap()
        {
            if (state == State.Alive)
                birdBehaviour.Flap();
        }

        public void Think(float dt)
        {
            if (state == State.Alive)
            {
                ObstacleFlappy obstacle = ObstacleManager.Instance.GetNextObstacle(transform.position);

                if (obstacle == null)
                    return;

                OnThink(dt, birdBehaviour, obstacle);

                birdBehaviour.UpdateBird(dt);

                if (transform.position.y > 5f || transform.position.y < -5f || ObstacleManager.Instance.IsColliding(transform.position))
                {
                    OnDead();
                    state = State.Dead;
                }
            }
        }

        protected virtual void OnDead()
        {

        }

        protected virtual void OnThink(float dt, BirdBehaviour birdBehaviour, ObstacleFlappy obstacle)
        {

        }

        protected virtual void OnReset()
        {

        }

    }
}