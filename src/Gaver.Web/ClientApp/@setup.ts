// This file is prefixed with @ to appear at the top when auto-sorting imports
import 'nprogress/nprogress.css'
import './css/site.css'
import './initial'
import { setupProgress } from './utils/progress'
import 'simplebar/dist/simplebar.min.css'
import OfflinePluginRuntime from 'offline-plugin/runtime'

setupProgress()

OfflinePluginRuntime.install()
