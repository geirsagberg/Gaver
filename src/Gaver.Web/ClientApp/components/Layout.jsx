import * as React from 'react'

class Layout extends React.Component {
  render() {
    return (
      <div className='container'>
        <div className="row">
          <div className='col-sm-12'>
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
