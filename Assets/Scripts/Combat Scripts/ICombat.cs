using UnityEngine;

namespace PlatformerCombat
{
    //Interfaces require the class to contain the methods necessary for any of your implementations.
    public interface ICombat
    {
        void HitDetected();
        void AttackForce(float forcePower);
        void RangedAttack();
        void MeleeAttack(Stats targetStats, float damage);
    }
}
