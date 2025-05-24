import mitt from 'mitt'
import type { GameCreatedEvent } from '@/events/GameCreatedEvent'
import type { GameInfoEvent } from '@/events/GameInfoEvent'
import type { MoveMadeEvent } from '@/events/MoveMadeEvent'
import type { BoardChangeEvent } from '@/events/BoardChangeEvent'

type Events = {
  onGameCreated: GameCreatedEvent
  onGameInfo: GameInfoEvent
  onMoveMade: MoveMadeEvent
  onBoardChange: BoardChangeEvent
}

export const emitter = mitt<Events>()
