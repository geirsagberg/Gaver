import React, { Component } from 'react'
import { List, ListItem, ListItemText, createStyles } from '@material-ui/core'
import { AppState } from '~/store'

const styles = createStyles({})

const mapStateToProps = (state: AppState) => ({
  wishList: selectMyList(state)
})

class MyList extends Component {
  render() {
    return (
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
    )
  }
}

export default MyList
