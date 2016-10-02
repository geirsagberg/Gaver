import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import * as currentListActions from 'store/currentList'
import Immutable from 'seamless-immutable'
import map from 'lodash/map'
import { logOut } from 'store/user'
import ReactTooltip from 'react-tooltip'
import './MyList.css'
import classNames from 'classnames'

class MyList extends React.Component {
  componentDidMount() {
    this.props.loadData()
  }

  onKeyUp(e) {
    if (e.which === 13) {
      this.addWish()
    }
  }

  addWish() {
    if (this.wishInput.value) {
      this.props.addWish(this.wishInput.value)
      this.wishInput.value = ''
    }
  }

  render() {
    return (
      <div>
        <header className="header">
          <h1 style={{ flex: 1 }}>Mine ønsker</h1>
          {this.props.userName && <div className="header_item">
            {this.props.userName}
          </div>}
          <button className={classNames('btn btn-default header_item')} onClick={this.props.shareList}>
            <span className="icon-share2 icon-before" />
            Del
          </button>
          <button className={classNames('btn btn-default header_item')} onClick={this.props.logOut}>
            <span className="icon-exit icon-before" />
            Logg ut
          </button>
        </header >
        <div className="wishList">
          <div className="input-group">
            <input className="form-control" placeholder="Jeg ønsker meg..." onKeyUp={:: this.onKeyUp} ref={el => (this.wishInput = el) } />
            <span className="input-group-btn">
              <button className="btn btn-default" type="button" onClick={:: this.addWish}>Legg til</button>
            </span>
          </div >
          <ul className="list-group">
            {map(this.props.wishes, wish =>
              <li className="list-group-item" key={wish.id}>
                <span>{wish.title}</span>
                <button className="btn btn-link pull-right no-padding" onClick={() => this.props.deleteWish(wish.id) }>Fjern</button>
              </li>
            )}
          </ul>
        </div>
        <ReactTooltip />
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
  userName: PropTypes.string,
  logOut: PropTypes.func,
}

const mapStateToProps = state => ({
  wishes: state.currentList.wishes || Immutable({}),
  userName: state.user.name
})

const actions = {
  ...currentListActions,
  logOut
}

export default connect(mapStateToProps, actions)(MyList)

