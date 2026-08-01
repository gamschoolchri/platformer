using UnityEngine;

public class Animations : StateMachineBehaviour
{
    public delegate void AnimationCallback();
    private AnimationCallback Callback;

    public void SetCallback(AnimationCallback callback)
    {
        Debug.Log(20);
        Callback = callback;
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(30);
        Callback?.Invoke();

        Callback -= Callback;
    }
}
