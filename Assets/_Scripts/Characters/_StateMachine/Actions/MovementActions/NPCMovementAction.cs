public abstract class NPCMovementAction
{
	public abstract void OnUpdate();

	public abstract void OnStateEnter();

	public abstract void OnStateExit();

	public abstract bool HasNextAction { get; }
}