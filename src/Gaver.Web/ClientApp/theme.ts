import { createMuiTheme } from '@material-ui/core'
import { PaletteOptions } from '@material-ui/core/styles/createPalette'

export const gaverColors = {
  first: '#0D3746',
  second: '#0AC1B1',
  third: '#0AC1B1',
  fourth: '#EC7D2B',
  fifth: '#DD395A'
}
const palette: PaletteOptions = {
  primary: { main: '#3F51B5' },
  secondary: { main: '#AB47BC' },
  type: 'dark'
}

const theme = createMuiTheme({
  palette,
  typography: {
    useNextVariants: true
  }
})

export default theme
