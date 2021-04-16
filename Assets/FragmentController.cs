using UnityEngine;

public class FragmentController : MonoBehaviour
{
    [SerializeField] private PlayerData.Crystal _type;

    public void Gain()
    {
        PlayerData.GainCrystal(_type);
        PlayerData.Save(); // Might move?
        Destroy(gameObject);
    }
}
