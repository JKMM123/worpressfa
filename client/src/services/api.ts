import axios, { type AxiosError, type InternalAxiosRequestConfig } from 'axios'
import { useAuthStore } from 'src/stores/authStore'
import { router } from 'src/router'

const API_BASE_URL = 'http://localhost:5000/api'

export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Attach access token to every request
api.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  // Access the store lazily to avoid circular dependency at module init time
  const authStore = useAuthStore()
  if (authStore.accessToken) {
    config.headers.Authorization = `Bearer ${authStore.accessToken}`
  }
  return config
})

// On 401 — attempt silent token refresh, then retry once
let isRefreshing = false
let pendingQueue: Array<{
  resolve: (value: string | null) => void
  reject: (reason?: unknown) => void
}> = []

function drainQueue(token: string | null, error?: unknown) {
  pendingQueue.forEach(({ resolve, reject }) => {
    if (token) resolve(token)
    else reject(error)
  })
  pendingQueue = []
}

api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retried?: boolean }

    if (error.response?.status !== 401 || originalRequest._retried) {
      return Promise.reject(error)
    }

    originalRequest._retried = true

    if (isRefreshing) {
      return new Promise((resolve, reject) => {
        pendingQueue.push({ resolve, reject })
      }).then((token) => {
        originalRequest.headers.Authorization = `Bearer ${token}`
        return api(originalRequest)
      })
    }

    isRefreshing = true

    const authStore = useAuthStore()
    const success = await authStore.refreshToken()

    isRefreshing = false

    if (success) {
      drainQueue(authStore.accessToken)
      originalRequest.headers.Authorization = `Bearer ${authStore.accessToken}`
      return api(originalRequest)
    } else {
      drainQueue(null, error)
      await router.push('/login')
      return Promise.reject(error)
    }
  }
)
