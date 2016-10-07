import React from 'react'
import classNames from 'classnames'
import map from 'lodash/map'
import ReactTooltip from 'react-tooltip'
import { connect } from 'react-redux'
import Immutable from 'seamless-immutable'
import * as sharedListActions from 'store/sharedList'
import { getIn } from 'utils/immutableExtensions'

class SharedList extends React.Component {
  static get propTypes() {
    const result = {}
    return result
  }

  componentDidMount() {
    this.props.loadSharedList(this.props.params.id)
  }

  render() {
    return (
      <div>
        <header className="header">
          <h1 style={{ flex: 1 }}>{this.props.owner}sine ønsker</h1>
          {this.props.userName && <div className="header_item">
            {this.props.userName}
          </div>}
          <div className="header_item" data-tip={this.props.users.join(', ')}>
            {this.props.count} <span className="icon-users" />
          </div>
          <button className={classNames('btn btn-default header_item')} onClick={this.props.logOut}>
            <span className="icon-exit icon-before" />
            Logg ut
          </button>
        </header>
        <div className="wishList">
          <ul className="list-group">
            {map(this.props.wishes, wish =>
              <li className="list-group-item" key={wish.id}>
                <span>{wish.title}</span>
              </li>
            )}
          </ul>
        </div>
        <ReactTooltip />
      </div>
    )
  }
}

const mapStateToProps = state => ({
  wishes: state.wishes || Immutable({}),
  users: state::getIn('sharedList.users.names', Immutable([])),
  count: state::getIn('sharedList.users.count', 0),
  owner: state::getIn('sharedList.owner', ''),
  userName: state.user.name
})

export default connect(mapStateToProps, sharedListActions)(SharedList)