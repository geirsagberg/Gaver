import { Box } from '@mui/material'
import { ReactNode } from 'react'

export const Center = ({ children }: { children: ReactNode }) => (
  <Box
    sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', alignSelf: 'center', textAlign: 'center' }}>
    {children}
  </Box>
)
