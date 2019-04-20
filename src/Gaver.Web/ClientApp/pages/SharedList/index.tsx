import { map } from 'lodash-es'
import React, { FC } from 'react'
import Loading from '~/components/Loading'
import { useOvermind } from '~/overmind'
import { pageWidth } from '~/theme'
import { createStylesHook } from '~/utils/materialUtils'
import SharedWishListItem from './SharedWishListItem'

const useStyles = createStylesHook({
  root: {
    width: '100%',
    height: '100%',
    maxWidth: pageWidth,
    position: 'relative'
  },
  list: {
    padding: '1rem',
    height: '100%',
    position: 'relative',
    transition: 'all 0.5s',
    userSelect: 'none'
  }
})

const SharedListPage: FC = () => {
  const classes = useStyles()
  const {
    state: {
      routing: { currentSharedList }
    }
  } = useOvermind()
  return currentSharedList ? (
    <div className={classes.root}>
      <div className={classes.list}>
        {map(currentSharedList.wishes, wish => (
          <SharedWishListItem key={wish.id} wish={wish} />
        ))}
      </div>
    </div>
  ) : (
    <Loading />
  )
}

export default SharedListPage
