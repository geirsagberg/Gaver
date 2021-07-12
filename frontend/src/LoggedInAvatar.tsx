import { Icon, IconButton, Menu, MenuItem, Tooltip } from '@material-ui/core'
import React, { FC, useState } from 'react'
import { useActions, useAppState } from './overmind'

export const LoggedInAvatar: FC = () => {
  const { auth } = useAppState()
  const {
    auth: { logOut },
  } = useActions()
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement>()
  const showProfileMenu = (event: React.MouseEvent<HTMLElement>) =>
    setMenuAnchorEl(event.currentTarget)
  const hideProfileMenu = () => setMenuAnchorEl(undefined)
  return auth.isLoggedIn && auth.user ? (
    <>
      <Tooltip title={auth.user.name}>
        <IconButton color="inherit" onClick={showProfileMenu}>
          <Icon>account_circle</Icon>
        </IconButton>
      </Tooltip>

      <Menu
        anchorEl={menuAnchorEl}
        open={!!menuAnchorEl}
        onClose={hideProfileMenu}>
        <MenuItem onClick={logOut}>Logg ut</MenuItem>
      </Menu>
    </>
  ) : null
}
