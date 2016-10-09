import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import { map } from 'utils/immutableExtensions'
import * as chatActions from 'store/chat'
import Immutable from 'seamless-immutable'

class ChatMessage extends React.Component {
  static get propTypes() {
    return {
      message: PropTypes.object.isRequired
    }
  }

  render() {
    return (
      <div className="chat_message">{this.props.message.text}</div>
    )
  }
}

class Chat extends React.Component {
  static get propTypes() {
    return {
      addMessage: PropTypes.func.isRequired,
      messages: PropTypes.object.isRequired,
      loadMessages: PropTypes.func.isRequired,
      listId: PropTypes.number.isRequired
    }
  }

  componentDidMount () {
    this.props.loadMessages(this.props.listId)
  }

  onKeyUp(e) {
    if (e.which === 13) {
      this.addMessage()
    }
  }

  addMessage() {
    if (this.chatInput.value) {
      this.props.addMessage({listId: this.props.listId, text: this.chatInput.value})
      this.chatInput.value = ''
    }
  }

  render() {
    return (
      <div className="chat">
        <div className="chat_messages">
          {this.props.messages::map(message => <ChatMessage {...{message}} key={message.id} />)}
        </div>
        <div className="chat_input input-group">
          <input className="form-control" placeholder="Skriv en melding..." ref={el => { this.chatInput = el }} onKeyUp={::this.onKeyUp} />
          <span className="input-group-btn">
            <button className="btn btn-default" type="button" onClick={:: this.addMessage}>Send</button>
          </span>
        </div>
      </div>
    )
  }
}

const mapStateToProps = state => ({
  messages: state.chat.messages || Immutable({}),
  listId: +window.location.pathname.match(/list\/([0-9]+)/i)[1]
})

const dispatchActions = {
  ...chatActions
}

export default connect(mapStateToProps, dispatchActions)(Chat)