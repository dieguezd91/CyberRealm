using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{
    public int CalculateMeleeDamage(BattleCharacters attacker, BattleCharacters defender)
    {
        float attackPower = attacker.strength + attacker.meleeWeaponDamage;
        float defenceAmount = defender.defence;
        float damageAmount = (attackPower - defenceAmount) * Random.Range(0.8f, 1.2f);

        int damageToGive = Mathf.Max(0, (int)damageAmount);
        return CalculateCritical(damageToGive);
    }

    public int CalculateRangeDamage(BattleCharacters attacker, BattleCharacters defender)
    {
        float attackPower = attacker.dexterity + attacker.rangeWeaponDamage;
        float defenceAmount = defender.defence;
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
        if (enemy.availableAttacks == null || enemy.availableAttacks.Length == 0)
            return AttackType.Melee;

        if (enemy.availableAttacks.Length == 3)
        {
            int n = Random.Range(1, 10);
            if (n == 9) return AttackType.Heal;
            if (n >= 5) return AttackType.Range;
            return AttackType.Melee;
        }
        else if (enemy.availableAttacks.Length == 2)
        {
            int i = Random.Range(1, 10);
            if (i <= 5) return AttackType.Range;
            return AttackType.Melee;
        }
        else
        {
            return enemy.availableAttacks[0];
        }
    }

    public int SelectRandomPlayerTarget(List<BattleCharacters> activeCharacters)
    {
        List<int> players = new List<int>();
        for (int n = 0; n < activeCharacters.Count; n++)
        {
            if (activeCharacters[n].IsPlayer() && activeCharacters[n].currentHP > 0)
            {
                players.Add(n);
            }
        }
        
        if (players.Count == 0) return -1;
        return players[Random.Range(0, players.Count)];
    }
}
