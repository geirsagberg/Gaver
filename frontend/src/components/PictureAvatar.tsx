import { Avatar } from '@mui/material'
import { AvatarProps } from '@mui/material/Avatar'
import React from 'react'
import { ChatUserDto } from '~/types/data'

const PictureAvatar = ({ user, ...rest }: { user: ChatUserDto } & AvatarProps) => {
  return user.pictureUrl ? (
    <Avatar src={user.pictureUrl} {...rest} />
  ) : (
    <Avatar {...rest}>
      {user.name
        .split(' ')
        .map((s) => (s.length > 0 ? s[0] : ''))
        .join('')
        .substring(0, 2)}
    </Avatar>
  )
}

export default PictureAvatar
