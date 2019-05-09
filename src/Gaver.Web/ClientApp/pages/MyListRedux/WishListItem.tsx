import { Icon, IconButton, Paper, Typography } from '@material-ui/core'
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
  actions: {}
})

const WishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const classes = useStyles()
  const {
    state: {
      myList: { wishes }
    },
    actions: {
      myList: { startEditingWish }
    }
  } = useOvermind()
  const wish = wishes[wishId]
  return (
    <Paper className={classes.root}>
      <Typography variant="body1">{wish.title}</Typography>
      <Expander />
      <IconButton onClick={() => startEditingWish(wishId)}>
        <Icon>edit</Icon>
      </IconButton>
    </Paper>
  )
}

export default WishListItem
