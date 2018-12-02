import { AppBar, Button, createStyles, Toolbar, Typography, withStyles, WithStyles } from '@material-ui/core'
import React, { Component } from 'react'
import { hot } from 'react-hot-loader'
import Expander from '~/components/Expander'
import { connect } from 'react-redux'
import { AppState, AppStateWithRouting } from '~/store'
import { RouteType } from '~/routing'

import StartPage from '~/pages/Start'

const styles = createStyles({
  root: {
    height: '100%'
  },
  content: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100%',
    paddingTop: '4rem'
  }
})

const mapStateToProps = (state: AppStateWithRouting) => ({
  routing: state.routing
})

type Props = WithStyles<typeof styles> & ReturnType<typeof mapStateToProps>

class Layout extends Component<Props> {
  getContent() {
    switch (this.props.routing.type) {
      case RouteType.FrontPage:
        return StartPage
    }
  }
  render() {
    const { classes } = this.props

    const Content = this.getContent()
    return (
      <div className={classes.root}>
        <AppBar>
          <Toolbar>
            <Typography variant="h6" color="inherit">
              Gaver
            </Typography>
          </Toolbar>
        </AppBar>
        <div className={classes.content}>
          <Content />
        </div>
      </div>
    )
  }
}

export default hot(module)(connect(mapStateToProps)(withStyles(styles)(Layout)))
