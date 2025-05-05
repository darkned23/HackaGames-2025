using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public string idName;
    public StatusPanel statusPanel;
    public List<StatusMod> statusMods;
    public CombatManager combatManager;
    protected Skill[] skills;
    protected Stats stats;

    public bool isAlive
    {
        get => this.stats.health > 0;
    }
    protected virtual void Start()
    {
        this.statusPanel.SetStats(this.idName, this.stats);
        this.skills = this.GetComponentsInChildren<Skill>();

        this.statusMods = new List<StatusMod>();
    }

    public void ModifyHealth(float amount)
    {
        this.stats.health = Mathf.Clamp(this.stats.health + amount, 0f, this.stats.maxHealth);

        this.stats.health = Mathf.Round(this.stats.health);
        this.statusPanel.SetHealth(this.stats.health, this.stats.maxHealth);

        /*if (this.isAlive == false)
        {
            Invoke("Die", 0.75f);
        }*/
    }

    public Stats GetCurrentStats()
    {
        Stats modedStats = this.stats;

        foreach (var mod in this.statusMods)
        {
            modedStats = mod.Apply(modedStats);
        }

        return modedStats;
    }
    public abstract void InitTurn();
    
}
