import React, { FC } from 'react'
import { makeStyles } from '@material-ui/styles'
import { pageWidth } from '~/theme'

const useStyles = makeStyles({
  root: {
    width: '100%',
    height: '100%',
    maxWidth: pageWidth
  }
})

const SharedListPage: FC = () => {
  const classes = useStyles()
  return <div className={classes.root}>hello</div>
}

export default SharedListPage
