import * as React from 'react'
import { browserHistory, Router, IndexRoute, Route } from 'react-router'
import { connect } from 'react-redux'
import Layout from './components/Layout'
import MyList from './components/MyList'
import Login from './components/Login'

class App extends React.Component {
  requireAuth (nextState, replace) {
    if (!this.props.isLoggedIn) {
      replace({
	pathname: '/login',
	state: { nextPathname: nextState.location.pathname }
      })
    }
  }
  render () {
    return (
      <Router history={browserHistory}>
	<Route path="/" component={Layout}>
	  <IndexRoute component={MyList} onEnter={::this.requireAuth} />
	  <Route path="login" component={Login} />
	</Route>
      </Router>
    )
  }
}

App.propTypes = {
  isLoggedIn: React.PropTypes.bool
}

const mapStateToProps = state => ({
  isLoggedIn: state.user.isLoggedIn
})

export default connect(mapStateToProps)(App)