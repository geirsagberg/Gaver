import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import * as currentListActions from 'store/currentList'
import Immutable from 'seamless-immutable'
import map from 'lodash/map'

class MyList extends React.Component {
  componentDidMount () {
    this.props.loadData()
    this.props.initializeListUpdates()
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
      <header style={{display: 'flex', alignItems: 'center'}}>
        <h1 style={{flex: 1}}>Mine ønsker</h1>
        <div style={{margin: '1rem'}}>
          {this.props.count} <span className="icon-users" />
        </div>
        <div className="btn-group">
          <button type="button" className="btn btn-default" onClick={this.props.shareList}><span className="icon-share2"></span> Del</button>
        </div>
      </header>
      <div className="input-group">
        <input className="form-control" placeholder="Jeg ønsker meg..." onKeyUp={::this.onKeyUp} ref={el => (this.wishInput = el)} />
        <span className="input-group-btn">
          <button className="btn btn-default" type="button" onClick={::this.addWish}>Legg til</button>
        </span>
      </div>
      <ul className="list-group">
        {map(this.props.wishes, wish =>
          <li className="list-group-item" key={wish.id}>
            <span>{wish.title}</span>
            <button className="btn btn-link pull-right no-padding" onClick={() => this.props.deleteWish(wish.id)}>Fjern</button>
          </li>
          )}
      </ul>
    </div>
    )
  }
}

MyList.propTypes = {
  wishes: PropTypes.object,
  count: PropTypes.number,
  addWish: PropTypes.func,
  loadData: PropTypes.func,
  deleteWish: PropTypes.func,
  shareList: PropTypes.func,
  initializeListUpdates: PropTypes.func
}

const mapStateToProps = state => ({
  wishes: state.currentList.wishes || Immutable([]),
  count: state.currentList.users.count || 0
})

export default connect(mapStateToProps, currentListActions)(MyList)

