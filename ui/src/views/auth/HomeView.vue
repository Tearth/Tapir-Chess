<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 w-[90%] md:w-[700px] lg:w-[700px] mb-8">
      <button
        v-for="(item, index) in timeControls"
        class="btn p-8 line-clamp-none h-full"
        :disabled="roomCreated && roomIndex != index"
        @click="createRoom(index)"
      >
        <div class="text-center text-4xl">{{ item.time }}+{{ item.increment }}</div>
        <div v-if="!roomCreated" class="text-center text-2xl font-normal">{{ item.name }}</div>
        <div v-else>
          <div v-if="roomIndex != index" class="text-center text-2xl font-normal">{{ item.name }}</div>
          <div v-else class="loading loading-spinner loading-xl"></div>
        </div>
      </button>

      <button class="btn p-8 line-clamp-none h-full">
        <div class="text-center text-4xl font-normal">Custom</div>
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { ERRORS } from '@/utils/errors'
import { HTTP } from '@/utils/http'

export default {
  data() {
    return {
      roomCreated: false,
      roomIndex: -1,
      roomId: '',
      timeControls: [
        { time: 1, increment: 0, name: 'Blitz' },
        { time: 2, increment: 1, name: 'Blitz' },
        { time: 3, increment: 0, name: 'Blitz' },
        { time: 3, increment: 2, name: 'Blitz' },
        { time: 5, increment: 0, name: 'Blitz' },
        { time: 5, increment: 3, name: 'Blitz' },
        { time: 10, increment: 0, name: 'Rapid' },
        { time: 10, increment: 5, name: 'Rapid' },
        { time: 15, increment: 10, name: 'Rapid' },
        { time: 30, increment: 0, name: 'Classical' },
        { time: 30, increment: 20, name: 'Classical' },
      ],
    }
  },
  methods: {
    async createRoom(index: number) {
      let timeControl = this.timeControls[index]

      if (!this.roomCreated) {
        this.roomCreated = true
        this.roomIndex = index

        HTTP.post('/api/rooms', {
          time: timeControl.time,
          increment: timeControl.increment,
        })
          .then((response) => {
            let location = response.headers.location
            let id = location.substring(location.lastIndexOf('/') + 1)

            this.roomId = id
          })
          .catch((error) => {
            this.roomCreated = false
            this.roomIndex = 0
          })
      } else {
        HTTP.delete('/api/rooms/' + this.roomId)

        this.roomCreated = false
        this.roomIndex = 0
        this.roomId = ''
      }
    },
  },
}
</script>
