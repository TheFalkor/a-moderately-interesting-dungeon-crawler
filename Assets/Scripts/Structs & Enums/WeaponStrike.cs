public struct WeaponStrike
{
    public Damage weaponDamage;
    public Tile tile;

    public WeaponStrike(Damage damage, Tile t)
    {
        weaponDamage = damage;
        tile = t;
    }
}
