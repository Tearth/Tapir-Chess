<script setup lang="ts">
import Navbar from './components/Navbar.vue'
</script>

<template>
  <Navbar />
  <RouterView />
</template>

<script lang="ts">
import { useUserStore } from '@/stores/user'
import { useProfileStore } from './stores/profile'
import * as WS from '@/utils/ws'

export default {
  async mounted() {
    const userStore = useUserStore()
    const profileStore = useProfileStore()

    await userStore.fetch()

    if (userStore.signedIn) {
      await profileStore.fetch()
    }

    await WS.open()
  },
}
</script>
