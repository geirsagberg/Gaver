import * as React from 'react'
import { connect } from 'react-redux'
import * as actions from 'store/user'

class Login extends React.Component {
  static get propTypes () {
    const result = {}
    return result
  }

  logIn () {
    if (this.nameInput) {
      this.props.logIn(this.nameInput.value)
    }
  }

  render () {
    return (
      <div className="container">
	<div className="well col-sm-6 col-centered">
	  <h1>Gaver</h1>
	  <div className="form-group">
	    <label htmlFor="nameInput" className="control-label">
	      Navn
	    </label>
	    <input ref={el => { this.nameInput = el } } type="text" className="form-control" onKeyDown={e => e.which === 13 && this.logIn() } />
	  </div>
	  <button className="btn btn-primary" onClick={::this.logIn}>
	    Logg inn
	  </button>
	</div>
      </div>
    )
  }
}

export default connect(null, actions)(Login)
