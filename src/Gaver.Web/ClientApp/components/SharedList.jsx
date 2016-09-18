import React from 'react'

class SharedList extends React.Component {
  render () {
    return (
      <div>
        <header style={{...flex, alignItems: 'center'}}>
          <h1 style={{ flex: 1 }}>Mine ønsker</h1>
          {this.props.userName && <div style={defaultMargin}>
            {this.props.userName}
          </div>}
          <div style={defaultMargin} data-tip={this.props.users.join(', ') }>
            {this.props.count} <span className="icon-users" />
          </div>
          <button className="btn btn-default" style={defaultMargin} onClick={this.props.shareList}>
            <span className="icon-share2 icon-before" />
            Del
          </button>
          <button className="btn btn-default" style={{...defaultMargin, marginRight: 0}} onClick={this.props.logOut}>
            <span className="icon-exit icon-before" />
            Logg ut
          </button>
        </header >
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
          ) }
        </ul>
        <ReactTooltip />
      </div>
    )
  }
}