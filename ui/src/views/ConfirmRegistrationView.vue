<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <fieldset class="fieldset w-xs bg-base-200 border border-base-300 p-4 rounded-box gap-3">
      <legend class="fieldset-legend">Confirm registration</legend>
      <div v-if="status == ConfirmRegistrationStatus.InProgress">Confirming your account...</div>
      <div v-else-if="status == ConfirmRegistrationStatus.Success">
        <div role="alert" class="alert alert-success alert-outline">
          <span>Thank you for joining us!</span>
        </div>
        <div class="text-sm pt-3">You can now <RouterLink to="/signin" class="link link-neutral">sign in</RouterLink> to your account.</div>
      </div>
      <div v-else-if="status == ConfirmRegistrationStatus.Error">
        <div role="alert" class="alert alert-error alert-outline">
          <span>Failed to activate your account, try again in a few minutes or contact our support.</span>
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

    HTTP.post('/api/auth/register/confirm', {
      userId: userId,
      token: token,
    })
      .then((response) => {
        this.status = ConfirmRegistrationStatus.Success
      })
      .catch((error) => {
        this.status = ConfirmRegistrationStatus.Error
      })
  },
}
</script>
