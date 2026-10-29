namespace Examples.Tanks
{
    public sealed class SchmittTrigger
    {
        private readonly float vtUpper;
        private readonly float vtLower;
        private bool state;

        public bool State => state;

        public SchmittTrigger(float vtUpper, float vtLower, bool initialState = false)
        {
            if (vtLower > vtUpper)
                throw new System.ArgumentException("Lower threshold must not exceed upper threshold.");

            this.vtUpper = vtUpper;
            this.vtLower = vtLower;
            state = initialState;
        }

        public float UpdateTrigger(float inputSignal)
        {
            if (!state && inputSignal >= vtUpper)
                state = true;
            else if (state && inputSignal <= vtLower)
                state = false;

            return state ? 1f : 0f;
        }

        public void Reset(bool initialState = false)
        {
            state = initialState;
        }
    }
}
