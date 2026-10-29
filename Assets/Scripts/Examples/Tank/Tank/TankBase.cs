using UnityEngine;

namespace Examples.Tanks
{
    public class TankBase : MonoBehaviour
    {
        public float Speed = 10.0f;
        public float RotSpeed = 20.0f;

        protected Genome genome;
        protected NeuralNetwork brain;
        protected GameObject nearMine;
        protected GameObject goodMine;
        protected GameObject badMine;
        protected float[] inputs;

        public void SetBrain(Genome genome, NeuralNetwork brain)
        {
            this.genome = genome;
            this.brain = brain;
            inputs = new float[brain.InputsCount];
            OnReset();
        }

        public void SetNearestMine(GameObject mine)
        {
            nearMine = mine;
        }

        public void SetGoodNearestMine(GameObject mine)
        {
            goodMine = mine;
        }

        public void SetBadNearestMine(GameObject mine)
        {
            badMine = mine;
        }

        protected bool IsGoodMine(GameObject mine)
        {
            return goodMine == mine;
        }

        protected Vector3 GetDirToMine(GameObject mine)
        {
            return (mine.transform.position - transform.position).normalized;
        }

        protected bool IsCloseToMine(GameObject mine)
        {
            return mine != null && (transform.position - mine.transform.position).sqrMagnitude <= 2f;
        }

        protected void SetForces(float leftForce, float rightForce, float dt)
        {
            Vector3 pos = transform.position;
            float rotFactor = Mathf.Clamp(rightForce - leftForce, -1.0f, 1.0f);
            transform.rotation *= Quaternion.AngleAxis(rotFactor * RotSpeed * dt, Vector3.up);
            pos += transform.forward * Mathf.Abs(rightForce + leftForce) * 0.5f * Speed * dt;
            transform.position = pos;
        }

        public void Think(float dt)
        {
            OnThink(dt);

            if (IsCloseToMine(nearMine))
            {
                GameObject objective = nearMine;

                if (PopulationManager.Instance.TryConsumeObjective(objective))
                {
                    OnTakeMine(objective);
                }
            }
        }

        protected virtual void OnThink(float dt)
        {

        }

        protected virtual void OnTakeMine(GameObject mine)
        {
        }

        protected virtual void OnReset()
        {

        }
    }
}