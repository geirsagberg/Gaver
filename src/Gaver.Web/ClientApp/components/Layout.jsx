import * as React from 'react'
import { NavMenu } from './NavMenu'
import { connect } from 'react-redux'

class Layout extends React.Component {
  render () {
    return (
    <div className="container-fluid">
      <div className="row">
        <div className="col-sm-3">
          <NavMenu />
        </div>
        <div className="col-sm-9">
          {this.props.children}
        </div>
      </div>
    </div>
    )
  }
}

Layout.propTypes = {
  children: React.PropTypes.element
}

export default Layout