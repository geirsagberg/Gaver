import { Icon, IconButton, Paper, Typography, Link } from '@material-ui/core'
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useOvermind } from '~/overmind'
import { createStylesHook } from '~/utils/materialUtils'

const useStyles = createStylesHook({
  root: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingLeft: '1rem',
    minHeight: '3rem'
  },
  content: {
    margin: '0.5rem 0'
  },
  actions: {}
})

const WishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const classes = useStyles({})
  const {
    state: {
      myList: { wishes, isDeleting }
    },
    actions: {
      myList: { startEditingWish, confirmDeleteWish }
    }
  } = useOvermind()
  const wish = wishes[wishId]
  return (
    <Paper className={classes.root}>
      <div className={classes.content}>
        <Typography variant="body1">{wish.title}</Typography>
        {wish.url && (
          <Link href={wish.url} variant="body2">
            {wish.url}
          </Link>
        )}
      </div>
      <Expander />
      <div className={classes.actions}>
        {isDeleting ? (
          <IconButton onClick={() => confirmDeleteWish(wishId)}>
            <Icon>delete</Icon>
          </IconButton>
        ) : (
          <IconButton onClick={() => startEditingWish(wishId)}>
            <Icon>edit</Icon>
          </IconButton>
        )}
      </div>
    </Paper>
  )
}

export default WishListItem
