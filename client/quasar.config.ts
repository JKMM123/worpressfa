import { configure } from 'quasar/wrappers'

export default configure((_ctx) => {
  return {
    boot: ['axios'],

    css: ['app.scss'],

    extras: ['material-icons'],

    build: {
      target: {
        browser: ['es2022', 'firefox115', 'chrome115', 'safari14'],
        node: 'node20'
      },
      typescript: {
        strict: true,
        vueShim: true
      },
      vueRouterMode: 'hash'
    },

    devServer: {
      port: 9000,
      open: false
    },

    framework: {
      config: {
        dark: true
      },
      plugins: ['Dark', 'Notify', 'LocalStorage']
    },

    animations: []
  }
})
