using System.Collections.Generic;

public static class CombatEnumAdapter
{
    private static readonly Dictionary<string, EnemyType> enemyTypeMap = new Dictionary<string, EnemyType>()
    {
        { "Vagabundo", EnemyType.Vagabundo },
        { "Traficante", EnemyType.Traficante },
        { "Dinnie", EnemyType.Dinnie },
        { "Dinnie Jefe", EnemyType.DinnieJefe },
        { "Maton", EnemyType.Maton },
        { "Mercenario", EnemyType.Mercenario },
        { "Ninja", EnemyType.Ninja },
        { "Ninja patrulla", EnemyType.NinjaPatrulla },
        { "Patrulla", EnemyType.Patrulla },
        { "Policia", EnemyType.Policia },
        { "Jefe Mercenario", EnemyType.JefeMercenario },
        { "Jefe Central", EnemyType.JefeCentral },
        { "CEO de OMNI TECH", EnemyType.CEOOmniTech }
    };

    private static readonly Dictionary<string, WeaponType> weaponTypeMap = new Dictionary<string, WeaponType>()
    {
        { "Pistola", WeaponType.Pistola },
        { "Subfusil", WeaponType.Subfusil },
        { "Escopeta", WeaponType.Escopeta },
        { "Cuchillo", WeaponType.Cuchillo },
        { "Bate", WeaponType.Bate },
        { "Katana", WeaponType.Katana }
    };

    public static EnemyType GetEnemyType(string name)
    {
        if (string.IsNullOrEmpty(name)) return EnemyType.None;
        if (enemyTypeMap.TryGetValue(name, out EnemyType type)) return type;
        return EnemyType.None;
    }

    public static WeaponType GetWeaponType(string name)
    {
        if (string.IsNullOrEmpty(name)) return WeaponType.None;
        if (weaponTypeMap.TryGetValue(name, out WeaponType type)) return type;
        return WeaponType.None;
    }
}
