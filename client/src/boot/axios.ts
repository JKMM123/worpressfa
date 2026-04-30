// src/boot/axios.ts
import { boot } from 'quasar/wrappers'
import { api } from 'src/services/api'
import { useAuthStore } from 'src/stores/authStore'
import type { AxiosError, InternalAxiosRequestConfig } from 'axios'

export default boot(({ router }) => {
  // 1. Request Interceptor
  api.interceptors.request.use((config: InternalAxiosRequestConfig) => {
    const authStore = useAuthStore()
    
    if (authStore.accessToken) {
      config.headers.Authorization = `Bearer ${authStore.accessToken}`
    }
    return config
  })

  // Variables for Token Refresh Logic
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

  // 2. Response Interceptor
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
        // ✅ Safe! Using the router provided by the boot file instead of importing it
        await router.push('/login')
        return Promise.reject(error)
      }
    }
  )
})