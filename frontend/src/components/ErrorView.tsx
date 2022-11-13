import { Button, colors, Icon, Typography } from '@mui/material'
import { ReactNode } from 'react'

import { Box } from '@mui/material'

interface Props {
  onBackClicked?: () => any
  children: ReactNode
}

const ErrorView = ({ children, onBackClicked }: Props) => {
  return (
    <Box
      sx={{
        height: '100%',
        position: 'relative',
        zIndex: 0,
        display: 'flex',
        justifyContent: 'center',
      }}>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          alignSelf: 'center',
        }}>
        <Typography variant="h1">Oisann!</Typography>
        <Icon
          sx={{
            color: colors.amber[600],
            fontSize: 80,
          }}>
          sentiment_very_dissatisfied
        </Icon>
        <Typography m="1rem">{children}</Typography>
        <Button href="/" color="primary" variant="contained" onClick={onBackClicked}>
          Tilbake
        </Button>
      </Box>
    </Box>
  )
}

export default ErrorView
