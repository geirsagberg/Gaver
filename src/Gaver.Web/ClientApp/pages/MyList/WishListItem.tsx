import { Icon, IconButton, Link, makeStyles, Paper, Typography } from '@material-ui/core'
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useOvermind } from '~/overmind'
import { listItemStyles } from '~/theme'

const useStyles = makeStyles(listItemStyles)

const WishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const classes = useStyles()
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
  return wish ? (
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
  ) : null
}

export default WishListItem
