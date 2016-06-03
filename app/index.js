import { IndexRoute, Route, Router } from 'react-router'
import { render } from 'react-dom'
import React from 'react'

const Home = () => (
  <div className="home">
    <h1>Gaver</h1>
  </div>
)

const App = () => (
  <Router>
    <Route path="/" component={Home} />
  </Router>
)

render(<App />, document.getElementById('main'))