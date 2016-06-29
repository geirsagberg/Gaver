const ADD_WISH = 'gaver/currentList/ADD_WISH'

export default (state = [], action) => {
  switch (action.type) {
    case ADD_WISH:
      return [
        ...state,
        action.wish
      ]
  }
  return state
}

export function addWish (wish) {
  return {
    type: ADD_WISH,
    wish
  }
}
