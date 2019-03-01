import React, { FC, useState, useEffect } from 'react'
import { makeStyles } from '@material-ui/styles'
import { Wish } from '~/types/data'
import {
  List,
  ListItem,
  ListItemText,
  Fab,
  Icon,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  TextField,
  DialogActions,
  Button,
  Portal
} from '@material-ui/core'
import { useOvermind } from '~/overmind'
import { map } from 'lodash-es'

const useStyles = makeStyles({
  root: {
    height: '100%',
    width: '100%',
    maxWidth: 600,
    position: 'relative'
  },
  fabWrapper: {
    position: 'absolute',
    bottom: '1rem',
    right: '1rem',
    width: 56,
    height: 56
  },
  addWishButton: {
    position: 'fixed'
  }
})

const WishListItem: FC<Wish> = wish => (
  <ListItem>
    <ListItemText primary={wish.title} />
  </ListItem>
)

const MyListPage: FC = () => {
  const classes = useStyles()
  const {
    state: {
      myList: { isAddingWish, newWish, wishes }
    },
    actions: {
      myList: { addWish, startAddingWish, cancelAddingWish, setTitle, loadWishes }
    }
  } = useOvermind()
  useEffect(() => {
    loadWishes()
  }, [])

  return (
    <div className={classes.root}>
      <List>
        {map(wishes, wish => (
          <WishListItem key={wish.id} {...wish} />
        ))}
      </List>

      <div className={classes.fabWrapper}>
        <Fab color="secondary" onClick={startAddingWish} className={classes.addWishButton}>
          <Icon>add_icon</Icon>
        </Fab>
      </div>

      <Dialog fullWidth open={isAddingWish} onClose={cancelAddingWish}>
        <DialogTitle>Nytt ønske</DialogTitle>
        <DialogContent>
          <DialogContentText>Hva ønsker du deg?</DialogContentText>
          <TextField
            onChange={event => setTitle(event.currentTarget.value)}
            autoFocus
            fullWidth
            value={newWish.title}
          />
        </DialogContent>
        <DialogActions>
          <Button variant="contained" color="primary" onClick={addWish}>
            Lagre
          </Button>
          <Button onClick={cancelAddingWish}>Avbryt</Button>
        </DialogActions>
      </Dialog>
    </div>
  )
}

export default MyListPage
