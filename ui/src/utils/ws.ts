import * as signalR from '@microsoft/signalr'
import * as BUS from '@/utils/bus'

const WS = new signalR.HubConnectionBuilder()
  .withUrl(import.meta.env.VITE_WS_URL)
  .withAutomaticReconnect()
  .build()

export async function open() {
  if (WS.state == signalR.HubConnectionState.Disconnected) {
    await WS.start()
  }
}

export async function reconnect() {
  await WS.stop()
  await WS.start()
}

export async function getGameInfo(id: string) {
  if (WS.state != signalR.HubConnectionState.Connected) {
    await open()
  }

  await WS.send('GetGameInfo', id)
}

WS.on('onGameCreated', function (id) {
  BUS.emitter.emit('onGameCreated', id)
})

WS.on('onGameInfo', function (data) {
  BUS.emitter.emit('onGameInfo', data)
})
