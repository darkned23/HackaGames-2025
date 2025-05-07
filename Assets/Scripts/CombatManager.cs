using UnityEngine;
using System.Collections;
public enum CombatStatus
{
    WAITING_FOR_FIGHTER,
    FIGHTER_ACTION,
    CHECK_ACTION_MESSAGES,
    CHECK_FOR_VICTORY,
    NEXT_TURN
}

public class CombatManager : MonoBehaviour
{
    public Fighter[] fighters;
    private int fighterIndex;

    private bool isCombatActive;

    private CombatStatus combatStatus;

    private Skill currentFighterSkill;

    public Animator SpriteAnim;
    public RuntimeAnimatorController AnimatorController;

    public SceneController sceneController;

    void Start()
    {
        LogPanel.Write("Battle initiated.");

        foreach (var fgtr in this.fighters)
        {
            fgtr.combatManager = this;
        }

        this.combatStatus = CombatStatus.NEXT_TURN;

        this.fighterIndex = -1;
        this.isCombatActive = true;
        StartCoroutine(this.CombatLoop());
    }

    IEnumerator CombatLoop()
    {
        while (this.isCombatActive)
        {
            switch (this.combatStatus)
            {
                case CombatStatus.WAITING_FOR_FIGHTER:
                    yield return null;
                    break;

                case CombatStatus.FIGHTER_ACTION:
                    LogPanel.Write($"{this.fighters[this.fighterIndex].idName} eligi� {currentFighterSkill.skillName}.");

                    yield return null;

                    // Executing fighter skill
                    currentFighterSkill.Run();

                    // Wait for fighter skill animation
                    yield return new WaitForSeconds(currentFighterSkill.animationDuration);
                    this.combatStatus = CombatStatus.CHECK_ACTION_MESSAGES;

                    break;
                case CombatStatus.CHECK_ACTION_MESSAGES:
                    string nextMessage = this.currentFighterSkill.GetNextMessage();

                    if (nextMessage != null)
                    {
                        LogPanel.Write(nextMessage);
                        yield return new WaitForSeconds(2f);
                    }
                    else
                    {
                        this.currentFighterSkill = null;
                        this.combatStatus = CombatStatus.CHECK_FOR_VICTORY;
                        yield return null;
                    }
                    break;
                case CombatStatus.CHECK_FOR_VICTORY:
                    bool someoneDied = false;
                    foreach (var fgtr in this.fighters)
                    {
                        if (fgtr.isAlive == false)
                        {
                            someoneDied = true;
                            break; // No necesitamos seguir verificando si alguien ya muri�
                        }
                    }

                    if (someoneDied)
                    {
                        this.isCombatActive = false;
                        LogPanel.Write("¡VICTORIA!");

                        // Cargar la escena de victoria
                        if (this.fighterIndex == 0)
                        {
                            sceneController.LoadSceneByIndex(6);
                        }
                        else
                        {
                            sceneController.LoadSceneByIndex(7);
                        }
                    }
                    else
                    {
                        this.combatStatus = CombatStatus.NEXT_TURN;
                    }
                    yield return null;
                    break;

                case CombatStatus.NEXT_TURN:

                    yield return new WaitForSeconds(1f);
                    this.fighterIndex = (this.fighterIndex + 1) % this.fighters.Length;

                    var currentTurn = this.fighters[this.fighterIndex];
                    LogPanel.Write($"{currentTurn.idName} tiene el turno");
                    currentTurn.InitTurn();

                    if (SpriteAnim != null)
                    {
                        SpriteAnim.runtimeAnimatorController = AnimatorController;
                    }


                    this.combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
                    break;
            }
        }
    }


    public Fighter GetOpposingFighter()
    {
        if (this.fighterIndex == 0)
        {
            return this.fighters[1];
        }
        else
        {
            return this.fighters[0];
        }
    }

    public void OnFighterSkill(Skill skill)
    {
        this.currentFighterSkill = skill;
        this.combatStatus = CombatStatus.FIGHTER_ACTION;
    }


}
