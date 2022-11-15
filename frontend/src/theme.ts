import { createTheme } from '@mui/material'
import { PaletteOptions, SxProps, Theme, ThemeOptions } from '@mui/material/styles'
import { merge } from 'lodash-es'

export const gaverColors = {
  first: '#0D3746',
  second: '#0AC1B1',
  third: '#0AC1B1',
  fourth: '#EC7D2B',
  fifth: '#DD395A',
}

const palette: PaletteOptions = {
  primary: { main: '#3F51B5' },
  secondary: { main: '#AB47BC' },
}

const colors = {
  darkGrey: '#121212',
  white87: 'rgba(255, 255, 255, 0.87)',
  white60: 'rgba(255, 255, 255, 0.60)',
  white38: 'rgba(255, 255, 255, 0.38)',
  white12: 'rgba(255, 255, 255, 0.12)',
  darkGreyOverlay05: '#1e1e1e',
  darkGreyOverlay07: '#222222',
  darkGreyOverlay08: '#242424',
  darkGreyOverlay09: '#242424',
  darkGreyOverlay11: '#272727',
  darkGreyOverlay12: '#2c2c2c',
  darkGreyOverlay14: '#2d2d2d',
  darkGreyOverlay15: '#333333',
  darkGreyOverlay16: '#383838',
}

const commonThemeOptions: ThemeOptions = {
  palette,
  components: {
    MuiDialog: {
      styleOverrides: {
        container: {
          alignItems: 'flex-start',
        },
        paper: {
          margin: '0 1rem',
        },
        paperFullWidth: {
          width: `calc(100% - 2rem)`,
        },
        paperScrollPaper: {
          maxHeight: '100%',
        },
      },
    },
  },
}

const theme = createTheme(commonThemeOptions)

const darkThemeOptions: ThemeOptions = {
  palette: {
    mode: 'dark',
    background: {
      paper: colors.darkGrey,
      default: colors.darkGrey,
    },
    text: {
      primary: colors.white87,
      secondary: colors.white60,
      disabled: colors.white38,
    },
  },
}

export const darkTheme = createTheme(merge(commonThemeOptions, darkThemeOptions))

export default theme

export const pageWidth = 600

export const dialogActions: SxProps<Theme> = {
  margin: '0.5rem',
  '& > :first-of-type': {
    marginLeft: 0,
  },
  '& > :last-of-type': {
    marginRight: 0,
  },
}

function makeSxProps<T extends Record<string, SxProps<Theme>>>(sx: Record<string, SxProps<Theme>>): T {
  return sx as T
}

export const listItemStyles = makeSxProps({
  root: {
    width: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingLeft: '1rem',
    minHeight: '3rem',
  },
  content: {
    margin: '0.5rem 0',
    minWidth: '2rem',
    overflow: 'hidden',
    textOverflow: 'ellipsis',
  },
  link: {
    whiteSpace: 'nowrap',
  },
})
