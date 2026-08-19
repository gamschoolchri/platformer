using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Entity))]
public class Playable : MonoBehaviour
{


    [Header("Components")]
    private Rigidbody2D rb;
    private Entity ent;
    public float gravityScale = 1.0f;

    private enum ActionType { none, jump, roll, skill_1, skill_2, skill_3, skill_4 };
    public float actionDelay = 0.2f;

    private Command commands;

    [Header("Running Settings")]
    public float runSpeedX = 5.0f;
    public float acceleration = 50.0f;
    public float deceleration = 30.0f;
    private float MoveDirectionX = 0.0f;

    [Header("Jumping Settings")]
    public float jumpDY = 6.0f;

    public float jumpStopDelay = 0.2f;
    private bool jumpStop = false;

    private float jumpSpeedY;

    [Header("Rolling Settings")]
    public float rollCooldown = 1.0f;
    public float rollDuration = 0.8f;
    public float rollDX = 6.0f;
    public float rollDY = 2.0f;
    private float rollSpeedX;
    private float rollSpeedY;
    private bool isAirRolled = false;


    [Header("State Monitor (Debug)")]
    [SerializeField] private ActionType actionQueue;

    private Dictionary<string, float> timers = new()
    {
        { "queue", 0f },
        { "action", 0f },
        { "jump", 0f },
        { "roll", 0f },
        { "Cooldown_roll", 0f }
    };

    public GameObject hitBox;




    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ent = GetComponent<Entity>();

        jumpSpeedY = Mathf.Sqrt(-2f * Physics2D.gravity.y * gravityScale * jumpDY);
        rollSpeedX = (rollDX / rollDuration) * 2f;
        rollSpeedY = Mathf.Sqrt(-2f * Physics2D.gravity.y * gravityScale * rollDY);
        commands ??= new Command();
    }

    void OnEnable()
    {
        commands ??= new Command();
        commands.Player.Jump.started += ctx => EnQueueAction(ActionType.jump);
        commands.Player.Roll.started += ctx => EnQueueAction(ActionType.roll);

        commands.Player.Jump.canceled += ctx => jumpStop = true;
        commands.Player.Skills.started += ctx =>
        {
            int skillIndex = (int)ctx.ReadValue<float>() - 1;
            SkillManager.Instance.SpawnSkill(gameObject, "Skill_001", ent.facing);
        };

        commands.Enable();
    }


    void FixedUpdate()
    {
        UpdateTimer();
        if (timers["queue"] == 0f) actionQueue = ActionType.none;
        if (timers["jump"] == 0f && jumpStop)
        {
            jumpStop = false;
            if (rb.linearVelocity.y > 0f) rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / 3f);
        }
        Vector2 currVelocity = rb.linearVelocity;


        float accelRate;
        if (timers["roll"] > 0f)
        {
            if (timers["roll"] > rollDuration * 0.5f)
            {
                MoveDirectionX = ent.facing * rollSpeedX;
                accelRate = acceleration;
            }
            else
            {
                MoveDirectionX = 0f;
                accelRate = deceleration;
            }
        }
        else
        {
            MoveDirectionX = runSpeedX * commands.Player.Move.ReadValue<Vector2>().x;
            accelRate = (MoveDirectionX != 0f) ? acceleration : deceleration;
            if (currVelocity.x * MoveDirectionX < 0f) accelRate *= 2f;
        }


        currVelocity.x = Mathf.MoveTowards(currVelocity.x, MoveDirectionX, accelRate * Time.fixedDeltaTime);



        if (ent.isfloor)
        {
            isAirRolled = false;
        }
        else
        {
            float gravityMultiplier = (currVelocity.y < 0f) ? 2f : 1f;
            currVelocity.y += Physics2D.gravity.y * gravityScale * gravityMultiplier * Time.fixedDeltaTime;

        }

        switch (actionQueue)
        {
            case ActionType.jump:
                if (CanDoAction(ActionType.jump))
                {
                    currVelocity.y = jumpSpeedY;


                    timers["roll"] = 0f;
                    timers["jump"] = jumpStopDelay;
                    timers["action"] = actionDelay;
                    if (!commands.Player.Jump.IsPressed()) jumpStop = true;
                    actionQueue = ActionType.none;
                }
                break;
            case ActionType.roll:
                if (CanDoAction(ActionType.roll))
                {
                    if (!ent.isfloor)
                    {
                        currVelocity.y = rollSpeedY;
                        currVelocity.x += ent.facing * rollDX / 5;
                        isAirRolled = true;
                    }

                    timers["roll"] = rollDuration;
                    timers["Cooldown_roll"] = rollCooldown;
                    timers["action"] = actionDelay;

                    actionQueue = ActionType.none;
                }
                break;
            default:
                break;
        }



        rb.linearVelocity = currVelocity;
    }

    void OnDisable()
    {
        commands.Disable();
    }




    private void UpdateTimer()
    {
        List<string> keys = new(timers.Keys);
        foreach (string key in keys)
        {
            timers[key] -= Time.fixedDeltaTime;
            if (timers[key] < 0f) timers[key] = 0f;
        }
    }
    private bool CanDoAction(ActionType action)
    {
        if (ent.isAction) return false;

        if (action == ActionType.jump)
        {
            if (ent.isfloor) return true;
            return false;
        }
        if (action == ActionType.roll)
        {
            if (timers["Cooldown_roll"] == 0f)
            {
                if (ent.isfloor || !isAirRolled) return true;
            }
            return false;
        }

        return false;
    }
    private void EnQueueAction(ActionType action)
    {
        if (actionQueue != ActionType.none) return;
        actionQueue = action;
        timers["queue"] = actionDelay;
    }

}



