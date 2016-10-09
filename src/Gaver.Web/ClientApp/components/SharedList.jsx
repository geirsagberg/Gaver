import React, { PropTypes } from 'react'
import classNames from 'classnames'
import ReactTooltip from 'react-tooltip'
import { connect } from 'react-redux'
import Immutable from 'seamless-immutable'
import * as sharedListActions from 'store/sharedList'
import { getIn, map } from 'utils/immutableExtensions'
import { logOut } from 'store/user'
import './SharedList.css'
import Chat from 'components/Chat'

class Wish extends React.Component {
  static get propTypes() {
    return {
      wish: PropTypes.object.isRequired,
      setBought: PropTypes.func.isRequired,
      userName: PropTypes.string.isRequired,
      userId: PropTypes.number.isRequired,
      users: PropTypes.object.isRequired
    }
  }

  render() {
    const { wish, setBought, userId, users } = this.props
    return (
      <li className="list-group-item wish">
        <span className={classNames('wish_title', {
          'wish_title-bought': !!wish.boughtByUser
        })}>{wish.title}</span>
        <a href={wish.url} className="wish_url">{wish.url}</a>
        {!wish.boughtByUser || wish.boughtByUser === userId
          ? <span className="checkbox wish_detail wish_detail-right">
            <label>
              <input type="checkbox" checked={wish.boughtByUser === userId} onChange={() => setBought({ listId: wish.wishListId, wishId: wish.id, isBought: wish.boughtByUser !== userId })} />
              <span>Jeg kjøper</span>
            </label>
          </span>
          : <span className="wish_detail wish_detail-right">Kjøpt av {users[wish.boughtByUser].name}</span>
        }
      </li>
    )
  }
}

class SharedList extends React.Component {
  static get propTypes() {
    const result = {
      logOut: PropTypes.func.isRequired,
      setBought: PropTypes.func.isRequired,
      userName: PropTypes.string.isRequired,
      userId: PropTypes.number.isRequired,
      params: PropTypes.object.isRequired,
    }
    return result
  }

  componentDidMount() {
    this.props.loadSharedList(this.props.params.id)
  }

  render() {
    return (
      <div>
        <header className="header">
          <h1 style={{ flex: 1 }}>{this.props.owner}&nbsp;sine ønsker</h1>
          {this.props.userName && <div className="header_item">
            {this.props.userName}
          </div>}
          <div className="header_item" data-tip={this.props.currentUsers::map(id => this.props.users[id].name).join(', ')}>
            {this.props.count} <span className="icon-users" />
          </div>
          <button className={classNames('btn btn-default header_item')} onClick={this.props.logOut}>
            <span className="icon-exit icon-before" />
            Logg ut
          </button>
        </header>
        <div className="row">
          <div className="wishList col-md-8">
            <ul className="list-group">
              {this.props.wishes::map(wish => <Wish {...{...this.props, wish}} key={wish.id}/>)}
            </ul>
          </div>
          <div className="col-md-4">
            <Chat />
          </div>
        </div>
        <ReactTooltip />
      </div>
    )
  }
}

const mapStateToProps = state => ({
  wishes: state.sharedList.wishes || Immutable({}),
  users: state::getIn('sharedList.users', Immutable({})),
  currentUsers: state::getIn('sharedList.currentUsers', Immutable([])),
  count: state::getIn('sharedList.currentUsers.count', 0),
  owner: state.sharedList.owner || '',
  userName: state.user.name || '',
  userId: state.user.id || 0,
})

const dispatchOptions = {
  ...sharedListActions,
  logOut
}

export default connect(mapStateToProps, dispatchOptions)(SharedList)