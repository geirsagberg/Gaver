import React from 'react'
import classNames from 'classnames'
import map from 'lodash/map'
import ReactTooltip from 'react-tooltip'
import { connect } from 'react-redux'
import Immutable from 'seamless-immutable'

class SharedList extends React.Component {
  static get propTypes() {
    const result = {}
    return result
  }

  render () {
    return (
      <div>
        <header className="header">
          <h1 style={{ flex: 1 }}>Mine ønsker</h1>
          {this.props.userName && <div className="header_item">
            {this.props.userName}
          </div>}
          <div className="header_item" data-tip={this.props.users.join(', ') }>
            {this.props.count} <span className="icon-users" />
          </div>
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
          <ul className="list-group">
            {map(this.props.wishes, wish =>
              <li className="list-group-item" key={wish.id}>
                <span>{wish.title}</span>
              </li>
            ) }
          </ul>
        </div>
        <ReactTooltip />
      </div>
    )
  }
}

const mapStateToProps = state => ({
  wishes: state.currentList.wishes || Immutable({}),
  users: state.currentList.users.names || Immutable([]),
  count: state.currentList.users.count || 0,
  userName: state.user.name
})

export default connect(mapStateToProps)(SharedList)