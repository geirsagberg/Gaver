import * as React from 'react'
import { Route } from 'react-router'
import Layout from './components/Layout'
import Home from './components/Home'
import MyList from './components/MyList'
export default (
<Route component={Layout}>
  <Route path="/" component={MyList} />
</Route>
)
