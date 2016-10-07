import PubSub from 'pubsub-js'
import NProgress from 'nprogress'
import { isDevelopment } from 'utils'
import * as topics from 'constants/topics'

let requests = 0

export default function () {
  PubSub.immediateExceptions = isDevelopment
  PubSub.subscribe(topics.AJAX_START, () => {
    if (requests <= 0) {
      NProgress.start()
    }
    requests += 1
  })
  PubSub.subscribe(topics.AJAX_STOP, () => {
    requests -= 1
    if (requests <= 0) {
      NProgress.done()
    }
  })
}
