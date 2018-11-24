import React from 'react'
import { connect } from 'react-redux'
import { map, size, values } from 'lodash-es'
import * as chatActions from '~/store/chat'
import classNames from 'classnames'
import './Chat.css'

type ChatMessageProps = {
  message
  users
  userId
  firstOfUser
}

class ChatMessage extends React.Component<ChatMessageProps> {
  render() {
    const isSelf = this.props.message.user === this.props.userId
    const { firstOfUser } = this.props
    return (
      <div
        className={classNames('chat_message', {
          'chat_message-first': firstOfUser,
          'chat_message-self': isSelf
        })}>
        {firstOfUser && (
          <span className="chat_user">
            {this.props.users[this.props.message.user].name}
            :&nbsp;
          </span>
        )}
        <div
          className={classNames('chat_innerMessage', {
            [`chat-user-${(this.props.message.user % 5) + 1}`]: !isSelf
          })}>
          {this.props.message.text}
        </div>
      </div>
    )
  }
}

type ChatProps = {
  addMessage
  messages
  loadMessages
  listId
  users
  userId
}

class Chat extends React.Component<ChatProps> {
  chatMessages
  chatInput
  componentDidMount() {
    this.props.loadMessages(this.props.listId)
  }

  componentWillReceiveProps(nextProps) {
    if (size(this.props.messages) !== size(nextProps.messages)) {
      setTimeout(() => {
        this.chatMessages.scrollTop = this.chatMessages && this.chatMessages.scrollHeight
      }, 0)
    }
  }

  onKeyUp = e => {
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

  render() {
    const { users, userId } = this.props
    const messages = values(this.props.messages)
    return (
      <div className="chat">
        <div
          className="chat_messages"
          ref={el => {
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
            ref={el => {
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

const mapStateToProps = state => ({
  messages: state.chat.messages || {},
  listId: +window.location.pathname.match(/list\/([0-9]+)/i)[1],
  users: state.chat.users || {},
  userId: state.user.id || 0
})

const dispatchActions = {
  ...chatActions
}

export default connect(
  mapStateToProps,
  dispatchActions
)(Chat)
