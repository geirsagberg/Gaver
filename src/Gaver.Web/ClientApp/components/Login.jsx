import * as React from 'react'
import { connect } from 'react-redux'
import * as actions from 'store/user'
import Cookies from 'js-cookie'

class Login extends React.Component {
  constructor(props) {
    super(props)
    this.state = { isLoggingIn: !!Cookies.get('user') }
  }

  componentDidMount() {
    const name = Cookies.get('user')
    if (name) {
      this.props.logIn(name, ::this.redirect)
    }
  }

  static get propTypes() {
    const result = {}
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
    if (this.nameInput) {
      this.props.logIn(this.nameInput.value, ::this.redirect)
    }
  }

  render() {
    return (
      <div className="container">
        {!this.state.isLoggingIn && <div className="well col-sm-6 col-centered">
          <h1>Gaver</h1>
          <div className="form-group">
            <label htmlFor="nameInput" className="control-label">
              Navn
            </label>
            <input ref={el => { this.nameInput = el } } type="text" className="form-control" onKeyDown={e => e.which === 13 && this.logIn()} />
          </div>
          <button className="btn btn-primary" onClick={() => this.logIn()}>
            Logg inn
          </button>
        </div>}
      </div>
    )
  }
}

export default connect(null, actions)(Login)
