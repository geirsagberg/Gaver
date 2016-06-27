import * as React from 'react';
import { provide } from 'redux-typed';
import * as CounterStore from '../store/Counter';
class Counter extends React.Component {
    render() {
        return <div>
            <h1>Counter</h1>

            <p>This is a simple example of a React component.</p>

            <p>Current count: <strong>{this.props.count}</strong></p>

            <button onClick={() => { this.props.increment(); }}>Increment</button>
        </div>;
    }
}
// Build the CounterProps type, which allows the component to be strongly typed
const provider = provide((state) => state.counter, // Select which part of global state maps to this component
CounterStore.actionCreators // Select which action creators should be exposed to this component
);
export default provider.connect(Counter);
//# sourceMappingURL=Counter.jsx.map