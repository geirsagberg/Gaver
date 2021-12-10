import { Icon, IconButton, Link, Paper, Typography } from '@mui/material';
import makeStyles from '@mui/styles/makeStyles';
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useActions, useAppState } from '~/overmind'
import { listItemStyles } from '~/theme'

const useStyles = makeStyles(listItemStyles)

const WishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const classes = useStyles()
  const {
    myList: { wishes, isDeleting },
    app: { isSavingOrLoading },
  } = useAppState()
  const {
    myList: { startEditingWish, confirmDeleteWish },
  } = useActions()
  const wish = wishes[wishId]
  return wish ? (
    <Paper className={classes.root}>
      <div className={classes.content}>
        <Typography variant="body1">{wish.title}</Typography>
        {wish.url && (
          <Link
            target="_blank"
            href={wish.url}
            variant="body2"
            className={classes.link}>
            {wish.url}
          </Link>
        )}
      </div>
      <Expander />
      <div>
        {isDeleting ? (
          <IconButton
            disabled={isSavingOrLoading}
            onClick={() => confirmDeleteWish(wishId)}
            size="large">
            <Icon>delete</Icon>
          </IconButton>
        ) : (
          <IconButton onClick={() => startEditingWish(wishId)} size="large">
            <Icon>edit</Icon>
          </IconButton>
        )}
      </div>
    </Paper>
  ) : null;
}

export default WishListItem
