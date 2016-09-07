import * as React from 'react'
import { IndexRoute, Route } from 'react-router'
import { connect } from 'react-redux'
import Layout from './components/Layout'
import MyList from './components/MyList'
import Login from './components/Login'

class Routes extends React.Component {
  requireAuth (nextState, replace) {
    if (!this.props.isLoggedIn) {
      replace({
	pathname: 'login',
	state: { nextPathname: nextState.location.pathname }
      })
    }
  }

  render () {
    return (
      <Route path="/" component={Layout}>
	<IndexRoute component={MyList} onEnter={::this.requireAuth} />
	<Route path="login" component={Login} />
      </Route>
    )
  }
}

Routes.propTypes = {
  isLoggedIn: React.PropTypes.boolean,
  children: React.PropTypes.node
}

const mapStateToProps = state => ({
  isLoggedIn: state.isLoggedIn
})

export default connect(mapStateToProps)(Routes)