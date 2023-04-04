import { createApp } from 'vue';
import App from './App.vue';
import { router } from './router';
import 'vant/lib/index.css'

import { Button } from 'vant';
import { NavBar } from 'vant';
import { Tabbar, TabbarItem } from 'vant';
import { Toast } from 'vant';


const app = createApp(App);
app.use(router);
app.use(Button);
app.use(NavBar);
app.use(Tabbar);
app.use(TabbarItem);
app.use(Toast);

app.mount('#app');