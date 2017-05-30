namespace Zoo.Animals
{
    public class Cat : Animal
    {
        public Cat(IAnimalStatusTracker statusTracker) : base(statusTracker)
        {
        }

        public override int LifeInterval
        {
            get
            {
                ValidateIfDisposed();
                return 13;
            }
        }

        public override int InfectionDeathInterval
        {
            get
            {
                ValidateIfDisposed();
                return 300;
            }
        }

        protected override void Dispose(bool disposing)
        {
            // Release the object only in case number of corpses > 100
            if (Zoo.NumCorpses <= 100) return;

            if (disposing)
            {
                Logger.LogYellow("Disposing cat!");
            }
            else
            {
                Logger.LogYellow("Finalizing cat!");
            }

            base.Dispose(disposing);
        }
    }
}