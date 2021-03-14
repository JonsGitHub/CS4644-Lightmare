using UnityEngine;

/// <summary>
/// Enemy controller outlining the behaviors relating
/// to a basic enemy.
/// </summary>
public class EnemyController : EntityController
{
    public Transform Target;
    private Animator Animator;

    /// <summary>
    /// Start method called before first update.
    /// </summary>
    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();

        Agent.speed = Speed;
        Agent.stoppingDistance = StoppingDistance;
        Agent.updateRotation = true;
    }

    /// <summary>
    /// Update called every physics frame
    /// </summary>
    private void FixedUpdate()
    {
        if (Target)
        {
            // TODO: Move this Distance check to async timer check
            if (Vector3.Distance(Target.position, transform.position) > StoppingDistance)
            {
                MoveTo(Target.position);
            }
            else
            {
                RotateTowards(Target.position);
            }
        }

        var magnitude = Agent.velocity.magnitude;
        if (magnitude >= 0.1f)
        {
            Animator.SetFloat("velocity", magnitude);
        }
        else
        {
            Animator.SetFloat("velocity", -1.0f);
        }
    }
}
