import { combineReducers } from 'redux-starter-kit'
import myList from './myList'
import sharedList from './sharedList'
import auth from './auth'
import invitations from './invitations'
import routing from './routing'
import app from './app'

export default combineReducers({
  myList,
  sharedList,
  auth,
  invitations,
  routing,
  app
})
