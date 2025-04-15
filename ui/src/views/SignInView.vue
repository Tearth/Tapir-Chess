<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <form @submit.prevent="submitForm">
      <fieldset class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
        <legend class="fieldset-legend">Sign in</legend>

        <label class="fieldset-label">Username</label>
        <input v-model="username" class="input" />

        <label class="fieldset-label">Password</label>
        <input type="password" v-model="password" class="input" />

        <div class="grid grid-cols-2 grid-rows-1 pt-1">
          <div>
            <label class="fieldset-label">
              <input type="checkbox" v-model="rememberMe" defaultChecked class="checkbox" />
              Remember me
            </label>
          </div>
          <div class="text-right pt-1">
            <RouterLink to="/reset-password" class="link link-neutral">Forgot password?</RouterLink>
          </div>
        </div>

        <button type="submit" class="btn btn-neutral mt-4">Sign in</button>

        <div class="text-center pt-2">
          Don't have an account?
          <RouterLink to="/register" class="link link-neutral">Join us!</RouterLink>
        </div>
      </fieldset>
    </form>
  </div>
</template>

<script lang="ts">
export default {
  data() {
    return {
      username: '',
      password: '',
      rememberMe: false,
    }
  },
  methods: {
    async submitForm() {
      alert(this.username + ' ' + this.password)
    },
  },
}
</script>
