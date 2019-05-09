import { Fab, Icon, Typography } from '@material-ui/core'
import classNames from 'classnames'
import { map, size } from 'lodash-es'
import React, { FC } from 'react'
import Loading from '~/components/Loading'
import { useOvermind } from '~/overmind'
import AddWishDialog from './AddWishDialog'
import EditWishDialog from './EditWishDialog'
import { useStyles } from './styles'
import WishListItem from './WishListItem'

const MyListPage: FC = () => {
  const classes = useStyles()
  const {
    state: {
      myList: { orderedWishes, wishesLoaded }
    },
    actions: {
      myList: { startAddingWish }
    }
  } = useOvermind()

  return wishesLoaded ? (
    <div className={classes.root}>
      <div className={classNames(classes.background, { [classes.emptyBackground]: !!size(orderedWishes) })}>
        <Typography className={classes.addWishHint}>Legg til et ønske ➔</Typography>
      </div>
      <div className={classNames(classes.list)}>
        <div>
          {map(orderedWishes, (wish, i) => (
            <div key={i} className={classes.listItem}>
              <WishListItem wishId={wish.id} />
            </div>
          ))}
        </div>
      </div>

      <div className={classes.fabOuterWrapper}>
        <div className={classes.fabWrapper}>
          <Fab color="secondary" onClick={startAddingWish} className={classes.addWishButton}>
            <Icon>add_icon</Icon>
          </Fab>
        </div>
      </div>

      <AddWishDialog />
      <EditWishDialog />
    </div>
  ) : (
    <Loading />
  )
}

export default MyListPage
