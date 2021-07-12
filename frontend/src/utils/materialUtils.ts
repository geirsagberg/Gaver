import { makeStyles } from '@material-ui/styles'
import { Styles } from '@material-ui/styles/withStyles'
import { Theme } from '@material-ui/core'

export const createStylesHook = <Props extends {}, ClassKey extends string>(styles: Styles<Theme, Props, ClassKey>) =>
  makeStyles(styles)
