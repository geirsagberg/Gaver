import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import * as currentListActions from 'store/currentList'
import Immutable from 'seamless-immutable'
import map from 'lodash/map'

class Home extends React.Component {

  render () {
    return (
    <div>
      <h1>Gaver</h1>
      <button className="button">Logg inn</button>
    </div>
    )
  }
}

Home.propTypes = {

}

export default connect()(Home)

