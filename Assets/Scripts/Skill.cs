using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header("Base Skill")]
    public string skillName;
    public float animationDuration;

    public bool selfInflicted;

    public Animator SpriteAnim;
    public RuntimeAnimatorController AnimatorController;

    protected Fighter emitter;
    protected Fighter receiver;

    protected Queue<string> messages;
    private void Awake()
    {
        this.messages = new Queue<string>();
    }
    private void Animate()
    {
        if (SpriteAnim == null) return;

        SpriteAnim.runtimeAnimatorController = AnimatorController;
    }

    public void Run()
    {
        if (this.selfInflicted)
        {
            this.receiver = this.emitter;
        }

        this.Animate();

        this.OnRun();
    }

    public void SetEmitterAndReceiver(Fighter _emitter, Fighter _receiver)
    {
        this.emitter = _emitter;
        this.receiver = _receiver;
    }

    public string GetNextMessage()
    {
        if (this.messages.Count != 0) 
            return this.messages.Dequeue();

        else
            return null;
        
    }
    protected abstract void OnRun();
}
