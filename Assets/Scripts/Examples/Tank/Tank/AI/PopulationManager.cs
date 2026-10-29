using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Examples.Tanks
{
    public enum NetworkMode
    {
        Standard,
        Convolutional,
        Deconvolutional
    }

    public class PopulationManager : MonoBehaviour
    {
        public GameObject TankPrefab;
        public GameObject MinePrefab;

        public int PopulationCount = 40;
        public int MinesCount = 50;

        public Vector3 SceneHalfExtents = new Vector3(20.0f, 0.0f, 20.0f);

        public float GenerationDuration = 20.0f;
        public int IterationCount = 1;

        public int EliteCount = 4;
        public float MutationChance = 0.10f;
        public float MutationRate = 0.01f;

        public int InputsCount = 5;
        public int HiddenLayers = 2;
        public int OutputsCount = 2;
        public int NeuronsCountPerHL = 7;
        public float Bias = 1f;
        public float P = 0.5f;

        public NetworkMode networkMode = NetworkMode.Standard;

        GeneticAlgorithm genAlg;

        List<Tank> populationGOs = new List<Tank>();
        List<Genome> population = new List<Genome>();
        List<NeuralNetwork> brains = new List<NeuralNetwork>();
        List<GameObject> mines = new List<GameObject>();
        List<GameObject> goodMines = new List<GameObject>();
        List<GameObject> badMines = new List<GameObject>();

        float accumTime = 0;
        bool isRunning = false;

        public int generation
        {
            get; private set;
        }

        public float bestFitness
        {
            get; private set;
        }

        public float avgFitness
        {
            get; private set;
        }

        public float worstFitness
        {
            get; private set;
        }

        private float getBestFitness()
        {
            float fitness = 0;
            foreach (Genome g in population)
            {
                if (fitness < g.fitness)
                    fitness = g.fitness;
            }

            return fitness;
        }

        private float getAvgFitness()
        {
            float fitness = 0;
            foreach (Genome g in population)
            {
                fitness += g.fitness;
            }

            return fitness / population.Count;
        }

        private float getWorstFitness()
        {
            float fitness = float.MaxValue;
            foreach (Genome g in population)
            {
                if (fitness > g.fitness)
                    fitness = g.fitness;
            }

            return fitness;
        }

        static PopulationManager instance = null;

        public static PopulationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<PopulationManager>();

                return instance;
            }
        }

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
        }

        public void StartSimulation()
        {
            // Create and confiugre the Genetic Algorithm
            genAlg = new GeneticAlgorithm(EliteCount, MutationChance, MutationRate);

            GenerateInitialPopulation();
            CreateMines();

            isRunning = true;
        }

        public void PauseSimulation()
        {
            isRunning = !isRunning;
        }

        public void StopSimulation()
        {
            isRunning = false;

            generation = 0;

            // Destroy previous tanks (if there are any)
            DestroyTanks();

            // Destroy all mines
            DestroyMines();
        }

        // Generate the random initial population
        void GenerateInitialPopulation()
        {
            generation = 0;

            // Destroy previous tanks (if there are any)
            DestroyTanks();

            for (int i = 0; i < PopulationCount; i++)
            {
                NeuralNetwork brain = CreateBrain();

                Genome genome = new Genome(brain.GetTotalWeightsCount());

                brain.SetWeights(genome.genome);
                brains.Add(brain);

                population.Add(genome);
                populationGOs.Add(CreateTank(genome, brain));
            }

            accumTime = 0.0f;
        }

        // Creates a new NeuralNetwork

        NeuralNetwork CreateBrain()
        {
            NeuralNetwork brain = new NeuralNetwork();

            // input 
            Debug.Log(InputsCount);

            brain.AddFirstNeuronLayer(InputsCount, Bias, P);

            // hidden 
            for (int i = 0; i < HiddenLayers; i++)
            {
                int neurons = GetHiddenNeuronCount(i);
                brain.AddNeuronLayer(neurons, Bias, P);
            }

            // output 
            brain.AddNeuronLayer(OutputsCount, Bias, P);

            return brain;
        }

        int GetHiddenNeuronCount(int layerIndex)
        {
            int maxNeurons = Mathf.Max(1, NeuronsCountPerHL);
            int minNeurons = Mathf.Max(1, maxNeurons / 3);

            if (networkMode == NetworkMode.Standard)
                return maxNeurons;

            float progress = HiddenLayers <= 1 ? 0f : layerIndex / (float)(HiddenLayers - 1);

            switch (networkMode)
            {
                case NetworkMode.Convolutional:
                    return Mathf.RoundToInt(Mathf.Lerp(maxNeurons, minNeurons, progress));

                case NetworkMode.Deconvolutional:
                    return Mathf.RoundToInt(Mathf.Lerp(minNeurons, maxNeurons, progress));

                default:
                    return maxNeurons;
            }
        }


        // Evolve!!!
        void Epoch()
        {
            // Increment generation counter
            generation++;

            // Calculate best, average and worst fitness
            bestFitness = getBestFitness();
            avgFitness = getAvgFitness();
            worstFitness = getWorstFitness();

            // Evolve each genome and create a new array of genomes
            Genome[] newGenomes = genAlg.Epoch(population.ToArray());

            // Clear current population
            population.Clear();

            // Add new population
            population.AddRange(newGenomes);

            // Set the new genomes as each NeuralNetwork weights
            for (int i = 0; i < PopulationCount; i++)
            {
                NeuralNetwork brain = brains[i];

                brain.SetWeights(newGenomes[i].genome);

                populationGOs[i].SetBrain(newGenomes[i], brain);
                populationGOs[i].transform.position = GetRandomPos();
                populationGOs[i].transform.rotation = GetRandomRot();
            }

            CreateMines();
        }

        void FixedUpdate()
        {
            if (!isRunning)
                return;

            float dt = Time.fixedDeltaTime;

            if (mines.Count == 0)
            {
                Epoch();
                return;
            }

            for (int i = 0; i < Mathf.Clamp((float)(IterationCount / 100.0f) * 50, 1, 50); i++)
            {
                foreach (Tank t in populationGOs)
                {
                    // Get the nearest mine
                    GameObject mine = GetNearestMine(t.transform.position);

                    // Set the nearest mine to current tank
                    t.SetNearestMine(mine);

                    mine = GetNearestGoodMine(t.transform.position);

                    // Set the nearest mine to current tank
                    t.SetGoodNearestMine(mine);

                    mine = GetNearestBadMine(t.transform.position);

                    // Set the nearest mine to current tank
                    t.SetBadNearestMine(mine);

                    // Think!! 
                    t.Think(dt);

                    // Just adjust tank position when reaching world extents
                    Vector3 pos = t.transform.position;
                    if (pos.x > SceneHalfExtents.x)
                        pos.x -= SceneHalfExtents.x * 2;
                    else if (pos.x < -SceneHalfExtents.x)
                        pos.x += SceneHalfExtents.x * 2;

                    if (pos.z > SceneHalfExtents.z)
                        pos.z -= SceneHalfExtents.z * 2;
                    else if (pos.z < -SceneHalfExtents.z)
                        pos.z += SceneHalfExtents.z * 2;

                    // Set tank position
                    t.transform.position = pos;
                }

                // Check the time to evolve
                accumTime += dt;
                if (accumTime >= GenerationDuration)
                {
                    accumTime -= GenerationDuration;
                    Epoch();
                    break;
                }
            }
        }

        #region Helpers
        Tank CreateTank(Genome genome, NeuralNetwork brain)
        {
            Vector3 position = GetRandomPos();
            GameObject go = Instantiate<GameObject>(TankPrefab, position, GetRandomRot());
            Tank t = go.GetComponent<Tank>();
            t.SetBrain(genome, brain);
            return t;
        }

        void DestroyMines()
        {
            foreach (GameObject go in mines)
                Destroy(go);

            mines.Clear();
            goodMines.Clear();
            badMines.Clear();
        }

        void DestroyTanks()
        {
            foreach (Tank go in populationGOs)
                Destroy(go.gameObject);

            populationGOs.Clear();
            population.Clear();
            brains.Clear();
        }

        void CreateMines()
        {
            // Destroy previous created mines
            DestroyMines();

            for (int i = 0; i < MinesCount; i++)
            {
                Vector3 position = GetRandomPos();
                GameObject go = Instantiate<GameObject>(MinePrefab, position, Quaternion.identity);

                bool good = Random.Range(-1.0f, 1.0f) >= 0;

                SetMineGood(good, go);

                mines.Add(go);
            }
        }

        void SetMineGood(bool good, GameObject go)
        {
            if (good)
            {
                go.GetComponent<Renderer>().material.color = Color.green;
                goodMines.Add(go);
            }
            else
            {
                go.GetComponent<Renderer>().material.color = Color.red;
                badMines.Add(go);
            }

        }

        public void RelocateMine(GameObject mine)
        {
            if (goodMines.Contains(mine))
                goodMines.Remove(mine);
            else
                badMines.Remove(mine);

            bool good = Random.Range(-1.0f, 1.0f) >= 0;

            SetMineGood(good, mine);

            mine.transform.position = GetRandomPos();
        }

        Vector3 GetRandomPos()
        {
            return new Vector3(Random.value * SceneHalfExtents.x * 2.0f - SceneHalfExtents.x, 0.0f, Random.value * SceneHalfExtents.z * 2.0f - SceneHalfExtents.z);
        }

        Quaternion GetRandomRot()
        {
            return Quaternion.AngleAxis(Random.value * 360.0f, Vector3.up);
        }


        GameObject GetNearestMine(Vector3 pos)
        {
            return GetNearestFromList(mines, pos);
        }

        GameObject GetNearestGoodMine(Vector3 pos)
        {
            return GetNearestFromList(goodMines, pos);
        }

        GameObject GetNearestBadMine(Vector3 pos)
        {
            return GetNearestFromList(badMines, pos);
        }

        GameObject GetNearestFromList(List<GameObject> objectives, Vector3 pos)
        {
            GameObject nearest = null;
            float bestDistance = float.MaxValue;

            foreach (GameObject objective in objectives)
            {
                if (objective == null)
                    continue;

                float distance = (objective.transform.position - pos).sqrMagnitude;

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    nearest = objective;
                }
            }

            return nearest;
        }


        internal bool TryConsumeObjective(GameObject objective)
        {
            if (objective == null || !mines.Remove(objective))
                return false;

            goodMines.Remove(objective);
            badMines.Remove(objective);

            Destroy(objective);

            return true;
        }

        #endregion

    }

}
