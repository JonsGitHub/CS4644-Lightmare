using UnityEngine;

/// <summary>
/// Entity Physics class containing behavior for entities
/// that can interacte with rigidbodies.
/// </summary>
public class EntityPhysics : MonoBehaviour
{
    [Tooltip("Amount of pushing force."), Range(0.1f, 20f)]
    public float PushForce = 2.0f;

    /// <summary>
    /// On Collider Hit callback method.
    /// </summary>
    /// <param name="hit">The collider that has interacted</param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.attachedRigidbody is Rigidbody body)
        {
            if (body.isKinematic || hit.moveDirection.y < -0.1f)
            {
                return;
            }
            // Push Direction * Push Force
            body.velocity = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * PushForce;
        }
    }
}
