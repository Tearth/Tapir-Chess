<template>
  <div class="flex flex-col items-center justify-center px-6 pt-16">
    <div class="grid md:grid-cols-[200px_auto] grid-cols-1 md:grid-cols-2 w-full lg:w-2/3">
      <div>
        <ul class="menu pt-0 w-full">
          <li><RouterLink to="/account/settings/profile">Edit profile</RouterLink></li>
          <li><RouterLink to="/account/settings/username">Change username</RouterLink></li>
          <li><RouterLink to="/account/settings/email">Change e-mail</RouterLink></li>
          <li><RouterLink to="/account/settings/password">Change password</RouterLink></li>
        </ul>
      </div>
      <div>
        <router-view />
      </div>
    </div>
  </div>
</template>
