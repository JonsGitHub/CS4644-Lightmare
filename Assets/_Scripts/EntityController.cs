using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

/// <summary>
/// Abstract Entity Controller class containing base methods
/// for all npc based entities.
/// </summary>
public abstract class EntityController : MonoBehaviour
{
    [Tooltip("Movement Speed."), Range(0.1f, 10f)]
    public float Speed = 4.0f;
    public float StoppingDistance = 2.0f;
    public float TurnSmoothing = 0.1f;

    protected NavMeshAgent Agent;

    private float SmoothVelocity;

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    protected void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(Agent, "EntityController must contain a NavMeshAgent Component.");

        Agent.speed = Speed;
        Agent.stoppingDistance = StoppingDistance;
        Agent.updateRotation = true;
    }

    /// <summary>
    /// Moves the entity towards the passed target position.
    /// </summary>
    /// <param name="target">The target destination</param>
    protected void MoveTo(Vector3 target)
    {
        Agent.destination = target;
    }

    /// <summary>
    /// Rotates the entity towards the passed target position
    /// </summary>
    /// <param name="target">The position to rotate towards</param>
    protected void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref SmoothVelocity, TurnSmoothing);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}