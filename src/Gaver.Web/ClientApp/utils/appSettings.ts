import { FeatureFlags } from '~/types/data'

export interface IAppSettings {
  audience: string
  clientId: string
  domain: string
  features: FeatureFlags
}

const AppSettings: IAppSettings = window['AppSettings']

export default AppSettings
