import * as React from 'react';
import { Link } from 'react-router';
import { provide } from 'redux-typed';
import * as WeatherForecastsState from '../store/WeatherForecasts';
class FetchData extends React.Component {
    componentWillMount() {
        // This method runs when the component is first added to the page 
        let startDateIndex = parseInt(this.props.params.startDateIndex) || 0;
        this.props.requestWeatherForecasts(startDateIndex);
    }
    componentWillReceiveProps(nextProps) {
        // This method runs when incoming props (e.g., route params) change
        let startDateIndex = parseInt(nextProps.params.startDateIndex) || 0;
        this.props.requestWeatherForecasts(startDateIndex);
    }
    render() {
        return <div>
            <h1>Weather forecast</h1>
            <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
            {this.renderForecastsTable()}
            {this.renderPagination()}
        </div>;
    }
    renderForecastsTable() {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
            {this.props.forecasts.map(forecast => <tr key={forecast.dateFormatted}>
                    <td>{forecast.dateFormatted}</td>
                    <td>{forecast.temperatureC}</td>
                    <td>{forecast.temperatureF}</td>
                    <td>{forecast.summary}</td>
                </tr>)}
            </tbody>
        </table>;
    }
    renderPagination() {
        let prevStartDateIndex = this.props.startDateIndex - 5;
        let nextStartDateIndex = this.props.startDateIndex + 5;
        return <p className='clearfix text-center'>
            <Link className='btn btn-default pull-left' to={`/fetchdata/${prevStartDateIndex}`}>Previous</Link>
            <Link className='btn btn-default pull-right' to={`/fetchdata/${nextStartDateIndex}`}>Next</Link>
            {this.props.isLoading ? <span>Loading...</span> : []}
        </p>;
    }
}
// Build the WeatherForecastProps type, which allows the component to be strongly typed
const provider = provide((state) => state.weatherForecasts, // Select which part of global state maps to this component
WeatherForecastsState.actionCreators // Select which action creators should be exposed to this component
).withExternalProps(); // Also include a 'params' property on WeatherForecastProps
export default provider.connect(FetchData);
//# sourceMappingURL=FetchData.jsx.map