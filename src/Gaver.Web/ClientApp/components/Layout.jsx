import * as React from 'react'
import Loading from './Loading'
import MyList from './MyList'
import SharedList from './SharedList'
import Login from './Login'
import { connect } from 'react-redux'
import {
  BrowserRouter as Router,
  Route,
  Redirect
} from 'react-router-dom'

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
                <Route path='/login' component={Login} />
              </div>
            </div>
          </div>}
      </Router>
    )
  }
}

Layout.propTypes = {
  children: React.PropTypes.element,
  route: React.PropTypes.any,
  isLoading: React.PropTypes.bool,
  isLoggedIn: React.PropTypes.bool
}

const mapStateToProps = state => ({
  isLoading: !!state.ui.isLoading,
  isLoggedIn: !!state.user.isLoggedIn
})

export default connect(mapStateToProps)(Layout)
