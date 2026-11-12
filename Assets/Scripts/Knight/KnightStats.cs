namespace Knight
{
    public class KnightStats
    {
        public float Health { get; set; } = 0;

        public bool IsDead => Health <= 0;

        public void ReceiveGoodFood()
        {
            this.Health++;
        }

        public void ReceiveBadFood()
        {
            this.Health--;
        }

        public string DeathMessage => "This is a death message ";

        /**
         * Le NearbyMessage, c'est le message qui est affiche lorsque l'oiseau se rapproche du chevalier.
         * Ca va lui indiquer
         */
        public string GetCurrentNearbyMessage()
        {
            string message = "";

            if (Health <= 3)
            {
                message =
                    "Your fallen Knight has not moved nor spoken a word in weeks. May rest - or perhaps aid - put an end to his slumber";
            }
            else if (Health <= 6)
            {
                message = "Your fallen Knight has begun to move and eat. However, he is still speechless. ";
            }
            else if (Health <= 9)
            {
                message = "Your fallen Knight has become verbal, altough his speech is slurrech and often confused. ";
            }

            return message;
        }
    }
}