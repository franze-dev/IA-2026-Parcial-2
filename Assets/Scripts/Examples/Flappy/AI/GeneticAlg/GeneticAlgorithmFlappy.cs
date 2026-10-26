using System.Collections.Generic;

namespace Examples.Flappy
{
    public class GeneticAlgorithmFlappy
    {
        List<GenomeFlappy> population = new List<GenomeFlappy>();
        List<GenomeFlappy> newPopulation = new List<GenomeFlappy>();

        float totalFitness;

        int eliteCount = 0;
        float mutationChance = 0.0f;
        float mutationRate = 0.0f;

        public GeneticAlgorithmFlappy(int eliteCount, float mutationChance, float mutationRate)
        {
            this.eliteCount = eliteCount;
            this.mutationChance = mutationChance;
            this.mutationRate = mutationRate;
        }

        public GenomeFlappy[] GetRandomGenomes(int count, int genesCount)
        {
            GenomeFlappy[] genomes = new GenomeFlappy[count];

            for (int i = 0; i < count; i++)
            {
                genomes[i] = new GenomeFlappy(genesCount);
            }

            return genomes;
        }


        public GenomeFlappy[] Epoch(GenomeFlappy[] oldGenomes)
        {
            totalFitness = 0;

            population.Clear();
            newPopulation.Clear();

            population.AddRange(oldGenomes);
            population.Sort(HandleComparison);

            foreach (GenomeFlappy g in population)
            {
                totalFitness += g.fitness;
            }

            SelectElite();

            while (newPopulation.Count < population.Count)
            {
                Crossover();
            }

            return newPopulation.ToArray();
        }

        void SelectElite()
        {
            for (int i = 0; i < eliteCount && newPopulation.Count < population.Count; i++)
            {
                newPopulation.Add(population[i]);
            }
        }

        void Crossover()
        {
            GenomeFlappy mom = RouletteSelection();
            GenomeFlappy dad = RouletteSelection();

            GenomeFlappy child1;
            GenomeFlappy child2;

            Crossover(mom, dad, out child1, out child2);

            newPopulation.Add(child1);
            newPopulation.Add(child2);
        }

        void Crossover(GenomeFlappy mom, GenomeFlappy dad, out GenomeFlappy child1, out GenomeFlappy child2)
        {
            child1 = new GenomeFlappy();
            child2 = new GenomeFlappy();

            child1.genome = new float[mom.genome.Length];
            child2.genome = new float[mom.genome.Length];

            int pivot = UnityEngine.Random.Range(0, mom.genome.Length);

            for (int i = 0; i < pivot; i++)
            {
                child1.genome[i] = mom.genome[i];

                if (ShouldMutate())
                    child1.genome[i] += UnityEngine.Random.Range(-mutationRate, mutationRate);

                child2.genome[i] = dad.genome[i];

                if (ShouldMutate())
                    child2.genome[i] += UnityEngine.Random.Range(-mutationRate, mutationRate);
            }

            for (int i = pivot; i < mom.genome.Length; i++)
            {
                child2.genome[i] = mom.genome[i];

                if (ShouldMutate())
                    child2.genome[i] += UnityEngine.Random.Range(-mutationRate, mutationRate);

                child1.genome[i] = dad.genome[i];

                if (ShouldMutate())
                    child1.genome[i] += UnityEngine.Random.Range(-mutationRate, mutationRate);
            }
        }

        bool ShouldMutate()
        {
            return UnityEngine.Random.Range(0.0f, 1.0f) < mutationChance;
        }

        int HandleComparison(GenomeFlappy x, GenomeFlappy y)
        {
            return x.fitness > y.fitness ? 1 : x.fitness < y.fitness ? -1 : 0;
        }


        public GenomeFlappy RouletteSelection()
        {
            float rnd = UnityEngine.Random.Range(0, UnityEngine.Mathf.Max(totalFitness, 0));

            float fitness = 0;

            for (int i = 0; i < population.Count; i++)
            {
                fitness += UnityEngine.Mathf.Max(population[i].fitness, 0);
                if (fitness >= rnd)
                    return population[i];
            }

            return null;
        }
    }

}
