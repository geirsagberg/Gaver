import { adaptV4Theme, createTheme, DeprecatedThemeOptions } from '@mui/material'
import { PaletteOptions } from '@mui/material/styles'
import createStyles from '@mui/styles/createStyles'
import { merge } from 'lodash-es'

export const gaverColors = {
  first: '#0D3746',
  second: '#0AC1B1',
  third: '#0AC1B1',
  fourth: '#EC7D2B',
  fifth: '#DD395A',
}

const palette: PaletteOptions = {
  // primary: { main: '#3F51B5' },
  // secondary: { main: '#AB47BC' },
  // type: 'dark',
  // background: {
  //   paper: '#222'
  // }
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

const commonThemeOptions: DeprecatedThemeOptions = {
  palette,
  overrides: {
    MuiDialog: {
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
}

const theme = createTheme(adaptV4Theme(commonThemeOptions))

const darkThemeOptions: DeprecatedThemeOptions = {
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
  overrides: {
    MuiPaper: {
      elevation1: {
        background: colors.darkGreyOverlay05,
      },
      elevation2: {
        background: colors.darkGreyOverlay07,
      },
      elevation3: {
        background: colors.darkGreyOverlay08,
      },
      elevation4: {
        background: colors.darkGreyOverlay09,
      },
    },
  },
}

export const darkTheme = createTheme(adaptV4Theme(merge(commonThemeOptions, darkThemeOptions)))

export default theme

export const pageWidth = 600

export const commonStyles = createStyles({
  centerContent: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    alignSelf: 'center',
    textAlign: 'center',
  },
  dialogActions: {
    margin: '0.5rem',
    '& > :first-child': {
      marginLeft: 0,
    },
    '& > :last-child': {
      marginRight: 0,
    },
  },
})

export const listItemStyles = createStyles({
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
