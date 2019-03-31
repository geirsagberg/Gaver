import { Fab, Icon, List } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import { map } from 'lodash-es'
import React, { FC, useEffect } from 'react'
import { useOvermind } from '~/overmind'
import WishListItem from './WishListItem'
import AddWishDialog from './AddWishDialog'
import EditWishDialog from './EditWishDialog'
import { pageWidth } from '~/theme'

const useStyles = makeStyles({
  root: {
    height: '100%',
    width: '100%',
    maxWidth: pageWidth,
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

const MyListPage: FC = () => {
  const classes = useStyles()
  const {
    state: {
      myList: { wishes }
    },
    actions: {
      myList: { startAddingWish, loadWishes }
    }
  } = useOvermind()
  useEffect(() => {
    loadWishes()
  }, [])

  return (
    <div className={classes.root}>
      <List>
        {map(wishes, wish => (
          <WishListItem key={wish.id} wishId={wish.id} />
        ))}
      </List>

      <div className={classes.fabWrapper}>
        <Fab color="secondary" onClick={startAddingWish} className={classes.addWishButton}>
          <Icon>add_icon</Icon>
        </Fab>
      </div>

      <AddWishDialog />
      <EditWishDialog />
    </div>
  )
}

export default MyListPage
