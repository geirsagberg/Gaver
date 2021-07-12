import NProgress from 'nprogress'
import { subscribe, Topic } from './pubSub'

let requests = 0

export function setupProgress() {
  subscribe(Topic.AjaxStart, () => {
    if (requests <= 0) {
      NProgress.start()
    }
    requests += 1
  })
  subscribe(Topic.AjaxStop, () => {
    requests -= 1
    if (requests <= 0) {
      NProgress.done()
    }
  })
}
