import React from 'react'
import { Avatar } from '@mui/material'
import { UserDto } from '~/types/data'
import { AvatarProps } from '@mui/material/Avatar'

const PictureAvatar = ({ user, ...rest }: { user: UserDto } & AvatarProps) => {
  return user.pictureUrl ? (
    <Avatar src={user.pictureUrl} {...rest} />
  ) : (
    <Avatar {...rest}>
      {user.name
        .split(' ')
        .map((s) => (s.length > 0 ? s[0] : ''))
        .join('')
        .substr(0, 2)}
    </Avatar>
  )
}

export default PictureAvatar
