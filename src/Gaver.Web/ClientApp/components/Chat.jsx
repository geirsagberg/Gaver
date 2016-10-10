import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import { map, size } from 'utils/immutableExtensions'
import * as chatActions from 'store/chat'
import Immutable from 'seamless-immutable'

class ChatMessage extends React.Component {
  static get propTypes() {
    return {
      message: PropTypes.object.isRequired,
      users: PropTypes.object.isRequired
    }
  }

  render() {
    return (
      <div className="chat_message">
        <span className="chat_user">{this.props.users[this.props.message.user].name}:&nbsp;</span>
        {this.props.message.text}
      </div>
    )
  }
}

class Chat extends React.Component {
  static get propTypes() {
    return {
      addMessage: PropTypes.func.isRequired,
      messages: PropTypes.object.isRequired,
      loadMessages: PropTypes.func.isRequired,
      listId: PropTypes.number.isRequired,
      users: PropTypes.object.isRequired
    }
  }

  componentDidMount () {
    this.props.loadMessages(this.props.listId)
  }

  componentWillReceiveProps (nextProps) {
    if (this.props.messages::size() !== nextProps.messages::size()) {
      setTimeout(() => {
        this.chatMessages.scrollTop = this.chatMessages.scrollHeight
      }, 0)
    }
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
    const { users } = this.props
    return (
      <div className="chat">
        <div className="chat_messages" ref={el => { this.chatMessages = el }}>
          {this.props.messages::map(message => <ChatMessage {...{message, users}} key={message.id} />)}
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
  listId: +window.location.pathname.match(/list\/([0-9]+)/i)[1],
  users: state.chat.users || Immutable({})
})

const dispatchActions = {
  ...chatActions
}

export default connect(mapStateToProps, dispatchActions)(Chat)