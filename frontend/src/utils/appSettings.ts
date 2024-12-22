import { FeatureFlags } from '~/types/data'
import { tryAjax } from './ajax'

export interface AuthProperties {
  audience: string
  clientId: string
  domain: string
}

export const authProperties = await tryAjax<AuthProperties>(() => fetch('/api/auth'))

export const features = await tryAjax<FeatureFlags>(() => fetch('/api/features'))
