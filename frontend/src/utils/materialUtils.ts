import { makeStyles } from '@mui/styles'
import { Styles } from '@mui/styles/withStyles'
import { Theme } from '@mui/material'

export const createStylesHook = <Props extends {}, ClassKey extends string>(styles: Styles<Theme, Props, ClassKey>) =>
  makeStyles(styles)
