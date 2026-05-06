<template>
  <q-page class="q-pa-lg">
    <div class="page-title q-mb-lg">Dashboard</div>

    <!-- KPI Cards -->
    <div class="row q-col-gutter-md q-mb-lg">
      <div class="col-12 col-sm-6 col-md-3">
        <q-card flat bordered class="metric-card">
          <q-card-section>
            <div class="text-caption text-grey-6 q-mb-xs">Total Revenue</div>
            <div class="text-h5 text-weight-bold text-positive">{{ formatCurrency(kpi.totalRevenue) }}</div>
            <div class="row items-center q-mt-sm">
              <q-icon name="trending_up" size="sm" color="positive" />
              <span class="text-caption text-grey-6 q-ml-xs">All time</span>
            </div>
          </q-card-section>
        </q-card>
      </div>

      <div class="col-12 col-sm-6 col-md-3">
        <q-card flat bordered class="metric-card">
          <q-card-section>
            <div class="text-caption text-grey-6 q-mb-xs">Total Expenses</div>
            <div class="text-h5 text-weight-bold text-negative">{{ formatCurrency(kpi.totalExpenses) }}</div>
            <div class="row items-center q-mt-sm">
              <q-icon name="receipt_long" size="sm" color="negative" />
              <span class="text-caption text-grey-6 q-ml-xs">All time</span>
            </div>
          </q-card-section>
        </q-card>
      </div>

      <div class="col-12 col-sm-6 col-md-3">
        <q-card flat bordered class="metric-card">
          <q-card-section>
            <div class="text-caption text-grey-6 q-mb-xs">Net Profit</div>
            <div
              class="text-h5 text-weight-bold"
              :class="kpi.netProfit >= 0 ? 'text-primary' : 'text-negative'"
            >
              {{ formatCurrency(kpi.netProfit) }}
            </div>
            <div class="row items-center q-mt-sm">
              <q-icon name="account_balance_wallet" size="sm" color="primary" />
              <span class="text-caption text-grey-6 q-ml-xs">Revenue − Expenses</span>
            </div>
          </q-card-section>
        </q-card>
      </div>

      <div class="col-12 col-sm-6 col-md-3">
        <q-card flat bordered class="metric-card">
          <q-card-section>
            <div class="text-caption text-grey-6 q-mb-xs">Monthly Growth</div>
            <div
              class="text-h5 text-weight-bold"
              :class="kpi.growthPercentage >= 0 ? 'text-info' : 'text-negative'"
            >
              {{ kpi.growthPercentage >= 0 ? '+' : '' }}{{ kpi.growthPercentage.toFixed(2) }}%
            </div>
            <div class="row items-center q-mt-sm">
              <q-icon name="show_chart" size="sm" color="info" />
              <span class="text-caption text-grey-6 q-ml-xs">vs last month</span>
            </div>
          </q-card-section>
        </q-card>
      </div>
    </div>

    <!-- Chart -->
    <q-card flat bordered>
      <q-card-section>
        <div class="text-subtitle1 text-weight-medium q-mb-md">Revenue Overview — Last 6 Months</div>
        <div v-if="chartLoading" class="flex flex-center q-pa-xl">
          <q-spinner size="3rem" color="primary" />
        </div>
        <canvas v-show="!chartLoading" ref="chartCanvas" style="max-height: 320px;" />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import { dashboardApi, type KpiSummary } from 'src/services/apiService'
import { Chart, registerables } from 'chart.js'
import { useQuasar } from 'quasar'

Chart.register(...registerables)

const $q = useQuasar()
const chartCanvas = ref<HTMLCanvasElement | null>(null)
let chartInstance: Chart | null = null
const chartLoading = ref(true)

const kpi = ref<KpiSummary>({
  totalRevenue: 0,
  totalExpenses: 0,
  netProfit: 0,
  growthPercentage: 0,
})

function formatCurrency(value: number): string {
  return `$${value.toFixed(2)}`
}

async function fetchData() {
  try {
    const [kpiRes, chartRes] = await Promise.all([
      dashboardApi.getKpiSummary(),
      dashboardApi.getChartData(),
    ])
    kpi.value = kpiRes.data
    renderChart(chartRes.data.monthlyData)
  } catch {
    $q.notify({ type: 'negative', message: 'Failed to load dashboard data' })
  } finally {
    chartLoading.value = false
  }
}

function renderChart(monthlyData: { month: string; sales: number; expenses: number; profit: number }[]) {
  if (!chartCanvas.value) return

  if (chartInstance) {
    chartInstance.destroy()
  }

  const ctx = chartCanvas.value.getContext('2d')
  if (!ctx) return

  chartInstance = new Chart(ctx, {
    type: 'line',
    data: {
      labels: monthlyData.map(d => d.month),
      datasets: [
        {
          label: 'Sales',
          data: monthlyData.map(d => d.sales),
          borderColor: '#22c55e',
          backgroundColor: 'rgba(34, 197, 94, 0.1)',
          tension: 0.4,
          fill: true,
        },
        {
          label: 'Expenses',
          data: monthlyData.map(d => d.expenses),
          borderColor: '#ef4444',
          backgroundColor: 'rgba(239, 68, 68, 0.1)',
          tension: 0.4,
          fill: true,
        },
        {
          label: 'Profit',
          data: monthlyData.map(d => d.profit),
          borderColor: '#3b82f6',
          backgroundColor: 'rgba(59, 130, 246, 0.1)',
          tension: 0.4,
          fill: true,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: true,
      plugins: {
        legend: { position: 'top' },
        tooltip: {
          callbacks: {
            label: ctx => ` ${ctx.dataset.label}: $${(ctx.parsed.y as number).toFixed(2)}`,
          },
        },
      },
      scales: {
        y: {
          beginAtZero: true,
          ticks: {
            callback: val => `$${val}`,
          },
        },
      },
    },
  })
}

onMounted(fetchData)
onBeforeUnmount(() => chartInstance?.destroy())
</script>

<style scoped>
.metric-card {
  height: 100%;
}
</style>
