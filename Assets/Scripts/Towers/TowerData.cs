using UnityEngine;

namespace TDLogic
{
    [CreateAssetMenu(fileName = "NewTowerData", menuName = "TDLogic/Tower Data", order = 0)]
    public class TowerData : ScriptableObject
    {
        [Header("Basic Stats")]
        public string towerName = "Default Tower";
        public int health = 100;
        public int damage = 10;
        public float radius = 3f;

        [Header("Visuals")]
        public GameObject hitEffect;
        public Sprite towerIcon;


    }
}
