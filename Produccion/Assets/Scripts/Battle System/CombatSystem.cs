using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{
    public int CalculateMeleeDamage(BattleCharacters attacker, BattleCharacters defender)
    {
        float attackPower = attacker.Strength + attacker.MeleeWeaponDamage;
        float defenceAmount = defender.Defence;
        float damageAmount = (attackPower - defenceAmount) * Random.Range(0.8f, 1.2f);

        int damageToGive = Mathf.Max(0, (int)damageAmount);
        return CalculateCritical(damageToGive);
    }

    public int CalculateRangeDamage(BattleCharacters attacker, BattleCharacters defender)
    {
        float attackPower = attacker.Dexterity + attacker.RangeWeaponDamage;
        float defenceAmount = defender.Defence;
        float damageAmount = (attackPower - defenceAmount) * Random.Range(0.8f, 1.2f);

        int damageToGive = Mathf.Max(0, (int)damageAmount);
        return CalculateCritical(damageToGive);
    }

    private int CalculateCritical(int damageToGive)
    {
        if (Random.value <= 0.1f)
        {
            return damageToGive * 2;
        }
        return damageToGive;
    }

    public AttackType DecideEnemyAttack(BattleCharacters enemy)
    {
        if (enemy.AvailableAttacks == null || enemy.AvailableAttacks.Length == 0)
            return AttackType.Melee;

        if (enemy.AvailableAttacks.Length == 3)
        {
            int n = Random.Range(1, 10);
            if (n == 9) return AttackType.Heal;
            if (n >= 5) return AttackType.Range;
            return AttackType.Melee;
        }
        else if (enemy.AvailableAttacks.Length == 2)
        {
            int i = Random.Range(1, 10);
            if (i <= 5) return AttackType.Range;
            return AttackType.Melee;
        }
        else
        {
            return enemy.AvailableAttacks[0];
        }
    }

    public int SelectRandomPlayerTarget(List<BattleCharacters> activeCharacters)
    {
        List<int> players = new List<int>();
        for (int n = 0; n < activeCharacters.Count; n++)
        {
            if (activeCharacters[n].IsPlayer && activeCharacters[n].CurrentHealth > 0)
            {
                players.Add(n);
            }
        }
        
        if (players.Count == 0) return -1;
        return players[Random.Range(0, players.Count)];
    }
}
