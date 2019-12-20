import React from 'react'
import { useNavContext } from '~/utils/hooks'
import { makeStyles, Fab, Icon, Dialog, DialogTitle, DialogContent, TextField } from '@material-ui/core'
import { pageWidth } from '~/theme'
import { useOvermind } from '~/overmind'

const useStyles = makeStyles({
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
  fabOuterWrapper: {
    width: '100%',
    maxWidth: pageWidth,
    position: 'fixed',
    bottom: 0,
    display: 'flex',
    justifyContent: 'flex-end'
  },
  addWishButton: {
    margin: '1rem'
  }
})

const AddGroupDialog = () => {
  const {
    state: {
      userGroups: { newGroup },
      app: { isSavingOrLoading }
    },
    actions: {
      userGroups: { cancelAddingGroup }
    }
  } = useOvermind()
  return newGroup ? (
    <Dialog fullWidth open={true} onClose={cancelAddingGroup}>
      <DialogTitle>Ny gruppe</DialogTitle>
      <DialogContent>
        <TextField
          label="Navn"
          autoFocus
          fullWidth
          required
          InputLabelProps={{ required: false }}
          margin="dense"
          disabled={isSavingOrLoading}
        />
      </DialogContent>
    </Dialog>
  ) : null
}

const UserGroupsPage = () => {
  const classes = useStyles({})
  useNavContext({ title: 'Mine grupper' }, [])
  const {
    actions: {
      userGroups: { startAddingGroup }
    }
  } = useOvermind()

  return (
    <div className={classes.root}>
      <div className={classes.fabOuterWrapper}>
        <div>
          <Fab title="Legg til gruppe" color="secondary" onClick={startAddingGroup} className={classes.addWishButton}>
            <Icon>add_icon</Icon>
          </Fab>
        </div>
      </div>
      <AddGroupDialog />
    </div>
  )
}

export default UserGroupsPage
