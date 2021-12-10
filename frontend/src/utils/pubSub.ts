// For documentation, see https://github.com/mroderick/PubSubJS
import PubSub from 'pubsub-js'
import { isDevelopment } from './'
;(PubSub as any)['immediateExceptions'] = isDevelopment

export function subscribe<T>(topic: string, func: PubSubJS.SubscriptionListener<T>): any {
  return PubSub.subscribe(topic, func)
}

export function unsubscribe(tokenOrFuncOrTopic: any) {
  PubSub.unsubscribe(tokenOrFuncOrTopic)
}

export function publish(topic: string, data?: any): boolean {
  return PubSub.publish(topic, data)
}

export enum Topic {
  AjaxStart = 'ajax.start',
  AjaxStop = 'ajax.stop',
}
