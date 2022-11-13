import { Box, Button, Icon, IconButton, Paper } from '@mui/material'
import Color from 'color'
import { map, size } from 'lodash-es'
import { FC } from 'react'
import Expander from '~/components/Expander'
import { useActions, useAppState } from '~/overmind'
import { UserGroup } from '~/overmind/userGroups/state'
import { pageWidth } from '~/theme'
import { useNavContext } from '~/utils/hooks'
import { AddGroupDialog, EditGroupDialog } from './dialogs'

const GroupItem = ({ value }: { value: UserGroup }) => {
  const {
    userGroups: { startEditingGroup },
  } = useActions()

  return (
    <Paper
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        paddingLeft: '1rem',
        minHeight: '3rem',
        marginBottom: '1rem',
      }}>
      <Box
        sx={{
          margin: '0.5rem 0',
          minWidth: '2rem',
          overflow: 'hidden',
          textOverflow: 'ellipsis',
        }}>
        <strong>{value.name}</strong>
        <div>{value.userIds.length} medlemmer</div>
      </Box>
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
  useNavContext({ title: 'Mine grupper' }, [])
  const {
    userGroups: { userGroups },
  } = useAppState()

  return (
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
          opacity: !!size(userGroups) ? 0 : 1,
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
        {map(userGroups, (g) => (
          <GroupItem key={g.id} value={g}></GroupItem>
        ))}
        <AddGroupButton />
      </Box>
      <AddGroupDialog />
      <EditGroupDialog />
    </Box>
  )
}

export default UserGroupsPage
