import * as React from 'react'
import { connect } from 'react-redux'
import * as actions from 'store/user'
import './Login.css'
import Loading from './Loading'
import { Redirect } from 'react-router-dom'
import { getIn } from 'utils/immutableExtensions'

class Login extends React.Component {
  static get propTypes() {
    const result = {
      location: React.PropTypes.object,
      logIn: React.PropTypes.func.isRequired,
      isLoggedIn: React.PropTypes.bool,
      isLoggingIn: React.PropTypes.bool
    }
    return result
  }

  logIn() {
    this.props.logIn()
  }

  render() {
    return this.props.isLoggedIn
      ? <Redirect to={this.props::getIn('location.state.from', '/')} />
      : (
      <div className="container">
        {this.props.isLoggingIn
        ? <Loading />
        : <div className="well col-sm-6 col-centered">
          <h1 className="headline">Gaver</h1>
          <button className="btn btn-primary" onClick={() => this.props.logIn()}>
            Logg inn
          </button>
        </div>}
      </div>
    )
  }
}

const mapStateToProps = state => ({
  isLoggedIn: state.user.isLoggedIn,
  isLoggingIn: state.user.isLoggingIn
})

export default connect(mapStateToProps, actions)(Login)
