using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoorLockDemo
{
    public class AppStateMachine
    {
        public enum State
        {
            LOCKED,
            REQUESTING_QR,
            QR_DISPLAY,
            SCANNING,
            UNLOCKED
        }
        public enum Event
        {
            QR_REQUEST,
            GOT_QR,
            QR_SCANNED,
            SCAN_DENIED,
            SCAN_ACCEPTED,
            CANCELLED,
            RESET
        }

        public static readonly Dictionary<State, Dictionary<Event, State>> StateTransitions = new Dictionary<State, Dictionary<Event, State>>()
        {
            {
                State.LOCKED, new Dictionary<Event, State>()
                { {Event.QR_REQUEST, State.REQUESTING_QR } }
            },
            {
                State.REQUESTING_QR, new Dictionary<Event, State>()
                { {Event.GOT_QR, State.QR_DISPLAY }, {Event.CANCELLED, State.LOCKED } }
            },
            {
                State.QR_DISPLAY, new Dictionary<Event, State>()
                { {Event.QR_SCANNED, State.SCANNING }, {Event.SCAN_ACCEPTED, State.UNLOCKED }, {Event.SCAN_DENIED, State.LOCKED } }
            },
            {
                State.SCANNING, new Dictionary<Event, State>()
                { {Event.SCAN_ACCEPTED, State.UNLOCKED }, {Event.SCAN_DENIED, State.LOCKED }, {Event.CANCELLED, State.LOCKED } }
            },
            {
                State.UNLOCKED, new Dictionary<Event, State>()
                { {Event.RESET, State.LOCKED } }
            },
        };

        

        private State currentState = State.LOCKED;
        private Dictionary<State, ThreadStart> stateInvokableMethods = new Dictionary<State, ThreadStart>();

        public void setStateCallback(State state, ThreadStart callback)
        {
            if (!stateInvokableMethods.ContainsKey(state))
                stateInvokableMethods.Add(state, callback);
        }

        public void doEvent(Event _event) {
            State nextState;
            if (currentState.GetNextState(_event, out nextState))
            {
                ThreadStart ts;
                if (stateInvokableMethods.TryGetValue(nextState, out ts))
                    new Thread(ts).Start();

                currentState = nextState;
            }
        }

        public void start()
        {
            ThreadStart ts;
            if (stateInvokableMethods.TryGetValue(currentState, out ts))
                new Thread(ts).Start();
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
