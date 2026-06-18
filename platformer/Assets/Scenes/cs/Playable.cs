using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Playable : MonoBehaviour
{
    // =================================================================
    // 1. VARIABLES (변수 선언부 - 기능별/접근제한자별 그룹화)
    // =================================================================

    [Header("Components")]
    private Rigidbody2D rb;
    public float gravityScale = 1.0f;

    private enum ActionType { none, jump, roll };
    [Header("Input Actions")]
    public float actionDelay = 0.2f;
    public InputAction runLeft;
    public InputAction runRight;
    public InputAction jump;
    public InputAction roll;

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


    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 boxSize = new(0.5f, 0.1f);
    public LayerMask groundLayer;

    [Header("State Monitor (Debug)")]
    [SerializeField] private int FacingDirection = 1;
    [SerializeField] private bool isGround = true;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isAction = false;
    [SerializeField] private ActionType actionQueue;

    private Dictionary<string, float> timers = new()
    {
        { "queue", 0f },
        { "action", 0f },
        { "jump", 0f },
        { "roll", 0f },
        { "Cooldown_roll", 0f }
    };



    // =================================================================
    // 2. UNITY LIFECYCLE METHODS (유니티 생명주기 함수 - 실행 순서 정렬)
    // =================================================================

    private void OnEnable()
    {

        jump.started += ctx => EnQueueAction(ActionType.jump);
        roll.started += ctx => EnQueueAction(ActionType.roll);

        jump.canceled += ctx =>
        {
            jumpStop = true;
        };


        runLeft.Enable();
        runRight.Enable();
        jump.Enable();
        roll.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 수학 연산 캐싱
        jumpSpeedY = Mathf.Sqrt(-2f * Physics2D.gravity.y * gravityScale * jumpDY);
        rollSpeedX = (rollDX / rollDuration) * 2f;
        rollSpeedY = Mathf.Sqrt(-2f * Physics2D.gravity.y * gravityScale * rollDY);
    }

    private void FixedUpdate()
    {
        UpdateTimer();
        if (timers["queue"] == 0f) actionQueue = ActionType.none;
        if (timers["jump"] == 0f && jumpStop)
        {
            jumpStop = false;
            if (rb.linearVelocity.y > 0f) rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / 3f);
        }
        Vector2 currVelocity = rb.linearVelocity;

        // 1. 상태 업데이트 (State Update)
        isGround = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer) != null;
        isMoving = currVelocity.sqrMagnitude > 0.01f;

        float accelRate;
        if (timers["roll"] > 0f)
        {
            if (timers["roll"] > rollDuration * 0.5f)
            {
                MoveDirectionX = FacingDirection * rollSpeedX;
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
            // 2. 달리기 입력 처리 (Horizontal Input)
            MoveDirectionX = 0f;
            if (runLeft.IsPressed()) MoveDirectionX -= runSpeedX;
            if (runRight.IsPressed()) MoveDirectionX += runSpeedX;
            // 3. X축 속도 갱신 (가속 및 감속)
            accelRate = (MoveDirectionX != 0f) ? acceleration : deceleration;
            if (currVelocity.x * MoveDirectionX < 0f) accelRate *= 2f;
        }
        FlipSprite(MoveDirectionX);
        currVelocity.x = Mathf.MoveTowards(currVelocity.x, MoveDirectionX, accelRate * Time.fixedDeltaTime);



        if (isGround)
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
                    if (!jump.IsPressed()) jumpStop = true;
                    actionQueue = ActionType.none;
                }
                break;
            case ActionType.roll:
                if (CanDoAction(ActionType.roll))
                {
                    if (!isGround)
                    {
                        currVelocity.y = rollSpeedY;
                        currVelocity.x += FacingDirection * rollDX / 5;
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



        // 5. 최종 속도 적용
        rb.linearVelocity = currVelocity;
    }

    private void OnDisable()
    {

        jump.Disable();
        runLeft.Disable();
        runRight.Disable();
        roll.Disable();
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, boxSize);
        }
    }

    // =================================================================
    // 3. CUSTOM METHODS & COROUTINES (커스텀 함수 및 코루틴 부)
    // =================================================================




    private IEnumerator DelayedToggle(Action toggle, float duration, Action afterAction = null)
    {
        toggle();
        yield return new WaitForSeconds(duration);
        toggle();
        afterAction();
    }
    private void FlipSprite(float direction)
    {
        int sign = Math.Sign(direction);
        if (sign == 0 || (FacingDirection * sign == 1)) { return; }
        FacingDirection = sign;
        Vector2 currentScale = transform.localScale;
        currentScale.x = Mathf.Abs(currentScale.x) * sign;
        transform.localScale = currentScale;
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
        if (isAction) return false;

        if (action == ActionType.jump)
        {
            if (isGround) return true;
            return false;
        }
        if (action == ActionType.roll)
        {
            if (timers["Cooldown_roll"] == 0f)
            {
                if (isGround || !isAirRolled) return true;
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