using UnityEngine;

namespace Examples.Tanks
{
    public class Genome
    {
        public float[] genome;
        public float fitness = 0;

        public Genome(float[] genes)
        {
            genome = genes;
            fitness = 0;
        }

        public Genome(int genesCount)
        {
            genome = new float[genesCount];

            for (int j = 0; j < genesCount; j++)
                genome[j] = Random.Range(-1.0f, 1.0f);

            fitness = 0;
        }

        public Genome()
        {
            fitness = 0;
        }

    }
}