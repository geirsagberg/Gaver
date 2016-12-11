import * as React from 'react'
import Loading from './Loading'
import { connect } from 'react-redux'

class Layout extends React.Component {
  render() {
    let children = null
    if (this.props.children) {
      children = React.cloneElement(this.props.children, {
        auth: this.props.route.auth // sends auth instance from route to children
      })
    }
    return (
      this.props.isLoading
      ? <Loading />
      : <div className='container'>
        <div className='row'>
          <div className='col-sm-12'>
            {children}
          </div>
        </div>
      </div>
    )
  }
}

Layout.propTypes = {
  children: React.PropTypes.element,
  route: React.PropTypes.any,
  isLoading: React.PropTypes.bool
}

const mapStateToProps = state => ({
  isLoading: state.ui.isLoading
})

export default connect(mapStateToProps)(Layout)
