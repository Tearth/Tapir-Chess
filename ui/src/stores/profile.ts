import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { useUserStore } from '@/stores/user'
import { HTTP } from '@/utils/http'

export const useProfileStore = defineStore('profile', () => {
  const aboutMe = ref('')
  const country = ref('')
  const loaded = ref(false)

  async function fetch() {
    try {
      const userStore = useUserStore()
      const user = await userStore.get()
      const response = await HTTP.get('/api/players/' + user.id.value)

      aboutMe.value = response.data.aboutMe
      country.value = response.data.country
      loaded.value = true
    } catch (ex) {
      throw ex
    }
  }

  async function get() {
    if (!loaded.value) {
      await fetch()
    }

    return {
      aboutMe: aboutMe.value,
      country: country.value,
    }
  }

  return { aboutMe, country, loaded, fetch, get }
})
