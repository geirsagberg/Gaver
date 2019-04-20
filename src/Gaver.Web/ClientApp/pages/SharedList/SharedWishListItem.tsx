import { Paper, Typography } from '@material-ui/core'
import React, { FC } from 'react'
import { SharedWishModel } from '~/types/data'
import { createStylesHook } from '~/utils/materialUtils'

const useStyles = createStylesHook({
  root: {
    minHeight: '3rem',
    paddingLeft: '1rem',
    display: 'flex',
    alignItems: 'center',
    marginBottom: '1rem'
  }
})

const SharedWishListItem: FC<{ wish: SharedWishModel }> = ({ wish }) => {
  const classes = useStyles()

  return (
    <Paper className={classes.root}>
      <Typography variant="body1">{wish.title}</Typography>
    </Paper>
  )
}

export default SharedWishListItem
