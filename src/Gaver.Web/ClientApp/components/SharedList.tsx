import React from 'react'
import classNames from 'classnames'
import ReactTooltip from 'react-tooltip'
import { connect } from 'react-redux'
import { actionCreators } from '~/store/sharedList'
import { map, size, get } from 'lodash-es'
import { logOut } from '~/store/user'
import Chat from '~/components/Chat'
import Loading from './Loading'
import { getQueryVariable } from '~/utils'

class Wish extends React.Component<any> {
  render() {
    const { wish, setBought, userId, users } = this.props
    return (
      <li className="list-group-item wish">
        <span
          className={classNames('wish_title', {
            'wish_title-bought': !!wish.boughtByUser
          })}>
          {wish.title}
        </span>
        {wish.url && (
          <a href={wish.url} className="wish_url">
            {wish.url}
          </a>
        )}
        {wish.description && <span className="wish_description">{wish.description}</span>}
        {!wish.boughtByUser || wish.boughtByUser === userId ? (
          <span className="checkbox wish_detail wish_detail-right">
            <label>
              <input
                type="checkbox"
                checked={wish.boughtByUser === userId}
                onChange={() =>
                  setBought({
                    listId: wish.wishListId,
                    wishId: wish.id,
                    isBought: wish.boughtByUser !== userId
                  })
                }
              />
              <span>Jeg kjøper</span>
            </label>
          </span>
        ) : (
          <span className="wish_detail wish_detail-right">Kjøpt av {get(users, [wish.boughtByUser, 'name'])}</span>
        )}
      </li>
    )
  }
}

class SharedList extends React.Component<any> {
  componentDidMount() {
    const listId = this.props.match.params.id
    const inviteToken = getQueryVariable('token')
    this.props.subscribeList(listId, inviteToken)
  }

  componentWillUnmount() {
    this.props.unsubscribeList(this.props.match.params.id)
  }

  render() {
    if (!this.props.owner || !this.props.isAuthorized) {
      return <Loading />
    }

    return (
      <div>
        <header className="header">
          <h1 className="header_title">
            {this.props.owner}
            &nbsp;sine ønsker
          </h1>
          <div className="header_items">
            {this.props.userName && <div className="header_item header_username">{this.props.userName}</div>}
            <div
              className="header_item"
              data-tip={this.props.currentUsers.map(id => this.props.users[id].name).join(', ')}>
              {this.props.count} <span className="icon-users" />
            </div>
            <button className="btn btn-default header_item" onClick={this.props.showMyList}>
              <span className="icon-home3 icon-before" />
              <span className="btn_text">Min liste</span>
            </button>
            <button className={classNames('btn btn-default header_item')} onClick={this.props.logOut}>
              <span className="icon-exit icon-before" />
              <span className="btn_text">Logg ut</span>
            </button>
          </div>
        </header>
        <div className="row">
          <div className="wishList col-md-8">
            <ul className="list-group">
              {size(this.props.wishes) > 0 ? (
                map(this.props.wishes, (wish: any) => <Wish {...{ ...this.props, wish }} key={wish.id} />)
              ) : (
                <li className="list-group-item wish wish-empty">Ingen ønsker enda...</li>
              )}
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
  wishes: state.sharedList.wishes,
  users: state.sharedList.users,
  currentUsers: state.sharedList.currentUsers,
  count: state.sharedList.currentUsers.length,
  owner: state.sharedList.owner || '',
  userName: state.user.name || '',
  userId: state.user.id || 0,
  isAuthorized: state.sharedList.isAuthorized
})

const dispatchOptions = {
  ...actionCreators,
  logOut
}

export default connect(
  mapStateToProps,
  dispatchOptions
)(SharedList)
