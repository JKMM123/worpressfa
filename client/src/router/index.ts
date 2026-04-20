import { createRouter, createWebHashHistory, type RouteRecordRaw } from 'vue-router'
import { useAuthStore } from 'src/stores/authStore'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/dashboard'
  },
  {
    path: '/',
    component: () => import('src/layouts/AuthLayout.vue'),
    children: [
      { path: 'login', name: 'login', component: () => import('src/pages/auth/LoginPage.vue') },
      {
        path: 'register',
        name: 'register',
        component: () => import('src/pages/auth/RegisterPage.vue')
      }
    ]
  },
  {
    path: '/',
    component: () => import('src/layouts/MainLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      {
        path: 'dashboard',
        name: 'dashboard',
        component: () => import('src/pages/DashboardPage.vue')
      },
      { path: 'sales', name: 'sales', component: () => import('src/pages/SalesPage.vue') },
      {
        path: 'expenses',
        name: 'expenses',
        component: () => import('src/pages/ExpensesPage.vue')
      },
      {
        path: 'products',
        name: 'products',
        component: () => import('src/pages/ProductsPage.vue')
      },
      {
        path: 'customers',
        name: 'customers',
        component: () => import('src/pages/CustomersPage.vue')
      },
      { path: 'reports', name: 'reports', component: () => import('src/pages/ReportsPage.vue') }
    ]
  },
  {
    path: '/:catchAll(.*)*',
    redirect: '/dashboard'
  }
]

export const router = createRouter({
  history: createWebHashHistory(),
  routes
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore()

  // Attempt to restore session from stored refresh token on first load
  if (!authStore.isAuthenticated && authStore.refreshTokenValue) {
    await authStore.refreshToken()
  }

  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)

  if (requiresAuth && !authStore.isAuthenticated) {
    return { name: 'login' }
  }

  if ((to.name === 'login' || to.name === 'register') && authStore.isAuthenticated) {
    return { name: 'dashboard' }
  }
})

export default router
