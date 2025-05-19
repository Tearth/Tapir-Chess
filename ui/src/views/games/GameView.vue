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
      <Board />
    </div>
    <div class="flex items-center">
      <div class="flex flex-row w-full lg:w-[300px] lg:flex-col flex-wrap justify-center content-center">
        <div class="grow-1 bg-green-600 border border-base-300 p-4 lg:rounded-t-md gap-3 text-5xl text-center font-medium">
          {{ moment.utc(timeWhite * 60).format('mm:ss') }}
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
        <div class="grow-1 bg-base-200 border border-base-300 p-4 lg:rounded-b-md gap-3 text-5xl text-center font-medium">
          {{ moment.utc(timeBlack * 60).format('mm:ss') }}
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

export default {
  data() {
    return {
      createdAt: moment(),
      usernameWhite: '',
      usernameBlack: '',
      time: 0,
      increment: 0,
      timeWhite: 0,
      timeBlack: 0,
      pgn: '',
    }
  },
  async mounted() {
    BUS.emitter.on('onGameInfo', this.onGameInfo)

    await WS.getGameInfo(this.$route.params.id.toString())
  },
  unmounted() {
    BUS.emitter.off('onGameInfo', this.onGameInfo)
  },
  methods: {
    onGameInfo(data: any) {
      this.createdAt = moment(data.createdAt)
      this.usernameWhite = data.usernameWhite
      this.usernameBlack = data.usernameBlack
      this.time = data.time
      this.increment = data.increment
      this.timeWhite = data.timeWhite
      this.timeBlack = data.timeBlack
      this.pgn = data.pgn
    },
  },
}
</script>
