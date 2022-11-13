import { Box, Icon, IconButton, Link, Paper, Typography } from '@mui/material'
import { FC } from 'react'
import Expander from '~/components/Expander'
import { useActions, useAppState } from '~/overmind'
import { listItemStyles } from '~/theme'

const WishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const {
    myList: { wishes, isDeleting },
    app: { isSavingOrLoading },
  } = useAppState()
  const {
    myList: { startEditingWish, confirmDeleteWish },
  } = useActions()
  const wish = wishes[wishId]
  return wish ? (
    <Paper sx={listItemStyles.root}>
      <Box sx={listItemStyles.content}>
        <Typography variant="body1">{wish.title}</Typography>
        {wish.url && (
          <Link target="_blank" href={wish.url} variant="body2" sx={listItemStyles.link}>
            {wish.url}
          </Link>
        )}
      </Box>
      <Expander />
      <div>
        {isDeleting ? (
          <IconButton disabled={isSavingOrLoading} onClick={() => confirmDeleteWish(wishId)} size="large">
            <Icon>delete</Icon>
          </IconButton>
        ) : (
          <IconButton onClick={() => startEditingWish(wishId)} size="large">
            <Icon>edit</Icon>
          </IconButton>
        )}
      </div>
    </Paper>
  ) : null
}

export default WishListItem
