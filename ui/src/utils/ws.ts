import * as signalR from '@microsoft/signalr'

export const WS = new signalR.HubConnectionBuilder()
  .withUrl(import.meta.env.VITE_WS_URL)
  .withAutomaticReconnect()
  .build()

export async function open() {
  await WS.start()
}

export async function reconnect() {
  await WS.stop()
  await WS.start()
}
