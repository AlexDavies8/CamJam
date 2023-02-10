using System;
using MultiState;
using Optional;

public class ActionState : IState
{
    private readonly Option<Action> _onTick = Option<Action>.None();
    private readonly Option<Action> _onEnter = Option<Action>.None();
    private readonly Option<Action> _onExit = Option<Action>.None();

    public ActionState OnTick(Action onTick) => new(_onTick.Bind(Then(onTick)).OrWrap(onTick), _onEnter, _onExit);
    public ActionState OnEnter(Action onEnter) => new(_onTick, _onEnter.Bind(Then(onEnter)).OrWrap(onEnter), _onExit);
    public ActionState OnExit(Action onExit) => new(_onTick, _onEnter, _onExit.Bind(Then(onExit)).OrWrap(onExit));

    public ActionState() {}
    
    private ActionState(Option<Action> onTick, Option<Action> onEnter, Option<Action> onExit)
    {
        _onTick = onTick;
        _onEnter = onEnter;
        _onExit = onExit;
    }

    public void Tick()
    {
        _onTick.Apply(f => f());
    }

    public void OnEnter()
    {
        _onEnter.Apply(f => f());
    }

    public void OnExit()
    {
        _onExit.Apply(f => f());
    }
    
    private static Func<Action, Option<Action>> Then(Action then) => first => {
            return Option<Action>.Some(() => {
                first();
                then();
            });
        };
}
