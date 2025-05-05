using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : Fighter
{
    [Header("Enemy Stats")]
    public int maxHealth = 50;
    public int attack = 20;
    public int defense = 10;
    public int speed = 5;
    public int level = 1;

    void Awake()
    {
        this.stats = new Stats(level, maxHealth, attack, defense, speed);
    }

    public override void InitTurn()
    {
        StartCoroutine(this.IA());
    }

    IEnumerator IA()
    {
        yield return new WaitForSeconds(1f);

        Skill skill = this.skills[Random.Range(0, this.skills.Length)];

        skill.SetEmitterAndReceiver(
            this,
            this.combatManager.GetOpposingFighter()
        );

        this.combatManager.OnFighterSkill(skill);
    }
}
