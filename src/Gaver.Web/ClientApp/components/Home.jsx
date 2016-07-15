import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import * as currentListActions from 'store/currentList'
import Immutable from 'seamless-immutable'
import map from 'lodash/map'

class Home extends React.Component {
  componentDidMount () {
    this.props.loadData()
  }

  onKeyUp (e) {
    if (e.which === 13) {
      this.addWish()
    }
  }

  addWish () {
    if (this.wishInput.value) {
      this.props.addWish(this.wishInput.value)
      this.wishInput.value = ''
    }
  }

  render () {
    return (
    <div>
      <h1>
        Mine ønsker
        <div className="btn-group pull-right">
          <button type="button" className="btn btn-default" onClick={this.props.shareList}><span className="glyphicon glyphicon-share"></span> Del</button>
        </div>
      </h1>
      <div className="input-group">
        <input className="form-control" placeholder="Jeg ønsker meg..." onKeyUp={::this.onKeyUp} ref={el => (this.wishInput = el)} />
        <span className="input-group-btn">
          <button className="btn btn-default" type="button" onClick={::this.addWish}>Legg til</button>
        </span>
      </div>
      <ul className="list-group">
        {map(this.props.wishes, wish =>
          <li className="list-group-item">
            <span>{wish.title}</span>
            <button className="btn btn-link pull-right no-padding" onClick={() => this.props.deleteWish(wish.id)}>Fjern</button>
          </li>
          )}
      </ul>
    </div>
    )
  }
}

Home.propTypes = {
  addWish: PropTypes.func,
  loadData: PropTypes.func,
  wishes: PropTypes.array,
  deleteWish: PropTypes.func,
  shareList: PropTypes.func
}

const mapStateToProps = state => ({
  wishes: state.currentList.wishes || Immutable([])
})

export default connect(mapStateToProps, currentListActions)(Home)

