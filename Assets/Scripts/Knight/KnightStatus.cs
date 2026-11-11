namespace Knight
{
    public class KnightStatus
    {
        public Stats Stat { get; private set; }

        public bool IsSatisfied { get; set; }

        public KnightStatus(Stats stat)
        {
            this.Stat = stat;
        }

        public void Toggle()
        {
            this.IsSatisfied = !IsSatisfied;
        }
    }
}