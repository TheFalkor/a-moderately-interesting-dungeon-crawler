using System.Collections.Generic;

public struct Damage
{
    public int damage;
    public DamageOrigin origin;
    public List<StatusEffect> statusEffects;

    public Damage(int damage, DamageOrigin origin, List<StatusEffect> statusEffects = null)
    {
        this.damage = damage;
        this.origin = origin;
        this.statusEffects = statusEffects;
    }
}