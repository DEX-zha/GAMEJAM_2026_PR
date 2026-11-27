
public class EntityStateMachine
{
    private IState _currentState;

    public void TransitionTo(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Tick(float dt) => _currentState?.Tick(dt);
}
