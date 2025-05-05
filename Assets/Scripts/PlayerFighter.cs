using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : Fighter
{
    [Header("UI")]
    public PlayerSkillPanel skillPanel;

    [Header("Player Stats")]
    public int level = 1; // Valor predeterminado del nivel

    public int maxHealth = 60; // Puedes agregar otras stats aquí si no las hereda de una base
    public int attack = 25;
    public int defense = 15;
    public int speed = 7;

    void Awake()
    {
        this.stats = new Stats(level, maxHealth, attack, defense, speed);
    }

    public override void InitTurn()
    {
        this.skillPanel.Show();
        
        for (int i = 0; i < this.skills.Length; i++)
        {
            this.skillPanel.ConfigureButton(i, this.skills[i].skillName);
        }
    }

    /// <summary>
    /// Se llama desde los botones del panel de habilidades.
    /// </summary>
    /// <param name="index"></param>
    public void ExecuteSkill(int index)
    {
        this.skillPanel.Hide();

        Skill skill = this.skills[index];

        skill.SetEmitterAndReceiver(
            this,
            this.combatManager.GetOpposingFighter()
        );

        this.combatManager.OnFighterSkill(skill);
    }
}
