import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { map, size, values } from 'lodash-es'
import * as chatActions from 'store/chat'
import Immutable from 'seamless-immutable'
import classNames from 'classnames'
import './Chat.css'

class ChatMessage extends React.Component {
  static get propTypes () {
    return {
      message: PropTypes.object.isRequired,
      users: PropTypes.object.isRequired,
      userId: PropTypes.number.isRequired,
      firstOfUser: PropTypes.bool.isRequired
    }
  }

  render () {
    const isSelf = this.props.message.user === this.props.userId
    const { firstOfUser } = this.props
    return (
      <div
        className={classNames('chat_message', {
          'chat_message-first': firstOfUser,
          'chat_message-self': isSelf
        })}>
        {firstOfUser && <span className="chat_user">{this.props.users[this.props.message.user].name}:&nbsp;</span>}
        <div
          className={classNames('chat_innerMessage', {
            [`chat-user-${this.props.message.user % 5 + 1}`]: !isSelf
          })}>
          {this.props.message.text}
        </div>
      </div>
    )
  }
}

class Chat extends React.Component {
  static get propTypes () {
    return {
      addMessage: PropTypes.func.isRequired,
      messages: PropTypes.object.isRequired,
      loadMessages: PropTypes.func.isRequired,
      listId: PropTypes.number.isRequired,
      users: PropTypes.object.isRequired,
      userId: PropTypes.number.isRequired
    }
  }

  componentDidMount () {
    this.props.loadMessages(this.props.listId)
  }

  componentWillReceiveProps (nextProps) {
    if (size(this.props.messages) !== size(nextProps.messages)) {
      setTimeout(() => {
        this.chatMessages.scrollTop = this.chatMessages.scrollHeight
      }, 0)
    }
  }

  onKeyUp = (e) => {
    if (e.which === 13) {
      this.addMessage()
    }
  }

  addMessage = () => {
    if (this.chatInput.value) {
      this.props.addMessage({ listId: this.props.listId, text: this.chatInput.value })
      this.chatInput.value = ''
    }
  }

  render () {
    const { users, userId } = this.props
    const messages = values(this.props.messages)
    return (
      <div className="chat">
        <div
          className="chat_messages"
          ref={(el) => {
            this.chatMessages = el
          }}>
          {map(messages, (message, i) => (
            <ChatMessage
              {...{
                message,
                users,
                userId,
                firstOfUser: i === 0 || message.user !== messages[i - 1].user
              }}
              key={message.id}
            />
          ))}
        </div>
        <div className="chat_input input-group">
          <input
            className="form-control"
            placeholder="Skriv en melding..."
            ref={(el) => {
              this.chatInput = el
            }}
            onKeyUp={this.onKeyUp}
          />
          <span className="input-group-btn">
            <button className="btn btn-default" type="button" onClick={this.addMessage}>
              Send
            </button>
          </span>
        </div>
      </div>
    )
  }
}

const mapStateToProps = (state) => ({
  messages: state.chat.messages || Immutable({}),
  listId: +window.location.pathname.match(/list\/([0-9]+)/i)[1],
  users: state.chat.users || Immutable({}),
  userId: state.user.id || 0
})

const dispatchActions = {
  ...chatActions
}

export default connect(mapStateToProps, dispatchActions)(Chat)
