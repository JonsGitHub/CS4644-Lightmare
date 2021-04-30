using UnityEngine;

public class FragmentController : InterfaceBase
{
    [SerializeField] private PlayerData.Crystal _type;

    [SerializeField] private InterfaceBaseEventChannelSO _removeInteraction = default;
    [SerializeField] private BoolEventChannelSO _collectedEventChannel = default;

    public void Gain()
    {
        PlayerData.GainCrystal(_type);
        PlayerData.Save(); // Might move?
        _removeInteraction?.RaiseEvent(this);

        _collectedEventChannel?.RaiseEvent(true);

        // Insert some form of playback here

        Destroy(gameObject);
    }

    public override void Interact() => Gain();
}
