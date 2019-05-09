import { createStylesHook } from '~/utils/materialUtils'
import { pageWidth } from '~/theme'
import Color from 'color'

export const useStyles = createStylesHook(theme => ({
  root: {
    height: '100%',
    width: '100%',
    maxWidth: pageWidth,
    position: 'relative'
  },
  list: {
    padding: '1rem',
    height: '100%',
    position: 'relative',
    transition: 'all 0.5s',
    userSelect: 'none'
  },
  background: {
    height: '100%',
    width: '100%',
    transition: 'all 0.5s',
    position: 'absolute',
    top: 0,
    left: 0,
    borderRadius: theme.shape.borderRadius,
    background: Color(theme.palette.background.paper)
      .fade(0.5)
      .toString()
  },
  emptyBackground: {
    opacity: 0
  },
  fabOuterWrapper: {
    width: '100%',
    maxWidth: pageWidth,
    position: 'fixed',
    bottom: 0,
    display: 'flex',
    justifyContent: 'flex-end'
  },
  fabWrapper: {},
  addWishButton: {
    margin: '1rem'
  },
  emptyList: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100%'
  },
  addWishHint: {
    position: 'absolute',
    bottom: '2rem',
    right: '5rem'
  },
  droppable: {
    marginBottom: '5.5rem'
  },
  listItem: {
    marginBottom: '1rem'
  }
}))
