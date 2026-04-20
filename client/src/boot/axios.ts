import { boot } from 'quasar/wrappers'
import axios from 'axios'

// The configured instance lives in services/api.ts.
// This file exists so Quasar includes axios in the bundle entry.
export default boot((_ctx) => {
  void axios
})
