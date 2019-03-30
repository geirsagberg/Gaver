import { Icon, IconButton, ListItem, ListItemSecondaryAction, ListItemText } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import React, { FC } from 'react'
import { useOvermind } from '~/overmind'

const useStyles = makeStyles({
  root: {
    '&:hover': {
      '& $actions': {
        display: 'initial'
      }
    }
  },
  actions: {
    display: 'none'
  }
})

const WishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const classes = useStyles()
  const {
    state: {
      myList: { wishes }
    },
    actions: {
      myList: { startEditingWish, confirmDeleteWish }
    }
  } = useOvermind()
  const wish = wishes[wishId]
  return (
    <ListItem classes={{ container: classes.root }}>
      <ListItemText primary={wish.title} />
      <ListItemSecondaryAction className={classes.actions}>
        <IconButton onClick={() => startEditingWish(wishId)}>
          <Icon>edit</Icon>
        </IconButton>
        <IconButton onClick={() => confirmDeleteWish(wishId)}>
          <Icon>delete</Icon>
        </IconButton>
      </ListItemSecondaryAction>
    </ListItem>
  )
}

export default WishListItem
