using System;
using System.Collections.Generic;
using Optional;
using UnityEngine;

namespace MultiState
{
    public class StateMachine
    {
        private const int MaxTransitionDepth = 100;
        
        private Option<IState> _currentState;
        private Dictionary<IState, List<Transition>> _transitions = new();
        private Option<List<Transition>> _currentTransitions = Option<List<Transition>>.None();
        private List<Transition> _anyTransitions = new();

        public void Tick()
        {
            var transition = GetTransition();
            int i = 0;
            while (transition.IsSome())
            {
                transition.Map(t => t.To).Apply(SetState);
                transition = GetTransition();
                if (++i > MaxTransitionDepth) throw new StackOverflowException("StateMachine recursion depth reached! You probably have a cycle");
            }

            _currentState.Apply(s => s.Tick());
        }

        public void SetState(IState state)
        {
            if (_currentState.Filter(current => current == state).IsSome()) return;

            _currentState.Apply(x => x.OnExit());
            
            _currentState = Option<IState>.Wrap(state);

            _currentTransitions = _currentState.Bind(x => _transitions.GetValue(x));
            _currentState.Apply(x => x.OnEnter());
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            _transitions.GetValue(from).OrElse(
                () => {
                    _transitions[from] = new List<Transition>();
                    return Option<List<Transition>>.Some(_transitions[from]);
                }
            ).Apply(x => x.Add(new Transition(to, condition)));
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            _anyTransitions.Add(new Transition(to, condition));
        }

        private Option<Transition> GetTransition()
        {
            return Option<List<Transition>>
                .Some(_anyTransitions)
                .Map(x => x.Find(t => t.Condition()))
                .Or(_currentTransitions.Map(x => x.Find(t => t.Condition())))
                .Filter(t => _currentState.Filter(curr => curr == t.To).IsNone());
        }
        
        private record Transition
        {
            public IState To;
            public Func<bool> Condition;

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }
    }
}