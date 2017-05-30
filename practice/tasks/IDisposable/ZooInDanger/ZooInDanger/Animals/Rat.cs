namespace Zoo.Animals
{
    public class Rat: Animal
    {
        public Rat(IAnimalStatusTracker statusTracker) : base(statusTracker)
        {
        }

        public override int LifeInterval
        {
            get
            {
                ValidateIfDisposed();
                return 3;
            }
        }

        public override int InfectionResistance
        {
            get
            {
                ValidateIfDisposed();
                return 101;
            }
        }

        public override int HungerDeathInterval
        {
            get
            {
                ValidateIfDisposed();
                return 100;
            }
        }
    }
}