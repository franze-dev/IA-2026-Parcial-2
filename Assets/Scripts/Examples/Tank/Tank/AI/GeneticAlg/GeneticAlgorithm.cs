using UnityEngine;
using System.Collections.Generic;

namespace Examples.Tanks
{
    public class GeneticAlgorithm
    {
        List<Genome> population = new List<Genome>();
        List<Genome> newPopulation = new List<Genome>();

        float totalFitness;

        int eliteCount = 0;
        float mutationChance = 0.0f;
        float mutationRate = 0.0f;

        public GeneticAlgorithm(int eliteCount, float mutationChance, float mutationRate)
        {
            this.eliteCount = eliteCount;
            this.mutationChance = mutationChance;
            this.mutationRate = mutationRate;
        }

        public Genome[] GetRandomGenomes(int count, int genesCount)
        {
            Genome[] genomes = new Genome[count];

            for (int i = 0; i < count; i++)
            {
                genomes[i] = new Genome(genesCount);
            }

            return genomes;
        }


        public Genome[] Epoch(Genome[] oldGenomes)
        {
            population.Clear();
            newPopulation.Clear();

            population.AddRange(oldGenomes);
            population.Sort(HandleComparison);
            RecalculateTotalFitness();

            SelectElite();

            while (newPopulation.Count < population.Count)
            {
                Crossover();
            }

            return newPopulation.ToArray();
        }

        private void RecalculateTotalFitness()
        {
            totalFitness = 0;

            foreach (Genome g in population)
            {
                totalFitness += g.fitness;
            }
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
            Genome mom = RouletteSelection();
            Genome dad = RouletteSelection();

            if (mom == null || dad == null)
            {
                Debug.LogWarning("Could not find mom/dad");
                return;
            }

            Crossover(mom, dad, out Genome child1, out Genome child2);

            if (newPopulation.Count < population.Count)
                newPopulation.Add(child1);

            if (newPopulation.Count < population.Count)
                newPopulation.Add(child2);
        }

        void Crossover(Genome mom, Genome dad, out Genome child1, out Genome child2)
        {
            int momGenesCount = mom.GenesCount;

            child1 = new Genome(momGenesCount);
            child2 = new Genome(momGenesCount);

            int pivotMom = Random.Range(0, momGenesCount);
            int pivotDad = Random.Range(0, momGenesCount);

            for (int i = 0; i < momGenesCount; i++)
            {
                InheritVariant(mom, i, pivotMom, out child1.chromosomeA[i], out child1.isDominantA[i]);

                InheritVariant(dad, i, pivotDad, out child1.chromosomeB[i], out child1.isDominantB[i]);

                InheritComplementaryVariant(mom, i, pivotMom, out child2.chromosomeA[i], out child2.isDominantA[i]);

                InheritComplementaryVariant(dad, i, pivotDad, out child2.chromosomeB[i], out child2.isDominantB[i]);

                if (ShouldMutate())
                    child1.chromosomeA[i] += Random.Range(-mutationRate, mutationRate);

                if (ShouldMutate())
                    child1.chromosomeB[i] += Random.Range(-mutationRate, mutationRate);

                if (ShouldMutate())
                    child2.chromosomeA[i] += Random.Range(-mutationRate, mutationRate);

                if (ShouldMutate())
                    child2.chromosomeB[i] += Random.Range(-mutationRate, mutationRate);
            }

            child1.RefreshPhenotype();
            child2.RefreshPhenotype();
        }

        void InheritVariant(Genome parent, int index, int pivot, out float variant, out bool dominant)
        {
            bool useA = index < pivot;

            variant = useA ? parent.chromosomeA[index] : parent.chromosomeB[index];

            dominant = useA ? parent.isDominantA[index] : parent.isDominantB[index];
        }

        void InheritComplementaryVariant(Genome parent, int index, int pivot, out float variant, out bool dominant)
        {
            bool useA = index >= pivot;

            variant = useA ? parent.chromosomeA[index] : parent.chromosomeB[index];

            dominant = useA ? parent.isDominantA[index] : parent.isDominantB[index];
        }

        bool ShouldMutate()
        {
            return Random.Range(0.0f, 1.0f) < mutationChance;
        }

        int HandleComparison(Genome x, Genome y)
        {
            return y.fitness.CompareTo(x.fitness);
        }

        public Genome RouletteSelection()
        {
            float rnd = Random.Range(0, Mathf.Max(totalFitness, 0));

            float fitness = 0;

            for (int i = 0; i < population.Count; i++)
            {
                fitness += Mathf.Max(population[i].fitness, 0);
                if (fitness >= rnd)
                    return population[i];
            }

            return null;
        }

    }
}