// For documentation, see https://github.com/mroderick/PubSubJS
import { isDevelopment } from './'
import PubSub from 'pubsub-js'

PubSub.immediateExceptions = isDevelopment

export function subscribe(topic: string, func: Function): any {
  return PubSub.subscribe(topic, func)
}

export function unsubscribe(tokenOrFuncOrTopic) {
  PubSub.unsubscribe(tokenOrFuncOrTopic)
}

export function publish(topic: string, data?): boolean {
  return PubSub.publish(topic, data)
}

export enum Topic {
  AjaxStart = 'ajax.start',
  AjaxStop = 'ajax.stop',
}
