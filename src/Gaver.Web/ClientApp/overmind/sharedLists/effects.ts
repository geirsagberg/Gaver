import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'
import { keyBy } from 'lodash-es'
import { UserModel } from '~/types/data'
import { isDevelopment } from '~/utils'
import AuthService from '~/utils/AuthService'

export const subscribeList = async (
  listId: number,
  {
    onRefresh,
    onUpdateUsers
  }: {
    onRefresh: () => any
    onUpdateUsers: (users: Dictionary<UserModel>) => any
  }
) => {
  const hubConnection = new HubConnectionBuilder()
    .withUrl(`${document.location.origin}/listHub`, {
      accessTokenFactory: () => AuthService.loadAccessToken()
    })
    .configureLogging(isDevelopment ? LogLevel.Debug : LogLevel.Warning)
    .build()
  hubConnection.on('refresh', onRefresh)
  hubConnection.on('updateUsers', (users: UserModel[]) => {
    onUpdateUsers(keyBy(users, u => u.id))
  })
  const reconnect = async () => {
    try {
      await hubConnection.start()
    } catch (error) {
      setTimeout(reconnect, 5000)
    }
  }
  hubConnection.onclose(reconnect)
  await hubConnection.start()
  await hubConnection.invoke('subscribe', listId)
}
