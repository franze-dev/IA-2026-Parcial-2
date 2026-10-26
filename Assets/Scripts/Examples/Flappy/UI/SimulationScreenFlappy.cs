using UnityEngine;
using UnityEngine.UI;

namespace Examples.Flappy
{
    public class SimulationScreenFlappy : MonoBehaviour
    {
        public Text generationsCountTxt;
        public Text bestFitnessTxt;
        public Text avgFitnessTxt;
        public Text worstFitnessTxt;
        public Text timerTxt;
        public Slider timerSlider;
        public Button pauseBtn;
        public Button stopBtn;
        public GameObject startConfigurationScreen;

        string generationsCountText;
        string bestFitnessText;
        string avgFitnessText;
        string worstFitnessText;
        string timerText;
        int lastGeneration = 0;

        void Start()
        {
            timerSlider.onValueChanged.AddListener(OnTimerChange);
            timerText = timerTxt.text;

            timerTxt.text = string.Format(timerText, PopulationManagerFlappy.Instance.IterationCount);

            if (string.IsNullOrEmpty(generationsCountText))
                generationsCountText = generationsCountTxt.text;
            if (string.IsNullOrEmpty(bestFitnessText))
                bestFitnessText = bestFitnessTxt.text;
            if (string.IsNullOrEmpty(avgFitnessText))
                avgFitnessText = avgFitnessTxt.text;
            if (string.IsNullOrEmpty(worstFitnessText))
                worstFitnessText = worstFitnessTxt.text;

            pauseBtn.onClick.AddListener(OnPauseButtonClick);
            stopBtn.onClick.AddListener(OnStopButtonClick);
        }

        void OnEnable()
        {
            if (string.IsNullOrEmpty(generationsCountText))
                generationsCountText = generationsCountTxt.text;
            if (string.IsNullOrEmpty(bestFitnessText))
                bestFitnessText = bestFitnessTxt.text;
            if (string.IsNullOrEmpty(avgFitnessText))
                avgFitnessText = avgFitnessTxt.text;
            if (string.IsNullOrEmpty(worstFitnessText))
                worstFitnessText = worstFitnessTxt.text;

            generationsCountTxt.text = string.Format(generationsCountText, 0);
            bestFitnessTxt.text = string.Format(bestFitnessText, 0);
            avgFitnessTxt.text = string.Format(avgFitnessText, 0);
            worstFitnessTxt.text = string.Format(worstFitnessText, 0);
        }

        void OnTimerChange(float value)
        {
            PopulationManagerFlappy.Instance.IterationCount = (int)value;
            timerTxt.text = string.Format(timerText, PopulationManagerFlappy.Instance.IterationCount);
        }

        void OnPauseButtonClick()
        {
            PopulationManagerFlappy.Instance.PauseSimulation();
        }

        void OnStopButtonClick()
        {
            PopulationManagerFlappy.Instance.StopSimulation();
            gameObject.SetActive(false);
            startConfigurationScreen.SetActive(true);
            lastGeneration = 0;
        }

        void LateUpdate()
        {
            if (lastGeneration != PopulationManagerFlappy.Instance.generation)
            {
                lastGeneration = PopulationManagerFlappy.Instance.generation;
                generationsCountTxt.text = string.Format(generationsCountText, PopulationManagerFlappy.Instance.generation);
                bestFitnessTxt.text = string.Format(bestFitnessText, PopulationManagerFlappy.Instance.bestFitness);
                avgFitnessTxt.text = string.Format(avgFitnessText, PopulationManagerFlappy.Instance.avgFitness);
                worstFitnessTxt.text = string.Format(worstFitnessText, PopulationManagerFlappy.Instance.worstFitness);
            }
        }
    }
}