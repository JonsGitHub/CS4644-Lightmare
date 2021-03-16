using UnityEngine;

/// <summary>
/// Ui3D abstract base class representing all 3d Uis.
/// </summary>
public abstract class Ui3D : MonoBehaviour
{
    private const int MAX_Z_DIST = 50;
    private RectTransform rectTransform;

    public Transform Transform;

    /// <summary>
    /// Awake called before Start of class
    /// </summary>
    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Attaches the Ui3D to the passed parent.
    /// </summary>
    /// <param name="parent">The parent to attach to</param>
    public void AttachTo(Transform parent)
    {
        transform.SetParent(parent);
        transform.SetAsFirstSibling();
    }

    /// <summary>
    /// Updates the Ui3D position on the screen.
    /// </summary>
    public void UpdateUI()
    {
        // Look into better way then double calculation
        var screenPoint = Camera.main.WorldToViewportPoint(Transform.position);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            var position = Camera.main.WorldToScreenPoint(Transform.position);
            gameObject.SetActive(true);
            rectTransform.position = position;
            rectTransform.localScale = Vector3.one * (1 - (position.z / MAX_Z_DIST));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Destroys the game object
    /// </summary>
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
