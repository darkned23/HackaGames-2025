using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthModType
{
    STAT_BASED, FIXED, PERCENTAGE
}
public class HealthModSkill : Skill
{
    [Header("Health Mod")]
    public float amount;

    public HealthModType modType;
    [Range(0f, 1f)]
    public float critChance = 0;
    protected override void OnRun()
    {
        float amount = this.GetModification();
        float dice = Random.Range(0f, 1f);

        if (dice <= this.critChance)
        {
            amount *= 2f;
            this.messages.Enqueue("da�o cr�tico");
        }

        this.receiver.ModifyHealth(amount);
    }

    public float GetModification()
    {
        switch (this.modType)
        {
            case HealthModType.STAT_BASED:
                Stats emitterStats = this.emitter.GetCurrentStats();
                Stats receiverStats = this.receiver.GetCurrentStats();

                // F�rmula: https://bulbapedia.bulbagarden.net/wiki/Damage
                if (receiverStats.deffense <= 0)
                {
                    receiverStats.deffense = 1f; // Evitar que la defensa sea 0
                }
                float rawDamage = (((2 * emitterStats.level) / 5f) + 2) * this.amount * (emitterStats.attack / receiverStats.deffense);
                Debug.Log("rawDamage: " + rawDamage);

                return (rawDamage / 50f) + 2;

            case HealthModType.FIXED:
                return this.amount;

            case HealthModType.PERCENTAGE:
                Stats rStats = this.receiver.GetCurrentStats();
                return rStats.maxHealth * this.amount;
        }

        throw new System.InvalidOperationException("HealthModSkill::GetDamage. Unreachable!");
    }
}

