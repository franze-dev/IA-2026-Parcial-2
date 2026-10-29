
using UnityEngine;

namespace Examples.Tanks
{
    public class Genome
    {
        public float[] genome;
        public float[] chromosomeA;
        public float[] chromosomeB;
        public bool[] isDominantA;
        public bool[] isDominantB;

        public float fitness = 0f;

        public int GenesCount => chromosomeA.Length;

        public Genome(int genesCount)
        {
            chromosomeA = new float[genesCount];
            chromosomeB = new float[genesCount];
            isDominantA = new bool[genesCount];
            isDominantB = new bool[genesCount];

            for (int i = 0; i < genesCount; i++)
            {
                chromosomeA[i] = Random.Range(-1f, 1f);
                chromosomeB[i] = Random.Range(-1f, 1f);

                isDominantA[i] = Random.value < 0.5f;
                isDominantB[i] = Random.value < 0.5f;
            }

            fitness = 0f;
            RefreshPhenotype();
        }

        public Genome(float[] genes)
        {
            int count = genes.Length;

            chromosomeA = (float[])genes.Clone();
            chromosomeB = (float[])genes.Clone();
            isDominantA = new bool[count];
            isDominantB = new bool[count];

            for (int i = 0; i < count; i++)
            {
                isDominantA[i] = Random.value < 0.5f;
                isDominantB[i] = Random.value < 0.5f;
            }

            fitness = 0f;
            RefreshPhenotype();
        }

        public Genome()
        {
            fitness = 0f;
        }

        public float[] GetExpressedPhenotype()
        {
            float[] phenotype = new float[GenesCount];

            for (int i = 0; i < phenotype.Length; i++)
            {
                phenotype[i] = isDominantA[i] && !isDominantB[i] ?
                               chromosomeA[i] :
                               !isDominantA[i] && isDominantB[i] ?
                               chromosomeB[i] :
                               (chromosomeA[i] + chromosomeB[i]) * 0.5f;
            }

            return phenotype;
        }

        public void RefreshPhenotype()
        {
            genome = GetExpressedPhenotype();
        }
    }
}
