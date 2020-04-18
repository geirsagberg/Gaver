import { Icon, IconButton, Link, Paper, Typography } from '@material-ui/core'
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
    minHeight: '3rem',
  },
  content: {
    margin: '0.5rem 0',
    minWidth: '2rem',
    overflow: 'hidden',
    textOverflow: 'ellipsis',
  },
  link: {
    whiteSpace: 'nowrap',
  },
})

const WishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const classes = useStyles({})
  const {
    state: {
      myList: { wishes, isDeleting },
      app: { isSavingOrLoading },
    },
    actions: {
      myList: { startEditingWish, confirmDeleteWish },
    },
  } = useOvermind()
  const wish = wishes[wishId]
  return (
    <Paper className={classes.root}>
      <div className={classes.content}>
        <Typography variant="body1">{wish.title}</Typography>
        {wish.url && (
          <Link target="_blank" href={wish.url} variant="body2" className={classes.link}>
            {wish.url}
          </Link>
        )}
      </div>
      <Expander />
      <div>
        {isDeleting ? (
          <IconButton disabled={isSavingOrLoading} onClick={() => confirmDeleteWish(wishId)}>
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
