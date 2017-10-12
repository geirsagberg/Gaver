import * as React from 'react'
import Loading from './Loading'
import MyList from './MyList'
import SharedList from './SharedList'
import Login from './Login'
import { getIn } from 'utils/immutableExtensions'
import { connect } from 'react-redux'
import {
  Route,
  Redirect,
  Switch
} from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'
import history from 'utils/history'
import PropTypes from 'prop-types'

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
    const { isLoggedIn } = this.props
    return (
      <ConnectedRouter history={history}>
        {this.props.isLoading || this.props.isLoggingIn
        ? <Loading />
        : <div className='container'>
            <div className='row'>
              <div className='col-sm-12'>
                <Switch>
                  <PrivateRoute exact path='/' component={MyList} isLoggedIn={isLoggedIn} />
                  <PrivateRoute path='/list/:id' component={SharedList} isLoggedIn={isLoggedIn} />
                  <LoginRoute isLoggedIn={isLoggedIn} />
                  <Route render={() => <div>lol</div>} />
                </Switch>
              </div>
            </div>
          </div>}
      </ConnectedRouter>
    )
  }
}

Layout.propTypes = {
  children: PropTypes.element,
  route: PropTypes.any,
  isLoading: PropTypes.bool,
  isLoggedIn: PropTypes.bool,
  isLoggingIn: PropTypes.bool
}

const mapStateToProps = state => ({
  isLoading: !!state.ui.isLoading,
  isLoggedIn: !!state.user.isLoggedIn,
  isLoggingIn: !!state.user.isLoggingIn
})

export default connect(mapStateToProps)(Layout)
