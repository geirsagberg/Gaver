import * as React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import * as actions from 'store/user'
import './Login.css'
import Loading from './Loading'

class Login extends React.Component {
  static get propTypes () {
    const result = {
      location: PropTypes.object,
      logIn: PropTypes.func.isRequired,
      isLoggingIn: PropTypes.bool
    }
    return result
  }

  logIn () {
    this.props.logIn()
  }

  render () {
    return (
      <div className="container">
        {this.props.isLoggingIn ? (
          <Loading />
        ) : (
          <div className="well col-sm-6 col-centered">
            <h1 className="headline">Gaver</h1>
            <button className="btn btn-primary" onClick={() => this.props.logIn()}>
              Logg inn
            </button>
          </div>
        )}
      </div>
    )
  }
}

const mapStateToProps = (state) => ({
  isLoggingIn: state.user.isLoggingIn
})

export default connect(mapStateToProps, actions)(Login)
