import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { HTTP } from '@/utils/http'

export const useUserStore = defineStore('user', () => {
  const id = ref('')
  const username = ref('')
  const email = ref('')
  const signedIn = ref(false)
  const loaded = ref(false)

  async function fetch() {
    try {
      const response = await HTTP.post('/api/account/info', {})

      id.value = response.data.id
      username.value = response.data.username
      email.value = response.data.email
      signedIn.value = true
      loaded.value = true
    } catch {
      id.value = ''
      username.value = ''
      email.value = ''
      signedIn.value = false
    }
  }

  async function get() {
    if (!loaded.value) {
      await fetch()
    }

    return {
      id,
      username,
      email,
      signedIn,
    }
  }

  return { id, username, email, signedIn, loaded, fetch, get }
})
