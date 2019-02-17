import { createStyles, List, ListItem, ListItemText, WithStyles, withStyles } from '@material-ui/core'
import { map } from 'lodash-es'
import React, { Component, SFC } from 'react'
import { connect } from 'react-redux'
import { AppStateWithRouting } from '~/store'
import { selectMyList } from '~/store/selectors'
import { Wish } from '~/types/data'

const styles = createStyles({
  root: {
    alignSelf: 'flex-start'
  }
})

const mapStateToProps = (state: AppStateWithRouting) => ({
  wishList: selectMyList(state)
})

type Props = WithStyles<typeof styles> & ReturnType<typeof mapStateToProps>

const WishListItem: SFC<Wish> = wish => (
  <ListItem>
    <ListItemText primary={wish.title} />
  </ListItem>
)

class MyList extends Component<Props> {
  render() {
    const { classes } = this.props
    return (
      <div className={classes.root}>
        {this.props.wishList && (
          <List>
            {map(this.props.wishList.wishes, wish => (
              <WishListItem {...wish} />
            ))}
            <ListItem button>
              <ListItemText primary="Legg til ønske" />
            </ListItem>
          </List>
        )}

        {/* <Fab>
          <Icon>add_icon</Icon>
        </Fab> */}
      </div>
    )
  }
}

export default connect(mapStateToProps)(withStyles(styles)(MyList))
