import Vue from 'vue'
import Router, { RouteConfig, RouterOptions } from 'vue-router'
import Login from './pages/login.vue'
import MyList from './pages/myList.vue'

Vue.use(Router)

const loginRoute: RouteConfig = {
  path: '/login',
  name: 'Login',
  component: Login
}

const myListRoute: RouteConfig = {
  path: '/',
  name: 'MyList',
  component: MyList
}

const options: RouterOptions = {
  mode: 'history',
  routes: [ loginRoute, myListRoute ]
}

const router = new Router(options)
router.beforeEach((to, from, next) => {

})

export default router
