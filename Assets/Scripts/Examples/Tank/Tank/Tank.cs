using UnityEngine;

namespace Examples.Tanks
{
    public class Tank : TankBase
    {
        private readonly SchmittTrigger targetDistanceTrigger = new SchmittTrigger(0.7f, 0.3f);

        float fitness = 0;
        protected override void OnReset()
        {
            fitness = 1;
            genome.fitness = fitness;
            targetDistanceTrigger.Reset();
        }


        protected override void OnThink(float dt)
        {
            if (inputs == null || inputs.Length < 5)
            {
                Debug.LogError($"Tank requires 5 inputs, but received {inputs?.Length ?? 0}.Check PopulationManager.InputsCount.");
                return;
            }

            if (nearMine == null)
                return;

            Vector3 dirToMine = GetDirToMine(nearMine);

            inputs[0] = dirToMine.x;
            inputs[1] = dirToMine.z;
            inputs[2] = transform.forward.x;
            inputs[3] = transform.forward.z;

            float distance = Vector3.Distance(transform.position, nearMine.transform.position);

            const float detectionRange = 20f;
            float proximity = Mathf.Clamp01(1f - distance / detectionRange);

            inputs[4] = targetDistanceTrigger.UpdateTrigger(proximity);

            float[] output = brain.Synapsis(inputs);

            SetForces(output[0], output[1], dt);
        }


        protected override void OnTakeMine(GameObject mine)
        {
            fitness *= 2;
            genome.fitness = fitness;
        }
    }
}