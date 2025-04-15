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
      <div class="navbar-center">
        <ul class="menu menu-horizontal text-lg">
          <li><RouterLink to="/SignIn">SIGN IN</RouterLink></li>
          <li><RouterLink to="/SignIn">JOIN US</RouterLink></li>
        </ul>
      </div>

      <!--<div class="dropdown dropdown-end">
          <div tabIndex={0} role="button" class="btn btn-ghost btn-circle avatar">
            <div class="w-10 rounded-full">
              <img
                alt="Tailwind CSS Navbar component"
                src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
            </div>
          </div>
          <ul
            tabIndex={0}
            class="menu menu-sm dropdown-content bg-base-100 rounded-box z-1 mt-3 w-52 p-2 shadow">
            <li><a>Profile</a></li>
            <li><a>Logout</a></li>
          </ul>
        </div> -->
    </div>
  </div>
</template>
