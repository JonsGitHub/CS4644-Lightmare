using UnityEngine;

public enum StaticPose
{
    Idle_1,
    Idle_2,
    Idle_3,
    Sitting_1,
    Sitting_2,
    Sitting_3,
    Sitting_4,
    Sitting_5,
    Drunk_Idle_1,
    Drunk_Idle_2,
    Talking_1,
    Talking_2,
    Yelling_1,
    Farming_1,
}

public class StaticPoseMachine : MonoBehaviour
{
    [SerializeField] private StaticPose _pose = StaticPose.Idle_1;

    private void Awake()
    {
        if (TryGetComponent(out Animator animator))
        {
            if (_pose == 0)
                return;

            animator.Play(_pose.ToString());
        }
    }
}
