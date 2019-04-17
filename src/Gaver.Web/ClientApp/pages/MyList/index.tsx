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
import { Flipper, Flipped } from 'react-flip-toolkit'

const useStyles = createStylesHook(theme => ({
  root: {
    height: '100%',
    width: '100%',
    maxWidth: pageWidth,
    position: 'relative'
  },
  list: {
    padding: '1rem 0',
    height: '100%',
    background: Color(theme.palette.background.paper)
      .fade(0.5)
      .toString(),
    borderRadius: theme.shape.borderRadius,
    position: 'relative',
    transition: 'all 0.5s',
    userSelect: 'none'
  },
  listNotEmpty: {
    background: 'transparent'
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
      <div className={classNames(classes.list, { [classes.listNotEmpty]: !!size(wishes) })}>
        {size(wishes) ? (
          <Flipper flipKey={map(wishes, wish => wish.id).join()}>
            {map(wishes, wish => (
              <Flipped key={wish.id} flipId={wish.id.toString()}>
                <div draggable>
                  <WishListItem wishId={wish.id} />
                </div>
              </Flipped>
            ))}
          </Flipper>
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
  )
}

export default MyListPage
