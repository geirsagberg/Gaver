import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import * as currentListActions from '../store/currentList'

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
      <h1>Mine ønsker</h1>
      <div className="input-group">
        <input className="form-control" placeholder="Jeg ønsker meg..." onKeyUp={::this.onKeyUp} ref={el => (this.wishInput = el)} />
        <span className="input-group-btn">
          <button className="btn btn-default" type="button" onClick={::this.addWish}>Legg til</button>
        </span>
      </div>
      <ul className="list-group">
        {this.props.currentList.map(wish =>
          <li className="list-group-item">{wish.title}</li>
          )}
      </ul>
    </div>
    )
  }
}

Home.propTypes = {
  addWish: PropTypes.func,
  loadData: PropTypes.func,
  currentList: PropTypes.array
}

const mapStateToProps = state => ({
  currentList: state.currentList
})

export default connect(mapStateToProps, currentListActions)(Home)

