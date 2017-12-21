import * as React from 'react'
import { connect } from 'react-redux'
import { actionCreators, UserState } from 'store/user'
import './Login.css'
import Loading from './Loading'

type Props = UserState & typeof actionCreators

class Login extends React.Component<Props> {
  render () {
    return (
      <div className='container'>
        {this.props.isLoggingIn ? (
          <Loading />
        ) : (
	  <div className='well col-sm-6 col-centered'>
	    <h1 className='headline'>Gaver</h1>
	    <button className='btn btn-primary' onClick={this.props.logIn}>
              Logg inn
            </button>
          </div>
        )}
      </div>
    )
  }
}

const mapStateToProps = (state) => state.user

export default connect(mapStateToProps, actionCreators)(Login) as any
