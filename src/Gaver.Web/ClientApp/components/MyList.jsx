import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import * as myListActions from 'store/myList'
import Immutable from 'seamless-immutable'
import { map, size } from 'lodash-es'
import { logOut } from 'store/user'
import ReactTooltip from 'react-tooltip'
import './MyList.css'
import classNames from 'classnames'
import { toggleSharedLists, setSharedListsVisible } from 'store/ui'
import { Link } from 'react-router-dom'

class Wish extends React.Component {
  static get propTypes () {
    return {
      wish: PropTypes.object,
      deleteWish: PropTypes.func.isRequired,
      listId: PropTypes.number,
      editUrl: PropTypes.func.isRequired,
      editDescription: PropTypes.func.isRequired
    }
  }

  render () {
    const { wish, listId, deleteWish, editUrl, editDescription } = this.props
    return (
      <li className="list-group-item wish">
        <span className="wish_title">{wish.title}</span>
        {wish.url ? (
          <span className="wish_url">
            <span className="icon-pencil clickable wish_edit" onClick={() => editUrl({ listId, wishId: wish.id })} />
            <a href={wish.url} className="wish_urlLink">
              {wish.url}
            </a>
          </span>
        ) : (
          <button className="btn btn-link wish_btn" onClick={() => editUrl({ listId, wishId: wish.id })}>
            Legg til lenke
          </button>
        )}
        {wish.description ? (
          <span className="wish_description">
            <span
              className="icon-pencil clickable wish_edit"
              onClick={() => editDescription({ listId, wishId: wish.id })}
            />
            <span className="wish_descriptionText">{wish.description}</span>
          </span>
        ) : (
          <button className="btn btn-link wish_btn" onClick={() => editDescription({ listId, wishId: wish.id })}>
            Legg til beskrivelse
          </button>
        )}
        <button
          className="btn btn-link wish_btn wish_btn-right"
          onClick={() => deleteWish({ listId, wishId: wish.id })}>
          Fjern
        </button>
      </li>
    )
  }
}

class MyList extends React.Component {
  static get propTypes () {
    return {
      wishes: PropTypes.object,
      invitations: PropTypes.object,
      count: PropTypes.number,
      addWish: PropTypes.func,
      loadMyList: PropTypes.func,
      deleteWish: PropTypes.func,
      shareList: PropTypes.func,
      userName: PropTypes.string,
      logOut: PropTypes.func,
      listId: PropTypes.number,
      editUrl: PropTypes.func,
      editDescription: PropTypes.func,
      toggleSharedLists: PropTypes.func,
      isShowingSharedLists: PropTypes.bool
    }
  }

  componentDidMount () {
    document.addEventListener('click', e => {
      if (this.props.isShowingSharedLists && !document.getElementById('sharedListsWrapper').contains(e.target)) {
        this.props.setSharedListsVisible(false)
      }
    })
    this.props.loadMyList()
  }

  componentWillUnmount () {
    this.props.setSharedListsVisible(false)
  }

  onKeyUp = (e) => {
    if (e.which === 13) {
      this.addWish()
    }
  }

  addWish = () => {
    if (this.wishInput.value) {
      this.props.addWish({ listId: this.props.listId, title: this.wishInput.value })
      this.wishInput.value = ''
    }
  }

  render () {
    const { listId, deleteWish, editUrl, editDescription, logOut, isShowingSharedLists, invitations } = this.props
    return (
      <div>
        <header className="header">
          <h1 className="header_title">Mine ønsker</h1>
          <div className="header_items">
            {this.props.userName && <div className="header_item header_username">{this.props.userName}</div>}
            <div className="header_actions">
              <div id="sharedListsWrapper" className="sharedListsWrapper header_item">
                <button
                  className={classNames('btn btn-default', {
                    active: isShowingSharedLists
                  })}
                  onClick={() => this.props.toggleSharedLists()}>
                  <span className="icon-list icon-before" />
                  <span className="btn_text">Venner</span>
                </button>
                {isShowingSharedLists && (
                  <ul className="list-group sharedLists">
                    {size(invitations) > 0 ? (
                      map(invitations, (invitation) => (
                        <li className="list-group-item" key={invitation.wishListId}>
                          <Link to={`/list/${invitation.wishListId}`}>{invitation.wishListUserName}</Link>
                        </li>
                      ))
                    ) : (
                      <li className="list-group-item item-empty">Ingen delte lister enda...</li>
                    )}
                  </ul>
                )}
              </div>
              <button
                className={classNames('btn btn-default header_item')}
                onClick={() => this.props.shareList(this.props.listId)}>
                <span className="icon-share2 icon-before" />
                <span className="btn_text">Del</span>
              </button>
              <button className={classNames('btn btn-default header_item')} onClick={logOut}>
                <span className="icon-exit icon-before" />
                <span className="btn_text">Logg ut</span>
              </button>
            </div>
          </div>
        </header>
        <div className="wishList">
          <div className="input-group">
            <input
              autoFocus
              className="form-control"
              placeholder="Jeg ønsker meg..."
              onKeyUp={this.onKeyUp}
              ref={(el) => (this.wishInput = el)}
            />
            <span className="input-group-btn">
              <button className="btn btn-default" type="button" onClick={this.addWish}>
                Legg til
              </button>
            </span>
          </div>
          <ul className="list-group">
            {map(this.props.wishes, (wish) => (
              <Wish key={wish.id} {...{ wish, listId, deleteWish, editUrl, editDescription }} />
            ))}
          </ul>
        </div>
        <ReactTooltip />
      </div>
    )
  }
}

const mapStateToProps = (state) => ({
  wishes: state.myList.wishes || Immutable({}),
  userName: state.user.name,
  listId: state.myList.listId,
  isShowingSharedLists: state.ui.isShowingSharedLists,
  invitations: state.myList.invitations || Immutable({})
})

const actions = {
  ...myListActions,
  logOut,
  toggleSharedLists,
  setSharedListsVisible
}

export default connect(mapStateToProps, actions)(MyList)
