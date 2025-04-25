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
            <div>10+5 • Rankingowa</div>
            <test>1 minutę temu</test>
          </div>
        </div>
        <div>⚪ Player A (1600)</div>
        <div>⚫ Player B (1750)</div>
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
        <div class="grow-1 bg-green-600 border border-base-300 p-4 lg:rounded-t-md gap-3 text-5xl text-center font-medium">10:00</div>
        <div class="hidden lg:block h-[200px] w-full bg-base-200 border border-base-300 p-4 gap-3">
          1. e4 e5 2. c7 c5 1.1. e4 e5 2. c7 c5 1.1. e4 e5 2. c7 c5 1.1. e4 e5 2. c7 c5 1.
        </div>
        <div class="hidden lg:flex flex-row">
          <button class="btn btn-soft grow-1 pt-6 pb-6">
            <img src="/assets/icons/draw.svg" class="button-icon" />
          </button>
          <button class="btn btn-soft grow-1 pt-6 pb-6">
            <img src="/assets/icons/flag.svg" class="button-icon" />
          </button>
        </div>
        <div class="grow-1 bg-base-200 border border-base-300 p-4 lg:rounded-b-md gap-3 text-5xl text-center font-medium">10:00</div>
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
