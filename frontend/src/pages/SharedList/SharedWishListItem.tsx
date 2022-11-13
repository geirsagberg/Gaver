import { Box, Button, Checkbox, Link, Paper, Typography } from '@mui/material'
import { FC } from 'react'
import Expander from '~/components/Expander'
import { useActions, useAppState } from '~/overmind'

const SharedWishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const {
    currentSharedList,
    sharedLists: { users },
    auth: { user },
  } = useAppState()
  const actions = useActions()

  const {
    sharedLists: { setBought },
  } = actions

  const wish = currentSharedList?.wishes[wishId]

  if (!wish || !user) return null

  const boughtByUser = wish.boughtByUserId ? users[wish.boughtByUserId] : null

  return (
    <Paper
      sx={{
        minHeight: '3rem',
        paddingLeft: '1rem',
        display: 'flex',
        alignItems: 'center',
        marginBottom: '1rem',
        justifyContent: 'space-between',
      }}>
      <Box
        sx={{
          margin: '0.5rem 0',
          minWidth: '2rem',
          textOverflow: 'ellipsis',
          overflow: 'hidden',
        }}>
        <Typography variant="body1">{wish.title}</Typography>
        {wish.url && (
          <Link
            target="_blank"
            href={wish.url}
            variant="body2"
            sx={{
              whiteSpace: 'nowrap',
            }}>
            {wish.url}
          </Link>
        )}
      </Box>
      <Expander />
      {boughtByUser ? (
        <>
          <Typography
            sx={{
              margin: '0.5rem 0',
              fontStyle: 'italic',
              '&:last-child': {
                marginRight: '1rem',
              },
              textAlign: 'right',
              textOverflow: 'ellipsis',
              overflow: 'hidden',
              marginLeft: '1rem',
            }}>
            Kjøpt av {boughtByUser.name}
          </Typography>
          {wish.boughtByUserId === user.id && (
            <Checkbox checked onClick={() => setBought({ wishId: wish.id, isBought: false })} />
          )}
        </>
      ) : (
        <Button
          color="primary"
          onClick={() => setBought({ wishId: wish.id, isBought: true })}
          sx={{
            marginRight: 6,
          }}>
          Kjøp
        </Button>
      )}
    </Paper>
  )
}

export default SharedWishListItem
