<template>
  <q-page class="q-pa-lg">
    <div class="page-title q-mb-lg">Dashboard</div>

    <div class="row q-gutter-md q-mb-lg">
      <div v-for="metric in metrics" :key="metric.label" class="col-12 col-sm-6 col-md-3">
        <q-card flat bordered class="metric-card">
          <q-card-section>
            <div class="row items-center no-wrap">
              <div>
                <div class="text-caption text-grey-6 q-mb-xs">{{ metric.label }}</div>
                <div class="text-h5 text-weight-bold">{{ metric.value }}</div>
              </div>
              <q-space />
              <q-icon :name="metric.icon" size="2rem" :color="metric.color" />
            </div>
            <div class="text-caption text-grey-5 q-mt-sm">Data will populate in Phase 2</div>
          </q-card-section>
        </q-card>
      </div>
    </div>

    <q-card flat bordered>
      <q-card-section>
        <div class="text-subtitle2 q-mb-md">Revenue Overview</div>
        <div class="placeholder-section">
          <div class="text-center">
            <q-icon name="bar_chart" size="3rem" color="grey-6" />
            <div class="text-grey-6 q-mt-sm">Charts coming in Phase 2</div>
          </div>
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
const metrics = [
  { label: 'Total Revenue', value: '$0', icon: 'trending_up', color: 'positive' },
  { label: 'Total Expenses', value: '$0', icon: 'receipt_long', color: 'negative' },
  { label: 'Net Profit', value: '$0', icon: 'account_balance_wallet', color: 'primary' },
  { label: 'Growth', value: '0%', icon: 'show_chart', color: 'info' }
]
</script>

<style scoped>
.metric-card {
  height: 100%;
}
</style>
