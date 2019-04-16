import { Icon, IconButton, Paper, Typography } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import React, { FC } from 'react'
import { useOvermind } from '~/overmind'

const useStyles = makeStyles({
  root: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    '&:hover': {
      '& $actions': {
        display: 'initial'
      }
    },
    padding: '1rem',
    minHeight: '5rem',
    marginBottom: '1rem'
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
    <Paper className={classes.root}>
      <Typography variant="h6">{wish.title}</Typography>
      <div className={classes.actions}>
        <IconButton onClick={() => startEditingWish(wishId)}>
          <Icon>edit</Icon>
        </IconButton>
        <IconButton onClick={() => confirmDeleteWish(wishId)}>
          <Icon>delete</Icon>
        </IconButton>
      </div>
    </Paper>
  )
}

export default WishListItem
