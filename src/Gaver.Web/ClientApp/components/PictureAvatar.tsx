import React from 'react'
import { Avatar } from '@material-ui/core'
import { UserModel } from '~/types/data'
import { AvatarProps } from '@material-ui/core/Avatar'

const PictureAvatar = ({ user, ...rest }: { user: UserModel } & AvatarProps) => {
  return user.pictureUrl ? (
    <Avatar src={user.pictureUrl} {...rest} />
  ) : (
    <Avatar {...rest}>
      {user.name
        .split(' ')
        .map(s => (s.length > 0 ? s[0] : ''))
        .join('')
        .substr(0, 2)}
    </Avatar>
  )
}

export default PictureAvatar
