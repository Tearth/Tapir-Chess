import * as signalR from '@microsoft/signalr'
import * as BUS from '@/utils/bus'
import { GameCreatedEvent } from '@/events/GameCreatedEvent'
import type { GameStartedEvent } from '@/events/GameStartedEvent'
import type { MoveMadeEvent } from '@/events/MoveMadeEvent'
import type { GameInfoEvent } from '@/events/GameInfoEvent'

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

WS.on('onGameCreated', function (data: GameCreatedEvent) {
  BUS.emitter.emit('onGameCreated', data)
})

WS.on('onGameInfo', function (data: GameInfoEvent) {
  BUS.emitter.emit('onGameInfo', data)
})

WS.on('onMoveMade', function (data: MoveMadeEvent) {
  BUS.emitter.emit('onMoveMade', data)
})
