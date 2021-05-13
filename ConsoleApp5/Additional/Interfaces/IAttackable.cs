namespace Forest_Game.Additional.Interfaces
{
    interface IAttackable
    {
        bool TakeAttack(float Attack, ICanAttack damager);
        StrHealth Health { get; }
    }
    interface ICanAttack
    {
        float Damage { get; }
    }
    public class StrHealth
    {
        public float MaxHealth;
        private float health;
        public virtual float Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health > MaxHealth) health = MaxHealth;
                else if (health < 0) health = 0;
            }
        }
        public float addSecHealth;
        public bool alive;
        public StrHealth(float MHealth, float addhealth)
        {
            MaxHealth = MHealth;
            health = MHealth;
            addSecHealth = addhealth;
            alive = true;
        }
    }
}
