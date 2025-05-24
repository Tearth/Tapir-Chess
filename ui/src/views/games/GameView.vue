<script setup lang="ts">
import Board from '@/components/Board.vue'
</script>

<template>
  <div class="flex flex-col-reverse lg:flex-row flex-wrap justify-center content-center gap-4 mt-3 mb-6">
    <div class="flex flex-col lg:w-[300px] gap-3">
      <div class="h-[140px] bg-base-200 border border-base-300 p-4 lg:rounded-t-md gap-3">
        <div class="flex flex-row">
          <img src="/assets/icons/turtle.svg" class="time-icon" />
          <div class="flex flex-col ml-4">
            <div>{{ time }}+{{ increment }} • Rankingowa</div>
            <div>{{ createdAt.format('DD.MM.YYYY HH:mm') }}</div>
          </div>
        </div>
        <div>⚪ {{ usernameWhite }} (1600)</div>
        <div>⚫ {{ usernameBlack }} (1750)</div>
      </div>
      <textarea
        class="textarea textarea-ghost grow-1 w-full bg-base-200 border border-base-300 p-4 lg:rounded-b-md gap-3"
        placeholder="Notes (will be saved automatically)"
      ></textarea>
    </div>
    <div class="flex flex-row lg:hidden">
      <button class="btn btn-soft grow-1 pt-6 pb-6">
        <img src="/assets/icons/draw.svg" class="button-icon" />
      </button>
      <button class="btn btn-soft grow-1 pt-6 pb-6">
        <img src="/assets/icons/flag.svg" class="button-icon" />
      </button>
    </div>
    <div>
      <Board ref="board" />
    </div>
    <div class="flex items-center">
      <div class="flex flex-row w-full lg:w-[300px] lg:flex-col flex-wrap justify-center content-center">
        <div
          :class="sideToMove == WHITE ? 'bg-green-600' : 'bg-base-200'"
          class="grow-1 w-0 lg:w-auto border border-base-300 p-4 lg:rounded-t-md gap-3 text-5xl text-center font-medium"
        >
          {{ moment.utc(timeWhite).format('mm:ss') }}
        </div>
        <div class="hidden lg:block h-[200px] w-full bg-base-200 border border-base-300 p-4 gap-3">
          {{ pgn }}
        </div>
        <div class="hidden lg:flex flex-row">
          <button class="btn btn-soft grow-1 pt-6 pb-6">
            <img src="/assets/icons/draw.svg" class="button-icon" />
          </button>
          <button class="btn btn-soft grow-1 pt-6 pb-6">
            <img src="/assets/icons/flag.svg" class="button-icon" />
          </button>
        </div>
        <div
          :class="sideToMove == BLACK ? 'bg-green-600' : 'bg-base-200'"
          class="grow-1 w-0 lg:w-auto border border-base-300 p-4 lg:rounded-b-md gap-3 text-5xl text-center font-medium"
        >
          {{ moment.utc(timeBlack).format('mm:ss') }}
        </div>
      </div>
    </div>
  </div>
</template>

<style>
.time-icon {
  width: 64px;
  height: 64px;
  margin-top: -8px;
}

.button-icon {
  width: 32px;
  height: 32px;
}

[data-theme='dark'] {
  .time-icon {
    filter: invert(1);
  }

  .button-icon {
    filter: invert(1);
  }
}
</style>

<script lang="ts">
import { ERRORS } from '@/utils/errors'
import { HTTP } from '@/utils/http'
import * as BUS from '@/utils/bus'
import router from '@/router'
import * as WS from '@/utils/ws'
import moment from 'moment'
import { WHITE, BLACK } from 'chess.js'
import { useUserStore } from '@/stores/user'
import BoardComponent from '@/components/Board.vue'
import { useRoute } from 'vue-router'
import type { GameCreatedEvent } from '@/events/GameCreatedEvent'
import type { MoveMadeEvent } from '@/events/MoveMadeEvent'
import type { GameInfoEvent } from '@/events/GameInfoEvent'
import type { BoardChangeEvent } from '@/events/BoardChangeEvent'

export default {
  data() {
    return {
      id: '',
      createdAt: moment(),
      usernameWhite: '',
      usernameBlack: '',
      time: 0,
      increment: 0,
      timeWhite: 0,
      timeBlack: 0,
      timeOriginal: 0,
      pgn: '',
      pov: '',
      sideToMove: WHITE,
      inProgress: false,
      clockCallback: 0,
      clockBase: moment(),
    }
  },
  async mounted() {
    this.id = useRoute().params.id!.toString()

    BUS.emitter.on('onGameInfo', this.onGameInfo)
    BUS.emitter.on('onMoveMade', this.onMoveMade)
    BUS.emitter.on('onBoardChange', this.onBoardChange)

    this.clockCallback = setInterval(this.updateClock, 100)

    await WS.getGameInfo(this.id)
  },
  unmounted() {
    BUS.emitter.off('onGameInfo', this.onGameInfo)
    BUS.emitter.off('onMoveMade', this.onMoveMade)
    BUS.emitter.off('onBoardChange', this.onBoardChange)

    clearInterval(this.clockCallback)
  },
  methods: {
    updateClock() {
      if (this.inProgress) {
        let difference = moment().diff(this.clockBase, 'milliseconds')

        switch (this.sideToMove) {
          case WHITE: {
            this.timeWhite = this.timeOriginal - difference
            break
          }
          case BLACK: {
            this.timeBlack = this.timeOriginal - difference
            break
          }
        }
      }
    },
    switchSideToMove() {
      switch (this.sideToMove) {
        case WHITE: {
          this.sideToMove = BLACK
          break
        }

        case BLACK: {
          this.sideToMove = WHITE
          break
        }
      }
    },
    onGameInfo(data: GameInfoEvent) {
      let board = this.$refs.board as InstanceType<typeof BoardComponent>
      let userStore = useUserStore()
      let pov = userStore.id == data.userIdWhite ? WHITE : BLACK

      board.setPgn(data.pgn, pov)

      this.createdAt = moment(data.createdAt)
      this.usernameWhite = data.usernameWhite
      this.usernameBlack = data.usernameBlack
      this.time = data.time
      this.increment = data.increment
      this.timeWhite = data.timeWhite
      this.timeBlack = data.timeBlack
      this.pgn = data.pgn
      this.pov = pov

      if (data.sideToMove == 'White') {
        this.sideToMove = WHITE
      } else {
        this.sideToMove = BLACK
      }

      this.inProgress = data.status == 'InProgress'
    },
    onMoveMade(data: MoveMadeEvent) {
      let board = this.$refs.board as InstanceType<typeof BoardComponent>

      this.timeWhite = data.timeWhite
      this.timeBlack = data.timeBlack

      if (this.id == data.id) {
        this.inProgress = true
      }

      // Don't go further if the move was made by us
      if ((data.side == 'White' && this.pov == WHITE) || (data.side == 'Black' && this.pov == BLACK)) {
        return
      }

      board.makeMove(data.move)

      this.clockBase = moment()

      switch (data.side) {
        case 'Black': {
          this.sideToMove = WHITE
          this.timeOriginal = this.timeWhite
          break
        }
        case 'White': {
          this.sideToMove = BLACK
          this.timeOriginal = this.timeBlack
          break
        }
      }
    },
    onBoardChange(data: BoardChangeEvent) {
      this.clockBase = moment()

      switch (this.pov) {
        case WHITE: {
          this.sideToMove = BLACK
          this.timeOriginal = this.timeBlack
          break
        }
        case BLACK: {
          this.sideToMove = WHITE
          this.timeOriginal = this.timeWhite
          break
        }
      }

      WS.makeMove(this.id, data.move)
    },
  },
}
</script>
