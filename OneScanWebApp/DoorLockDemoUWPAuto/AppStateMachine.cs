
using System;
using System.Collections.Generic;

namespace DoorLockDemoUWPAuto
{
    public class AppStateMachine
    {
        public enum State
        {
           FETCHING_QR,
           DISPLAYING_QR,
           SCANNING,
           SUCCESS,
           FAILED,
           RESETTING
        }
        public enum Event
        {
            QR_RECIEVED,
            QR_SCANNED,
            SCAN_ACCEPTED,
            SCAN_DENIED,
            START_RESET,
            RESET
        }

        public static readonly Dictionary<State, Dictionary<Event, State>> StateTransitions = new Dictionary<State, Dictionary<Event, State>>()
        {
            {
                State.FETCHING_QR, new Dictionary<Event, State>()
                { {Event.QR_RECIEVED, State.DISPLAYING_QR } }
            },
            {
                State.DISPLAYING_QR, new Dictionary<Event, State>()
                { {Event.QR_SCANNED, State.SCANNING }, {Event.SCAN_ACCEPTED, State.SUCCESS }, {Event.SCAN_DENIED, State.FAILED } }
            },
            {
                State.SCANNING, new Dictionary<Event, State>()
                { {Event.SCAN_ACCEPTED, State.SUCCESS }, {Event.SCAN_DENIED, State.FAILED } }
            },
            {
                State.SUCCESS, new Dictionary<Event, State>()
                { {Event.START_RESET, State.RESETTING } }
            },
            {
                State.FAILED, new Dictionary<Event, State>()
                { {Event.START_RESET, State.RESETTING } }
            },
            {
                State.RESETTING, new Dictionary<Event, State>()
                { {Event.RESET, State.FETCHING_QR } }
            },
        };

        

        private State currentState = State.FETCHING_QR;
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
