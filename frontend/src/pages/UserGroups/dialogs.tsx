import {
  Button,
  Checkbox,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  TextField,
  Typography,
} from '@mui/material';
import makeStyles from '@mui/styles/makeStyles';
import { map, without } from 'lodash-es'
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useActions, useAppState } from '~/overmind'
import { useFriends } from '~/overmind/sharedLists'
import { NewUserGroup, UserGroup } from '~/overmind/userGroups/state'
import { commonStyles } from '~/theme'

const useDialogStyles = makeStyles({
  actions: commonStyles.dialogActions,
})

interface GroupDetailsDialogProps {
  group: UserGroup | NewUserGroup | undefined
  updateGroup: (update: Partial<UserGroup>) => void
  onSave: () => void
  onCancel: () => void
  onDelete?: () => void
}

const GroupDetailsDialog: FC<GroupDetailsDialogProps> = ({
  group,
  updateGroup,
  onSave,
  onCancel,
  onDelete,
}) => {
  const {
    app: { isSavingOrLoading },
    auth: { user: currentUser },
  } = useAppState()
  const classes = useDialogStyles({})
  const users = useFriends()

  if (!group || !currentUser) return null

  const { name } = group

  const canSave = !isSavingOrLoading && name?.length

  return (
    <Dialog fullWidth open={true} onClose={onCancel}>
      <DialogTitle>Gruppe</DialogTitle>
      <DialogContent>
        <TextField
          label="Navn"
          autoFocus
          fullWidth
          required
          InputLabelProps={{ required: false }}
          margin="dense"
          disabled={isSavingOrLoading}
          value={name}
          onChange={(e) => updateGroup({ name: e.target.value })}
          inputProps={{ maxLength: 40 }}
        />
        <Typography variant="subtitle1" style={{ marginTop: '1rem' }}>
          Medlemmer:
        </Typography>
        <List style={{ margin: '0 -1.5rem' }}>
          <ListItem>
            <ListItemIcon>
              <Checkbox tabIndex={-1} disabled checked />
            </ListItemIcon>
            <ListItemText primary={currentUser.name + ' (meg)'} />
          </ListItem>
          {map(users, (user) => (
            <ListItem
              key={user.id}
              button
              onClick={() => {
                const userIds = group.userIds.includes(user.id)
                  ? without(group.userIds, user.id)
                  : [...group.userIds, user.id]
                updateGroup({ userIds })
              }}>
              <ListItemIcon>
                <Checkbox
                  tabIndex={-1}
                  checked={group.userIds.includes(user.id)}
                />
              </ListItemIcon>
              <ListItemText primary={user.name} />
            </ListItem>
          ))}
        </List>
      </DialogContent>
      <DialogActions className={classes.actions}>
        {'id' in group && group.createdByUserId === currentUser.id && (
          <Button disabled={isSavingOrLoading} onClick={onDelete}>
            Slett
          </Button>
        )}
        <Expander />
        <Button
          variant="contained"
          color="primary"
          disabled={!canSave}
          onClick={onSave}>
          Lagre
        </Button>
        <Button onClick={onCancel} disabled={isSavingOrLoading}>
          Avbryt
        </Button>
      </DialogActions>
    </Dialog>
  )
}

export const AddGroupDialog = () => {
  const {
    userGroups: { newGroup },
  } = useAppState()
  const {
    userGroups: { createUserGroup, cancelAddingGroup, updateNewGroup },
  } = useActions()
  return (
    <GroupDetailsDialog
      group={newGroup}
      onCancel={cancelAddingGroup}
      onSave={createUserGroup}
      updateGroup={updateNewGroup}
    />
  )
}

export const EditGroupDialog = () => {
  const {
    userGroups: { editingGroup },
  } = useAppState()
  const {
    userGroups: {
      updateUserGroup,
      cancelEditingGroup,
      updateEditingGroup,
      deleteEditingGroup,
    },
  } = useActions()
  return (
    <GroupDetailsDialog
      group={editingGroup}
      onCancel={cancelEditingGroup}
      onSave={updateUserGroup}
      updateGroup={updateEditingGroup}
      onDelete={deleteEditingGroup}
    />
  )
}
