import React from 'react'

export default class Loading extends React.Component {
  shouldComponentUpdate() {
    return false
  }

  render() {
    return (
      <div className="lds-roller">
        <div />
        <div />
        <div />
        <div />
        <div />
        <div />
        <div />
        <div />
      </div>
    )
  }
}
