import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import * as myListActions from 'store/myList'
import Immutable from 'seamless-immutable'
import map from 'lodash/map'
import { logOut } from 'store/user'
import ReactTooltip from 'react-tooltip'
import './MyList.css'
import classNames from 'classnames'

class Wish extends React.Component {
  static get propTypes() {
    return {
      wish: PropTypes.object,
      deleteWish: PropTypes.func.isRequired,
      listId: PropTypes.number,
      editUrl: PropTypes.func.isRequired,
      editDescription: PropTypes.func.isRequired
    }
  }

  render() {
    const { wish, listId, deleteWish, editUrl, editDescription } = this.props
    return (
      <li className="list-group-item wish">
        <span className="wish_title">{wish.title}</span>
        {wish.url
          ? <span className="wish_url"><a href={wish.url} className="wish_urlLink">{wish.url}</a><span className="icon-pencil clickable wish_edit" onClick={() => editUrl({listId, wishId: wish.id})} /></span>
          : <button className="btn btn-link wish_btn" onClick={() => editUrl({listId, wishId: wish.id})}>Legg til lenke</button>}
        {wish.description
          ? <span className="wish_description">{wish.description}<span className="icon-pencil clickable wish_edit" onClick={() => editDescription({listId, wishId: wish.id})} /></span>
          : <button className="btn btn-link wish_btn" onClick={() => editDescription({listId, wishId: wish.id})}>Legg til beskrivelse</button>}
        <button className="btn btn-link wish_btn wish_btn-right" onClick={() => deleteWish({listId, wishId: wish.id}) }>Fjern</button>
      </li>
    )
  }
}

class MyList extends React.Component {
  static get propTypes() {
    return {
      wishes: PropTypes.object,
      count: PropTypes.number,
      addWish: PropTypes.func,
      loadMyList: PropTypes.func,
      deleteWish: PropTypes.func,
      shareList: PropTypes.func,
      userName: PropTypes.string,
      logOut: PropTypes.func,
      listId: PropTypes.number,
      editUrl: PropTypes.func,
      editDescription: PropTypes.func
    }
  }

  componentDidMount() {
    this.props.loadMyList()
  }

  onKeyUp(e) {
    if (e.which === 13) {
      this.addWish()
    }
  }

  addWish() {
    if (this.wishInput.value) {
      this.props.addWish({listId: this.props.listId, title: this.wishInput.value})
      this.wishInput.value = ''
    }
  }

  render() {
    const { listId, deleteWish, editUrl, editDescription } = this.props
    return (
      <div>
        <header className="header">
          <h1 style={{ flex: 1 }}>Mine ønsker</h1>
          {this.props.userName && <div className="header_item">
            {this.props.userName}
          </div>}
          <button className={classNames('btn btn-default header_item')} onClick={() => this.props.shareList(this.props.listId)}>
            <span className="icon-share2 icon-before" />
            Del
          </button>
          <button className={classNames('btn btn-default header_item')} onClick={this.props.logOut}>
            <span className="icon-exit icon-before" />
            Logg ut
          </button>
        </header>
        <div className="wishList">
          <div className="input-group">
            <input className="form-control" placeholder="Jeg ønsker meg..." onKeyUp={:: this.onKeyUp} ref={el => (this.wishInput = el) } />
            <span className="input-group-btn">
              <button className="btn btn-default" type="button" onClick={:: this.addWish}>Legg til</button>
            </span>
          </div>
          <ul className="list-group">
            {map(this.props.wishes, wish => <Wish key={wish.id} {...{wish, listId, deleteWish, editUrl, editDescription}} />)}
          </ul>
        </div>
        <ReactTooltip />
      </div>
    )
  }
}

const mapStateToProps = state => ({
  wishes: state.myList.wishes || Immutable({}),
  userName: state.user.name,
  listId: state.myList.listId
})

const actions = {
  ...myListActions,
  logOut
}

export default connect(mapStateToProps, actions)(MyList)

