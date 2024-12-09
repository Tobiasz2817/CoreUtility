using UnityEngine;
using System;

namespace CoreUtility {
    public class RequestSystem {
        float _time = 0;
        float _fixedTime = 0;
        
        RequestData? _request;
        RequestCallbacks? _events;

        public bool IsProcess(RequestData requestData) => (_request.HasValue &&
                                                    requestData.GetHashCode() == _request.GetHashCode()) ||
                                                   (_events.HasValue &&
                                                    requestData.Event.HasValue &&
                                                    _events.Value.GetHashCode() == requestData.Event.Value.GetHashCode());
        /// <summary>
        /// Force the request, ensuring cancel previous
        /// </summary>
        /// <param name="requestData"> data for request </param>
        public void Force(RequestData requestData) {
            _time = 0;
            _fixedTime = 0f;
            
            _request = requestData;
            
            ForceStopCallbacks();
        }

        public void ForceStopCallbacks() {
            _events?.End?.Invoke();
            _events = null;
        }
        
        // TODO: Add request update tick 
        public void Tick() {
            if (!_request.HasValue)
                return;

            var request = _request.Value;
            if (_time > request.BufferTime) {
                _request = null;
                return;
            }

            if (request.Condition.Invoke()) {
                request.Action?.Invoke();

                _events?.End?.Invoke();
                _events = _request.Value.Event;
                _request = null;

                return;
            }
                
            _time += Time.deltaTime;
        }

        public void TickFixed() {
            if (!_events.HasValue)
                return;

            var request = _events.Value;
            if (_fixedTime > request.CallbacksTime) {
                _events?.End?.Invoke();
                _events = null;
                return;
            }
            
            _fixedTime += Time.fixedDeltaTime;
            request.TickFixed?.Invoke(_fixedTime / request.CallbacksTime);
        }

    }

    public struct RequestCallbacks {
        public float CallbacksTime;
        public Action End;
        public Action<float> Tick;
        public Action<float> TickFixed;
        
        public override int GetHashCode() => HashCode.Combine(CallbacksTime, End, Tick, TickFixed);
    }
    
    public struct RequestData {
        public Action Action;
        public Func<bool> Condition;
        public float BufferTime;

        public RequestCallbacks? Event;
        
        public override int GetHashCode() => HashCode.Combine(Action, Condition, BufferTime, Event);
    }
}