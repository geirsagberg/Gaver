export interface IAppSettings {
  audience: string
  clientId: string
  domain: string
}

const AppSettings: IAppSettings = window['AppSettings']

export default AppSettings
