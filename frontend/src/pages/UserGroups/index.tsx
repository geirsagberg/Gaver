import { Button, Icon, IconButton, Paper } from '@mui/material'
import makeStyles from '@mui/styles/makeStyles'
import classNames from 'classnames'
import Color from 'color'
import { map, size } from 'lodash-es'
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useActions, useAppState } from '~/overmind'
import { UserGroup } from '~/overmind/userGroups/state'
import { pageWidth } from '~/theme'
import { useNavContext } from '~/utils/hooks'
import { AddGroupDialog, EditGroupDialog } from './dialogs'

const useStyles = makeStyles((theme) => ({
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
    display: 'flex',
    flexDirection: 'column',
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
  background: {
    height: '100%',
    width: '100%',
    transition: 'all 0.5s',
    position: 'absolute',
    top: 0,
    left: 0,
    borderRadius: theme.shape.borderRadius,
    background: Color(theme.palette.background.paper).fade(0.5).toString(),
  },
  emptyBackground: {
    opacity: 0,
  },
}))

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
  const classes = useGroupItemStyles()
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
        <IconButton title="Rediger gruppe" onClick={() => startEditingGroup(value.id)} size="large">
          <Icon>edit</Icon>
        </IconButton>
      </div>
    </Paper>
  )
}

const AddGroupButton: FC = () => {
  const {
    userGroups: { startAddingGroup },
  } = useActions()
  return (
    <div
      style={{
        alignSelf: 'center',
        marginBottom: '1rem',
        position: 'sticky',
        bottom: '1rem',
      }}>
      <Button variant="contained" color="primary" onClick={startAddingGroup}>
        Legg til gruppe
      </Button>
    </div>
  )
}

const UserGroupsPage = () => {
  const classes = useStyles({})
  useNavContext({ title: 'Mine grupper' }, [])
  const {
    userGroups: { userGroups },
  } = useAppState()

  return (
    <div className={classes.root}>
      <div
        className={classNames(classes.background, {
          [classes.emptyBackground]: !!size(userGroups),
        })}></div>
      <div className={classes.list}>
        {map(userGroups, (g) => (
          <GroupItem key={g.id} value={g}>
            {g.name}
          </GroupItem>
        ))}
        <AddGroupButton />
      </div>
      <AddGroupDialog />
      <EditGroupDialog />
    </div>
  )
}

export default UserGroupsPage
