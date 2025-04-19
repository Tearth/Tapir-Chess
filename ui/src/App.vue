<script setup lang="ts">
import Navbar from './components/Navbar.vue'
</script>

<template>
  <Navbar />
  <RouterView />
</template>

<script lang="ts">
import { useUserStore } from '@/stores/user'

export default {
  mounted() {
    useUserStore().fetch()
  },
}
</script>
