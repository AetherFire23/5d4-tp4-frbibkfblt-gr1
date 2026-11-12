
namespace Knight.Pure
{
    public class KnightAffliction
    {
        public KnightAfflictionTypes KnightAfflictionType { get; private set; }

        public bool IsCured { get; private set; }

        /**
         * The Affliction is Prevented from being inflicted this curse. The next Infliction does not apply.
         */
        public bool IsPrevented { get; set; } = false;

        /**
         * The Affliction is Persistent, the next cure does not apply.
         */
        public bool IsPersistent { get; set; } = false;

        public KnightAffliction(KnightAfflictionTypes knightAfflictionType)
        {
            this.KnightAfflictionType = knightAfflictionType;
        }

        /**
         * Tries to cure. If the Affliction is Persistent, it is not cured but the Persistent status is set to false.
         */
        public void Cure()
        {
            if (IsPersistent)
            {
                IsPersistent = false;
                return;
            }

            IsCured = true;
        }

        /**
         * Tries to inflict damage. If the
         */
        public void Inflict()
        {
            if (IsPrevented)
            {
                this.IsPrevented = false;
                return;
            }

            IsCured = false;
        }

        public void Prevent()
        {
            this.IsPrevented = true;
        }

        public void Persist()
        {
            this.IsPersistent = true;
        }
    }
}