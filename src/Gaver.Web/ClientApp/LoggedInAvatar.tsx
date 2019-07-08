import { Icon, IconButton, Menu, MenuItem, Tooltip } from '@material-ui/core'
import React, { FC, useState } from 'react'
import { useOvermind } from './overmind'

export const LoggedInAvatar: FC = () => {
  const {
    state: { auth },
    actions: {
      auth: { logOut }
    }
  } = useOvermind()
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement>(null)
  const showProfileMenu = (event: React.MouseEvent<HTMLElement>) => setMenuAnchorEl(event.currentTarget)
  const hideProfileMenu = () => setMenuAnchorEl(null)
  return auth.isLoggedIn ? (
    <>
      <Tooltip title={auth.user.name}>
        <IconButton color="inherit" onClick={showProfileMenu}>
          <Icon>account_circle</Icon>
        </IconButton>
      </Tooltip>

      <Menu anchorEl={menuAnchorEl} open={!!menuAnchorEl} onClose={hideProfileMenu}>
        <MenuItem onClick={logOut}>Logg ut</MenuItem>
      </Menu>
    </>
  ) : null
}
