import { BottomNavigation, BottomNavigationAction, Icon, makeStyles } from '@material-ui/core'
import { map } from 'lodash-es'
import React, { FC, useState } from 'react'
import Loading from '~/components/Loading'
import { useOvermind } from '~/overmind'
import { pageWidth } from '~/theme'
import SharedWishListItem from './SharedWishListItem'

const useStyles = makeStyles(theme => ({
  root: {
    width: '100%',
    height: '100%',
    maxWidth: pageWidth,
    position: 'relative'
  },
  list: {
    padding: '1rem',
    height: '100%',
    position: 'relative',
    transition: 'all 0.5s',
    userSelect: 'none'
  },
  bottomNav: {
    position: 'fixed',
    bottom: 0,
    left: 0,
    right: 0
  }
}))

const SharedListPage: FC = () => {
  const classes = useStyles({})
  const {
    state: { currentSharedOrderedWishes }
  } = useOvermind()
  const [tab, setTab] = useState(0)
  return currentSharedOrderedWishes ? (
    <div className={classes.root}>
      <div className={classes.list}>
        {map(currentSharedOrderedWishes, wish => (
          <SharedWishListItem key={wish.id} wishId={wish.id} />
        ))}
      </div>
      <BottomNavigation
        showLabels
        color="primary"
        className={classes.bottomNav}
        value={tab}
        onChange={(_, value) => setTab(value)}>
        <BottomNavigationAction label="Liste" icon={<Icon>list</Icon>} />
        <BottomNavigationAction label="Chat" icon={<Icon>chat</Icon>} />
      </BottomNavigation>
    </div>
  ) : (
    <Loading />
  )
}

export default SharedListPage
