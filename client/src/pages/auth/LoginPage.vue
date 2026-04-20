<template>
  <q-card-section>
    <q-form class="q-gutter-md" @submit.prevent="handleLogin">
      <q-input
        v-model="form.email"
        type="email"
        label="Email"
        outlined
        dense
        :rules="[required, validEmail]"
        lazy-rules
      />

      <q-input
        v-model="form.password"
        :type="showPassword ? 'text' : 'password'"
        label="Password"
        outlined
        dense
        :rules="[required]"
        lazy-rules
      >
        <template #append>
          <q-icon
            :name="showPassword ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="showPassword = !showPassword"
          />
        </template>
      </q-input>

      <q-banner v-if="errorMessage" class="bg-negative text-white q-rounded-borders" dense>
        {{ errorMessage }}
      </q-banner>

      <q-btn
        type="submit"
        label="Sign In"
        color="primary"
        class="full-width"
        :loading="loading"
        unelevated
      />
    </q-form>
  </q-card-section>

  <q-card-section class="text-center q-pt-sm">
    <span class="text-caption text-grey-6">Don't have an account? </span>
    <router-link to="/register" class="text-primary text-caption">Create one</router-link>
  </q-card-section>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from 'src/stores/authStore'

const router = useRouter()
const authStore = useAuthStore()

const form = ref({ email: '', password: '' })
const showPassword = ref(false)
const loading = ref(false)
const errorMessage = ref('')

const required = (val: string) => !!val || 'This field is required'
const validEmail = (val: string) => /.+@.+\..+/.test(val) || 'Enter a valid email'

async function handleLogin() {
  loading.value = true
  errorMessage.value = ''

  try {
    await authStore.login(form.value)
    await router.push('/dashboard')
  } catch (err: unknown) {
    const axiosError = err as { response?: { data?: { message?: string } } }
    errorMessage.value =
      axiosError.response?.data?.message ?? 'Login failed. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>
