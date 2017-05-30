namespace Zoo.Animals
{
    public class Dog : Animal
    {
        public Dog(IAnimalStatusTracker statusTracker) : base(statusTracker)
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

        public override int EatInterval
        {
            get
            {
                ValidateIfDisposed();
                return 5;
            }
        }

        public override int HungerDeathInterval
        {
            get
            {
                ValidateIfDisposed();
                return 10;
            }
        }
    }
}