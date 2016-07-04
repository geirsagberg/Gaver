import * as React from 'react'
import { Route } from 'react-router'
import { Layout } from './components/Layout'
import Home from './components/Home'
export default (
<Route component={Layout}>
  <Route path="/" component={Home} />
</Route>
)
