<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <fieldset class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
      <legend class="fieldset-legend">Confirm e-mail change</legend>
      <div v-if="status == ConfirmRegistrationStatus.InProgress">Changing the e-mail address...</div>
      <div v-else-if="status == ConfirmRegistrationStatus.Success">
        <div role="alert" class="alert alert-success alert-outline">
          <span>E-mail address has been changed!</span>
        </div>
      </div>
      <div v-else-if="status == ConfirmRegistrationStatus.Error">
        <div role="alert" class="alert alert-error alert-outline">
          <span>Failed to change the e-mail address, try again in a few minutes or contact our support.</span>
        </div>
      </div>
    </fieldset>
  </div>
</template>

<script lang="ts">
import router from '@/router'
import { useRouter, useRoute } from 'vue-router'
import { HTTP } from '@/utils/http'
import { ERRORS } from '@/utils/errors'
import { useUserStore } from '@/stores/user'

export enum ConfirmRegistrationStatus {
  InProgress,
  Success,
  Error,
}

export default {
  data() {
    return {
      status: ConfirmRegistrationStatus.InProgress,
      ConfirmRegistrationStatus: ConfirmRegistrationStatus,
    }
  },
  mounted() {
    const route = useRoute()
    let userId = route.query.userId
    let token = route.query.token
    let email = route.query.email

    HTTP.post('/api/account/change-email/confirm', {
      userId: userId,
      token: token,
      email: email,
    })
      .then(async (response) => {
        let userStore = useUserStore()
        userStore.email = atob(email!.toString())

        this.status = ConfirmRegistrationStatus.Success

        await HTTP.post('/api/auth/refresh-token')
      })
      .catch((error) => {
        this.status = ConfirmRegistrationStatus.Error
      })
  },
}
</script>
