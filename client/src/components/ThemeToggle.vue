<template>
  <q-btn
    :icon="$q.dark.isActive ? 'light_mode' : 'dark_mode'"
    flat
    round
    :aria-label="$q.dark.isActive ? 'Switch to light mode' : 'Switch to dark mode'"
    @click="toggle"
  />
</template>

<script setup lang="ts">
import { useQuasar } from 'quasar'

const $q = useQuasar()

const THEME_KEY = 'sme_theme_preference'

// Apply stored preference on mount
const stored = localStorage.getItem(THEME_KEY)
if (stored !== null) {
  $q.dark.set(stored === 'dark')
}

function toggle() {
  $q.dark.toggle()
  localStorage.setItem(THEME_KEY, $q.dark.isActive ? 'dark' : 'light')
}
</script>
