namespace Examples.Flappy
{
    public class GenomeFlappy
    {
        public float[] genome;
        public float fitness = 0;

        public GenomeFlappy(float[] genes)
        {
            this.genome = genes;
            fitness = 0;
        }

        public GenomeFlappy(int genesCount)
        {
            genome = new float[genesCount];

            for (int j = 0; j < genesCount; j++)
                genome[j] = UnityEngine.Random.Range(-1.0f, 1.0f);

            fitness = 0;
        }

        public GenomeFlappy()
        {
            fitness = 0;
        }

    }

}
