using UnityEngine;

public class EntityPhysics : MonoBehaviour
{
    public float PushForce = 2.0f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic || hit.moveDirection.y < -0.1f)
            return;
                        // Push Direction
        body.velocity = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * PushForce;
    }
}
