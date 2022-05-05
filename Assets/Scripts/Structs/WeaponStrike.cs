public struct WeaponStrike
{
    public Damage weaponDamage;
    public float splashMultiplier;
    public Tile tile;

    public WeaponStrike(Damage damage, float splash, Tile t)
    {
        weaponDamage = damage;
        splashMultiplier = splash;
        tile = t;
    }
}
