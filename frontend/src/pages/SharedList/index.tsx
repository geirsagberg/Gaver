import { BottomNavigation, BottomNavigationAction, Icon, Paper, Typography } from '@mui/material';
import makeStyles from '@mui/styles/makeStyles';
import { map } from 'lodash-es'
import React, { FC, useState } from 'react'
import Chat from '~/components/Chat'
import Loading from '~/components/Loading'
import { useAppState } from '~/overmind'
import { pageWidth } from '~/theme'
import { useNavContext } from '~/utils/hooks'
import SharedWishListItem from './SharedWishListItem'

const useStyles = makeStyles({
  root: {
    width: '100%',
    height: '100%',
    maxWidth: pageWidth,
    position: 'relative',
  },
  list: {
    padding: '1rem',
    height: '100%',
    position: 'relative',
    transition: 'all 0.5s',
    userSelect: 'none',
  },
  bottomNav: {
    position: 'fixed',
    bottom: 0,
    left: 0,
    right: 0,
  },
  empty: {
    padding: '1rem',
  },
})

const SharedListPage: FC = () => {
  const classes = useStyles({})
  const { currentSharedOrderedWishes, currentSharedListOwner } = useAppState()
  useNavContext(
    {
      title: currentSharedListOwner ? currentSharedListOwner.name : '',
    },
    [currentSharedListOwner]
  )
  const [tab, setTab] = useState(0)
  return currentSharedOrderedWishes ? (
    <div className={classes.root}>
      <div className={classes.list}>
        {map(currentSharedOrderedWishes, (wish) => (
          <SharedWishListItem key={wish.id} wishId={wish.id} />
        ))}
        {currentSharedOrderedWishes.length === 0 && (
          <Paper className={classes.empty}>
            <Typography>Ingen ønsker enda.</Typography>
          </Paper>
        )}
      </div>
      <Chat />
      {false && (
        <BottomNavigation
          showLabels
          color="primary"
          className={classes.bottomNav}
          value={tab}
          onChange={(_, value) => setTab(value)}>
          <BottomNavigationAction label="Liste" icon={<Icon>list</Icon>} />
          <BottomNavigationAction label="Chat" icon={<Icon>chat</Icon>} />
        </BottomNavigation>
      )}
    </div>
  ) : (
    <Loading />
  )
}

export default SharedListPage
