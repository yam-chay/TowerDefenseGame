namespace TDLogic
{
    public interface IDamagable
    {
        int Health { get; }
        void TakeDamage(int damage);
        void Heal(int amount);
    }

}
