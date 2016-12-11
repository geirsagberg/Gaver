import * as React from 'react'
import { connect } from 'react-redux'
import * as actions from 'store/user'
import './Login.css'
import AuthService from 'utils/authService'

class Login extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      isLoggingIn: false
    }
  }

  componentDidMount() {
  }

  static get propTypes() {
    const result = {
      location: React.PropTypes.object,
      auth: React.PropTypes.instanceOf(AuthService)
    }
    return result
  }

  static get contextTypes() {
    return {
      router: React.PropTypes.object.isRequired,
      store: React.PropTypes.object
    }
  }

  redirect() {
    const { location } = this.props
    const { router } = this.context
    if (location.state && location.state.nextPathname) {
      router.replace(location.state.nextPathname)
    } else {
      router.replace('/')
    }
  }

  logIn() {
    this.props.logIn()
  }

  render() {
    return (
      <div className="container">
        {!this.state.isLoggingIn && <div className="well col-sm-6 col-centered">
          <h1 className="headline">Gaver</h1>
          <button className="btn btn-primary" onClick={() => this.props.auth.login()}>
            Logg inn
          </button>
        </div>}
      </div>
    )
  }
}

export default connect(null, actions)(Login)
