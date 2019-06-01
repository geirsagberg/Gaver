import { Fab, Icon, Typography } from '@material-ui/core'
import classNames from 'classnames'
import Color from 'color'
import { map, size } from 'lodash-es'
import React, { FC } from 'react'
import { DragDropContext, Draggable, Droppable } from 'react-beautiful-dnd'
import Loading from '~/components/Loading'
import { useOvermind } from '~/overmind'
import { pageWidth } from '~/theme'
import { createStylesHook } from '~/utils/materialUtils'
import AddWishDialog from './WishDetails/AddWishDialog'
import EditWishDialog from './WishDetails/EditWishDialog'
import WishListItem from './WishListItem'

const useStyles = createStylesHook(theme => ({
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

const MyListPage: FC = () => {
  const classes = useStyles({})
  const {
    state: {
      myList: { orderedWishes, wishesLoaded }
    },
    actions: {
      myList: { startAddingWish, wishOrderChanged }
    }
  } = useOvermind()

  return wishesLoaded ? (
    <div className={classes.root}>
      <div className={classNames(classes.background, { [classes.emptyBackground]: !!size(orderedWishes) })}>
        <Typography className={classes.addWishHint}>Legg til et ønske ➔</Typography>
      </div>
      <div className={classNames(classes.list)}>
        <DragDropContext
          onDragEnd={result => {
            if (!result.destination) {
              return
            }
            wishOrderChanged({
              oldIndex: result.source.index,
              newIndex: result.destination.index,
              wishId: +result.draggableId
            })
          }}>
          <Droppable droppableId="myList">
            {provided => (
              <div {...provided.droppableProps} ref={provided.innerRef} className={classes.droppable}>
                {map(orderedWishes, (wish, i) => (
                  <Draggable key={wish.id} draggableId={wish.id.toString()} index={i}>
                    {provided => (
                      <div
                        ref={provided.innerRef}
                        className={classes.listItem}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}>
                        <WishListItem wishId={wish.id} />
                      </div>
                    )}
                  </Draggable>
                ))}
                {provided.placeholder}
              </div>
            )}
          </Droppable>
        </DragDropContext>
      </div>

      <div className={classes.fabOuterWrapper}>
        <div className={classes.fabWrapper}>
          <Fab color="secondary" onClick={startAddingWish} className={classes.addWishButton}>
            <Icon>add_icon</Icon>
          </Fab>
        </div>
      </div>

      <AddWishDialog />
      <EditWishDialog />
    </div>
  ) : (
    <Loading />
  )
}

export default MyListPage
