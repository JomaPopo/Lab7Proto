// Interfaces.cs
using UnityEngine;

public interface IAimable
{
    Vector2 Position { get; set; }
}

public interface IMoveable
{
    void Move(Vector2 direction);
}

public interface IAttackable
{
    void Attack(Vector2 position);
}

// Nuevas interfaces para tipos específicos de ataque
public interface IBulletAttackable : IAttackable
{
    void ShootBullet(Vector2 direction);
}

public interface IExplosionAttackable : IAttackable
{
    void TriggerExplosion(Vector2 position);
}

public interface IChargeAttackable : IAttackable
{
    void Charge(Vector2 direction);
}