import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'
import { keyBy } from 'lodash-es'
import { UserDto, ChatMessageDto } from '~/types/data'
import { isDevelopment } from '~/utils'
import AuthService from '~/utils/AuthService'

export const subscribeList = async (
  listId: number,
  {
    onRefresh,
    onUpdateUsers,
    onMessageAdded
  }: {
    onRefresh: () => any
    onUpdateUsers: (users: Dictionary<UserDto>) => any
    onMessageAdded: (message: ChatMessageDto) => any
  }
) => {
  const hubConnection = new HubConnectionBuilder()
    .withUrl(`${document.location.origin}/listHub`, {
      accessTokenFactory: () => AuthService.loadAccessToken()
    })
    .configureLogging(isDevelopment ? LogLevel.Debug : LogLevel.Warning)
    .build()
  hubConnection.on('refresh', onRefresh)
  hubConnection.on('updateUsers', (users: UserDto[]) => {
    onUpdateUsers(keyBy(users, u => u.id))
  })
  hubConnection.on('messageAdded', onMessageAdded)
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
