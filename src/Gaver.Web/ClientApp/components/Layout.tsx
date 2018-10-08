import * as React from 'react'
import Loading from './Loading'
import MyList from './MyList'
import SharedList from './SharedList'
import Login from './Login'
import { get } from 'lodash-es'
import { connect } from 'react-redux'
import { Route, Redirect, Switch } from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'
import history from 'utils/history'
import { SFC, ComponentClass } from 'react'
import { RouteComponentProps, RouteProps } from 'react-router'

type PrivateRouteProps = {
  isLoggedIn: boolean
} & RouteProps

const PrivateRoute: SFC<PrivateRouteProps> = ({ component, isLoggedIn, ...rest }) => (
  <Route
    {...rest}
    render={props =>
      isLoggedIn ? (
        React.createElement(component, props)
      ) : (
        <Redirect
          to={{
            pathname: '/login',
            state: { from: props.location }
          }}
        />
      )
    }
  />
)

const LoginRoute: SFC<{ isLoggedIn: boolean }> = ({ isLoggedIn }) => (
  <Route
    path="/login"
    render={props => (isLoggedIn ? <Redirect to={get(props, 'location.state.props', '/')} /> : <Login />)}
  />
)

type LayoutProps = {
  isLoggedIn: boolean
  isLoading: boolean
  isLoggingIn: boolean
}

class Layout extends React.Component<LayoutProps> {
  render() {
    const { isLoggedIn, isLoading, isLoggingIn } = this.props
    return (
      <ConnectedRouter history={history}>
        {isLoading || isLoggingIn ? (
          <Loading />
        ) : (
          <div className="container">
            <div className="row">
              <div className="col-sm-12">
                <Switch>
                  <PrivateRoute exact path="/" component={MyList} isLoggedIn={isLoggedIn} />
                  <PrivateRoute path="/list/:id" component={SharedList} isLoggedIn={isLoggedIn} />
                  <LoginRoute isLoggedIn={isLoggedIn} />
                </Switch>
              </div>
            </div>
          </div>
        )}
      </ConnectedRouter>
    )
  }
}

const mapStateToProps = state => ({
  isLoading: !!state.ui.isLoading,
  isLoggedIn: !!state.user.isLoggedIn,
  isLoggingIn: !!state.user.isLoggingIn
})

export default connect(mapStateToProps)(Layout)
