using UnityEngine;

namespace MultiState
{
    public class SubMachineState : IState
    {
        private readonly StateMachine _stateMachine;

        public SubMachineState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
    
        public void Tick()
        {
            _stateMachine.Tick();
        }

        public void OnEnter()
        {
            _stateMachine.Enter();
        }

        public void OnExit()
        {
            _stateMachine.Exit();
        }
    }
}
