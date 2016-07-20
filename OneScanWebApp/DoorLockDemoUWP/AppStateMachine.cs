
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoorLockDemoUWP
{
    public class AppStateMachine
    {
        public enum State
        {
            LOADING,
            LOCKED,
            REQUESTING_QR,
            QR_DISPLAY,
            SCANNING,
            UNLOCKED,
            CANCELLING
        }
        public enum Event
        {
            LOADED,
            QR_REQUEST,
            GOT_QR,
            QR_SCANNED,
            SCAN_DENIED,
            SCAN_ACCEPTED,
            CANCEL_REQUEST,
            CANCELLED,
            RESET
        }

        public static readonly Dictionary<State, Dictionary<Event, State>> StateTransitions = new Dictionary<State, Dictionary<Event, State>>()
        {
             {
                State.LOADING, new Dictionary<Event, State>()
                { {Event.LOADED, State.LOCKED } }
            },
            {
                State.LOCKED, new Dictionary<Event, State>()
                { {Event.QR_REQUEST, State.REQUESTING_QR } }
            },
            {
                State.REQUESTING_QR, new Dictionary<Event, State>()
                { {Event.GOT_QR, State.QR_DISPLAY }, {Event.CANCEL_REQUEST, State.CANCELLING } }
            },
            {
                State.QR_DISPLAY, new Dictionary<Event, State>()
                { {Event.QR_SCANNED, State.SCANNING }, {Event.SCAN_ACCEPTED, State.UNLOCKED }, {Event.SCAN_DENIED, State.LOCKED } }
            },
            {
                State.SCANNING, new Dictionary<Event, State>()
                { {Event.SCAN_ACCEPTED, State.UNLOCKED }, {Event.SCAN_DENIED, State.LOCKED }, {Event.CANCEL_REQUEST, State.CANCELLING } }
            },
            {
                State.UNLOCKED, new Dictionary<Event, State>()
                { {Event.RESET, State.LOCKED } }
            },
            {
                State.CANCELLING, new Dictionary<Event, State>()
                { {Event.CANCELLED, State.LOCKED } }
            },
        };

        

        private State currentState = State.LOADING;
        private Dictionary<State, Action> stateInvokableMethods = new Dictionary<State, Action>();

        public void setStateCallback(State state, Action callback)
        {
            if (!stateInvokableMethods.ContainsKey(state))
                stateInvokableMethods.Add(state, callback);
        }

        public void doEvent(Event _event) {
            State nextState;
            if (currentState.GetNextState(_event, out nextState))
            {
                Action action;
                if (stateInvokableMethods.TryGetValue(nextState, out action))
                {
                    action.Invoke();
                    currentState = nextState;
                }

                
            }
        }

        public void start()
        {
            Action action;
            if (stateInvokableMethods.TryGetValue(currentState, out action))
                action.Invoke();
        }
    }

    public static class StateEnumExtentions
    {
        public static bool GetNextState(this AppStateMachine.State state, AppStateMachine.Event _event, out AppStateMachine.State nextState)
        {
            nextState = state;
            if (AppStateMachine.StateTransitions[state].TryGetValue(_event, out nextState))
                return true;

            return false;
        }
    }
}
