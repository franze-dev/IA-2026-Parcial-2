using UnityEngine;
using UnityEngine.UI;

namespace Examples.Flappy
{
    public class StartConfigurationScreenFlappy : MonoBehaviour
    {
        public Text populationCountTxt;
        public Slider populationCountSlider;
        public Text eliteCountTxt;
        public Slider eliteCountSlider;
        public Text mutationChanceTxt;
        public Slider mutationChanceSlider;
        public Text mutationRateTxt;
        public Slider mutationRateSlider;
        public Text hiddenLayersCountTxt;
        public Slider hiddenLayersCountSlider;
        public Text neuronsPerHLCountTxt;
        public Slider neuronsPerHLSlider;
        public Text biasTxt;
        public Slider biasSlider;
        public Text sigmoidSlopeTxt;
        public Slider sigmoidSlopeSlider;
        public Text inputsTxt;
        public Slider inputsSlider;
        public Text outputsTxt;
        public Slider outputsSlider;
        public Button startButton;
        public GameObject simulationScreen;

        string populationText;
        string minesText;
        string generationDurationText;
        string elitesText;
        string mutationChanceText;
        string mutationRateText;
        string hiddenLayersCountText;
        string biasText;
        string sigmoidSlopeText;
        string neuronsPerHLCountText;
        string inputsText;
        string outputsText;

        void Start()
        {
            populationCountSlider.onValueChanged.AddListener(OnPopulationCountChange);
            eliteCountSlider.onValueChanged.AddListener(OnEliteCountChange);
            mutationChanceSlider.onValueChanged.AddListener(OnMutationChanceChange);
            mutationRateSlider.onValueChanged.AddListener(OnMutationRateChange);
            hiddenLayersCountSlider.onValueChanged.AddListener(OnHiddenLayersCountChange);
            neuronsPerHLSlider.onValueChanged.AddListener(OnNeuronsPerHLChange);
            biasSlider.onValueChanged.AddListener(OnBiasChange);
            sigmoidSlopeSlider.onValueChanged.AddListener(OnSigmoidSlopeChange);
            inputsSlider.onValueChanged.AddListener(OnInputsChange);
            outputsSlider.onValueChanged.AddListener(OnOutputsChange);

            populationText = populationCountTxt.text;
            elitesText = eliteCountTxt.text;
            mutationChanceText = mutationChanceTxt.text;
            mutationRateText = mutationRateTxt.text;
            hiddenLayersCountText = hiddenLayersCountTxt.text;
            neuronsPerHLCountText = neuronsPerHLCountTxt.text;
            biasText = biasTxt.text;
            sigmoidSlopeText = sigmoidSlopeTxt.text;
            inputsText = inputsTxt.text;
            outputsText = outputsTxt.text;

            populationCountSlider.value = PopulationManagerFlappy.Instance.PopulationCount;
            eliteCountSlider.value = PopulationManagerFlappy.Instance.EliteCount;
            mutationChanceSlider.value = Mathf.Round(PopulationManagerFlappy.Instance.MutationChance * 100.0f);
            mutationRateSlider.value = Mathf.Round(PopulationManagerFlappy.Instance.MutationRate * 100.0f);
            hiddenLayersCountSlider.value = PopulationManagerFlappy.Instance.HiddenLayers;
            neuronsPerHLSlider.value = PopulationManagerFlappy.Instance.NeuronsCountPerHL;
            biasSlider.value = PopulationManagerFlappy.Instance.Bias;
            sigmoidSlopeSlider.value = PopulationManagerFlappy.Instance.Sigmoid;
            inputsSlider.value = PopulationManagerFlappy.Instance.InputsCount;
            outputsSlider.value = PopulationManagerFlappy.Instance.OutputsCount;

            startButton.onClick.AddListener(OnStartButtonClick);

            Refresh();
        }

        void OnPopulationCountChange(float value)
        {
            PopulationManagerFlappy.Instance.PopulationCount = (int)value;

            populationCountTxt.text = string.Format(populationText, PopulationManagerFlappy.Instance.PopulationCount);
        }

        void OnEliteCountChange(float value)
        {
            PopulationManagerFlappy.Instance.EliteCount = (int)value;

            eliteCountTxt.text = string.Format(elitesText, PopulationManagerFlappy.Instance.EliteCount);
        }

        void OnMutationChanceChange(float value)
        {
            PopulationManagerFlappy.Instance.MutationChance = value / 100.0f;

            mutationChanceTxt.text = string.Format(mutationChanceText, (int)(PopulationManagerFlappy.Instance.MutationChance * 100));
        }

        void OnMutationRateChange(float value)
        {
            PopulationManagerFlappy.Instance.MutationRate = value / 100.0f;

            mutationRateTxt.text = string.Format(mutationRateText, (int)(PopulationManagerFlappy.Instance.MutationRate * 100));
        }

        void OnHiddenLayersCountChange(float value)
        {
            PopulationManagerFlappy.Instance.HiddenLayers = (int)value;


            hiddenLayersCountTxt.text = string.Format(hiddenLayersCountText, PopulationManagerFlappy.Instance.HiddenLayers);
        }

        void OnNeuronsPerHLChange(float value)
        {
            PopulationManagerFlappy.Instance.NeuronsCountPerHL = (int)value;

            neuronsPerHLCountTxt.text = string.Format(neuronsPerHLCountText, PopulationManagerFlappy.Instance.NeuronsCountPerHL);
        }

        void OnBiasChange(float value)
        {
            PopulationManagerFlappy.Instance.Bias = -value;

            biasTxt.text = string.Format(biasText, PopulationManagerFlappy.Instance.Bias.ToString("0.00"));
        }

        void OnSigmoidSlopeChange(float value)
        {
            PopulationManagerFlappy.Instance.Sigmoid = value;

            sigmoidSlopeTxt.text = string.Format(sigmoidSlopeText, PopulationManagerFlappy.Instance.Sigmoid.ToString("0.00"));
        }


        void OnStartButtonClick()
        {
            PopulationManagerFlappy.Instance.StartSimulation();
            gameObject.SetActive(false);
            simulationScreen.SetActive(true);
        }

        void OnInputsChange(float value)
        {
            PopulationManagerFlappy.Instance.InputsCount = (int)value;

            inputsTxt.text = string.Format(inputsText, PopulationManagerFlappy.Instance.InputsCount);
        }

        void OnOutputsChange(float value)
        {
            PopulationManagerFlappy.Instance.OutputsCount = (int)value;

            outputsTxt.text = string.Format(outputsText, PopulationManagerFlappy.Instance.OutputsCount);
        }

        void Refresh()
        {
            populationCountTxt.text = string.Format(populationText, PopulationManagerFlappy.Instance.PopulationCount);
            eliteCountTxt.text = string.Format(elitesText, PopulationManagerFlappy.Instance.EliteCount);
            mutationChanceTxt.text = string.Format(mutationChanceText, (int)(PopulationManagerFlappy.Instance.MutationChance * 100));
            mutationRateTxt.text = string.Format(mutationRateText, (int)(PopulationManagerFlappy.Instance.MutationRate * 100));
            hiddenLayersCountTxt.text = string.Format(hiddenLayersCountText, PopulationManagerFlappy.Instance.HiddenLayers);
            neuronsPerHLCountTxt.text = string.Format(neuronsPerHLCountText, PopulationManagerFlappy.Instance.NeuronsCountPerHL);
            biasTxt.text = string.Format(biasText, PopulationManagerFlappy.Instance.Bias.ToString("0.00"));
            sigmoidSlopeTxt.text = string.Format(sigmoidSlopeText, PopulationManagerFlappy.Instance.Sigmoid.ToString("0.00"));
            inputsTxt.text = string.Format(inputsText, PopulationManagerFlappy.Instance.InputsCount);
            outputsTxt.text = string.Format(outputsText, PopulationManagerFlappy.Instance.OutputsCount);
        }
    }
}