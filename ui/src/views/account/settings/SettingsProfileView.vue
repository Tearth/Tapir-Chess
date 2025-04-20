<template>
  <div class="bg-base-200 border border-base-300 p-4 rounded-box gap-3">
    <form @submit.prevent="submitForm">
      <fieldset class="fieldset w-full bg-base-200 border-base-300 p-4 rounded-box gap-3">
        <div v-if="showSuccess" role="alert" class="alert alert-success alert-outline">
          <span>Profile has been saved!</span>
        </div>
        <div v-if="error" role="alert" class="alert alert-error alert-outline">
          <span>{{ error }}</span>
        </div>
        <label class="fieldset-label bold">About me</label>
        <textarea v-model="aboutMe" rows="4" class="input w-full p-2.5 h-40"></textarea>
        <span v-if="usernameValidation" class="text-red-500">{{ usernameValidation }}</span>

        <label class="fieldset-label">Country</label>
        <input v-model="country" class="input w-full" />
        <span v-if="passwordValidation" class="text-red-500">{{ passwordValidation }}</span>

        <div>
          <button type="submit" class="btn btn-success mt-4 w-max float-right" :disabled="inProgress">Save</button>
        </div>
      </fieldset>
    </form>
  </div>
</template>

<script lang="ts">
import router from '@/router'
import { mapStores } from 'pinia'
import { HTTP } from '@/utils/http'
import { ERRORS } from '@/utils/errors'
import { useUserStore } from '@/stores/user'
import { useProfileStore } from '@/stores/profile'

export default {
  data() {
    return {
      aboutMe: '',
      country: '',
      inProgress: false,
      showSuccess: false,

      error: '',
      aboutMeValidation: '',
      countryValidation: '',
    }
  },
  computed: {
    ...mapStores(useProfileStore),
  },
  async mounted() {
    let profileStore = useProfileStore()
    let profile = await profileStore.get()

    this.aboutMe = profile.aboutMe
    this.country = profile.country
  },
  methods: {
    async submitForm() {
      let userStore = useUserStore()
      let profileStore = useProfileStore()

      this.inProgress = true

      HTTP.patch('/api/players/' + userStore.id, {
        id: userStore.id,
        aboutMe: this.aboutMe,
        country: this.country,
      })
        .then((response) => {
          this.showSuccess = true
        })
        .catch((error) => {
          let response = error.response.data

          this.error = ''
          this.aboutMeValidation = ''
          this.countryValidation = ''

          // Validation errors
          if (response.errors != null) {
            if (response.errors.aboutMe != null) {
              this.aboutMeValidation = ERRORS.get(response.errors.aboutMe[0])
            }

            if (response.errors.country != null) {
              this.countryValidation = ERRORS.get(response.errors.country[0])
            }
          } else if (response.detail != null) {
            this.error = ERRORS.get(response.detail)
          }
        })
        .finally(() => {
          this.inProgress = false
        })
    },
  },
}
</script>
