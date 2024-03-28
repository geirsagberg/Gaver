import { Box, Button } from '@mui/material'
import Color from 'color'
import { map, size } from 'lodash-es'
import { DragDropContext, Draggable, Droppable } from 'react-beautiful-dnd'
import Loading from '~/components/Loading'
import { useActions, useAppState } from '~/overmind'
import { pageWidth } from '~/theme'
import { useNavContext } from '~/utils/hooks'
import AddWishDialog from './WishDetails/AddWishDialog'
import EditWishDialog from './WishDetails/EditWishDialog'
import WishListItem from './WishListItem'

const AddWishButton = () => {
  const {
    myList: { startAddingWish },
  } = useActions()
  return (
    <div
      style={{
        alignSelf: 'center',
        marginBottom: '1rem',
        position: 'sticky',
        bottom: '1rem',
      }}>
      <Button variant="contained" color="primary" onClick={startAddingWish}>
        Legg til ønske
      </Button>
    </div>
  )
}

const MyListPage = () => {
  const {
    myList: { orderedWishes, wishesLoaded },
  } = useAppState()
  const {
    myList: { wishOrderChanged },
  } = useActions()
  useNavContext({ title: 'Mine ønsker' }, [])

  return wishesLoaded ? (
    <Box
      sx={{
        height: '100%',
        width: '100%',
        maxWidth: pageWidth,
        position: 'relative',
      }}>
      <Box
        sx={(theme) => ({
          height: '100%',
          width: '100%',
          transition: 'all 0.5s',
          position: 'absolute',
          top: 0,
          left: 0,
          borderRadius: theme.shape.borderRadius,
          background: Color(theme.palette.background.paper).fade(0.5).toString(),
          opacity: !!size(orderedWishes) ? 0 : 1,
        })}></Box>
      <Box
        sx={{
          padding: '1rem',
          height: '100%',
          position: 'relative',
          transition: 'all 0.5s',
          userSelect: 'none',
          display: 'flex',
          flexDirection: 'column',
        }}>
        <DragDropContext
          onDragEnd={(result) => {
            if (!result.destination) {
              return
            }
            wishOrderChanged({
              oldIndex: result.source.index,
              newIndex: result.destination.index,
              wishId: +result.draggableId,
            })
          }}>
          <Droppable droppableId="myList">
            {(provided) => (
              <div {...provided.droppableProps} ref={provided.innerRef}>
                {map(orderedWishes, (wish, i) => (
                  <Draggable key={wish.id} draggableId={wish.id!.toString()} index={i}>
                    {(provided) => (
                      <Box
                        ref={provided.innerRef}
                        sx={{
                          marginBottom: '1rem',
                        }}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}>
                        <WishListItem wishId={wish.id!} />
                      </Box>
                    )}
                  </Draggable>
                ))}
                {provided.placeholder}
              </div>
            )}
          </Droppable>
        </DragDropContext>
        <AddWishButton />
      </Box>
      <AddWishDialog />
      <EditWishDialog />
    </Box>
  ) : (
    <Loading />
  )
}

export default MyListPage
