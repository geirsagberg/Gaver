var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { fetch } from 'domain-task/fetch';
import { typeName, isActionType, Action } from 'redux-typed';
// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.
let RequestWeatherForecasts = class RequestWeatherForecasts extends Action {
    constructor(startDateIndex) {
        super();
        this.startDateIndex = startDateIndex;
    }
};
RequestWeatherForecasts = __decorate([
    typeName("REQUEST_WEATHER_FORECASTS")
], RequestWeatherForecasts);
let ReceiveWeatherForecasts = class ReceiveWeatherForecasts extends Action {
    constructor(startDateIndex, forecasts) {
        super();
        this.startDateIndex = startDateIndex;
        this.forecasts = forecasts;
    }
};
ReceiveWeatherForecasts = __decorate([
    typeName("RECEIVE_WEATHER_FORECASTS")
], ReceiveWeatherForecasts);
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
export const actionCreators = {
    requestWeatherForecasts: (startDateIndex) => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        if (startDateIndex !== getState().weatherForecasts.startDateIndex) {
            fetch(`/api/SampleData/WeatherForecasts?startDateIndex=${startDateIndex}`)
                .then(response => response.json())
                .then((data) => {
                dispatch(new ReceiveWeatherForecasts(startDateIndex, data));
            });
            dispatch(new RequestWeatherForecasts(startDateIndex));
        }
    }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
const unloadedState = { startDateIndex: null, forecasts: [], isLoading: false };
export const reducer = (state, action) => {
    if (isActionType(action, RequestWeatherForecasts)) {
        return { startDateIndex: action.startDateIndex, isLoading: true, forecasts: state.forecasts };
    }
    else if (isActionType(action, ReceiveWeatherForecasts)) {
        // Only accept the incoming data if it matches the most recent request. This ensures we correctly
        // handle out-of-order responses.
        if (action.startDateIndex === state.startDateIndex) {
            return { startDateIndex: action.startDateIndex, forecasts: action.forecasts, isLoading: false };
        }
    }
    // For unrecognized actions (or in cases where actions have no effect), must return the existing state
    // (or default initial state if none was supplied)
    return state || unloadedState;
};
//# sourceMappingURL=WeatherForecasts.js.map