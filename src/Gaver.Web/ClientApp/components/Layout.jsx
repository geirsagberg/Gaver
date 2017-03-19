import * as React from 'react'
import Loading from './Loading'
import MyList from './MyList'
import SharedList from './SharedList'
import Login from './Login'
import { getIn } from 'utils/immutableExtensions'
import { connect } from 'react-redux'
import {
  BrowserRouter as Router,
  Route,
  Redirect
} from 'react-router-dom'

const PropTypes = React.PropTypes

const PrivateRoute = ({ component, isLoggedIn, ...rest }) => (
  <Route {...rest} render={props => (
    isLoggedIn ? (
      React.createElement(component, props)
    ) : (
      <Redirect to={{
        pathname: '/login',
        state: { from: props.location }
      }}/>
    )
  )}/>
)
PrivateRoute.propTypes = {
  component: PropTypes.any,
  isLoggedIn: PropTypes.bool,
  location: PropTypes.object
}

const LoginRoute = ({isLoggedIn}) => (
  <Route path='/login' render={props => (
    isLoggedIn
      ? <Redirect to={props::getIn('location.state.props', '/')} />
      : <Login />
    )}/>
)
LoginRoute.propTypes = {
  isLoggedIn: PropTypes.bool
}

class Layout extends React.Component {
  render() {
    return (
      <Router>
        {this.props.isLoading
        ? <Loading />
        : <div className='container'>
            <div className='row'>
              <div className='col-sm-12'>
                <PrivateRoute exact path='/' component={MyList} isLoggedIn={this.props.isLoggedIn} />
                <Route path='/list/:id' component={SharedList} />
                <LoginRoute isLoggedIn={this.props.isLoggedIn} />
              </div>
            </div>
          </div>}
      </Router>
    )
  }
}

Layout.propTypes = {
  children: PropTypes.element,
  route: PropTypes.any,
  isLoading: PropTypes.bool,
  isLoggedIn: PropTypes.bool
}

const mapStateToProps = state => ({
  isLoading: !!state.ui.isLoading,
  isLoggedIn: !!state.user.isLoggedIn
})

export default connect(mapStateToProps)(Layout)
