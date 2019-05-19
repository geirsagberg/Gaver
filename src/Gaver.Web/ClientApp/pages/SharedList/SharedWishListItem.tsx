import { Button, Checkbox, Link, Paper, Typography } from '@material-ui/core'
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useOvermind } from '~/overmind'
import { createStylesHook } from '~/utils/materialUtils'

const useStyles = createStylesHook({
  root: {
    minHeight: '3rem',
    paddingLeft: '1rem',
    display: 'flex',
    alignItems: 'center',
    marginBottom: '1rem',
    justifyContent: 'space-between'
  },
  content: {
    margin: '0.5rem 0'
  },
  boughtBy: {
    fontStyle: 'italic',
    '&:last-child': {
      marginRight: '1rem'
    },
    textAlign: 'right'
  },
  buyButton: {
    marginRight: 6
  }
})

const SharedWishListItem: FC<{ wishId: number }> = ({ wishId }) => {
  const classes = useStyles()
  const {
    actions: {
      sharedLists: { setBought }
    },
    state: {
      currentSharedList,
      sharedLists: { users },
      auth: { user }
    }
  } = useOvermind()

  const wish = currentSharedList.wishes[wishId]

  const boughtByUser = wish.boughtByUserId ? users[wish.boughtByUserId] : null

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
      {boughtByUser ? (
        <>
          <Typography className={classes.boughtBy}>Kjøpt av {boughtByUser.name}</Typography>
          {wish.boughtByUserId === user.id && (
            <Checkbox checked onClick={() => setBought({ wishId: wish.id, isBought: false })} />
          )}
        </>
      ) : (
        <Button
          color="primary"
          onClick={() => setBought({ wishId: wish.id, isBought: true })}
          className={classes.buyButton}>
          Kjøp
        </Button>
      )}
    </Paper>
  )
}

export default SharedWishListItem
