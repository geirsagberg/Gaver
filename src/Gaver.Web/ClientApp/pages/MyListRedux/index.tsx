import { Fab, Icon, Typography } from '@material-ui/core'
import classNames from 'classnames'
import { map, size } from 'lodash-es'
import React, { FC } from 'react'
import { connect } from 'react-redux'
import Loading from '~/components/Loading'
import { startAddingWish } from '~/redux/reducers/myList/actions'
import { createMapDispatchToProps } from '~/redux/reduxUtils'
import { ReduxState } from '~/redux/store'
import AddWishDialog from './AddWishDialog'
import EditWishDialog from './EditWishDialog'
import { useStyles } from './styles'
import WishListItem from './WishListItem'

const mapStateToProps = (state: ReduxState) => ({
  orderedWishes: state.myList.wishesOrder.map(i => state.myList.wishes[i]),
  wishesLoaded: state.myList.wishesLoaded
})

const mapDispatchToProps = createMapDispatchToProps({
  startAddingWish
})

type Props = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>

const MyListPage: FC<Props> = ({ orderedWishes, wishesLoaded, startAddingWish }) => {
  const classes = useStyles()

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

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(MyListPage)
