using UnityEngine;

public static class Movement
{
    /// <summary>
    /// 이동
    /// </summary>
    /// <param name="target">움직일 오브젝트</param>
    /// <param name="direction">이동 방향</param>
    /// <param name="currentSpeed">현재 속도</param>
    /// <param name="maxSpeed">최고 속도</param>
    /// <param name="acceleration">가속도</param>
    /// <param name="deceleration">감속도</param>
    public static float Move(Transform target, Vector3 direction, float currentSpeed, float maxSpeed, float acceleration, float deceleration)
    {
        if (direction.magnitude == 0 && currentSpeed == 0) { return currentSpeed; }
        if (direction.magnitude > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        if (currentSpeed > 0)
        {
            // 이동할 방향 결정 (입력이 끊겼을 때는 멈추는 중이므로 오브젝트가 바라보는 방향이나 마지막 방향 유지)
            Vector3 moveDir = direction.magnitude > 0 ? direction.normalized : target.forward;
            target.Translate(moveDir * currentSpeed * Time.deltaTime, Space.World);
        }

        // 3. 변화된 현재 속도를 스크립트가 기억할 수 있도록 돌려줍니다.
        return currentSpeed;
    }
}
