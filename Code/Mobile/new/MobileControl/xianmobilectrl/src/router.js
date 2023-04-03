import { createRouter, createWebHashHistory } from 'vue-router';

const router = createRouter({
    history: createWebHashHistory(),
    routes: [{
        path: '/',
        component: () =>
            import ('./components/Index/TestName')
    }, {
        path: '/abc',
        component: () =>
            import ('./components/HelloWorld')
    }, ],
});

export default router;