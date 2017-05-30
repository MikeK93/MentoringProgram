namespace Zoo.Animals
{
    public class Snake: Animal
    {
        public Snake(IAnimalStatusTracker statusTracker) : base(statusTracker)
        {
        }

        public override int LifeInterval
        {
            get
            {
                ValidateIfDisposed();
                return 1;
            }
        }

        public override int InfectionResistance
        {
            get
            {
                ValidateIfDisposed();
                return 80;
            }
        }
    }
}