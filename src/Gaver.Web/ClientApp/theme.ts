import { createMuiTheme } from '@material-ui/core'
import { PaletteOptions } from '@material-ui/core/styles/createPalette'
import { CSSProperties } from '@material-ui/styles/withStyles'

export const gaverColors = {
  first: '#0D3746',
  second: '#0AC1B1',
  third: '#0AC1B1',
  fourth: '#EC7D2B',
  fifth: '#DD395A'
}
const palette: PaletteOptions = {
  // primary: { main: '#3F51B5' },
  // secondary: { main: '#AB47BC' },
  // type: 'dark',
  // background: {
  //   paper: '#222'
  // }
}

const theme = createMuiTheme({
  palette,
  typography: {
    useNextVariants: true
  },
  overrides: {
    MuiDialog: {
      container: {
        alignItems: 'flex-start'
      }
    }
  }
})

export default theme

export const pageWidth = 600

export const centerContent: CSSProperties = {
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  alignSelf: 'center',
  textAlign: 'center'
}
