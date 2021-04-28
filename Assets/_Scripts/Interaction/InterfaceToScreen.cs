using UnityEngine;

public class InterfaceToScreen : InterfaceBase
{
    [SerializeField] private GameScreen _screen;
    [SerializeField] private GameScreenEventChannelSO _gameScreenEvent = default;

    public override void Interact()
    {
        _gameScreenEvent.RaiseEvent(_screen);
    }
}
