import { Fab, Icon, Typography } from '@material-ui/core'
import { map, size } from 'lodash-es'
import React, { FC, useEffect } from 'react'
import { useOvermind } from '~/overmind'
import { pageWidth } from '~/theme'
import { createStylesHook } from '~/utils/materialUtils'
import AddWishDialog from './AddWishDialog'
import EditWishDialog from './EditWishDialog'
import WishListItem from './WishListItem'
import Color from 'color'
import classNames from 'classnames'
import { Container, Draggable } from 'react-smooth-dnd'
import Loading from '~/components/Loading'

const useStyles = createStylesHook(theme => ({
  root: {
    height: '100%',
    width: '100%',
    maxWidth: pageWidth,
    position: 'relative',
    paddingBottom: '4rem'
  },
  list: {
    padding: '1rem',
    height: '100%',
    borderRadius: theme.shape.borderRadius,
    position: 'relative',
    transition: 'all 0.5s',
    userSelect: 'none'
  },
  listEmpty: {
    background: Color(theme.palette.background.paper)
      .fade(0.5)
      .toString()
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
  },
  emptyList: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100%'
  },
  addWishHint: {
    position: 'absolute',
    bottom: '2rem',
    right: '5rem'
  }
}))

const MyListPage: FC = () => {
  const classes = useStyles()
  const {
    state: {
      myList: { orderedWishes, wishesLoaded }
    },
    actions: {
      myList: { startAddingWish, loadWishes, wishOrderChanged }
    }
  } = useOvermind()
  useEffect(() => {
    loadWishes()
  }, [])

  return wishesLoaded ? (
    <div className={classes.root}>
      <div className={classNames(classes.list, { [classes.listEmpty]: !size(orderedWishes) })}>
        {size(orderedWishes) ? (
          <Container
            style={{ paddingBottom: '4.5rem' }}
            onDrop={e => wishOrderChanged({ oldIndex: e.removedIndex, newIndex: e.addedIndex, wishId: e.payload })}
            getChildPayload={i => orderedWishes[i].id}>
            {map(orderedWishes, wish => (
              <Draggable key={wish.id}>
                <WishListItem wishId={wish.id} />
              </Draggable>
            ))}
          </Container>
        ) : (
          <Typography className={classes.addWishHint}>Legg til et ønske ➔</Typography>
        )}
      </div>

      <div className={classes.fabWrapper}>
        <Fab color="secondary" onClick={startAddingWish} className={classes.addWishButton}>
          <Icon>add_icon</Icon>
        </Fab>
      </div>

      <AddWishDialog />
      <EditWishDialog />
    </div>
  ) : (
    <Loading />
  )
}

export default MyListPage
