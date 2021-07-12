import { Fab, Icon, IconButton, makeStyles, Paper } from '@material-ui/core'
import { map, size } from 'lodash-es'
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useActions, useAppState } from '~/overmind'
import { UserGroup } from '~/overmind/userGroups/state'
import { pageWidth } from '~/theme'
import { useNavContext } from '~/utils/hooks'
import { AddGroupDialog, EditGroupDialog } from './dialogs'

const useStyles = makeStyles({
  root: {
    height: '100%',
    width: '100%',
    maxWidth: pageWidth,
    position: 'relative',
  },
  list: {
    padding: '1rem',
    height: '100%',
    position: 'relative',
    transition: 'all 0.5s',
    userSelect: 'none',
  },
  fabOuterWrapper: {
    width: '100%',
    maxWidth: pageWidth,
    position: 'fixed',
    bottom: 0,
    display: 'flex',
    justifyContent: 'flex-end',
  },
  addWishButton: {
    margin: '1rem',
  },
})

const useGroupItemStyles = makeStyles({
  root: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingLeft: '1rem',
    minHeight: '3rem',
    marginBottom: '1rem',
  },
  content: {
    margin: '0.5rem 0',
    minWidth: '2rem',
    overflow: 'hidden',
    textOverflow: 'ellipsis',
  },
})

const GroupItem: FC<{ value: UserGroup }> = ({ value }) => {
  const classes = useGroupItemStyles({})
  const {
    userGroups: { startEditingGroup },
  } = useActions()

  return (
    <Paper className={classes.root}>
      <div className={classes.content}>
        <strong>{value.name}</strong>
        <div>{value.userIds.length} medlemmer</div>
      </div>
      <Expander />
      <div>
        <IconButton
          title="Rediger gruppe"
          onClick={() => startEditingGroup(value.id)}>
          <Icon>edit</Icon>
        </IconButton>
      </div>
    </Paper>
  )
}

const UserGroupsPage = () => {
  const classes = useStyles({})
  useNavContext({ title: 'Mine grupper' }, [])
  const {
    userGroups: { userGroups },
  } = useAppState()
  const {
    userGroups: { startAddingGroup },
  } = useActions()

  return (
    <div className={classes.root}>
      <div className={classes.list}>
        {userGroups &&
          (size(userGroups) ? (
            map(userGroups, (g) => (
              <GroupItem key={g.id} value={g}>
                {g.name}
              </GroupItem>
            ))
          ) : (
            <div>Ingen grupper</div>
          ))}
      </div>
      <div className={classes.fabOuterWrapper}>
        <div>
          <Fab
            title="Legg til gruppe"
            color="secondary"
            onClick={startAddingGroup}
            className={classes.addWishButton}>
            <Icon>add_icon</Icon>
          </Fab>
        </div>
      </div>
      <AddGroupDialog />
      <EditGroupDialog />
    </div>
  )
}

export default UserGroupsPage
