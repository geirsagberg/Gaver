import React, { Component } from 'react'
import { List, ListItem, ListItemText, createStyles, WithStyles } from '@material-ui/core'
import { AppState, AppStateWithRouting } from '~/store'
import { selectMyList } from '~/store/selectors'

const styles = createStyles({
  root: {
    alignSelf: 'flex-start'
  }
})

const mapStateToProps = (state: AppStateWithRouting) => ({
  wishList: selectMyList(state)
})

type Props = WithStyles<typeof styles>

class MyList extends Component {
  render() {
    const { classes } = this.props
    return (
      <div className={classes.root}>
        <List>
          <ListItem>
            <ListItemText primary="Sjokolade" />
          </ListItem>
          <ListItem>
            <ListItemText primary="Sjokolade" />
          </ListItem>
          <ListItem>
            <ListItemText primary="Sjokolade" />
          </ListItem>
          <ListItem>
            <ListItemText primary="Sjokolade" />
          </ListItem>
        </List>
      </div>
    )
  }
}

export default MyList
