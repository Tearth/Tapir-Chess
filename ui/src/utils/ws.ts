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

export async function makeMove(id: string, move: string) {
  if (WS.state != signalR.HubConnectionState.Connected) {
    await open()
  }

  await WS.send('MakeMove', id, move)
}

WS.on('onGameCreated', function (id) {
  BUS.emitter.emit('onGameCreated', id)
})

WS.on('onGameStarted', function (id) {
  BUS.emitter.emit('onGameStarted', id)
})

WS.on('onGameInfo', function (data) {
  BUS.emitter.emit('onGameInfo', data)
})

WS.on('onMoveMade', function (data) {
  BUS.emitter.emit('onMoveMade', data)
})
