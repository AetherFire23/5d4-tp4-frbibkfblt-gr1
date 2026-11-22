using System;

namespace Knight.Pure
{
    public class KnightAfflictionUpdatedEvent
    {
        public KnightAfflictionTypes Affliction { get; private set; }
        public bool IsCured { get; private set; }

        public KnightAfflictionUpdatedEvent(KnightAfflictionTypes affliction, bool isCured)
        {
            Affliction = affliction;
            IsCured = isCured;
        }
    }

    public class KnightAffliction
    {
        public event EventHandler<KnightAfflictionUpdatedEvent> OnUpdated;
        private bool _isCured;
        public KnightAfflictionTypes KnightAfflictionType { get; private set; }

        public bool IsCured
        {
            get => _isCured;
            private set
            {
                _isCured = value;
                // inform the UI that this has changed baby 
                OnUpdated?.Invoke(this, new KnightAfflictionUpdatedEvent(this.KnightAfflictionType, this.IsCured));
            }
        }

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