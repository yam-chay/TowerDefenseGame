using UnityEngine;

namespace KingdomScratch
{
    [CreateAssetMenu(fileName = "newCharacterData", menuName = "TDLogic/Character Data", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [Header("Basic Stats")]
        public string characterName = "Default Character";
        public int health = 100;
        public int damage = 10;
        public float range = 2f;
    }
}
