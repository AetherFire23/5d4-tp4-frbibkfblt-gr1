using Codice.Client.BaseCommands;

namespace Knight.Pure
{
    public class KnightAffliction
    {
        public KnightAfflictionTypes KnightAfflictionType { get; private set; }

        public bool IsCured { get; private set; }
        public bool IsProtected { get; set; } = false;

        public KnightAffliction(KnightAfflictionTypes knightAfflictionType)
        {
            this.KnightAfflictionType = knightAfflictionType;
        }

        public void Cure()
        {
            IsCured = true;
        }

        /**
         * Tries to inflict damage, Will not flip IsCured to False if it is protected.
         */
        public void Inflict()
        {
            if (IsProtected)
            {
                this.IsProtected = false;
                return;
            }

            IsCured = false;
        }

        public void Protect()
        {
            this.IsProtected = true;
        }
    }
}