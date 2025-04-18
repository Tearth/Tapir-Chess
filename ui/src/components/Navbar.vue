<template>
  <div class="navbar bg-base-100 shadow-sm">
    <div class="flex">
      <a class="btn btn-ghost text-3xl pb-1">Tapir Chess</a>
    </div>
    <div class="flex-1 gap-2">
      <div class="navbar-center">
        <ul class="menu menu-horizontal text-lg">
          <li><a>PLAYERS</a></li>
          <li><a>GAMES</a></li>
        </ul>
      </div>
    </div>
    <div class="flex gap-2">
      <div v-if="!userStore.signedIn" class="navbar-center">
        <ul class="menu menu-horizontal text-lg">
          <li><RouterLink to="/signin">SIGN IN</RouterLink></li>
          <li><RouterLink to="/register">JOIN US</RouterLink></li>
        </ul>
      </div>
      <div v-else class="navbar-center">
        <div class="dropdown dropdown-end">
          <div tabIndex="{0}" role="button" class="btn btn-ghost text-lg">
            {{ userStore.username }}
          </div>
          <ul tabIndex="{0}" class="menu menu-md dropdown-content bg-base-100 rounded-box z-1 mt-3 w-52 p-2 shadow">
            <li><RouterLink to="/profile">Profile</RouterLink></li>
            <li><RouterLink to="/settings">Settings</RouterLink></li>
            <li><a @click="signout">Sign out</a></li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import router from '@/router'
import { useUserStore } from '@/stores/user'
import { mapStores } from 'pinia'
import { HTTP } from '@/utils/http'

export default {
  computed: {
    ...mapStores(useUserStore),
  },
  methods: {
    signout() {
      HTTP.post('/api/auth/signout', {}).finally(() => {
        useUserStore().fetch()
        router.push('/')
      })
    },
  },
}
</script>
