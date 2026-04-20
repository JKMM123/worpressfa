<template>
  <q-card-section>
    <q-form class="q-gutter-md" @submit.prevent="handleRegister">
      <q-input
        v-model="form.businessName"
        label="Business Name"
        outlined
        dense
        :rules="[required]"
        lazy-rules
      />

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
        :rules="[required, minLength]"
        lazy-rules
      >
        <template #append>
          <q-icon
            :name="showPassword ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="showPassword = !showPassword"
          />
        </template>
        <template #hint>
          <div class="q-mt-xs">
            <span
              v-for="(segment, i) in passwordStrengthSegments"
              :key="i"
              :class="['strength-segment', segment]"
            />
            <span class="q-ml-sm text-caption">{{ passwordStrengthLabel }}</span>
          </div>
        </template>
      </q-input>

      <q-input
        v-model="form.confirmPassword"
        :type="showConfirm ? 'text' : 'password'"
        label="Confirm Password"
        outlined
        dense
        :rules="[required, passwordsMatch]"
        lazy-rules
      >
        <template #append>
          <q-icon
            :name="showConfirm ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="showConfirm = !showConfirm"
          />
        </template>
      </q-input>

      <q-banner v-if="errorMessage" class="bg-negative text-white q-rounded-borders" dense>
        {{ errorMessage }}
      </q-banner>

      <q-btn
        type="submit"
        label="Create Account"
        color="primary"
        class="full-width"
        :loading="loading"
        unelevated
      />
    </q-form>
  </q-card-section>

  <q-card-section class="text-center q-pt-sm">
    <span class="text-caption text-grey-6">Already have an account? </span>
    <router-link to="/login" class="text-primary text-caption">Sign in</router-link>
  </q-card-section>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from 'src/stores/authStore'

const router = useRouter()
const authStore = useAuthStore()

const form = ref({ businessName: '', email: '', password: '', confirmPassword: '' })
const showPassword = ref(false)
const showConfirm = ref(false)
const loading = ref(false)
const errorMessage = ref('')

const required = (val: string) => !!val || 'This field is required'
const validEmail = (val: string) => /.+@.+\..+/.test(val) || 'Enter a valid email'
const minLength = (val: string) => val.length >= 8 || 'Password must be at least 8 characters'
const passwordsMatch = (val: string) => val === form.value.password || 'Passwords do not match'

const passwordStrength = computed(() => {
  const p = form.value.password
  if (!p) return 0
  let score = 0
  if (p.length >= 8) score++
  if (/[A-Z]/.test(p)) score++
  if (/[0-9]/.test(p)) score++
  if (/[^A-Za-z0-9]/.test(p)) score++
  return score
})

const strengthColors = ['', 'bg-negative', 'bg-warning', 'bg-info', 'bg-positive']
const strengthLabels = ['', 'Weak', 'Fair', 'Good', 'Strong']

const passwordStrengthSegments = computed(() =>
  [1, 2, 3, 4].map((i) => (i <= passwordStrength.value ? strengthColors[passwordStrength.value] : 'bg-grey-3'))
)

const passwordStrengthLabel = computed(() => strengthLabels[passwordStrength.value] ?? '')

async function handleRegister() {
  loading.value = true
  errorMessage.value = ''

  try {
    await authStore.register({
      email: form.value.email,
      password: form.value.password,
      businessName: form.value.businessName
    })
    await router.push('/dashboard')
  } catch (err: unknown) {
    const axiosError = err as { response?: { data?: { message?: string } } }
    errorMessage.value =
      axiosError.response?.data?.message ?? 'Registration failed. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.strength-segment {
  display: inline-block;
  width: 48px;
  height: 4px;
  border-radius: 2px;
  margin-right: 3px;
}
</style>
