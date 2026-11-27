using System;

namespace Knight.Pure
{
    public class KnightAffliction
    {
        public delegate void KnightAfflictionUpdated(KnightAffliction affliction);

        public event KnightAfflictionUpdated TriggerOnUpdated;
        private bool _isCured;
        public KnightAfflictionTypes KnightAfflictionType { get; private set; }

        public bool IsCured { get; set; }

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

        public KnightAffliction()
        {
            
        }

        /**
         * Tries to cure. If the Affliction is Persistent, it is not cured but the Persistent status is set to false.
         */
        public void Cure()
        {
            if (IsPersistent)
            {
                IsPersistent = false;
                TriggerOnUpdated?.Invoke(this);
                return;
            }

            IsCured = true;

            TriggerOnUpdated?.Invoke(this);
        }

        /**
         * Tries to inflict damage. If the
         */
        public void Inflict()
        {
            if (IsPrevented)
            {
                this.IsPrevented = false;
                TriggerOnUpdated?.Invoke(this);
                return;
            }

            IsCured = false;
            TriggerOnUpdated?.Invoke(this);
        }

        public void Prevent()
        {
            this.IsPrevented = true;
            TriggerOnUpdated?.Invoke(this);
        }

        public void Persist()
        {
            this.IsPersistent = true;
            TriggerOnUpdated?.Invoke(this);
        }

        public void Refresh()
        {
            this.TriggerOnUpdated.Invoke(this);
        }
    }
}