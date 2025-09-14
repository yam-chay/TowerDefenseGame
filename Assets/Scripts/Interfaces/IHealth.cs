namespace TDLogic
{
    public interface IHealth
    {
        int CurrentHealth { get; }
        void TakeDamage(int amount);
        void Heal(int amount);
    }
}
