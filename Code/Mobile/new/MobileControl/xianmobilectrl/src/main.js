import { createApp } from 'vue'
import App from './App.vue'
import Vant from 'vant';
import 'vant/lib/index.css';
import { Toast } from 'vant';
import { ConfigProvider } from 'vant';
import { router } from './router';
const app = createApp(App);

app.use(router);
app.use(Toast);
app.use(ConfigProvider);
// Method 1. via app.use
//app.use(Button);
app.mount('#app')