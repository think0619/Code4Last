import { createApp } from 'vue'
import App from './App.vue'
import Vant from 'vant';
import 'vant/lib/index.css';
import { Button } from 'vant';

const app = createApp(App);

// Method 1. via app.use
//app.use(Button);
app.mount('#app')