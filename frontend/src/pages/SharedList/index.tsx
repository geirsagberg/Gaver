import { BottomNavigation, BottomNavigationAction, Box, Icon, Paper, Typography } from '@mui/material'
import { map } from 'lodash-es'
import { FC, useState } from 'react'
import Chat from '~/components/Chat'
import Loading from '~/components/Loading'
import { useAppState } from '~/overmind'
import { pageWidth } from '~/theme'
import { useNavContext } from '~/utils/hooks'
import SharedWishListItem from './SharedWishListItem'

const SharedListPage: FC = () => {
  const { currentSharedOrderedWishes, currentSharedListOwner } = useAppState()
  useNavContext(
    {
      title: currentSharedListOwner ? currentSharedListOwner.name : '',
    },
    [currentSharedListOwner]
  )
  const [tab, setTab] = useState(0)
  return currentSharedOrderedWishes ? (
    <Box
      sx={{
        width: '100%',
        height: '100%',
        maxWidth: pageWidth,
        position: 'relative',
      }}>
      <Box
        sx={{
          padding: '1rem',
          height: '100%',
          position: 'relative',
          transition: 'all 0.5s',
          userSelect: 'none',
        }}>
        {map(currentSharedOrderedWishes, (wish) => (
          <SharedWishListItem key={wish.id} wishId={wish.id} />
        ))}
        {currentSharedOrderedWishes.length === 0 && (
          <Paper
            sx={{
              padding: '1rem',
            }}>
            <Typography>Ingen ønsker enda.</Typography>
          </Paper>
        )}
      </Box>
      <Chat />
      {false && (
        <BottomNavigation
          showLabels
          color="primary"
          sx={{
            position: 'fixed',
            bottom: 0,
            left: 0,
            right: 0,
          }}
          value={tab}
          onChange={(_, value) => setTab(value)}>
          <BottomNavigationAction label="Liste" icon={<Icon>list</Icon>} />
          <BottomNavigationAction label="Chat" icon={<Icon>chat</Icon>} />
        </BottomNavigation>
      )}
    </Box>
  ) : (
    <Loading />
  )
}

export default SharedListPage
