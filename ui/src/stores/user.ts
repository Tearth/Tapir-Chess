import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { HTTP } from '@/utils/http'

export const useUserStore = defineStore('user', () => {
  const id = ref('')
  const username = ref('')
  const email = ref('')
  const signedIn = ref(false)

  function fetch() {
    HTTP.post('/api/account/info', {})
      .then((response) => {
        id.value = response.data.id
        username.value = response.data.username
        email.value = response.data.email
        signedIn.value = true
      })
      .catch((error) => {
        id.value = ''
        username.value = ''
        email.value = ''
        signedIn.value = false
      })
  }

  return { id, username, email, signedIn, fetch }
})
